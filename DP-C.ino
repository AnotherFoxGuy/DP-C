#include <Servo.h>
const int pingPin = 7; // Trigger pin of ultrasonic Sensor
const int echoPin = 6; // Echo Pin of Ultra Sensor
int val; // value van servo
Servo servo;

void setup() {
  DDRB = 0x3f;
  servo.attach(9);
  Serial.begin(9600);
}

void loop() {

  
  long duration, inches, cm;
  pinMode(pingPin, OUTPUT);
  digitalWrite(pingPin, LOW);
  delayMicroseconds(2);
  digitalWrite(pingPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(pingPin,LOW);
  pinMode(echoPin, INPUT);
  duration = pulseIn(echoPin,HIGH);
  cm = microsecondsToCentimeters(duration);
  Serial.print(cm);
  Serial.println();
  if(cm < 10 && cm < 100)
  {
    servo.write(90);
  
  }
  else
  {
    servo.write(0);

  }
  delay(2000);

}

long microsecondsToCentimeters(long microseconds) {
   return microseconds / 29 / 2;
}
