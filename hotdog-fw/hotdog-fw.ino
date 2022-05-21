#include <SPI.h>
#include <RF24.h>
#include <BTLE.h>
#include <pt.h>

// sensors: A0 to A4
// A0 is the topmost sensor
const int sensors = 5;

// how much the sensor reading has to increase to consider it a shadow
const float threshold = 1.2;

// the sensor readings at calibration
int baseValues[sensors];

struct pt sensorThread, broadcastThread;

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
  PT_INIT(&sensorThread);
  PT_INIT(&broadcastThread);
}

void loop() {
  static int level = 0;
  sensorLoop(&sensorThread, &level);
  broadcastLoop(&broadcastThread, &level);
}

void toTcode(char* out, int level) {
  sprintf(out, "L0%d", 100 - level);
}

PT_THREAD(sensorLoop(struct pt* pt, int* level)) {
  PT_BEGIN(pt);
  static float lvl;
  static int i;
  while (true) {
    lvl = 0;
    for (i = 0; i < sensors; i++) {
      PT_YIELD(pt);
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
    *level = int(lvl * 100);
  }
  PT_END(pt);
}

PT_THREAD(broadcastLoop(struct pt* pt, int* level)) {
  PT_BEGIN(pt);
  while (true) {
    char message[10];
    toTcode(message, *level);
    btle.advertise(0x16, &message, strlen(message));
    btle.hopChannel();
    delay(1);
    Serial.println(message);
    PT_YIELD(pt);
  }
  PT_END(pt);
}
