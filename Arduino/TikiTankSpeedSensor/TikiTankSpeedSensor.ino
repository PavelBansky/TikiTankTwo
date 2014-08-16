/**
* Tiki Tank Speed Sensor 2.0
* http://www.tikitank.com/
* 
* This is the firmware for the speed sensor CPU that reads the HAL sensor values
* 
* Intended CPU: ATMega 328
*
*/

// Pin where the HAL sensor comparator is connected
const int sensorPin = 3;
// Pin for status LED pin
const int ledPin = 7;
// Timeout interval in milliseconds
const int timeout = 1300;
// sleep time in milliseconds
const int sleepTime = 1;

volatile int ticksCounted = 0;
volatile int tickEdge = 0;
volatile int state = 1;
volatile unsigned long lastInterrupt = 0;
int ticksWritten = 0;



void setup()
{
	Serial.begin(9600);
	pinMode(ledPin, OUTPUT);
	pinMode(sensorPin, INPUT);
   
	Serial.write((uint8_t)0);
	attachInterrupt(1, interruptHandler, CHANGE);
}

void loop()
{
	// Reflect sensor status to LED. This is visual control when adjusting the sensor sensitivity
	//digitalWrite(ledPin, digitalRead(sensorPin));

        int delta = ticksCounted - ticksWritten;
        if (delta != 0)
        {
          delta = (delta > 9) ? 9 : delta;
          Serial.write((uint8_t)delta);
          ticksWritten += delta;
        }
            
        delay(sleepTime); // 20 miliseconds;
 }

void interruptHandler()
{
  unsigned long now = millis();
  if ((now - lastInterrupt) >= 10)
  {
    tickEdge = !tickEdge;
    //ticksCounted += tickEdge;
  
    if (tickEdge)
    {
      state = !state;
      digitalWrite(ledPin, state);
    
      ++ticksCounted;
    }
  }
     lastInterrupt = now;   
}
