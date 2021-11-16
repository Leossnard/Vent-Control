
#include <Adafruit_ADS1X15.h>
#include "max6675.h"
#include <DHT.h>;
#include <Wire.h>

#define HUM_1 2
#define HUM_2 4
#define alpha 0.05//Se define a alpha como 0.05 AGREGUE ESTO
const float multiplicador=0.125F; //Multiplicador que depende de la ganancia Agregue esto
int RL_MQ135=20; //Resistencia de la carga en 20 KΩ PARA MQ135 AGREGUE ESTO
int RL_MQ7=10;//Resistencia de la carga en 10 KΩ PARA MQ7 AGREGUE ESTO
const float V0=5.0F;  //AGREGUE ESTO
int16_t CO2_1Filtrado,CO2_2Filtrado,CO_1Filtrado,CO_2Filtrado=0;
//Calibracion
long previousMillis_Calibracion=0;
long interval_Calibracion=60000*1; //Define intervalo de tiempo durante el cual calibrará
long Rs_sumado_CO2_1,Rs_sumado_CO2_2,Rs_sumado_CO_1,Rs_sumado_CO_2;
long total_datos_CO2_1,total_datos_CO2_2,total_datos_CO_1,total_datos_CO_2; 
float Rs_media_CO2_1,Rs_media_CO2_2,Rs_media_CO_1,Rs_media_CO_2;
float R0_CO2_1,R0_CO2_2,R0_CO_1,R0_CO_2;

bool calibrar=true;

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
int16_t CO2_1, CO2_2, CO_1, CO_2;

DHT dht[]= {{HUM_1, DHT22},{HUM_2, DHT22}};


float humedad[2];

Adafruit_ADS1115 ads;
const float multiplier = 0.1875F;

//Servo servoMotor;//deja algunos pines sin funcionar buscar segun modelo de placa arduino



void setup() {
  
  Serial.begin(9600);   // Configura la velocidad del puerto USB (9600 es un estandar)
  //servoMotor.attach(SERVO1PIN);//setea el pin de salida para el servomotor
  ads.setGain(GAIN_ONE);
  //pinMode(x, OUTPUT);  // Configura el pin 3 como Salida Digital
  ads.begin();
  for (auto& sensor : dht) {
   sensor.begin();
  }
  CalibracionSensores_MQ7_MQ135();
}

void CalibracionSensores_MQ7_MQ135(){
  Serial.println("Calibrando sensores de gases MQ-7 y MQ-135");
  while (calibrar==true){ //Mientras calibrar sea equivalente a true
    unsigned long currentMillis=millis(); //millis() obtiene tiempo en milisegundos 
    
    CO_2=ads.readADC_SingleEnded(0); //Lee la posición A4 del sensor ADS1115
    CO_2Filtrado= (alpha*CO_2)+((1-alpha)*CO_2Filtrado);//Filtro EMA a la lectura 
    float Vi_CO_2=(CO_2Filtrado*multiplicador)/1000; //Convierte a voltaje la lectura analógica
    float Rs_CO_2=RL_MQ7*((V0/Vi_CO_2)-1); //Se obtiene la resistencia del sensor
    Rs_sumado_CO_2+=Rs_CO_2; //Se suma la resistencia obtenida anteriormente a una variable
    total_datos_CO_2+=1; //Se suma un dato al total, es un contador.

    CO_1=ads.readADC_SingleEnded(3); //Lee la posición A4 del sensor ADS1115
    CO_1Filtrado= (alpha*CO_1)+((1-alpha)*CO_1Filtrado);//Filtro EMA a la lectura 
    float Vi_CO_1=(CO_1Filtrado*multiplicador)/1000; //Convierte a voltaje la lectura analógica
    float Rs_CO_1=RL_MQ7*((V0/Vi_CO_1)-1); //Se obtiene la resistencia del sensor
    Rs_sumado_CO_1+=Rs_CO_1; //Se suma la resistencia obtenida anteriormente a una variable
    total_datos_CO_1+=1; //Se suma un dato al total, es un contador.

    CO2_1=ads.readADC_SingleEnded(2); //Lee la posición A4 del sensor ADS1115
    CO2_1Filtrado= (alpha*CO2_1)+((1-alpha)*CO2_1Filtrado);//Filtro EMA a la lectura 
    float Vi_CO2_1=(CO2_1Filtrado*multiplicador)/1000; //Convierte a voltaje la lectura analógica
    float Rs_CO2_1=RL_MQ135*((V0/Vi_CO2_1)-1); //Se obtiene la resistencia del sensor
    Rs_sumado_CO2_1+=Rs_CO2_1; //Se suma la resistencia obtenida anteriormente a una variable
    total_datos_CO2_1+=1; //Se suma un dato al total, es un contador.

    CO2_2=ads.readADC_SingleEnded(1); //Lee la posición A4 del sensor ADS1115
    CO2_2Filtrado= (alpha*CO2_2)+((1-alpha)*CO2_2Filtrado);//Filtro EMA a la lectura 
    float Vi_CO2_2=(CO2_2Filtrado*multiplicador)/1000; //Convierte a voltaje la lectura analógica
    float Rs_CO2_2=RL_MQ135*((V0/Vi_CO2_2)-1); //Se obtiene la resistencia del sensor
    Rs_sumado_CO2_2+=Rs_CO2_2; //Se suma la resistencia obtenida anteriormente a una variable
    total_datos_CO2_2+=1; //Se suma un dato al total, es un contador.
    
    if(currentMillis-previousMillis_Calibracion>interval_Calibracion){ //Si la resta de tiempos supera al intervalo 
      Rs_media_CO_2=Rs_sumado_CO_2/total_datos_CO_2; //Se calcula el promedio de la resistencia del sensor
      Rs_media_CO_1=Rs_sumado_CO_1/total_datos_CO_1;  
      Rs_media_CO2_1=Rs_sumado_CO2_1/total_datos_CO2_1; //Se calcula el promedio de la resistencia del sensor
      Rs_media_CO2_2=Rs_sumado_CO2_2/total_datos_CO2_2;  
      calibrar=false; //Se cambia la variable calibrar a falso y deja de ingresar al bucle 
    }
  }
  R0_CO_2=Rs_media_CO_2/(20.669*pow(0.56,-0.656)); //Se despeja R0 MQ7
  R0_CO_1=Rs_media_CO_1/(20.669*pow(0.56,-0.656)); //Se despeja R0
  R0_CO2_2=Rs_media_CO2_2/(5.2735*pow(411.85,-0.35)); //Se despeja R0
  R0_CO2_1=Rs_media_CO2_1/(5.2735*pow(411.85,-0.35)); //Se despeja R0
  Serial.print("Calibrado…");
}

