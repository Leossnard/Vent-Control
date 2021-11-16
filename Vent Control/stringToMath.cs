using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vent_Control
{
	public class StringToMath
	{
		bool retornar;
		public string verificarSintaxis(string formulaString)
		{
			formulaString = formulaString.ToUpper();
			// Verificar que no existan espacios
			if (formulaString.Contains(" "))
			{
				return "Error: No pueden haber espacios.";
			}
			// Verificar que solo existan las funciones disponibes ademas de X e Y
			StringBuilder stringTemp = new StringBuilder(formulaString);
			stringTemp.Replace("A", "1");
			stringTemp.Replace("X", "1");
			stringTemp.Replace("Y", "1");
			stringTemp.Replace("(", "");
			stringTemp.Replace(")", "");

			stringTemp.Replace("^", "");
			stringTemp.Replace("+", "");
			stringTemp.Replace("-", "");
			stringTemp.Replace("*", "");
			stringTemp.Replace("/", "");
			stringTemp.Replace("SEN", "");
			stringTemp.Replace("COS", "");
			stringTemp.Replace("TAN", "");
			stringTemp.Replace("SQRT", "");

			double numeroTemp;
			bool esNumero = double.TryParse(stringTemp.ToString(), out numeroTemp);
			if (esNumero == false)
			{
				return "Error: Funcion no identificada, asegurese de elegir una funcion Compatible o un Numero valido.";
			}


			StringBuilder stringFormulaOK = new StringBuilder(formulaString);
			//contar los parentesis
			List<int> parentesisIzquierda = new List<int>();
			List<int> parentesisDerecha = new List<int>();
			List<int> sumaResta = new List<int>();
			for (int i = 0; i < stringFormulaOK.Length; i++)
			{
				if (stringFormulaOK[i] == '(')
				{
					parentesisIzquierda.Add(i);
					if (i >= 1)
					{
						if (stringFormulaOK[i-1] != '*' && stringFormulaOK[i-1] != '/' && stringFormulaOK[i-1] != '+' && stringFormulaOK[i-1] != '-')
						{
							if (i >= 4)
							{
								if (stringFormulaOK.ToString().Substring(i - 3, 3) != "SEN" && stringFormulaOK.ToString().Substring(i - 3, 3) != "COS" && stringFormulaOK.ToString().Substring(i - 3, 3) != "TAN" && stringFormulaOK.ToString().Substring(i - 4, 4) != "SQRT")
								{
									return "Error: la Funcion u operacion antes del parentesis no se reconoce (1)";
								}
							}
							else if (i >= 3)
							{
								if (stringFormulaOK.ToString().Substring(i - 3, 3) != "SEN" && stringFormulaOK.ToString().Substring(i - 3, 3) != "COS" && stringFormulaOK.ToString().Substring(i - 3, 3) != "TAN")
								{
									return "Error: la Funcion u operacion antes del parentesis no se reconoce (2)";
								}
							}
							else
							{
								return "Error: la Funcion u operacion antes del parentesis no se reconoce (3)";

							}
						}
					}

				}
				else if (stringFormulaOK[i] == ')')
				{
					parentesisDerecha.Add(i);
				}
				else if(stringFormulaOK[i] == '+' || stringFormulaOK[i] == '-')
				{
					if(i != 0)
					{
						sumaResta.Add(i);
					}
					
					
				}
			}

			// Verificar numero parentesis
			if(parentesisIzquierda.Count != parentesisDerecha.Count)
			{
				return "Error: No Coincide el numero de parentesis abiertos con los cerrados.";
			}


			return "OK";
		}

		//realizar el calculo, considerando que la funcion ya esta validada
		public string stringToResultadoMath(string stringFormula, string equis, int numerodecimales = 4, string area ="1", string i_griega="1")
		{
			//si no hay nada mas que un numero devolver el mismo numero
			if (stringFormula.ToUpper() == "X")
			{

				string retornoRapido = Math.Round(double.Parse(equis.ToString().Replace('.', ',')), numerodecimales).ToString();
				return retornoRapido;
			}


			retornar = false;
			if (area == "")
			{
				area = "1";
			}
			if(i_griega == "")
			{
				i_griega = "1";
			}
			StringBuilder formulaAEvaluar = new StringBuilder(stringFormula.ToUpper().Replace('.', ','));
			formulaAEvaluar.Replace("X", equis).Replace("A", area).Replace("Y", i_griega);



			//identificar Parentesis e iterar mientras existan
			List<int> paresParentesisIterar = identificarParentesisIzquierdoConDerecho(formulaAEvaluar);
			int largoEvaluacion;
			char[] CharTemp;
			StringBuilder reducirParentesis;
			while (contarNumeroApariciones(formulaAEvaluar, '^', '^', 0) != -1 || contarNumeroApariciones(formulaAEvaluar, '*', '/', 0) != -1 || contarNumeroApariciones(formulaAEvaluar, '+','-', 0) != -1 )
			{

				if (paresParentesisIterar.Count > 0)
				{
					largoEvaluacion = paresParentesisIterar[1] - paresParentesisIterar[0] - 1;
					CharTemp = new char[largoEvaluacion];
					formulaAEvaluar.CopyTo(paresParentesisIterar[0] + 1, CharTemp, 0, largoEvaluacion);
					reducirParentesis = new StringBuilder(new string(CharTemp));
				}
				else
				{
					reducirParentesis = formulaAEvaluar;
				}


				int ubicacionOperador;
				//potencia
				ubicacionOperador = contarNumeroApariciones(reducirParentesis, '^', '^', 0);
				while (ubicacionOperador != -1)
				{

					InsertarResultado(ubicacionOperador, reducirParentesis[ubicacionOperador], reducirParentesis);
					ubicacionOperador = contarNumeroApariciones(reducirParentesis, '^', '^', ubicacionOperador);
				}
				//multiplicacion y division

				ubicacionOperador = contarNumeroApariciones(reducirParentesis, '*', '/', 0);
				while (ubicacionOperador != -1)
				{

					InsertarResultado(ubicacionOperador, reducirParentesis[ubicacionOperador], reducirParentesis);
					ubicacionOperador = contarNumeroApariciones(reducirParentesis, '*', '/', ubicacionOperador);//aca esta fallando aveces llega un * a la linea 180
				}
				//suma y resta
				ubicacionOperador = contarNumeroApariciones(reducirParentesis, '+', '-', 0);
				while (ubicacionOperador != -1)
				{

					InsertarResultado(ubicacionOperador, reducirParentesis[ubicacionOperador], reducirParentesis);
					ubicacionOperador = contarNumeroApariciones(reducirParentesis, '+', '-', ubicacionOperador);
				}

				//las operaciones dentro del parentesis estan ok, ahora aplicar la funcion correspondiente si es que existe
				string toti = reducirParentesis.ToString();
				double operacionResultado = double.Parse(reducirParentesis.ToString().Replace('.',','));
				//evaluar la funcion matematica antes del parentesis izquierdo; +, -, *, /, SEN, COS, SQRT, TAN, ^, siempre y cuando dentro del parentesis no queden operaciones
				int retroceso = 0;
				if (paresParentesisIterar.Count > 0)
				{
					if (paresParentesisIterar[0] - 4 >= 0)
					{
						if (formulaAEvaluar.ToString().Substring(paresParentesisIterar[0] - 4, 4) == "SQRT")
						{
							operacionResultado = Math.Sqrt(Math.Abs(operacionResultado));
							retroceso = 4;
						}
					}
					else if (paresParentesisIterar[0] - 3 >= 0)
					{
						if (formulaAEvaluar.ToString().Substring(paresParentesisIterar[0] - 3, 3) == "SEN")
						{
							operacionResultado = Math.Sin(operacionResultado);
							retroceso = 3;
						}
						else if (formulaAEvaluar.ToString().Substring(paresParentesisIterar[0] - 3, 3) == "COS")
						{
							operacionResultado = Math.Cos(operacionResultado);
							retroceso = 3;

						}
						else if (formulaAEvaluar.ToString().Substring(paresParentesisIterar[0] - 3, 3) == "TAN")
						{
							operacionResultado = Math.Tan(operacionResultado);
							retroceso = 3;
						}
					}
					//borrar lo anterior
					formulaAEvaluar.Remove(paresParentesisIterar[0] - retroceso, paresParentesisIterar[1] - paresParentesisIterar[0] + retroceso + 1);
					//Insertar Resultado
					formulaAEvaluar.Insert(paresParentesisIterar[0] - retroceso, operacionResultado);
				}

				//Actualizar reducirParentesis 
				paresParentesisIterar = identificarParentesisIzquierdoConDerecho(formulaAEvaluar);

			}

			//funcion que transforma notacion cinetifica en normal
			StringBuilder QuitarNotacionExp(StringBuilder formulaAEvaluarString)
			{
				string temp = formulaAEvaluar.ToString();
				while (temp.Contains("E"))
				{
					for (int i = 0; i < formulaAEvaluarString.Length; i++)
					{
						if (formulaAEvaluarString[i] == 'E')
						{
							int cantidadCeros = 0;
							for (int j = i + 2; j < formulaAEvaluarString.Length; j++)
							{
								//para buscar donde termina el exponente
								if (formulaAEvaluarString[j] == '+' || formulaAEvaluarString[j] == '-' ||
									formulaAEvaluarString[j] == '*' || formulaAEvaluarString[j] == '/'|| j == formulaAEvaluarString.Length -1)
								{
									bool positivo = true;
									int empezar = i + 1;
									int largo = j - i - 1;
									if (formulaAEvaluarString[i + 1] == '-')
									{
										empezar = i + 2;
										largo = j - i - 2;
										positivo = false;
									}
									cantidadCeros = int.Parse(formulaAEvaluarString.ToString(empezar, largo));

									string tempCeros = "";
									for (int k = 0; k < cantidadCeros; k++)
									{
										tempCeros = tempCeros + "0";
									}

									if (positivo)
									{
										temp = formulaAEvaluarString.ToString().Replace("E", "1" + tempCeros);
									}
									else
									{
										temp = formulaAEvaluarString.ToString().Replace("E-", "1/1" + tempCeros);
									}

								}
							}

						}
					}

				}
				return new StringBuilder(temp);

			}


			//funcion que otorga solo los parentesis que estan indexados o solos para que se ejecuten primero
			List<int> identificarParentesisIzquierdoConDerecho(StringBuilder formula)
			{
				List<int> paresParentesis = new List<int>();
				int contadorParentesisDerecho = 0;
				int contadorParentesisIzquierdo = 1;
				for (int i = 0; i < formula.Length; i++)
				{
					if (formula[i] == '(')
					{
						for (int j = i+1; j < formula.Length; j++)
						{
							if(formula[j] == '(')
							{
								contadorParentesisIzquierdo++;
							}
							else if(formula[j] == ')')
							{
								contadorParentesisDerecho++;
								if(contadorParentesisDerecho == contadorParentesisIzquierdo)
								{
									if(contadorParentesisDerecho == 1)
									{
										paresParentesis.Add(i);
										paresParentesis.Add(j);
										return paresParentesis;
									}
									contadorParentesisDerecho = 0;
									contadorParentesisIzquierdo = 1;
									break;
								}
								
							}
						}
					}
				}
				return paresParentesis;
			}
			
			// funcion que cuenta el numero de apariciones de un caracter en un string
			int contarNumeroApariciones(StringBuilder strignEvaluado, char caracter, char caracter2, int inicio)
			{

				if (strignEvaluado.ToString().Contains(caracter) || strignEvaluado.ToString().Contains(caracter2))
				{
					if(inicio >= strignEvaluado.Length-1)
					{
						inicio = 0;
					}
					
				}
				//if (strignEvaluado.ToString().Contains("E-"))
				//{
				//	return -1;
				//}
				if (retornar == true)
				{
					return -1;
				}
				



				int ubicacion = -1;
				for (int i = inicio; i < strignEvaluado.Length; i++)
				{
					if(strignEvaluado[i] == caracter || strignEvaluado[i] == caracter2)
					{
						if(i == 0)
						{

							if (strignEvaluado[i] == '-')
							{
								//esto pasa cuando se restan dos numeros negativos
								if (strignEvaluado.ToString().Contains("--"))
								{
									continue;
								} 
								return -1;
							}
						}
						ubicacion=i;
						return ubicacion;
					}

				}

				return ubicacion;
			}

			// funcion que otorga la posicion de inicial y final de los operandos de una funcion (indide operador, caracter del operador)
			void InsertarResultado(int operador,char operacionCaracter, StringBuilder formulaenString)
			{
				int inicioConE =0;
				//reemplazar los E cuando el numero esta expresado en notacion cientifica
				if (formulaenString.ToString().Contains("E"))
				{
					//formulaenString = QuitarNotacionExp(formulaenString);
					inicioConE = operador - 1;
				}

				int finNumero = 0;
				int inicioNumero = 0;
				int i = operador;
				bool negativoConNegativo1 = false;
				bool pasardelargo = false;

				for (int j = i-1; j >= 0; j--)//verificar que funcione bien
				{
					if (formulaenString[j] == '+' || formulaenString[j] == '-' || formulaenString[j] == '*' || formulaenString[j] == '/' || formulaenString[j] == '^')
					{
						if(j > 0)
						{
							if (formulaenString[j - 1] == 'E')
							{
								continue;
							}
						}
						inicioNumero = j+1;

						if(formulaenString[j] == '-')
						{
							negativoConNegativo1 = true;
							inicioNumero = j;
						}

						
						break;
					}
				}
				for (int k = i+2; k < formulaenString.Length; k++)//se salta 2 en el caso de que fuera negativo es segundo operando
				{
					if (formulaenString[k] == '+' || formulaenString[k] == '-' || formulaenString[k] == '*' || formulaenString[k] == '/' || formulaenString[k] == '^')
					{
						finNumero = k - 1;
						break;//primera aparicion
					}
				}
				if (finNumero == 0)//Que pasa cuando este valor es cero
				{
					finNumero = formulaenString.Length - 1;
				}

				string numeroExpsinOperadores = formulaenString.ToString(inicioConE, formulaenString.Length - inicioConE);
				if (numeroExpsinOperadores.Contains("E"))
				{
					operador = 0;
				}

				//validacion para condiciones de borde en caso de que sea un numero negativo o en notacion cientifica
				if (operador == 0)//es un numero negativo solamente
				{
					pasardelargo = true;
				}

				double op1;
				double op2;
				double operacionResultado = 0;
				if (pasardelargo == false)
				{
					op1 = double.Parse(formulaenString.ToString().Substring(inicioNumero, operador - inicioNumero).Replace('.', ','));
					op2 = double.Parse(formulaenString.ToString().Substring(operador + 1, finNumero - operador).Replace('.', ','));
					if (operacionCaracter == '^')
					{
						operacionResultado = Math.Pow(op1, op2);
					}
					else if (operacionCaracter == '*')
					{
						operacionResultado = op1 * op2;
					}
					else if (operacionCaracter == '/')
					{
						operacionResultado = op1 / op2;
					}
					else if (operacionCaracter == '+')
					{
						operacionResultado = op1 + op2;
					}
					else if (operacionCaracter == '-')
					{
						operacionResultado = op1 - op2;
					}
				}
				else
				{
					operacionResultado = double.Parse(formulaenString.ToString().Replace('.', ','));
					retornar = true;
				}


				


				//remover la operacion actual ya realizada del string
				formulaenString.Remove(inicioNumero, finNumero - inicioNumero + 1);
				//agregar el resultado
				if (operacionResultado > 0 && negativoConNegativo1 == true)
				{
					formulaenString.Insert(inicioNumero, "+" + operacionResultado.ToString());
				}
				else
				{
					string ver = operacionResultado.ToString();
					formulaenString.Insert(inicioNumero, operacionResultado.ToString());
				}
				
			}
			//lo que devuelve la funcion principal es decir el resultado final
			string retorno = Math.Round(double.Parse(formulaAEvaluar.ToString()), numerodecimales).ToString();
			return retorno;

		}

	}
}
