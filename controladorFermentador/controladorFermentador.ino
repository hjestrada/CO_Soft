
String ph = "";
String temp = "";
String Co2 = "";
String delimitador = "/";
String enviar = "";

void setup() {
  Serial.begin(9600);
}


void loop() {

  ph = random(1, 14);
  temp = random(1, 100);
  Co2 = random(10, 1000);

  enviar =    ph + delimitador + temp + delimitador + Co2;

  Serial.println(enviar);
  delay(1000);
  }
