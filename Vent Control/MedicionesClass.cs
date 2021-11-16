using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vent_Control
{
	public static class MedicionesClass
	{
		// Caudal, Concentracion de gases, humedad, temperatura u otro
		public static Dictionary<string, List<string>> medicionesDiccionario = new Dictionary<string, List<string>>();

		public static void agregarMediciones(string nombreMedicionKey, string magnitudFisica , string unidadMedida, string galeriaContenedora, string componente1, string componente2, string restriccionMinima, string restriccionMaxima,
			string puertoCOM, string posicionCOM1, string posicionCOM2, string formulaMatematica,string valorInicial)
		{
			// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2,funcionMatematica,valorInicial, vacioPorBotonEditar, incremento/decremento Acumulador, optimizacionSalida, tiempoIntervaloOPtimizacion
			//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10      ,        11       ,     12     ,         13         ,                 14              ,          15       ,             16
			medicionesDiccionario.Add(nombreMedicionKey, new List<string> { nombreMedicionKey, magnitudFisica, unidadMedida, galeriaContenedora, componente1, componente2, restriccionMinima, restriccionMaxima, puertoCOM,
				posicionCOM1, posicionCOM2, formulaMatematica, valorInicial, "", "","",""});
		}
		

	}
}
