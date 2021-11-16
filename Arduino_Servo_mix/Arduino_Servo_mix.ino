#include <Servo.h>//Libreria Servo deja algunos pines sin funcionar buscar segun modelo de placa arduino
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
int intervalo = 500;//indica cada cuantos milisegundos se quiere hacer una medicion
bool modomanual;


// Valores iniciales para calibracion de los motores
// Variables Dinamicas, agregar aca los pines de las demas sensores/servo motores
int PINSERVO1 = 2;
int PULSOMIN1 = 650;
int PULSOMAX1 = 2670;
int pot1 = 1;//pin en donde se conecta el potenciometro 1
int valpot1;//varible que lee el valor de los potenciometros


int PINSERVO2 = 4;
int PULSOMIN2 = 670;
int PULSOMAX2 = 2380;
int pot2 = 2;//pin en donde se conecta el potenciometro 2
int valpot2;//varible que lee el valor de los potenciometros

int PINSERVO3 = 6;
int PULSOMIN3 = 670;
int PULSOMAX3 = 2380;
int pot3 = 3;//pin en donde se conecta el potenciometro 3
int valpot3;//varible que lee el valor de los potenciometros

int PINSERVO4 = 8;
int PULSOMIN4 = 670;
int PULSOMAX4 = 2380;
int pot4 = 4;//pin en donde se conecta el potenciometro 4
int valpot4;//varible que lee el valor de los potenciometros

Servo servo1;
Servo servo2;
Servo servo3;
Servo servo4;

int umbral= 100;//en voltaje es la sensibilidad del potenciometro


int valpot1_i;
int valpot2_i;
int valpot3_i;
int valpot4_i;

void setup() {
  
  Serial.begin(9600);   // Configura la velocidad del puerto USB (9600 es un estandar)
  servo1.attach(PINSERVO1, PULSOMIN1, PULSOMAX1);//setea el pin de salida para el servomotor y su calibracion
  servo2.attach(PINSERVO2, PULSOMIN2, PULSOMAX2);
  servo3.attach(PINSERVO3, PULSOMIN3, PULSOMAX3);
  servo4.attach(PINSERVO4, PULSOMIN4, PULSOMAX4);
  Serial.begin(9600);
  servo1.write(0);//deja los servos en 0 grados inicialmente
  servo2.write(0);
  servo3.write(0);
  servo4.write(0);

  //Capturar el valor inicial de los potencion
  valpot1_i = analogRead(pot1);  
  valpot2_i = analogRead(pot2);
  valpot3_i = analogRead(pot3);
  valpot4_i = analogRead(pot4);

  modomanual = false;//Para que no entre en la primera iteracion
  
  //pinMode(x, OUTPUT);  // Configura el pin 3 como Salida Digital

}

void loop() {
  if(modomanual){
      // Movimiento con potenciometro
  //si el valor de cada potenciometro excede un umbral definido quiere decir que se ha movido, por ende 
  //se realiza el moviento manual del servomotor
  valpot1 = analogRead(pot1);  
  valpot2 = analogRead(pot2);
  valpot3 = analogRead(pot3);
  valpot4 = analogRead(pot4);

  //Serial.println("1: " + String(valpot1));
  //Serial.println(valpot2);
  //Serial.println(valpot3);
  //Serial.println(valpot4);


  if(abs(valpot1_i-valpot1) > umbral){
    Serial.println("1: " + String(valpot1_i-valpot1));
    valpot1_i = valpot1;
    valpot1 = map(valpot1, 0, 1023, 0, 90);
     servo1.write(valpot1);
     
  }
   if(abs(valpot2_i-valpot2) > umbral){
    Serial.println("1: " + String(valpot2_i-valpot2));
    valpot2_i = valpot2;
    valpot2 = map(valpot2, 0, 1023, 0, 100);
     servo2.write(valpot2);
     
  }
    if(abs(valpot3_i-valpot3) > umbral){
      Serial.println("1: " + String(valpot3_i-valpot3));
      valpot3_i = valpot3;
    valpot3 = map(valpot3, 0, 1023, 0, 100);
     servo3.write(valpot3);
     
  }
    else if(abs(valpot4_i-valpot4) > umbral){
      Serial.println("1: " + String(valpot4_i-valpot4));
    valpot4_i = valpot4;
    valpot4 = map(valpot4, 0, 1023, 0, 110);
     servo4.write(valpot4);
     
  }

  }


  // Leer la informacion que proviene desde el USB
  while(Serial.available() > 0){
    inChar = Serial.read();
    if(inChar == '#' || empiezaString == true){// si el caracter recibido es #, significa que viene en camino un comando
      empiezaString = true;
      StringRecibido = String(StringRecibido + inChar);
    }
    if(inChar == '\n' || inChar == '*'){
      terminaString = true;
    }
  }
  // si un comando llega desde el puerto USB (un String que empiece y termine con #, '\n', respectivamente)
  if(terminaString){
    // Si hay un mensaje se ejecuta esta parte del codigo
    ComandoToEjecutar = StringRecibido.substring(0,4);
    indicesComando = StringRecibido.substring(4);//revisar la idea es que saque el numero de datos
    Serial.println(indicesComando);
    
    // En el Caso que llege un comando para modificar unos de los Servo Motores disponibles ("SER" de Servo Motor)
    if(ComandoToEjecutar == "#SER"){
      indiceIntArray = 0;//Otorga el largo menos uno de angulos Compuertas Array en esta iteracion
      String stringTemporal;
      const char charArray[indicesComando.length() + 1];// = indicesComando;//32*3 (n compuertas y 3 largo del caracteres maximos
      strcpy(charArray, indicesComando.c_str());
      for(int i=0; i < strlen(charArray);i++){
        char c = charArray[i];
        stringTemporal = String(stringTemporal + c);
        if(c == ';' || c == '\n' || c == '*'){
          angulosCompuertas[indiceIntArray] = stringTemporal.toInt();
          indiceIntArray++;
          stringTemporal = "";
        }
      }
      // Poner en orden los servo motores que se quieran mover, reemplazar el valor entre corchetes
      servo1.write(map(angulosCompuertas[0],0,90,0,90));//posicion 4 y 5 indican los valores donde se obtuvieron 0 y 90 grados en realidad, se usa para calibrar
      servo2.write(map(angulosCompuertas[1],0,90,0,110));
      servo3.write(map(angulosCompuertas[2],0,90,0,105));
      servo4.write(map(angulosCompuertas[3],0,90,0,105));
      modomanual = false;
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
    if(modomanual == false){
      modomanual = true;
    }
    // Concatenar la medicion de los sensores, debe ser print no println
    //Serial.println("#SEN" +String(lecturaTEMP)+ ";" + String(lecturaLuz));
    
  } 
}
