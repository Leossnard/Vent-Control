//#include <Servo.h>
#include "max6675.h"

// Definir Variables Globales
String StringRecibido; // Donde se almacena toda la data entrante
char inChar; // Caracter que se recibe
String ComandoToEjecutar;
String indicesComando;
bool empiezaString = false;
bool terminaString = false;
int angulosCompuertas[32];//acepta hasta 32 compuertas por arduino, pueden ser mas
int indiceIntArray = 0;
unsigned long millisAnterior = 0;
int intervalo = 1000;//indica cada cuantos milisegundos se quiere hacer una medicion
// Variables Dinamicas, agregar aca los pines de las demas sensores/servo motores
int thermoDO = 2;
int thermoCS = 3;
int thermoCLK = 4;

int thermoDO2 = 5;
int thermoCS2 = 6;
int thermoCLK2 = 7;

int thermoDO3 = 8;
int thermoCS3 = 9;
int thermoCLK3 = 10;

int thermoDO4 = 11;
int thermoCS4 = 12;
int thermoCLK4 = 13;

//Servo servoMotor;
//Servo servoMotor2;
//Servo servoMotor3;
//Servo servoMotor4;


//Servo servoMotor;//deja algunos pines sin funcionar buscar segun modelo de placa arduino en arduino mega son los pines 11 y 12



//variables para calibracion sensores de presion

int offset1, offset2,offset3,offset4,offset5,offset6,offset7, offset8;
int offset[] = {offset1, offset2, offset3, offset4, offset5, offset6, offset7, offset8};

int pinesAnalogicos[] = {A0, A1, A2, A3, A4, A5, A6, A7};

void setup() {
  //servoMotor.attach(31);
  //servoMotor2.attach(35);
  //servoMotor3.attach(39);
  //servoMotor4.attach(43);
  
  Serial.begin(9600);   // Configura la velocidad del puerto USB (9600 es un estandar)
  //pinMode(x, OUTPUT);  // Configura el pin 3 como Salida Digital

  CalibrarSensoresPresion();
}


