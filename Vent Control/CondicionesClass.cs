using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vent_Control
{
	public static class CondicionesClass
	{
		//Caudal, Concentracion de gases, humedad, temperatura u otro
		//Llave  : noombreMedicion; Orden: nombreMedicion, Ventilador/Compuerta , nombreGaleria, condicion, tiempoEspera, Incremento, UnidadMedida, puertoCOM, Aumentar/Disminuir
		//indices:                       :       0       ,          1           ,      2       ,     3    ,      4      ,     5     ,      6      ,     7    ,         8 
		public static List<List<string>> CondicionesListaAumentar = new List<List<string>>();
		public static List<List<string>> CondicionesListaDisminuir = new List<List<string>>();
		//public static Dictionary<string, List<string>> CondicionesDiccionarioAumentar = new Dictionary<string, List<string>>();
		//public static Dictionary<string, List<string>> CondicionesDiccionarioDisminuir = new Dictionary<string, List<string>>();

		//diccionario que almacena solo los componentes de salida
		public static Dictionary<string, List<string>> DiccionarioComponentesSalidas = new Dictionary<string, List<string>>();

	}
}