void loop() {
  // Leer la informacion que proviene desde el USB
  while(Serial.available() > 0){
    inChar = Serial.read();
    if(inChar == '#' || empiezaString == true){// si el caracter recibido es #, significa que viene en camino un comando
      empiezaString = true;
      StringRecibido = String(StringRecibido + inChar);
    }
    if(inChar == '\n'){
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
        if(c == ';' || c == '\n'){
          angulosCompuertas[indiceIntArray] = stringTemporal.toInt();
          indiceIntArray++;
          stringTemporal = "";
        }
      }
      // Poner en orden los servo motores que se quieran mover, reemplazar el valor entre corchetes
      //servoMotor.write(map(angulosCompuertas[0],0,90,0,100));//posicion 4 y 5 indican los valores donde se obtuvieron 0 y 90 grados en realidad, se usa para calibrar
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
    //int lecturaTEMP = analogRead(TEMP1PIN);
    //int lecturaLuz = analogRead(LUZ1PIN);
    humedad[0] = dht[0].readHumidity();
    humedad[1] = dht[1].readHumidity();

    // Sensor MQ-135 dióxido de Carbono Galería 1
    CO2_1 = ads.readADC_SingleEnded(2);
    CO2_1Filtrado=(alpha*CO2_1)+((1-alpha)*CO2_1Filtrado); //Agregue esto
    float Vi_1=(CO2_1Filtrado*multiplicador)/1000; //Agregue esto
    float Rs_1=RL_MQ135*((V0/Vi_1)-1); //Se obtiene la resistencia del sensor
    float ppmCo2_1 = pow(((Rs_1/R0_CO2_1)/(5.2735)),(1/-0.35));

    // Sensor MQ-135 dióxido de Carbono Galería 2
    CO2_2= ads.readADC_SingleEnded(1);
    CO2_2Filtrado=(alpha*CO2_2)+((1-alpha)*CO2_2Filtrado); //Agregue esto
    float Vi_2=(CO2_2Filtrado*multiplicador)/1000; //Agregue esto
    float Rs_2=RL_MQ135*((V0/Vi_2)-1); //Se obtiene la resistencia del sensor
    float ppmCo2_2 = pow(((Rs_2/R0_CO2_2 )/(5.2735)),(1/-0.35));

    // Sensor MQ-7 Monóxido de Carbono Galería 1
    CO_1 = ads.readADC_SingleEnded(3);
    CO_1Filtrado=(alpha*CO_1)+((1-alpha)*CO_1Filtrado); //Agregue esto
    float Vi_CO_1=(CO_1Filtrado*multiplicador)/1000; //Agregue esto
    float Rs_CO_1=RL_MQ7*((V0/Vi_CO_1)-1); //Se obtiene la resistencia del sensor
    float ppmCo_1 = pow(((Rs_CO_1/R0_CO_1)/(20.669)),(1/-0.656));

    // Sensor MQ-7 Monóxido de Carbono Galería 2
    CO_2 = ads.readADC_SingleEnded(0);
    CO_2Filtrado=(alpha*CO_2)+((1-alpha)*CO_2Filtrado); //Agregue esto
    float Vi_CO_2=(CO_2Filtrado*multiplicador)/1000; //Agregue esto
    float Rs_CO_2=RL_MQ7*((V0/Vi_CO_2)-1); //Se obtiene la resistencia del sensor
    float ppmCo_2 = pow(((Rs_CO_2/R0_CO_2)/(20.669)),(1/-0.656));
    
    // Concatenar la medicion de los sensores, debe ser print no println
    Serial.println("#SEN" +String(humedad[0])+ ";" + String(humedad[1])+ ";" + String(ppmCo2_1)+ ";" + String(ppmCo2_2)+ ";" + String(ppmCo_1)+ ";" + String(ppmCo_2));
    
  }
 

 
  
}