void loop() {
  // Leer la informacion que proviene desde el USB
  while(Serial.available() > 0){
    inChar = Serial.read();
    if(inChar == '#' || empiezaString == true){// si el caracter recibido es #, significa que viene en camino un comando
      empiezaString = true;
      StringRecibido = String(StringRecibido + inChar);
    }
    if(inChar == '-' || '\n'){//'\n'
      terminaString = true;
    }
  }
  // si un comando llega desde el puerto USB (un String que empiece y termine con #, '\n', respectivamente)
  if(terminaString){
    // Si hay un mensaje se ejecuta esta parte del codigo
    ComandoToEjecutar = StringRecibido.substring(0,4);
    indicesComando = StringRecibido.substring(4);
    // En el Caso que llege un comando para modificar unos de los Servo Motores disponibles ("SER" de Servo Motor)
    if(ComandoToEjecutar == "#SER"){
      indiceIntArray = 0;//Otorga el largo menos uno de angulos Compuertas Array en esta iteracion
      String stringTemporal;
      const char charArray[indicesComando.length() + 1];// = indicesComando;//32*3 (n compuertas y 3 largo del caracteres maximos
      strcpy(charArray, indicesComando.c_str());
      for(int i=0; i < strlen(charArray);i++){
        char c = charArray[i];
        stringTemporal = String(stringTemporal + c);
        if(c == ';' || c == '\n' || c == '-'){//ELIMINAR EL -
          angulosCompuertas[indiceIntArray] = stringTemporal.toInt();
          indiceIntArray++;
          stringTemporal = "";
        }
      }
      // Poner en orden los servo motores que se quieran mover, reemplazar el valor entre corchetes
      //servoMotor.write(map(angulosCompuertas[0],0,90,0,100));//posicion 4 y 5 indican los valores donde se obtuvieron 0 y 90 grados en realidad, se usa para calibrar
      //servoMotor2.write(map(angulosCompuertas[1],0,90,0,100));
      //servoMotor3.write(map(angulosCompuertas[2],0,90,0,100));
      //servoMotor4.write(map(angulosCompuertas[3],0,90,0,100));
      //Serial.println(String(angulosCompuertas[0])+ ";" + String(angulosCompuertas[1])+ + ";" + String(angulosCompuertas[2])+ ";" + String(angulosCompuertas[3]));
      //servoMotor.write(map(angulosCompuertas[1],0,90,angulo donde se obtuvo 0°,angulo donde se obtuvo 90°));//en el caso de que sean mas de un servomotor
    }
    //else if(ComandoToEjecutar == "#XXX")...para mas casos de entradas
    
    // Reestablece las variables de entrada
    StringRecibido = "";
    empiezaString = false;
    terminaString = false;
  }
   //lee la informacion de los sensores y la envia por el puerto USB hacia una PC, con un determinado tiempo por cada medicion, intervalo se configura en variables globales
  unsigned long millisActual = millis();
  if(millisActual - millisAnterior >= intervalo){
    millisAnterior = millisActual;

    MAX6675 thermocouple(thermoCLK, thermoCS, thermoDO);
    MAX6675 thermocouple2(thermoCLK2, thermoCS2, thermoDO2);
    MAX6675 thermocouple3(thermoCLK3, thermoCS3, thermoDO3);
    MAX6675 thermocouple4(thermoCLK4, thermoCS4, thermoDO4);

    double lecturaTEMP = thermocouple.readCelsius();
    double lecturaTEMP2 = thermocouple2.readCelsius();
    double lecturaTEMP3 = thermocouple3.readCelsius();
    double lecturaTEMP4 = thermocouple4.readCelsius();
  
    double lecturaPresion = transformarVoltajeSensorP(0);
    double lecturaPresion2 = transformarVoltajeSensorP(1);
    double lecturaPresion3 = transformarVoltajeSensorP(2);
    double lecturaPresion4 = 700.72;//transformarVoltajeSensorP(3);//despues cambiar esto cuando el nuevo sensor llegue (lo mismo con el sensor 7)
    double lecturaPresion5 = transformarVoltajeSensorP(4);
    double lecturaPresion6 = transformarVoltajeSensorP(5);
    double lecturaPresion7 = transformarVoltajeSensorP(6);
    double lecturaPresion8 = 700.21;//transformarVoltajeSensorP(7);

    // Concatenar la medicion de los sensores, debe ser print no println
    Serial.println("#SEN" +String(lecturaTEMP)+ ";" + String(lecturaTEMP2)+ + ";" + String(lecturaTEMP3)+ ";" + String(lecturaTEMP4)
    + ";" + String(lecturaPresion)+ ";" + String(lecturaPresion2)+ ";" + String(lecturaPresion3)+ ";" + String(lecturaPresion4)
    + ";" + String(lecturaPresion5)+ ";" + String(lecturaPresion6)+ ";" + String(lecturaPresion7)+ ";" + String(lecturaPresion8)
    );
  }
  
}

void CalibrarSensoresPresion(){
   int sumTemp = 0;
   int sensorValue = 0;
   int ciclos = 100;
   
  int j = 0;
   
   Serial.println("Calibrando sensores de presion ...");
   for (j=0; j<sizeof(pinesAnalogicos);j++){
   sumTemp = 0;
   int i = 0;
    for(i=0;i<ciclos;i++)
    {
         sensorValue= analogRead(pinesAnalogicos[j]);
         sumTemp+=sensorValue;
    }
    offset[j]=sumTemp/double(ciclos);
    Serial.println("Calibracion Sensor " + String((j+1)) + " OK");
   }

  }

 double transformarVoltajeSensorP(int pinAnalogico){
    double Vout= 0;
    int pinesAnalogicos3[] = {A0, A1, A2, A3, A4, A5, A6, A7};
    int medicionesParaPromedio = 30;
    int medicionesTemp = 0;
    for(int ii=0; ii<medicionesParaPromedio; ii++){
      medicionesTemp += analogRead(pinesAnalogicos3[pinAnalogico]) - offset[pinAnalogico];
    }
    double lecturaTemp = medicionesTemp / double(medicionesParaPromedio);

    //double lecturaTemp = double(analogRead(pinesAnalogicos3[pinAnalogico]))-offset[pinAnalogico];
    Vout = 5*abs(lecturaTemp/1023.0);//Valor absoluto por que luego se restaran
    return (Vout-2.5)*1000;
    
  }
