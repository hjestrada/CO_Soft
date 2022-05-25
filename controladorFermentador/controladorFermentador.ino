
int ph = 0;
int temp = 0;
int Co2 = 0;

void setup() {
  Serial.begin(9600);
}


void loop() {

  ph = random(0, 14);
  temp = random(0, 100);
  Co2 = random(0, 1000);


  Serial.print(ph);
  Serial.print(";");
  Serial.print(temp);
  Serial.print(";");
  Serial.print(Co2);
  Serial.println();
  delay(1000);

}
