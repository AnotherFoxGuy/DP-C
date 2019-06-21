#include <Servo.h>
#include <SPI.h>
#include <Ethernet.h>
#include <NewRemoteReceiver.h>
#include <NewRemoteTransmitter.h>

const int pingPin = 7; // Trigger pin of ultrasonic Sensor
const int echoPin = 6; // Echo Pin of Ultra Sensor
int val; // value van servo
Servo servo;
String data;
byte mac[] = {0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED};
IPAddress ip(192, 168, 1, 2);  //fallback ipadress
EthernetServer server(8000);   //server port

// Create a transmitter on address 123, using digital pin 11 to transmit, 
// with a period duration of 260ms (default), repeating the transmitted
// code 2^3=8 times.
NewRemoteTransmitter transmitter(123, 3, 260, 3);

void setup() {
    DDRB = 0x3f;
    servo.attach(9);
    Serial.begin(115200);

    //no dhcp >> use ip given above
    if (Ethernet.begin(mac) == 0) {
        Serial.println("No DHCP");
        Ethernet.begin(mac, ip);
    }

    server.begin();
    Serial.print("Server ip is: ");
    Serial.println(Ethernet.localIP());

    NewRemoteReceiver::init(0, 2, showCode);

}

void loop() {

    EthernetClient client = server.available();
    if (client) {
        Serial.println("New client connected");
        while (client.connected()) {
            if (client.available()) {
                //recive data
                char c = client.read();
                char t = 'T';
                char f = 'F';
                Serial.println(c);

                if (c == t) {
                    Serial.println("true");
                    servo.write(90);
                }
                if (c == f) {
                    Serial.println("false");
                    servo.write(0);
                    //RemoteTransmitter::sendCode(3, 9999, 6, 3612);

                }
            }
        }
        Serial.println("Client disconnected");
        delay(2000);
    } else {
        long duration, inches, cm;
        pinMode(pingPin, OUTPUT);
        digitalWrite(pingPin, LOW);
        delayMicroseconds(2);
        digitalWrite(pingPin, HIGH);
        delayMicroseconds(10);
        digitalWrite(pingPin, LOW);
        pinMode(echoPin, INPUT);
        duration = pulseIn(echoPin, HIGH);
        cm = microsecondsToCentimeters(duration);
        Serial.print(cm);
        Serial.println();
        if (cm < 100) {
            if (cm < 10) {
                servo.write(90);
                // Switch unit 2 off
                transmitter.sendUnit(2, true);
            }
            if (cm > 10) {
                servo.write(0);
            }
        }

        delay(2000);
    }
}

// Callback function is called only when a valid code is received.
void showCode(NewRemoteCode receivedCode) {
    // Note: interrupts are disabled. You can re-enable them if needed.

    // Print the received code.
    Serial.print("Addr ");
    Serial.print(receivedCode.address);

    switch (receivedCode.switchType) {
        case NewRemoteCode::off:
            Serial.print(" off");
            servo.write(0);
            break;
        case NewRemoteCode::on:
            Serial.print(" on");
            servo.write(90);
            break;
    }

}


long microsecondsToCentimeters(long microseconds) {
    return microseconds / 29 / 2;
}
