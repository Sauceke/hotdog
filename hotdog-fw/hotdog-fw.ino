#include <SPI.h>
#include <RF24.h>
#include <BTLE.h>
#include <avrcontext_arduino.h>

#define STACK_SIZE 256

static uint8_t sensorStack[STACK_SIZE], broadcastStack[STACK_SIZE];
static avr_coro_t sensorState, broadcastState;

// sensors: A0 to A4
// A0 is the topmost sensor
const int sensors = 5;

// how much the sensor reading has to increase to consider it a shadow
const float threshold = 1.2;

// the sensor readings at calibration
int baseValues[sensors];

// the current depth level (100: fully inserted)
int level = 0;

RF24 radio(9, 10, 1000000);
BTLE btle(&radio);

int readSensor(int pin) {
  pinMode(pin, INPUT_PULLUP);
  delay(1);
  int value = analogRead(pin);
  pinMode(pin, INPUT);
  return value;
}

void calibrate() {
  // One measurement is no measurement.
  int rounds = 10;
  for (int i = 0; i < sensors; i++) {
    for (int j = 0; j < rounds; j++) {
      baseValues[i] += readSensor(A0 + i);
      delay(10);
    }
    baseValues[i] /= rounds;
  }
}

void setup() {
  Serial.begin(9600);
  delay(1000);
  btle.begin("hotdog");
  radio.setPALevel(RF24_PA_LOW);
  calibrate();
  avr_coro_init(&sensorState, (void*)sensorStack, STACK_SIZE,
      (avr_coro_func_t)sensorLoop);
  avr_coro_init(&broadcastState, (void*)broadcastStack, STACK_SIZE,
      (avr_coro_func_t)broadcastLoop);
}

void loop() {
  avr_coro_resume(&sensorState, (void *)&level);
  avr_coro_resume(&broadcastState, (void *)&level);
}

static void* sensorLoop(avr_coro_t *self, size_t *data) {
  while (true) {
    float lvl = 0;
    for (int i = 0; i < sensors; i++) {
      avr_coro_yield(self, (void*)data);
      int value = readSensor(A0 + i);
      // stop at the first sensor, from top, that detects a shadow
      if (value > baseValues[i] * threshold) {
        lvl += sensors - i;
        break;
      }
      lvl = (((float)value) / baseValues[i] - 1) / (threshold - 1);
    }
    lvl /= sensors;
    lvl = max(min(lvl, 1), 0);
    *data = int(lvl * 100);
  }
}

static void* broadcastLoop(avr_coro_t *self, size_t *data) {
  while (true) {
    size_t lvl = *data;
    char message[10];
    sprintf(message, "L0%d", 100 - lvl);
    btle.advertise(0x16, &message, strlen(message));
    btle.hopChannel();
    Serial.println(message);
    delay(1);
    avr_coro_yield(self, (void*)data);
  }
}
