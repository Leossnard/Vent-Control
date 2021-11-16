using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vent_Control
{
	//Clase que es la encargada de manejar la comunicacion con los arduinos o Dispositivos que se conecten Via Puertos COM
	public static class ArduinoConexion
	{
		// Diccionario con las configuraciones
		// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion1, valor1, dir2, val2
		//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6     ,   7   ,  8  ,   9
		public static Dictionary<string, List<string>> diccionarioConfigsCOM = new Dictionary<string, List<string>>();
		
		// Campos/Propiedades

		public static Dictionary<string, SerialPort> PuertosDisponibles = new Dictionary<string, SerialPort>();
		public static List<string> datosEntrantes = new List<string>();
		private static string puertoActivo = "";

		// Metodos
		public static void obtenerPuertoCOM()
		{
			string[] puertosActuales = SerialPort.GetPortNames();
			foreach (string puertoNombre in puertosActuales)
			{
				SerialPort serialPortCOM = new SerialPort(puertoNombre, 9600, Parity.None, 8, StopBits.One);
				if (PuertosDisponibles.ContainsKey(puertoNombre + "-Enviar") == false)
				{
					PuertosDisponibles.Add(puertoNombre + "-Enviar", serialPortCOM);
				}
				if(PuertosDisponibles.ContainsKey(puertoNombre + "-Recibir") == false)
				{
					PuertosDisponibles.Add(puertoNombre + "-Recibir", serialPortCOM);
				}
			}
			////solo para prueba despues borrar estas lineas de abajo
			//if (PuertosDisponibles.ContainsKey("COM VENT" + "-Enviar") == false)
			//{
			//	PuertosDisponibles.Add("COM VENT" + "-Enviar", new SerialPort("COM VENT" + "-Enviar", 9600, Parity.None, 8, StopBits.One));
			//}
			//if (PuertosDisponibles.ContainsKey("COM VENT" + "-Recibir") == false)
			//{
			//	PuertosDisponibles.Add("COM VENT" + "-Recibir", new SerialPort("COM VENT" + "-Recibir", 9600, Parity.None, 8, StopBits.One));
			//}
		}


		public static async Task<string> puertoActivoDataAsync(int espera = 10000)//por defecto 10 segundos de espera
		{
			// Configura el Puerto Activo como string vacio
			puertoActivo = "";
			// Agrega al diccionario cada puerto Activo y se suscribe a su evento DataReceived
			Dictionary<SerialPort, SerialDataReceivedEventHandler> diccionarioHandlerCOM = new Dictionary<SerialPort, SerialDataReceivedEventHandler>();
			foreach (string puertoKey in PuertosDisponibles.Keys)
			{
				if (diccionarioHandlerCOM.ContainsKey(PuertosDisponibles[puertoKey]) == false)
				{
					SerialDataReceivedEventHandler eventHandlerCOM = (sender, e) => puertoActivo = puertoKey.Split('-')[0];
					PuertosDisponibles[puertoKey].Open();
					PuertosDisponibles[puertoKey].DataReceived += eventHandlerCOM;
					diccionarioHandlerCOM.Add(PuertosDisponibles[puertoKey], eventHandlerCOM);
				}
			}
			
			// Durante XX segundos si el valor del string no cambia se retorna el string vacio
			// En cambio si hay un cambio en el string se retorna el nombre del puerto en el cual se primero envio datos

			int numeroIntentos = espera / 10;
			while (puertoActivo == "" && numeroIntentos < espera)
			{
				await Task.Delay(10);
			}

			// desuscribirse de los eventos y cerrar los puertos
			foreach (SerialPort puertoCOM in diccionarioHandlerCOM.Keys)
			{
				puertoCOM.DataReceived -= diccionarioHandlerCOM[puertoCOM];
				puertoCOM.Close();
			}

			return puertoActivo;
		}

		// Funcion que otorga un string con la informacion que proviene del Puerto especificado
		public static async Task<string> ObtenerLecturaUSB(string nombrePuerto)
		{
			string leerUSB()
			{
				try
				{
					return PuertosDisponibles[nombrePuerto + "-Recibir"].ReadLine();
				}
				catch
				{
					return "Arduino Desconectado!\n";
				}
			}
			return await Task.Run(() => leerUSB());
		}

		//Funcion que envia un string a un determinado puerto (recordar agregar -Enviar)
		public static void EnviarComando(string comando, string puerto)
		{
			PuertosDisponibles[puerto].Write(comando+"\n");
		}
	}
}
