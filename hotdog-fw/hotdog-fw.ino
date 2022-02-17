// random base64 string
const String protocolId = "HQNDmlUP";
const String depthKey = "lvl";
const char separator = ' ';

// sensors: A0 to A4
// A0 is the topmost sensor
const int sensors = 5;

// number of outputs per second
const int sampleRate = 30;

// how much the sensor reading has to increase to consider it a shadow
const float threshold = 1.2;

// the sensor readings at calibration
int baseValues[sensors];

int readSensor(int pin) {
  pinMode(pin, INPUT_PULLUP);
  delay(1000 / (sampleRate * sensors * 2));
  int value = analogRead(pin);
  pinMode(pin, INPUT);
  delay(1000 / (sampleRate * sensors * 2));
  return value;
}

void calibrate() {
  // One measurement is no measurement.
  int rounds = 10;
  for (int i = 0; i < sensors; i++) {
    for (int j = 0; j < rounds; j++) {
      baseValues[i] += readSensor(A0 + i);
    }
    baseValues[i] /= rounds;
  }
}

void setup() {
  Serial.begin(9600);
  calibrate();
}

void loop() {
  float lvl = 0;
  for (int i = 0; i < sensors; i++) {
    int value = readSensor(A0 + i);
    // stop at the first sensor, from top, that detects a shadow
    if (value > baseValues[i] * threshold) {
      lvl += sensors - i;
      delay((sensors - i - 1) * 1000 / (sampleRate * sensors));
      break;
    }
    lvl = (((float)value) / baseValues[i] - 1) / (threshold - 1);
  }
  lvl /= sensors;
  lvl = max(min(lvl, 1), 0);
  Serial.println(protocolId + separator + depthKey + separator + String(lvl, 2));
  Serial.flush();
}
