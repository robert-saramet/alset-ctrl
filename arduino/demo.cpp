unsigned long lastTx20MS = millis();
unsigned long lastTx3S = millis();

uint8_t distF = 37;
uint8_t throttle = 0;

void setup() {
    Serial.begin(115200);
    Serial.setTimeout(30);
}

void txPC() {
    if (Serial.availableForWrite() > 25) {
        if (millis() - lastTx20MS >= 20) {
            Serial.print(F("<ui8:distF:"));
            Serial.print(distF);
            Serial.println("/>");
            Serial.print(F("Time: "));
            Serial.println(millis() - lastTx20MS);
            lastTx20MS = millis();
        }
    }
}

void rxPC() {
    while (Serial.available() > 0) {
        String input = Serial.readStringUntil('>');
        short nameStart = input.indexOf(':');
        short nameEnd = input.lastIndexOf(':');
        short stringEnd = input.lastIndexOf('/');
        String type = input.substring(1, nameStart);
        String name = input.substring(nameStart + 1, nameEnd);
        String value = input.substring(nameEnd + 1, stringEnd);
        
        Serial.print(type);
        Serial.print(" | ");
        Serial.print(name);
        Serial.print(" | ");
        Serial.println(value);
        Serial.println(input);
    }
}

void loop() {
    txPC();
    rxPC();
    
}
