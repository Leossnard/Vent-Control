using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vent_Control
{
	public partial class Form_Configurar_Componente : Form
	{
		string galeriaSeleccionada = "";
		private Configuraciones FormConfiguraciones;
		private string nombreMedicionKeyGlobal = "";
		private string posicion1original = "";
		private string posicion2original = "";


		public Form_Configurar_Componente(string GaleriaSeleccionada, Configuraciones formConfiguraciones, string nombreMedicionKey = "")
		{
			InitializeComponent();

			nombreMedicionKeyGlobal = nombreMedicionKey;

			FormConfiguraciones = formConfiguraciones;

			galeriaSeleccionada = GaleriaSeleccionada;

			textBoxGalContenedora.Enabled = false;
			textBoxGalContenedora.Text = GaleriaSeleccionada;

			comboBoxSensor2.Enabled = false;
			textBoxPosicionCOMS2.Enabled = false;
			textBoxValorInicial.Enabled = false;

			comboBoxSensor1.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxSensor2.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxPuertoCOM.DropDownStyle = ComboBoxStyle.DropDownList;


			comboBoxMagnitudes.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxMagnitudes.SelectedValueChanged += ComboBoxMagnitudes_SelectedValueChanged;
			comboBoxMagnitudes.Items.Add("");
			comboBoxMagnitudes.Items.Add("Caudal");
			comboBoxMagnitudes.Items.Add("Concentracion de Gases");
			comboBoxMagnitudes.Items.Add("Humedad");
			comboBoxMagnitudes.Items.Add("Temperatura");
			comboBoxMagnitudes.Items.Add("Ventilador");
			comboBoxMagnitudes.Items.Add("Consumo Ventilador");//si es consumo ventilador solo se puede agregar a un ventilador (falta hacer esa validacion)
			comboBoxMagnitudes.Items.Add("Compuerta");
			comboBoxMagnitudes.Items.Add("Presion");
			comboBoxMagnitudes.Items.Add("Otro");

			// Si la variable de entrada es distinta de vacio cargar info del diccionario al form para modificar
			if (nombreMedicionKey != "")
			{
				// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2, formulaMath, valor inicial
				//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,     10     ,     11     ,      12
				textBoxNombreMedicion.Text = nombreMedicionKey;
				comboBoxMagnitudes.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][1];
				textBoxUnidadMedida.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][2];
				// Galeria ya esta actualizada por defecto
				comboBoxSensor1.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][4];
				comboBoxSensor2.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][5];
				textBoxValorMinimo.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][6];
				textBoxValorMaximo.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][7];
				textBoxPosicionCOMS1.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][9];
				textBoxPosicionCOMS2.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][10];
				comboBoxMagnitudes.Enabled = false;
				textBoxNombreMedicion.Enabled = false;
				buttonAgregarMedicion.Text = "Modificar Medicion";
				posicion1original = MedicionesClass.medicionesDiccionario[nombreMedicionKey][9];
				posicion2original = MedicionesClass.medicionesDiccionario[nombreMedicionKey][10];
				textBoxTransformacion.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][11];
				textBoxValorInicial.Text = MedicionesClass.medicionesDiccionario[nombreMedicionKey][12];
			}

		}
		// bloquear o habilitar textboxes segun eleccion
		private void ComboBoxMagnitudes_SelectedValueChanged(object sender, EventArgs e)
		{
			// en el caso de que sea caudal deja seleccionar dos componentes (Pt y Pe)
			if (comboBoxMagnitudes.Text == "Caudal")
			{
				comboBoxSensor2.Enabled = true;
				textBoxPosicionCOMS2.Enabled = true;

			}
			else
			{
				comboBoxSensor2.Enabled = false;
				textBoxPosicionCOMS2.Enabled = false;
			}
			if(comboBoxMagnitudes.Text == "Ventilador" || comboBoxMagnitudes.Text == "Consumo Ventilador")
			{
				textBoxPosicionCOMS1.Enabled = false;
				textBoxTransformacion.Enabled = false;
			}
			else
			{
				textBoxTransformacion.Enabled = true;
			}
			if(comboBoxMagnitudes.Text == "Compuerta")
			{
				textBoxTransformacion.Enabled = false;
			}
			else
			{
				textBoxTransformacion.Enabled = true;
			}
			agregarSensoresComboboxes();
		}


		private void agregarSensoresComboboxes()
		{
			comboBoxSensor1.Items.Clear();
			comboBoxSensor2.Items.Clear();
			// dejar seleccionar los componentes dependiendo de lo que se quiera medir
			List<string> tiposdeComponentes = new List<string>();
			if (comboBoxMagnitudes.Text == "Caudal" || comboBoxMagnitudes.Text == "Presion" )
			{
				tiposdeComponentes.Add("PRES");
			}
			else if (comboBoxMagnitudes.Text == "Concentracion de Gases")
			{
				tiposdeComponentes.Add("GAS");
			}
			else if (comboBoxMagnitudes.Text == "Humedad")
			{
				tiposdeComponentes.Add("HUMID");
			}
			else if (comboBoxMagnitudes.Text == "Temperatura")
			{
				tiposdeComponentes.Add("TEMP");
			}
			else if (comboBoxMagnitudes.Text == "Ventilador" || comboBoxMagnitudes.Text == "Consumo Ventilador")
			{
				tiposdeComponentes.Add("VENT");
			}
			else if (comboBoxMagnitudes.Text == "Compuerta")
			{
				tiposdeComponentes.Add("COMPUERTA");
			}
			else if (comboBoxMagnitudes.Text == "Otro")
			{
				tiposdeComponentes.Add("CUSTOM");//agregar para que se puedan agregar componentes personalizados
			}


			foreach (string componentesPorAgregar in tiposdeComponentes)
			{
				foreach (string componenteGeneral in Configuraciones.DiccionarioGaleriaSensores[galeriaSeleccionada])
				{
					if (componenteGeneral.Contains(componentesPorAgregar))
					{
						comboBoxSensor1.Items.Add(componenteGeneral);
						if(comboBoxSensor2.Enabled == true)
						{
							comboBoxSensor2.Items.Add(componenteGeneral);
						}
					}
				}
			}

            if (comboBoxSensor1.Items.Count == 0)
            {
                MessageBox.Show("No se encontro un componente del tipo a configurar en la galeria seleccionada. Agregue un componente e intentelo de nuevo.");
            }
        }
        //Agrega al combobox los puertos Enviar (caso Ventiladores o Compuertas) o Recibir en los otros casos
        private void comboBoxPuertoCOM_Enter(object sender, EventArgs e)
		{
			comboBoxPuertoCOM.Items.Clear();
			ArduinoConexion.obtenerPuertoCOM();
			bool salirdelLoop = false;
			foreach (string nombrePuertoKey in ArduinoConexion.PuertosDisponibles.Keys)
			{
                if (salirdelLoop)//en el caso de que ya encontro el ventilador configurado
                {
					break;
                }

				//demas codigo
				if(comboBoxMagnitudes.Text == "Ventilador" || comboBoxMagnitudes.Text == "Compuerta")
				{
					if (nombrePuertoKey.Contains("Recibir") == false)
					{
						comboBoxPuertoCOM.Items.Add(nombrePuertoKey);
					}
				}
				else
				{
					if(comboBoxMagnitudes.Text != "Otro")
					{
						if (nombrePuertoKey.Contains("Enviar") == false)
						{
							// En el caso de la potencia solo agregar el puerto donde el ventilador este configurado
							// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2,funcionMatematica,valorInicial, vacioPorBotonEditar, incremento/decremento Acumulador, optimizacionSalida, tiempoIntervaloOPtimizacion
							//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10      ,        11       ,     12     ,         13         ,                 14              ,          15       ,             16

							if (comboBoxMagnitudes.Text == "Consumo Ventilador")
                            {

								foreach (string llaveMediciones in MedicionesClass.medicionesDiccionario.Keys)
								{
									if (MedicionesClass.medicionesDiccionario[llaveMediciones][4].Contains("VENT"))
									{
										comboBoxPuertoCOM.Items.Add(MedicionesClass.medicionesDiccionario[llaveMediciones][8].Split('-')[0] + "-Recibir");
										salirdelLoop = true;
										break;
										
									}
								}
							}
                            else
                            {
								comboBoxPuertoCOM.Items.Add(nombrePuertoKey);
							}

						}
					}
					else
					{
						comboBoxPuertoCOM.Items.Add(nombrePuertoKey);
					}
				}
				
			}
		}
		// en el caso de seleccionar un puerto de salida (enviar)
		private void comboBoxPuertoCOM_SelectedValueChanged(object sender, EventArgs e)
		{
			if (comboBoxPuertoCOM.Text.Contains("Enviar"))
			{
				textBoxValorInicial.Enabled = true;
			}
			else
			{
				textBoxValorInicial.Text = "";
				textBoxValorInicial.Enabled = false;
			}

		}


		private void buttonAgregarMedicion_Click(object sender, EventArgs e)
		{
			//Si el puerto lo esta usando un ventilador no deja registrarlo
			if(comboBoxMagnitudes.Text != "Ventilador")
			{
				foreach (string nombremedicionkey in MedicionesClass.medicionesDiccionario.Keys)
				{
					if (MedicionesClass.medicionesDiccionario[nombremedicionkey][4].Contains("VENT") && comboBoxPuertoCOM.Text == MedicionesClass.medicionesDiccionario[nombremedicionkey][8])
					{
						MessageBox.Show("No se puede agregar un Componente al Puerto Utilizado por un Ventilador");
						return;
					}

				}
			}

			//Compuertas y ventiladores no se pueden agregar dos veces, comprueba que existan
			if ((comboBoxMagnitudes.Text == "Ventilador" || comboBoxMagnitudes.Text == "Compuerta") && nombreMedicionKeyGlobal == "" )
			{
				foreach (string nombremedicionkey in MedicionesClass.medicionesDiccionario.Keys)
				{
					if (MedicionesClass.medicionesDiccionario[nombremedicionkey][4] == comboBoxSensor1.Text)
					{
						MessageBox.Show("No se puede agregar un Ventilador o una Compuerta dos veces!");
						return;
					}
				}
			}

			

			//Verifica que todos los checkbox tengan texto
			double doubleTest;
			List<TextBox> listaTextBoxes = new List<TextBox>() { textBoxValorMinimo, textBoxValorMaximo, textBoxPosicionCOMS1, textBoxNombreMedicion, textBoxUnidadMedida };
			List<ComboBox> comboboxes = new List<ComboBox>() { comboBoxMagnitudes, comboBoxSensor1, comboBoxPuertoCOM };
			if (comboBoxMagnitudes.Text == "Caudal" && comboBoxSensor2.Text != "")
			{
				listaTextBoxes.Add(textBoxPosicionCOMS2);
			}

			if(comboBoxMagnitudes.Text == "Ventilador" || comboBoxMagnitudes.Text == "Consumo Ventilador")
			{
				listaTextBoxes.Remove(textBoxPosicionCOMS1);
			}

			foreach (TextBox TexboxActual in listaTextBoxes)
			{
				if (TexboxActual.Text.Length == 0)
				{
					MessageBox.Show("Rellene todas las Casillas!");
					return;//si encuentra un texbox Vacio
				}
			}
			foreach (ComboBox comboboxActual in comboboxes)
			{
				if(comboboxActual.Text.Length == 0)
				{
					MessageBox.Show("Rellene todas las Casillas!");
					return;//si encuentra un combobox Vacio
				}
			}



			// Verificar que los texbox sean numeros
			List<TextBox> listaTextBoxes2 = new List<TextBox>() { textBoxValorMinimo, textBoxValorMaximo, textBoxPosicionCOMS1 };
			if (comboBoxMagnitudes.Text == "Caudal" && comboBoxSensor2.Text != "")
			{
				listaTextBoxes2.Add(textBoxPosicionCOMS2);
			}
			if (comboBoxMagnitudes.Text == "Ventilador" || comboBoxMagnitudes.Text == "Consumo Ventilador")
			{
				listaTextBoxes2.Remove(textBoxPosicionCOMS1);
			}

			foreach (TextBox TexboxActual in listaTextBoxes2)
			{
				if (double.TryParse(TexboxActual.Text, out doubleTest) == false)
				{
					MessageBox.Show(TexboxActual.Text + " , no es un Numero, por favor ingrese un Numero Valido.");
					return;//si encuentra un texbox con un valor que no sea un numero se sale.
				}
			}

			//verificar que la restriccion mayor sea mayor que la menor
			if(Convert.ToDouble(textBoxValorMaximo.Text) <= Convert.ToDouble(textBoxValorMinimo.Text))
			{
				MessageBox.Show("El Valor Maximo debe ser Mayor al Valor Minimo!");
				return;
			}

			
			// Verificar que la posicioncom 1 y 3 sean diferentes
			if(textBoxPosicionCOMS1.Text == textBoxPosicionCOMS2.Text && comboBoxMagnitudes.Text != "Ventilador" && comboBoxMagnitudes.Text != "Consumo Ventilador")
			{
				MessageBox.Show("Las posiciones utilizadas en el puerto COM deben ser distintas!");
				return;
			}

			// Antes de agregar o modificar una medicion verificar que la posicion del puerto no este en uso
			bool realizarVerificacionCOM = false;
			if(nombreMedicionKeyGlobal != "")//En el caso de querer modificar la medicion
			{
				if (posicion1original != textBoxPosicionCOMS1.Text || posicion2original != textBoxPosicionCOMS2.Text)
				{
					realizarVerificacionCOM = true;
				}
			}
			else//en el caso de agregar
			{
				realizarVerificacionCOM = true;
			}

			// para el ventilador no es necesario realizar esta verificacion, pero si verificar que no este en uso otro puerto
			if(comboBoxMagnitudes.Text == "Ventilador")
			{
				bool verificarPuerto = false;
				if(nombreMedicionKeyGlobal == "")//nuevo Ventilador
				{
					verificarPuerto = true;
				}
				else//Modificar ventilador
				{
					if(MedicionesClass.medicionesDiccionario[nombreMedicionKeyGlobal][8] != comboBoxPuertoCOM.Text)
					{
						verificarPuerto = true;
					}
				}
				if (verificarPuerto)
				{
					foreach (string nombreMedicionKeys in MedicionesClass.medicionesDiccionario.Keys)
					{
						if (comboBoxPuertoCOM.Text == MedicionesClass.medicionesDiccionario[nombreMedicionKeys][8])
						{
							MessageBox.Show("El Puerto: " + comboBoxPuertoCOM.Text + " ya esta siendo utilizado, seleccione otro.");
							return;
						}
					}
				}
				realizarVerificacionCOM = false;//verifica la posicion del Puerto COM, como es ventilador no lo necesita
			}
			//Verificar que la posicion en el puerto seleccionado no esta utilizada
			if (realizarVerificacionCOM)
			{
				foreach (string nombreMedicionKeys in MedicionesClass.medicionesDiccionario.Keys)
				{
					if (comboBoxPuertoCOM.Text == MedicionesClass.medicionesDiccionario[nombreMedicionKeys][8])
					{
						if (textBoxPosicionCOMS1.Text == MedicionesClass.medicionesDiccionario[nombreMedicionKeys][9] || textBoxPosicionCOMS1.Text == MedicionesClass.medicionesDiccionario[nombreMedicionKeys][10])
						{
							if(MedicionesClass.medicionesDiccionario[nombreMedicionKeys][3] == textBoxGalContenedora.Text && MedicionesClass.medicionesDiccionario[nombreMedicionKeys][4] == comboBoxSensor1.Text 
								&& MedicionesClass.medicionesDiccionario[nombreMedicionKeys][5] == comboBoxSensor2.Text)
							{
								break;
							}
							MessageBox.Show("La posicion: " + textBoxPosicionCOMS1.Text + " ya esta siendo utilizada en el Puerto: " + comboBoxPuertoCOM.Text + ". Seleccione otra Posicion.");
							return;
						}

						if (textBoxPosicionCOMS2.Text != "")//verificar pos2
						{
							if (MedicionesClass.medicionesDiccionario[nombreMedicionKeys][3] == textBoxGalContenedora.Text && MedicionesClass.medicionesDiccionario[nombreMedicionKeys][4] == comboBoxSensor1.Text
								&& MedicionesClass.medicionesDiccionario[nombreMedicionKeys][5] == comboBoxSensor2.Text)
							{
								break;
							}
							if (textBoxPosicionCOMS2.Text == MedicionesClass.medicionesDiccionario[nombreMedicionKeys][9] || textBoxPosicionCOMS2.Text == MedicionesClass.medicionesDiccionario[nombreMedicionKeys][10])
							{
								MessageBox.Show("La posicion: " + textBoxPosicionCOMS2.Text + " ya esta siendo utilizada en el Puerto: " + comboBoxPuertoCOM.Text + ". Seleccione otra Posicion.");
								return;
							}
						}
					}
				}
			}



			// si se eligen dos sensores, debe tener formula y Si se eligen dos sensores la posicion debe ser consecutiva
			if (comboBoxSensor1.Text != "" && comboBoxSensor2.Text != "")
			{
				if (textBoxTransformacion.Text.ToUpper().Contains("Y") == false)
				{
					MessageBox.Show("Debe incluir al segundo sensor en la transformacion, introduzcalo con la letra Y");
					return;
				}
				else if (Convert.ToInt32(textBoxPosicionCOMS1.Text) + 1 != Convert.ToInt32(textBoxPosicionCOMS2.Text)) //Verificar que las posiciones sean consecutivas
				{
					MessageBox.Show("Las posiciones de los Sensores en el puerto deben ser consecutivas.");
				}
			}
			// verificar que exista la X
			if(textBoxTransformacion.Text != "")
			{
				if (textBoxTransformacion.Text.ToUpper().Contains("X") == false)
				{
					MessageBox.Show("Debe incluir al sensor en la transformacion, introduzcalo con la letra X");
					return;
				}
				// si se pone el area de la galeria, verificar que esta este configurada
				FormConfiguraciones.actualizarMostarAreaGaleria(textBoxGalContenedora.Text);
				if (textBoxTransformacion.Text.ToUpper().Contains("A") == true && FormConfiguraciones.valortextBoxArea() == "")
				{
					MessageBox.Show("La Galeria Contenedora: " + textBoxGalContenedora.Text + " no tiene configurado el valor del Area, por favor Configurarlo Primero.");
					return;
				}
			}


			// Verificar que la formula ingresada sea correcta
			if(textBoxTransformacion.Text != "")
			{
				StringToMath claseMathLeo = new StringToMath();
				string validacion = claseMathLeo.verificarSintaxis(textBoxTransformacion.Text);
				if (validacion != "OK")
				{
					MessageBox.Show(validacion, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			//En el caso que sea un componente de salida verificar que el valor inicial sea un numero
			if (comboBoxPuertoCOM.Text.Contains("Enviar"))
			{
				double tempDouble;
				if(double.TryParse(textBoxValorInicial.Text, out tempDouble) == false)
				{
					MessageBox.Show("Debe Ingresar el Valor Inicial para los Componentes de Salida");
					return;
				}
			}

			// si se apreta agregar medicion sin agregar una formula pone un "X" por defecto y en el caso que no sea una compuerta.

			if(textBoxTransformacion.Text.Replace(" ","") == "")//borra los espacios
			{
				if(comboBoxMagnitudes.Text.Contains("Compuerta") == false)
				{
					textBoxTransformacion.Text = "X";
				}
				
			}

			// en el caso de actualizar una medicion
			if (nombreMedicionKeyGlobal != "")
			{
				// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2,FormulaMath, valorInicialSalida
				//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10      ,     11    ,         12
				MedicionesClass.medicionesDiccionario[nombreMedicionKeyGlobal] = new List<string>() {
				textBoxNombreMedicion.Text,
				comboBoxMagnitudes.Text,
				textBoxUnidadMedida.Text,
				textBoxGalContenedora.Text,
				comboBoxSensor1.Text,
				comboBoxSensor2.Text,
				textBoxValorMinimo.Text,
				textBoxValorMaximo.Text,
				comboBoxPuertoCOM.Text,
				textBoxPosicionCOMS1.Text,
				textBoxPosicionCOMS2.Text,
				textBoxTransformacion.Text,
				textBoxValorInicial.Text,"",MedicionesClass.medicionesDiccionario[nombreMedicionKeyGlobal][14],MedicionesClass.medicionesDiccionario[nombreMedicionKeyGlobal][15],
				MedicionesClass.medicionesDiccionario[nombreMedicionKeyGlobal][16]};
				FormConfiguraciones.actualizarConditionClassDesdeTab2();
				FormConfiguraciones.buttonAceptarTab2_Click(sender, e);
				FormConfiguraciones.buttonAplicarTab3_Click(sender, e);
			}
			// en el caso de agregar una medicion
			else
			{
				// Si todo esta OK, ingresar la informacion al diccionario y luego al datagridview
				//Verificar que la Key no exista antes de agregar
				if(MedicionesClass.medicionesDiccionario.ContainsKey(textBoxNombreMedicion.Text) == false)
				{
					MedicionesClass.agregarMediciones(textBoxNombreMedicion.Text, comboBoxMagnitudes.Text, textBoxUnidadMedida.Text, textBoxGalContenedora.Text, comboBoxSensor1.Text, comboBoxSensor2.Text,
						textBoxValorMinimo.Text, textBoxValorMaximo.Text, comboBoxPuertoCOM.Text, textBoxPosicionCOMS1.Text, textBoxPosicionCOMS2.Text,textBoxTransformacion.Text,textBoxValorInicial.Text);
				}
				else
				{
					MessageBox.Show("El Nombre de la Medicion: " + textBoxNombreMedicion.Text + ". Ya Existe, elija otro Nombre.");
					return;
				}
			}
			FormConfiguraciones.actualizarDataGridViewMediciones(textBoxGalContenedora.Text);
			FormConfiguraciones.actualizarConfigGaleriasContenedoras();
			FormConfiguraciones.Enabled = true;
			FormConfiguraciones.formulario1.Enabled = true;

			this.Close();
		}

		private void Form_Configurar_Componente_FormClosed(object sender, FormClosedEventArgs e)
		{
			FormConfiguraciones.Enabled = true;
			FormConfiguraciones.formulario1.Enabled = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Escriba la funcion correctora en la cual X corresponda al voltaje recibido del primer Sensor e Y del segundo Sensor.\nLa letra A representa el area de la galeria Contenedora.\nFunciones Disponibles: Sen(), Cos(),Tan() y Sqrt()\n^, *, /, +, -.\nEjemplo: sqrt(2*X/0.9)");

		}

		private void Form_Configurar_Componente_FormClosing(object sender, FormClosingEventArgs e)
		{
			
		}

	}
}
