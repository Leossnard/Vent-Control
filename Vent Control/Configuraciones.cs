using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vent_Control
{
	public partial class Configuraciones : Form
	{

		// -------------------------------------------------------- Varibles Generales ------------------------------------------------------
		// Variable que indica el componente al que se le ha hecho doble click
		public string sensorSeleccionadoClick { get; set; }
		List<string> galeriasContenedoras = new List<string>();
		public static Dictionary<string, List<string>> DiccionarioGaleriaSensores;
		public Form1 formulario1;
		int seHizoUnCambio = 0;//indica la tab en la cual se realizo el cambio


		// Constructor de la clase (Formulario)
		public Configuraciones(string IDGaleriaInicial, Form1 Form1)
		{
			InitializeComponent();
			galeriaSeleccionada = IDGaleriaInicial;
			sensorSeleccionadoClick = IDGaleriaInicial;
			DiccionarioGaleriaSensores = new Dictionary<string, List<string>>();
			formulario1 = Form1;
		}



		// Si se cambia la tab (usada para asignar valores iniciales)
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)//-------------------------------------------evento al cambiar de tab----------------------
		{
			// Si se realiza un cambio y se cambia de TAB preguntar si se desea guardar la configuracion antes de cambiar la TAB
			if (seHizoUnCambio != 0)
			{
				DialogResult mensaje = MessageBox.Show("Se ha realizado un Cambio, desea Aceptar los Cambios antes de Cambiar de Pestaña?", "Recomendacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (mensaje == DialogResult.Yes)
				{
					if (seHizoUnCambio == 1)
					{
						buttonAceptar_Click(sender, e);
					}
					else if (seHizoUnCambio == 2)
					{
						buttonAceptarTab2_Click(sender, e);
					}
					else if (seHizoUnCambio == 3)
					{
						buttonAplicarTab3_Click(sender, e);
					}
					else if (seHizoUnCambio == 4)
					{
						buttonAceptarCOM_Click(sender, e);
					}
				}
			}


			if (tabControl1.SelectedTab == tabPage2)//Componentes
			{
				seHizoUnCambio = 0;
				labelActualizarComponentes.Text = "";
				//Actualizar el listbox de las galerias
				ActualizarListBoxGaleriasTab2();
				listBoxComponentesDisponibles.Enabled = false;
				if (listBoxGaleriasContenedoras.Items.Count != 0)
				{
					listBoxGaleriasContenedoras.SelectedIndex = 0;
					buttonAgregarMedicion.Enabled = true;
				}
				else
				{
					buttonAgregarMedicion.Enabled = false;
				}
			}
			else if (tabControl1.SelectedTab == tabPage3)//Ventiladores-Compuertas
			{
				seHizoUnCambio = 0;
				actualizarConditionClassDesdeTab2();
				labelActualizarVentComp.Text = "";
				ActualizarListBoxGaleriasTab3();
				ActualizarVentCompListBox();
				buttonAgregarCondicionAumento.Enabled = false;
				if (listBoxGaleriasTab3.Items.Count != 0)
				{
					buttonAgregarCondicionAumento.Enabled = false;
					buttonAgregarCondicionDisminuir.Enabled = false;
					listBoxGaleriasTab3.SelectedIndex = 0;
				}
			}
			else if (tabControl1.SelectedTab == tabPage4)//Puertos COM
			{
				seHizoUnCambio = 0;
				labelActualizarCOM.Text = "";
				estadoControlesInicial();
				comboBoxProtocolo.Enabled = false;
				button1.Enabled = false;
			}


		}

		// Funcion que muestra un mensaje y luego lo borra.
		private async void mostrarOcultarLabel(Label labelAceptar)
		{
			labelAceptar.Text = "Configuraciones Actualizadas";
			await Task.Delay(1500);
			labelAceptar.Text = "";
		}

		// Evento que pregunta antes de salir
		private void Configuraciones_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (seHizoUnCambio != 0)
			{
				if (MessageBox.Show("Cambios Detectados ¿Quiere Salir de las Configuraciones?", "Precaucion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
				{
					e.Cancel = true;
				}
			}
		}

		//--------------------------------------------------------- Tab Configurar Galerias TAB1 --------------------------------------------------
		List<string> sensoresTotales = new List<string>();
		bool esperarDobleClick = false;


		//public static string sensorSeleccionadoClick;
		string galeriaSeleccionada;
		string sensorSeleccionado;



		// Evento que agrega los sensores disponibles a su listbox 
		private void Configuraciones_Load(object sender, EventArgs e)
		{
			if (esperarDobleClick == false)
			{
				// Configurar Tab inicial segun el dobleclick
				if (sensorSeleccionadoClick != null)
				{
					if (sensorSeleccionadoClick.Contains("GAL"))
					{
						tabControl1.SelectedIndex = 0;
					}
					else//para todos los demas (sensores)
					{
						tabControl1.SelectedIndex = 1;
					}
				}
			}

			// Desactivar los listbox
			listBoxSensores.Enabled = false;
			listBoxSensoresOK.Enabled = false;
			// desactivar los botones
			buttonAsociar.Enabled = false;
			buttonAsociarClick.Enabled = false;
			buttonDisociar.Enabled = false;
			//label
			labelActualizarGalerias.Text = "";

			// Lista de galerias contenedoras, agrega solo las galerias que son contenedoras
			foreach (string llave in Form1.propiedadesDinamicasComponentes.Keys)
			{
				if (llave != "COMPORTS")
				{
					if (Form1.propiedadesDinamicasComponentes[llave][2] == "1")
					{
						galeriasContenedoras.Add(llave);
					}
				}
			}

			// Sensores asociados a la galeria
			// Cargar desde propiedades dinamicas, a partir del index 3 todos las llaves seran los sensores asociados a la galeria, cuando terminen seran las siguientes propiedades, solo se cargan los componentes

			foreach (string llave in galeriasContenedoras)
			{
				for (int i = 3; i < Form1.propiedadesDinamicasComponentes[llave].Count; i++)
				{
					//aca agregar los otros valores que no se quieren agregar como las configuraciones con &&----------------------------------------------------------------------------------------------> aca
					if (Form1.propiedadesDinamicasComponentes[llave][i].Contains("AREAGAL") == false && Form1.propiedadesDinamicasComponentes[llave][i].Contains("CONFIG-") == false &&
						Form1.propiedadesDinamicasComponentes[llave][i].Contains("CONDITION-") == false && Form1.propiedadesDinamicasComponentes[llave][i].Contains("COMPORTS") == false)
					{
						if (DiccionarioGaleriaSensores.ContainsKey(llave))
						{
							DiccionarioGaleriaSensores[llave].Add(Form1.propiedadesDinamicasComponentes[llave][i]);
						}
						else
						{
							DiccionarioGaleriaSensores.Add(llave, new List<string> { Form1.propiedadesDinamicasComponentes[llave][i] });
						}
					}

				}
			}
			// Sensores disponibles en el panel, aca poner los que no se quieren agregar
			foreach (string sensorID in Form1.propiedadesDinamicasComponentes.Keys)
			{
				if (sensorID.Contains("GAL") == false && sensorID.Contains("LIVE") == false && sensorID.Contains("COMPORTS") == false)
				{
					//Agrega todos los componentes que no sean galeria
					sensoresTotales.Add(sensorID);
					// Eliminar los sensores que ya estan asociados al diccionario propiedadesDinamicasComponentes
					foreach (string llaveDiccionarioInterno in DiccionarioGaleriaSensores.Keys)
					{
						foreach (string componente in DiccionarioGaleriaSensores[llaveDiccionarioInterno])
						{
							sensoresTotales.Remove(componente);
						}
					}
				}
			}
			// agregar informacion a los listboxes
			actualizarListbox(true, true, true, galeriaSeleccionada);

			//En el caso de que se haga doble click en un sensor o ventilador
			if (sensorSeleccionadoClick != null)
			{
				if (sensorSeleccionadoClick.Contains("VENT"))
				{
					tabControl1.SelectedIndex = 2;
					//llamar al evento q actualiza la tab 2
				}
				else if (sensorSeleccionadoClick.Contains("GAL"))
				{
					// No hace nada pero lo puse para que se haga esta validacion antes que la de los sensores (la linea siguiente)
				}
				else if (sensorSeleccionadoClick.Contains("COMPONENT") || sensorSeleccionadoClick.Contains("LIVE"))
				{
					tabControl1.SelectedIndex = 1;
					tabControl1_SelectedIndexChanged(sender, e);
				}
			}
		}

		// funcion que actualiza los listboxes
		private void actualizarListbox(bool galerias = false, bool sensores = false, bool sensoresOK = false, string galeriaLLave = null)
		{
			if (galerias)
			{
				listBoxGalerias.Items.Clear();
				foreach (string galeria in galeriasContenedoras)
				{
					listBoxGalerias.Items.Add(galeria);
				}
				int indiceGalInicial = listBoxGalerias.FindStringExact(galeriaSeleccionada);
				if (indiceGalInicial != -1)
				{
					listBoxGalerias.SetSelected(indiceGalInicial, true);
				}
			}
			if (sensores)
			{
				listBoxSensores.Items.Clear();
				foreach (string sensor in sensoresTotales)
				{
					listBoxSensores.Items.Add(sensor);
				}
			}
			if (sensoresOK)
			{
				listBoxSensoresOK.Items.Clear();
				if (galeriaLLave != null)
				{
					if (DiccionarioGaleriaSensores.ContainsKey(galeriaLLave))
					{
						foreach (string sensor in DiccionarioGaleriaSensores[galeriaLLave])
						{
							listBoxSensoresOK.Items.Add(sensor);
						}
					}
				}
			}
		}


		private void button3_Click(object sender, EventArgs e)
		{
			Form_Identificar_COM FormIdentificarCOM = new Form_Identificar_COM();
			FormIdentificarCOM.Show();
		}

		private void buttonAsociar_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 1;
			buttonAceptar.Enabled = true;
			if (listBoxSensores.SelectedItem != null)
			{
				sensorSeleccionado = listBoxSensores.SelectedItem.ToString();
				galeriaSeleccionada = listBoxGalerias.SelectedItem.ToString();
				if (DiccionarioGaleriaSensores.ContainsKey(galeriaSeleccionada))
				{
					DiccionarioGaleriaSensores[galeriaSeleccionada].Add(sensorSeleccionado);
				}
				else
				{
					DiccionarioGaleriaSensores.Add(galeriaSeleccionada, new List<string> { sensorSeleccionado });
				}
				sensoresTotales.Remove(sensorSeleccionado);
				actualizarListbox(false, true, true, galeriaSeleccionada);
			}
		}

		// Al cambiar el indice en el listbox Galerias
		private void listBoxGalerias_SelectedIndexChanged(object sender, EventArgs e)
		{
			listBoxSensores.Enabled = true;
			listBoxSensoresOK.Enabled = true;
			buttonAsociar.Enabled = false;
			buttonAsociarClick.Enabled = true;
			buttonDisociar.Enabled = false;
			if (listBoxGalerias.SelectedItem != null)
			{
				galeriaSeleccionada = listBoxGalerias.SelectedItem.ToString();
				actualizarListbox(false, true, true, galeriaSeleccionada);
				actualizarMostarAreaGaleria(galeriaSeleccionada);
			}
		}

		public string valortextBoxArea()
		{
			return textBoxArea.Text;
		}

		public void actualizarMostarAreaGaleria(string galeriaSeleccionadaKey, bool actualizar = false)
		{
			//leer informacion de propiedadesDinamicasComponentes
			int indice = 3;
			List<string> listaSubPropiedades = new List<string>();
			bool encontrado = false;
			//cuando encuentre AREAGAL crea una lista con las subpropiedades y tambien se puede obtener la ubicacion en la lista
			while (indice < Form1.propiedadesDinamicasComponentes[galeriaSeleccionadaKey].Count)
			{
				if (Form1.propiedadesDinamicasComponentes[galeriaSeleccionadaKey][indice].Contains("AREAGAL"))
				{
					encontrado = true;
					// Formato: AREAGAL ; VALOR ; UNIDAD MEDIDA
					listaSubPropiedades = Form1.propiedadesDinamicasComponentes[galeriaSeleccionadaKey][indice].Split(';').ToList();
					break;
				}
				indice++;
			}

			if (encontrado)
			{
				if (actualizar)
				{
					Form1.propiedadesDinamicasComponentes[galeriaSeleccionadaKey][indice] = "AREAGAL;" + textBoxArea.Text + ";" + textBoxUMArea.Text;
				}
				else
				{
					textBoxArea.Text = listaSubPropiedades[1];
					textBoxUMArea.Text = listaSubPropiedades[2];
				}
			}
			else
			{
				if (actualizar)
				{
					Form1.propiedadesDinamicasComponentes[galeriaSeleccionadaKey].Add("AREAGAL;" + textBoxArea.Text + ";" + textBoxUMArea.Text);
				}
				else
				{
					textBoxArea.Text = "";
					textBoxUMArea.Text = "";
				}
			}

		}

		private void listBoxSensores_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonAsociar.Enabled = true;
			buttonAsociarClick.Enabled = true;
		}

		private void listBoxSensoresOK_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonDisociar.Enabled = true;
		}

		private void buttonAsociarClick_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 1;
			esperarDobleClick = true;
			MessageBox.Show("Seleccionar con Doble Click el Sensor que Desea agregar a la Galeria: " + galeriaSeleccionada + ".", "Seleccionar Sensor", MessageBoxButtons.OK, MessageBoxIcon.Information);
			this.Enabled = false;
			buttonAceptar.Enabled = true;
			Form1.seleccionarComponente = true;
			this.WindowState = FormWindowState.Minimized;
		}

		// funcion que ve si el sensor seleccionado (doble click), esta disponible en la lista sensoresTotales, si esta disponible lo pasa a la galeria seleccionada en el listboxgalerias, si no esta disponible mandar un mensaje de error
		// llamar a la funcion desde el form1
		public void manejarSensorSeleccionadoClick()
		{
			this.Enabled = true;//ver si se  mantiene seleccionado despues de cambiar el enable
			galeriaSeleccionada = listBoxGalerias.SelectedItem.ToString();
			foreach (string IDSensor in sensoresTotales)
			{
				if (IDSensor == sensorSeleccionadoClick)
				{
					if (DiccionarioGaleriaSensores.ContainsKey(galeriaSeleccionada))
					{
						DiccionarioGaleriaSensores[galeriaSeleccionada].Add(sensorSeleccionadoClick);
					}
					else
					{
						DiccionarioGaleriaSensores.Add(galeriaSeleccionada, new List<string> { sensorSeleccionadoClick });
					}
					sensoresTotales.Remove(sensorSeleccionadoClick);
					actualizarListbox(false, true, true, galeriaSeleccionada);
					return;
				}
			}
			// si no encuetra el sensor seleccionado el return nunca se activa por lo que sigue la ejecucion
			MessageBox.Show("El Sensor Seleccionado: " + sensorSeleccionadoClick + ". Ya esta Asigando a una Galeria");
			sensorSeleccionadoClick = null;
		}

		private void buttonDisociar_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 1;
			buttonAceptar.Enabled = true;
			if (listBoxSensoresOK.SelectedItem != null)
			{
				// Elimina del diccionario el componente seleccionado y lo agrega a la lista de los sensores
				string sensoramover = listBoxSensoresOK.SelectedItem.ToString();
				galeriaSeleccionada = listBoxGalerias.SelectedItem.ToString();
				sensoresTotales.Add(sensoramover);
				DiccionarioGaleriaSensores[galeriaSeleccionada].Remove(sensoramover);
				actualizarListbox(false, true, true, galeriaSeleccionada);
			}
		}

		private void buttonEliminarContenedor_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 1;
			// Cambiar la propiedad de contenedor a 0, eliminar la galeria de la lista de galerias (listbox), eliminar sus sensores si es que los tiene
			DialogResult MensajeContenedor = MessageBox.Show("Si la Galeria seleccionada deja de ser contenedora, se disociaran todos sus sensores, configuraciones y condiciones.\nEsta Accion es Irreversible. Para volver volver a configurar la galeria como contenedora, darle doble click en el panel principal ¿Desea Continuar",
							"Precaucion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (MensajeContenedor == DialogResult.Yes)
			{
				buttonAceptar.Enabled = true;
				galeriaSeleccionada = listBoxGalerias.SelectedItem.ToString();
				// hacer esto como una funcion y llamarla desde el form1 luego de borrar un sensor------------------------------------------------------------------------------------------

				// Actualizar diccionarios en clases tras borrar una medicion o una condicion
				formulario1.actualizarDiccionariosClases();

				//// Elimina todas las mediciones y condiciones que tengan como galeria contenedora la galeria seleccionada

				// vuelve a gregar los sensores de la galeria que se va a borrar a la lista de sensores disponibles
				if (DiccionarioGaleriaSensores.ContainsKey(galeriaSeleccionada))
				{
					foreach (string sensor in DiccionarioGaleriaSensores[galeriaSeleccionada])
					{
						sensoresTotales.Add(sensor);
					}
					DiccionarioGaleriaSensores.Remove(galeriaSeleccionada);
				}

				galeriasContenedoras.Remove(galeriaSeleccionada);
				Form1.propiedadesDinamicasComponentes[galeriaSeleccionada][2] = "0";
				// reiniciar el formulario.
				this.Controls.Clear();
				this.InitializeComponent();
				actualizarListbox(true, true, true, galeriaSeleccionada);
			}

		}

		private void buttonActualizarArea_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 1;
			//verificar que la unidad no este vacia
			if (textBoxUMArea.Text == "")
			{
				MessageBox.Show("Ingrese una unidad");
				return;
			}
			double areaIngresada;
			if (double.TryParse(textBoxArea.Text, out areaIngresada))
			{
				if (areaIngresada <= 0)
				{
					MessageBox.Show("El Area debe ser positiva");
				}
				galeriaSeleccionada = listBoxGalerias.SelectedItem.ToString();
				actualizarMostarAreaGaleria(galeriaSeleccionada, true);
			}
			else
			{
				MessageBox.Show("Ingrese un Numero Valido");
			}
		}

		private void buttonAceptar_Click(object sender, EventArgs e)
		{
			// agregar o modificar la informacion de las galerias al diccionario propiedadesDinamicasComponentes

			for (int i = 0; i < galeriasContenedoras.Count; i++)
			{
				int final = 0;//indica el final de los sensores asociados en el diccionario
				string llave = galeriasContenedoras[i];
				for (int j = 0; j < Form1.propiedadesDinamicasComponentes[llave].Count; j++)
				{
					if (Form1.propiedadesDinamicasComponentes[llave][j].Contains("COMPONENT"))//otorga la ultima posicion de un componente, se piensa asi dado que en un futuro pueden haber mas valores como configuraciones
					{
						final = j;
					}
				}
				// tengo el inicio y el final de la lista en dentro de un elemento del diccionario, borrar por los indices y luego insertar los componentes que se setearon en este formulario

				if (final != 0)
				{
					//borrar los sensores asociados originales
					Form1.propiedadesDinamicasComponentes[llave].RemoveRange(3, (final - 3 + 1));
				}
				// Insertar los nuevos sensores
				if (DiccionarioGaleriaSensores.ContainsKey(llave))
				{
					string[] sensoresArray = DiccionarioGaleriaSensores[llave].ToArray();
					Form1.propiedadesDinamicasComponentes[llave].InsertRange(3, sensoresArray);
				}
			}

			mostrarOcultarLabel(labelActualizarGalerias);
			seHizoUnCambio = 0;
		}



		//--------------------------------------------------------- Tab Configurar Componentes (TAB 2) --------------------------------------------------


		string galeriaSeleccionadaTab2 = "";
		Dictionary<string, List<string>> DiccionarioGaleriaSensoresTab2 = new Dictionary<string, List<string>>();

		private void ActualizarListBoxGaleriasTab2()
		{
			listBoxGaleriasContenedoras.Items.Clear();
			foreach (string galeriasKeys in galeriasContenedoras)
			{
				listBoxGaleriasContenedoras.Items.Add(galeriasKeys);
			}
		}

		private void buttonAgregarMedicion_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 2;
			galeriaSeleccionadaTab2 = listBoxGaleriasContenedoras.SelectedItem.ToString();
			Form_Configurar_Componente configurarMedicionesForm = new Form_Configurar_Componente(galeriaSeleccionadaTab2, this);
			configurarMedicionesForm.Show();
			this.Enabled = false;
			formulario1.Enabled = false;
		}

		private void listBoxGaleriasContenedoras_SelectedIndexChanged(object sender, EventArgs e)
		{
			listBoxComponentesDisponibles.Items.Clear();
			galeriaSeleccionadaTab2 = listBoxGaleriasContenedoras.SelectedItem.ToString();
			if (DiccionarioGaleriaSensores.Count != 0)
			{
				foreach (string componentes in DiccionarioGaleriaSensores[galeriaSeleccionadaTab2])
				{

					listBoxComponentesDisponibles.Items.Add(componentes);
				}
			}
			else
			{
				return;
			}

			actualizarDataGridViewMediciones(galeriaSeleccionadaTab2);
		}


		public void actualizarDataGridViewMediciones(string galeriaSeleccionadaTab2)
		{
			dataGridViewMediciones.Rows.Clear();
			foreach (string nombreMedicionKey in MedicionesClass.medicionesDiccionario.Keys)
			{
				// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2
				//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10

				if (MedicionesClass.medicionesDiccionario[nombreMedicionKey][3] == galeriaSeleccionadaTab2)
				{

					string componente1 = MedicionesClass.medicionesDiccionario[nombreMedicionKey][4];
					string componente2 = MedicionesClass.medicionesDiccionario[nombreMedicionKey][5];
					string componentes = componente1;

					string posicion1 = MedicionesClass.medicionesDiccionario[nombreMedicionKey][9];
					string posicion2 = MedicionesClass.medicionesDiccionario[nombreMedicionKey][10];
					string posiciones = posicion1;

					if (componente2 != "")
					{
						componentes = componentes + ";" + componente2;
						posiciones = posiciones + ";" + posicion2;
					}

					List<string> fila = new List<string>();
					int index = 0;
					while (index < MedicionesClass.medicionesDiccionario[nombreMedicionKey].Count)
					{
						if (index == 4)
						{
							fila.Add(componentes);
							index++;
						}
						else if (index == 9)
						{
							fila.Add(posiciones);
							index++;
						}
						else
						{
							fila.Add(MedicionesClass.medicionesDiccionario[nombreMedicionKey][index]);
						}
						index++;
					}
					fila[11] ="Editar";
					string[] filaArray = fila.ToArray();
					dataGridViewMediciones.Rows.Add(filaArray);
				}
			}
		}

		// Evento al presionar el boton editar de cada fila del datagridview----------------------------------------------------------------------
		private void dataGridViewMediciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			seHizoUnCambio = 2;
			var senderGrid = (DataGridView)sender;
			if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
			{
				string galeriaContenedoraEnGrid = dataGridViewMediciones.Rows[e.RowIndex].Cells[3].Value.ToString();
				string nombreMedicionKey = dataGridViewMediciones.Rows[e.RowIndex].Cells[0].Value.ToString();
				Form_Configurar_Componente formConfigurarMediciones = new Form_Configurar_Componente(galeriaContenedoraEnGrid, this, nombreMedicionKey);
				formConfigurarMediciones.Show();

			}
		}


		//Eliminar la medicion seleccionada junto con su LIVE si es que este existe en el panel
		private void buttonEliminarMedicion_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 2;
			if (dataGridViewMediciones.Rows.Count > 0)
			{
				string keyNombreMedicion = dataGridViewMediciones.SelectedRows[0].Cells[0].Value.ToString();
				string nombreGaleriaActual = dataGridViewMediciones.SelectedRows[0].Cells[3].Value.ToString();
				DialogResult pregunta = MessageBox.Show("Esta seguro que quiere eliminar la medicion: " + keyNombreMedicion + "?", "Precaucion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (pregunta == DialogResult.Yes)
				{
					//Eliminar Medicion del diccionario
					MedicionesClass.medicionesDiccionario.Remove(keyNombreMedicion);
					//Eliminar la Medicion del Panel (si es que existe)
					if (Form1.propiedadesDinamicasComponentes.ContainsKey("LIVE-" + keyNombreMedicion))
					{
						formulario1.eliminarComponentes("LIVE-" + keyNombreMedicion);
						// la funcion eliminar componentes llama a la funcion actualizar diccionarios, la cual borrar de otros diccionarios la informacion que no encuentre en los otros
					}
				}

				actualizarDataGridViewMediciones(nombreGaleriaActual);
				actualizarConfigGaleriasContenedoras();
			}
			else
			{
				MessageBox.Show("Ninguna Medicion que Borrar");
			}
		}

		public void buttonAceptarTab2_Click(object sender, EventArgs e)
		{
			actualizarConfigGaleriasContenedoras();
			mostrarOcultarLabel(labelActualizarComponentes);
			seHizoUnCambio = 0;
		}


		// Agrega la informacion por cada galeria al diccionario Form1.propiedadesDinamicasComponentes

		public void actualizarConfigGaleriasContenedoras()
		{
			// Elimina todas las configuraciones existentes en el diccionario Form1.propiedadesDinamicasComponentes
			foreach (string componenteKey in galeriasContenedoras)
			{
				int indiceListaPropDinamicas = 0;
				while (indiceListaPropDinamicas < Form1.propiedadesDinamicasComponentes[componenteKey].Count)
				{
					if (Form1.propiedadesDinamicasComponentes[componenteKey][indiceListaPropDinamicas].Contains("CONFIG-"))
					{
						Form1.propiedadesDinamicasComponentes[componenteKey].RemoveAt(indiceListaPropDinamicas);
						indiceListaPropDinamicas = indiceListaPropDinamicas - 2;
					}
					indiceListaPropDinamicas++;
				}
			}


			//Agrega las configuraciones (mediciones) del diccionario MedicionesClass.medicionesDiccionario al diccionario Form1.propiedadesDinamicasComponentes, para que luego se puedan guardar en el txt
			foreach (string nombreMedicionKey in MedicionesClass.medicionesDiccionario.Keys)
			{
				string galeriaActualMedicionKey = MedicionesClass.medicionesDiccionario[nombreMedicionKey][3];
				// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2
				//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10
				string configuracionMedicion = "CONFIG-" + string.Join(";", MedicionesClass.medicionesDiccionario[nombreMedicionKey].ToArray());
				Form1.propiedadesDinamicasComponentes[galeriaActualMedicionKey].Add(configuracionMedicion);
			}
		}

		// agrega la medicion seleccionada al panel para que se muestre el valor de los sensores en tiempo real
		private void buttonAddMediccionPanel_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 2;
			if (dataGridViewMediciones.Rows.Count > 0)
			{
				string keyNombreMedicion = dataGridViewMediciones.SelectedRows[0].Cells[0].Value.ToString();
				string valorUnidad = dataGridViewMediciones.SelectedRows[0].Cells[2].Value.ToString();
				// si la medicion selecciona ya esta en el panel1 salir
				if (Form1.propiedadesDinamicasComponentes.ContainsKey("LIVE-" + keyNombreMedicion))
				{
					MessageBox.Show("La Medicion Seleccionada ya se encuentra en el Panel, no se puede agregar dos veces.");
				}
				else
				{
					formulario1.AgregarComponentes(keyNombreMedicion, false, false, true);
					this.WindowState = FormWindowState.Minimized;
					Form1.DiccionarioComponentes["LIVE-" + keyNombreMedicion].Item1.Controls[0].Text = valorUnidad;
					Form1.DiccionarioComponentes["LIVE-" + keyNombreMedicion].Item1.Controls[1].Text = keyNombreMedicion;
				}
			}
			else
			{
				MessageBox.Show("Ninguna Medicion que agregar al panel");
			}
		}

		//--------------------------------------------------------- Tab Configurar Ventiladores y Compuertas --------------------------------------------------

		string galeriaSeleccionadaTab3 = "";
		string medicionSeleccionada = "";
		string condicionSeleccionada = "";
		string vent_comp_seleccionadoa = "";

		private void ActualizarListBoxGaleriasTab3()
		{
			listBoxGaleriasTab3.Items.Clear();
			foreach (string galeriaKey in galeriasContenedoras)
			{
				listBoxGaleriasTab3.Items.Add(galeriaKey);
			}
			actualizarDataGridVentCompuerta();
		}

		private void ActualizarVentCompListBox()
		{
			listBoxVenCompDisponibles.Items.Clear();
			foreach (string medicionKey in MedicionesClass.medicionesDiccionario.Keys)
			{
				if (MedicionesClass.medicionesDiccionario[medicionKey][8].Split('-')[1] == "Enviar")
				{
					if (listBoxVenCompDisponibles.Items.Contains(medicionKey) == false)
					{
						listBoxVenCompDisponibles.Items.Add(medicionKey);

					}
				}
			}
			if (listBoxVenCompDisponibles.Items.Count != 0)
			{
				listBoxVenCompDisponibles.SelectedIndex = 0;
			}
		}
		//evento que actualiza las mediciones disponibles por cada galeria
		private void listBoxGaleriasTab3_SelectedIndexChanged(object sender, EventArgs e)
		{
			listBoxMedicionesVentComp.Items.Clear();
			listBoxValoresMinMax.Items.Clear();
			galeriaSeleccionadaTab3 = listBoxGaleriasTab3.SelectedItem.ToString();
			foreach (string medicionesKeys in MedicionesClass.medicionesDiccionario.Keys)
			{
				if (MedicionesClass.medicionesDiccionario[medicionesKeys][3] == galeriaSeleccionadaTab3)
				{
					if (MedicionesClass.medicionesDiccionario[medicionesKeys][4].Contains("VENT") == false && MedicionesClass.medicionesDiccionario[medicionesKeys][4].Contains("COMPUERTA") == false)
					{
						listBoxMedicionesVentComp.Items.Add(medicionesKeys);
					}
				}
			}
			if (listBoxMedicionesVentComp.Items.Count != 0)
			{
				listBoxMedicionesVentComp.SelectedIndex = 0;
			}

		}

		private void listBoxMedicionesVentComp_SelectedIndexChanged(object sender, EventArgs e)
		{
			medicionSeleccionada = listBoxMedicionesVentComp.SelectedItem.ToString();
			actualizarlistBoxValoresMinMax();
			if (listBoxValoresMinMax.Items.Count != 0)
			{
				listBoxValoresMinMax.SelectedIndex = 0;
				buttonAgregarCondicionAumento.Enabled = true;
				buttonAgregarCondicionDisminuir.Enabled = true;
			}
		}

		//Actualizar el listBoxValoresMinMax
		private void actualizarlistBoxValoresMinMax()
		{
			listBoxValoresMinMax.Items.Clear();
			medicionSeleccionada = listBoxMedicionesVentComp.SelectedItem.ToString();
			if (listBoxVenCompDisponibles.SelectedItem != null)
			{
				vent_comp_seleccionadoa = listBoxVenCompDisponibles.SelectedItem.ToString();
			}

			//recorrer los dos datagridview en busquedad para el valor minimo y para el maximo
			//agregar los valores minimos y maximos al listbox 
			foreach (string medicionKey in MedicionesClass.medicionesDiccionario.Keys)
			{
				//agregar solo si los valores que no estan siendo utlizados en los datagridview segun ventilador/compuerta y mediciones

				if (medicionSeleccionada == medicionKey)
				{
					string valorMaximo = "Valor Maximo : " + MedicionesClass.medicionesDiccionario[medicionKey][7] + " " + MedicionesClass.medicionesDiccionario[medicionKey][2];
					string valorMinimo = "Valor Minimo : " + MedicionesClass.medicionesDiccionario[medicionKey][6] + " " + MedicionesClass.medicionesDiccionario[medicionKey][2];
					bool agregarMax = true;
					bool agregarMin = true;
					foreach (DataGridViewRow fila in dataGridViewAumentar.Rows)
					{
						if (vent_comp_seleccionadoa == fila.Cells[1].Value.ToString() && medicionSeleccionada == fila.Cells[0].Value.ToString())
						{
							if (valorMaximo == fila.Cells[3].Value.ToString())//puede ser con un contains
							{
								agregarMax = false;
							}
							if (valorMinimo == fila.Cells[3].Value.ToString())
							{
								agregarMin = false;
							}
						}
					}
					foreach (DataGridViewRow fila in dataGridViewDisminuir.Rows)
					{
						if (vent_comp_seleccionadoa == fila.Cells[1].Value.ToString() && medicionSeleccionada == fila.Cells[0].Value.ToString())
						{
							if (valorMaximo == fila.Cells[3].Value.ToString())//puede ser con un contains
							{
								agregarMax = false;
							}
							if (valorMinimo == fila.Cells[3].Value.ToString())
							{
								agregarMin = false;
							}
						}
					}
					if (agregarMax)
					{
						listBoxValoresMinMax.Items.Add(valorMaximo);
					}
					if (agregarMin)
					{
						listBoxValoresMinMax.Items.Add(valorMinimo);
					}
					break;
				}
			}

			if (listBoxValoresMinMax.Items.Count != 0)
			{
				listBoxValoresMinMax.SelectedIndex = 0;
			}


		}

		private void buttonAgregarCondicionAumento_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 3;
			//verificar que la lista  de minmax tenga valores
			if (listBoxValoresMinMax.Items.Count == 0)
			{
				MessageBox.Show("No hay valores de Restriccion Disponibles.");
				return;
			}


			// Orden:nombreMedicion, nombreVentCompu ,galeria, condicion, tiempoEspera, incremento, UnidadMedida, puertoCOM
			medicionSeleccionada = listBoxMedicionesVentComp.SelectedItem.ToString();
			vent_comp_seleccionadoa = listBoxVenCompDisponibles.SelectedItem.ToString();
			galeriaSeleccionadaTab3 = listBoxGaleriasTab3.SelectedItem.ToString();
			condicionSeleccionada = listBoxValoresMinMax.SelectedItem.ToString();
			// Del Ventilador
			string unidadMedida = MedicionesClass.medicionesDiccionario[vent_comp_seleccionadoa][2];
			string puertoCOM = MedicionesClass.medicionesDiccionario[vent_comp_seleccionadoa][8];


			// Verificar que no se pueda agregar un valor minimo o maximo si ya existe uno en un datagridview
			if (verificarMinMaxExistente(dataGridViewAumentar) == false)
			{
				MessageBox.Show("No Pueden Haber Restricciones Contradictorias en el Mismo panel Condicional.");
				return;
			}

			string[] filaAumentar = { medicionSeleccionada, vent_comp_seleccionadoa, galeriaSeleccionadaTab3, condicionSeleccionada, "", "", unidadMedida, puertoCOM };
			dataGridViewAumentar.Rows.Add(filaAumentar);
			actualizarlistBoxValoresMinMax();

			//Actualizar los listboxes
			if (listBoxValoresMinMax.Items.Count != 0)
			{
				listBoxValoresMinMax.SelectedIndex = 0;
			}

		}

		// Verificar si el minimo o el maximo ya existe en el dataGridView Aumentar o Disminuir
		private bool verificarMinMaxExistente(DataGridView datagridviewAumentarDisminuir)
		{
			medicionSeleccionada = listBoxMedicionesVentComp.SelectedItem.ToString();
			vent_comp_seleccionadoa = listBoxVenCompDisponibles.SelectedItem.ToString();
			condicionSeleccionada = listBoxValoresMinMax.SelectedItem.ToString();
			bool resultado = true;

			//para el datagridviewAumentar
			foreach (DataGridViewRow fila in datagridviewAumentarDisminuir.Rows)
			{
				//se la medicion y el ventilador/compuerta son iguales a los seleccionados, retorna false
				if (fila.Cells[0].Value.ToString() == medicionSeleccionada && fila.Cells[1].Value.ToString() == vent_comp_seleccionadoa)
				{
					if (fila.Cells[3].Value.ToString().Contains("Valor Minimo") && condicionSeleccionada.Contains("Valor Maximo"))
					{
						resultado = false;
					}
					else if (fila.Cells[3].Value.ToString().Contains("Valor Maximo") && condicionSeleccionada.Contains("Valor Minimo"))
					{
						resultado = false;
					}
				}
			}
			return resultado;

		}

		private void buttonAgregarCondicionDisminuir_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 3;
			//verificar que la lista  de minmax tenga valores
			if (listBoxValoresMinMax.Items.Count == 0)
			{
				MessageBox.Show("No hay valores de Restriccion Disponibles.");
				return;
			}

			// Orden:nombreMedicion, nombreVentCompu ,galeria, condicion, tiempoEspera, incremento, UnidadMedida, puertoCOM
			medicionSeleccionada = listBoxMedicionesVentComp.SelectedItem.ToString();
			vent_comp_seleccionadoa = listBoxVenCompDisponibles.SelectedItem.ToString();
			galeriaSeleccionadaTab3 = listBoxGaleriasTab3.SelectedItem.ToString();
			condicionSeleccionada = listBoxValoresMinMax.SelectedItem.ToString();
			// Del Ventilador
			string unidadMedida = MedicionesClass.medicionesDiccionario[vent_comp_seleccionadoa][2];
			string puertoCOM = MedicionesClass.medicionesDiccionario[vent_comp_seleccionadoa][8];


			// Verificar que no se pueda agregar un valor minimo o maximo si ya existe uno en un datagridview
			if (verificarMinMaxExistente(dataGridViewDisminuir) == false)
			{
				MessageBox.Show("No Pueden Haber Restricciones Contradictorias en el Mismo panel Condicional.");
				return;
			}

			string[] filaAumentar = { medicionSeleccionada, vent_comp_seleccionadoa, galeriaSeleccionadaTab3, condicionSeleccionada, "", "", unidadMedida, puertoCOM };
			dataGridViewDisminuir.Rows.Add(filaAumentar);
			actualizarlistBoxValoresMinMax();

			//Actualizar los listboxes
			if (listBoxValoresMinMax.Items.Count != 0)
			{
				listBoxValoresMinMax.SelectedIndex = 0;
			}
		}

		private void listBoxVenCompDisponibles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxMedicionesVentComp.Items.Count != 0)
			{
				actualizarlistBoxValoresMinMax();
			}
		}
		private void buttonEliminarCondicionAumento_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 3;
			if (dataGridViewAumentar.Rows.Count > 0)
			{
				string keyNombreMedicion = dataGridViewAumentar.SelectedRows[0].Cells[0].Value.ToString();
				string condicion = dataGridViewAumentar.SelectedRows[0].Cells[3].Value.ToString();
				DialogResult pregunta = MessageBox.Show("Esta seguro que quiere eliminar la Condicion: " + condicion + ", en la medicion: " + keyNombreMedicion + "?", "Precaucion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (pregunta == DialogResult.Yes)
				{
					dataGridViewAumentar.Rows.Remove(dataGridViewAumentar.SelectedRows[0]);
				}
				actualizarlistBoxValoresMinMax();
			}
			else
			{
				MessageBox.Show("Ninguna Condicion que Borrar");
			}
		}

		private void buttonEliminarCondicionDisminuir_Click(object sender, EventArgs e)
		{
			seHizoUnCambio = 3;
			if (dataGridViewDisminuir.Rows.Count > 0)
			{
				string keyNombreMedicion = dataGridViewDisminuir.SelectedRows[0].Cells[0].Value.ToString();
				string condicion = dataGridViewDisminuir.SelectedRows[0].Cells[3].Value.ToString();
				DialogResult pregunta = MessageBox.Show("Esta seguro que quiere eliminar la Condicion: " + condicion + ", en la medicion: " + keyNombreMedicion + "?", "Precaucion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (pregunta == DialogResult.Yes)
				{
					dataGridViewDisminuir.Rows.Remove(dataGridViewDisminuir.SelectedRows[0]);
				}
				actualizarlistBoxValoresMinMax();
			}
			else
			{
				MessageBox.Show("Ninguna Condicion que Borrar");
			}
		}

		//Aplicar los cambios: pasar la info de los datagridviews 
		bool problemaAlAplicar = false;
		public void buttonAplicarTab3_Click(object sender, EventArgs e)
		{
			double textoTest;
			problemaAlAplicar = false;
			//verificar que tiempoEspera (4) e incremento (5) sean numeros
			foreach (DataGridViewRow fila in dataGridViewAumentar.Rows)
			{
				if (double.TryParse(fila.Cells[4].Value.ToString(), out textoTest) == false)
				{
					MessageBox.Show("El tiempo de espera en " + fila.Cells[0].Value.ToString() + " deber ser un numero");
					problemaAlAplicar = true;
					return;
				}
				if (double.TryParse(fila.Cells[5].Value.ToString(), out textoTest) == false)
				{
					MessageBox.Show("El Incremento en " + fila.Cells[0].Value.ToString() + " deber ser un numero");
					problemaAlAplicar = true;
					return;
				}
			}

			foreach (DataGridViewRow fila in dataGridViewDisminuir.Rows)
			{
				if (double.TryParse(fila.Cells[4].Value.ToString(), out textoTest) == false)
				{
					MessageBox.Show("El tiempo de espera en " + fila.Cells[0].Value.ToString() + " deber ser un numero");
					problemaAlAplicar = true;
					return;
				}
				if (double.TryParse(fila.Cells[5].Value.ToString(), out textoTest) == false)
				{
					MessageBox.Show("El Decremento en " + fila.Cells[0].Value.ToString() + " deber ser un numero");
					problemaAlAplicar = true;
					return;
				}
			}

			//Eliminar la informacion en el diccionario Condiciones, esto para no tener problemas con las keys
			CondicionesClass.CondicionesListaAumentar.Clear();
			CondicionesClass.CondicionesListaDisminuir.Clear();

			//Agrega la informacion de los datagridviews 
			//Llave  : noombreMedicion; Orden: nombreMedicion, Galeria/Compuerta , nombreGaleria, condicion, tiempoEspera, Incremento, UnidadMedida, puertoCOM, Aumentar/Disminuir
			//indices:                       :      0        ,        1          ,      2       ,     3    ,      4      ,     5     ,      6      ,     7      ,         8 
			foreach (DataGridViewRow fila in dataGridViewAumentar.Rows)
			{
				List<string> listaParaDiccionario = new List<string>();
				for (int i = 0; i < fila.Cells.Count; i++)
				{
					listaParaDiccionario.Add(fila.Cells[i].Value.ToString());
				}
				listaParaDiccionario.Add("Aumentar");
				CondicionesClass.CondicionesListaAumentar.Add(listaParaDiccionario);
			}

			foreach (DataGridViewRow fila in dataGridViewDisminuir.Rows)
			{
				List<string> listaParaDiccionario = new List<string>();
				for (int i = 0; i < fila.Cells.Count; i++)
				{
					listaParaDiccionario.Add(fila.Cells[i].Value.ToString());
				}
				listaParaDiccionario.Add("Disminuir");
				CondicionesClass.CondicionesListaDisminuir.Add(listaParaDiccionario);
			}

			//agrega la informacion del diccionarioCondiciones al diccionario propiedades dinamicas del Form1, con la siguiente estructura: CONDITION- del Form1.propiedadesDinamicasComponentes de su respectiva galeria contenedora
			// Elimina todo lo relacionado con CONDITION-
			foreach (string componenteKey in galeriasContenedoras)
			{
				int indiceListaPropDinamicas = 0;
				while (indiceListaPropDinamicas < Form1.propiedadesDinamicasComponentes[componenteKey].Count)
				{
					if (Form1.propiedadesDinamicasComponentes[componenteKey][indiceListaPropDinamicas].Contains("CONDITION-"))
					{
						Form1.propiedadesDinamicasComponentes[componenteKey].RemoveAt(indiceListaPropDinamicas);
						indiceListaPropDinamicas = indiceListaPropDinamicas - 2;
					}
					indiceListaPropDinamicas++;
				}
			}

			//Agrega todo lo relacionado con CONDITION-
			//para Lista Aumentar
			foreach (List<string> ListaAumentar in CondicionesClass.CondicionesListaAumentar)
			{
				//Orden: nombreMedicion, nombreVentiladorCompuerta ,Galeria/Compuerta , nombreGaleria, condicion, tiempoEspera, Incremento, UnidadMedida, puertoCOM, Aumentar/Disminuir
				string conditionVentCompu = "CONDITION-" + string.Join(";", ListaAumentar.ToArray());
				//se agrega la condicion a la respectiva galeria contenedora
				Form1.propiedadesDinamicasComponentes[ListaAumentar[2]].Add(conditionVentCompu);
			}
			//para lista Disminuir
			foreach (List<string> ListaDisminuir in CondicionesClass.CondicionesListaDisminuir)
			{
				//Orden: nombreMedicion, nombreVentiladorCompuerta ,Galeria/Compuerta , nombreGaleria, condicion, tiempoEspera, Incremento, UnidadMedida, puertoCOM, Aumentar/Disminuir
				string conditionVentCompu = "CONDITION-" + string.Join(";", ListaDisminuir.ToArray());
				//se agrega la condicion a la respectiva galeria contenedora
				Form1.propiedadesDinamicasComponentes[ListaDisminuir[2]].Add(conditionVentCompu);
			}

			mostrarOcultarLabel(labelActualizarVentComp);
			seHizoUnCambio = 0;
		}

		// Funcion que Actualiza el valor minimo/maximo desde medicionesClass, para que se actualice si se modifica el valor minimo maximo desde la tab2 solo se puede llamar desde TAB2
		public void actualizarConditionClassDesdeTab2()
		{
			// buscar la llave de los datagridviews en mediccionesClass y actualizar los valores
			//      Para CondicionesListaAumentar
			foreach (string medicionNombreKeyMedicion in MedicionesClass.medicionesDiccionario.Keys)
			{
				foreach (List<string> listaAumentar in CondicionesClass.CondicionesListaAumentar)
				{
					if (medicionNombreKeyMedicion == listaAumentar[0])
					{
						if (listaAumentar[3].Contains("Valor Minimo"))
						{
							listaAumentar[3] = "Valor Minimo : " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][6] + " " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][2];
						}
						else if (listaAumentar[3].Contains("Valor Maximo"))
						{
							listaAumentar[3] = "Valor Maximo : " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][7] + " " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][2];
						}
					}

					// Actualizar la unidad de medida y el puerto COM del ventilador/compuerta -------------------------- Verificar si funciona
					if (listaAumentar[1] == medicionNombreKeyMedicion)
					{
						listaAumentar[6] = MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][2];
						listaAumentar[7] = MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][8];
					}
				}
				//      Para CondicionesListaDisminuir
				foreach (List<string> listaDisminuir in CondicionesClass.CondicionesListaDisminuir)
				{
					if (medicionNombreKeyMedicion == listaDisminuir[0])
					{
						if (listaDisminuir[3].Contains("Valor Minimo"))
						{
							listaDisminuir[3] = "Valor Minimo : " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][6] + " " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][2];
						}
						else if (listaDisminuir[3].Contains("Valor Maximo"))
						{
							listaDisminuir[3] = "Valor Maximo : " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][7] + " " + MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][2];
						}
					}

					// Actualizar la unidad de medida y el puerto COM del ventilador/compuerta -------------------------- Verificar si funciona
					if (listaDisminuir[1] == medicionNombreKeyMedicion)
					{
						listaDisminuir[6] = MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][2];
						listaDisminuir[7] = MedicionesClass.medicionesDiccionario[medicionNombreKeyMedicion][8];
					}
				}

				actualizarDataGridVentCompuerta();
			}
		}



		//leer el diccionario con la informacion de los ventiladores y compuertas y cargarla a los datagridviews
		private void actualizarDataGridVentCompuerta()
		{
			dataGridViewAumentar.Rows.Clear();
			dataGridViewDisminuir.Rows.Clear();
			//Orden:   nombreMedicion, Galeria/Compuerta , nombreGaleria, condicion, tiempoEspera, Incremento, UnidadMedida, puertoCOM, Aumentar/Disminuir
			//indices:      0        ,        1          ,      2       ,     3    ,      4      ,     5     ,      6      ,     7      ,         8 
			foreach (List<string> nombreMedicionKey in CondicionesClass.CondicionesListaAumentar)
			{
				dataGridViewAumentar.Rows.Add(nombreMedicionKey.ToArray());
			}
			foreach (List<string> nombreMedicionKey in CondicionesClass.CondicionesListaDisminuir)
			{
				dataGridViewDisminuir.Rows.Add(nombreMedicionKey.ToArray());
			}

		}


		//---------------------------------------- TAB 4 (Puertos COM)-------------------------------------
		string puertoSeleccionado;
		private void estadoControlesInicial()
		{
			comboBoxProtocolo.Items.Clear();
			comboBoxProtocolo.Items.Add("");
			comboBoxProtocolo.Items.Add("ModBus");
			comboBoxProtocolo.Items.Add("Personalizado");

			textBoxComandoEnviar.Enabled = false;
			textBoxCharSeparatorEnviar.Enabled = false;
			textBoxComandoRecibir.Enabled = false;
			textBoxCharSeparatorRecibir.Enabled = false;

			listBoxPuertos.Items.Clear();
			ArduinoConexion.obtenerPuertoCOM();
			foreach (var puertosActivos in ArduinoConexion.PuertosDisponibles.Keys)
			{
				if (listBoxPuertos.Items.Contains(puertosActivos.Split('-')[0]) == false)
				{
					foreach (string dispConfigKey in MedicionesClass.medicionesDiccionario.Keys)
					{
						if(MedicionesClass.medicionesDiccionario[dispConfigKey][8] == puertosActivos)
						{
							if(listBoxPuertos.Items.Contains(puertosActivos.Split('-')[0]) == false)
							{
								listBoxPuertos.Items.Add(puertosActivos.Split('-')[0]);
							}
							
						}
					}
					
				}
			}
		}

		private void listBoxPuertos_SelectedValueChanged(object sender, EventArgs e)
		{
			//leer el diccionario COM y cargar las configuraciones
			dataGridViewReciben.Rows.Clear();
			dataGridViewEnvian.Rows.Clear();
			if (listBoxPuertos.Items.Count != 0)
			{
				if(listBoxPuertos.SelectedItem != null)
				{
					puertoSeleccionado = listBoxPuertos.SelectedItem.ToString();
					rellenarControles(puertoSeleccionado);
					comboBoxProtocolo.Enabled = true;
				}

			}

			
		}

		private void rellenarControles(string puertoSeleccionadoActual)
		{
			comboBoxProtocolo.Text = "";

			textBoxComandoRecibir.Text = "";
			textBoxCharSeparatorRecibir.Text = "";
			textBoxComandoEnviar.Text = "";
			textBoxCharSeparatorEnviar.Text = "";


			dataGridViewEnvian.Rows.Clear();
			dataGridViewEnvian.Rows.Clear();

			int maximoEnviar = 0;
			int maximoRecibir = 0;


			// Protocolo del Puerto
			// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar
			//                                               :     0    ,           1          ,            2            ,           3         ,           4        
			//key formato: puertoNombre + "-Enviar" o puertoNombre + "-Recibir"
			if (ArduinoConexion.diccionarioConfigsCOM.ContainsKey(puertoSeleccionadoActual + "-Enviar"))
			{
				comboBoxProtocolo.Text = ArduinoConexion.diccionarioConfigsCOM[puertoSeleccionadoActual + "-Enviar"][0];
			}
			if (ArduinoConexion.diccionarioConfigsCOM.ContainsKey(puertoSeleccionadoActual + "-Recibir"))
			{
				comboBoxProtocolo.Text = ArduinoConexion.diccionarioConfigsCOM[puertoSeleccionadoActual + "-Recibir"][0];
			}


			//Maximo de los datagridviews, ve las posiciones de las configuraciones
			List<int> posicionesEnviar = new List<int>();
			List<int> posicionesRecibir = new List<int>();
			Dictionary<int, string> diccionarioEnviar = new Dictionary<int, string>();
			Dictionary<int, string> diccionarioRecibir = new Dictionary<int, string>();

			foreach (string medicionKey in MedicionesClass.medicionesDiccionario.Keys)
			{
				if (puertoSeleccionadoActual + "-Enviar" == MedicionesClass.medicionesDiccionario[medicionKey][8])
				{
					int numeroProvar1;
					bool conversion1 = Int32.TryParse(MedicionesClass.medicionesDiccionario[medicionKey][9], out numeroProvar1);
					if (conversion1)
					{
						posicionesEnviar.Add(numeroProvar1);
						diccionarioEnviar.Add(numeroProvar1, medicionKey);
					}

					int numeroProvar2;
					bool conversion2 = Int32.TryParse(MedicionesClass.medicionesDiccionario[medicionKey][10], out numeroProvar2);
					if (conversion2)
					{
						posicionesEnviar.Add(numeroProvar2);
						diccionarioEnviar.Add(numeroProvar2, medicionKey);

					}

				}
				else if (puertoSeleccionadoActual + "-Recibir" == MedicionesClass.medicionesDiccionario[medicionKey][8])
				{
					int numeroProvar1;
					bool conversion1 = Int32.TryParse(MedicionesClass.medicionesDiccionario[medicionKey][9], out numeroProvar1);
					if (conversion1)
					{
						posicionesRecibir.Add(numeroProvar1);
						diccionarioRecibir.Add(numeroProvar1, medicionKey);

					}

					int numeroProvar2;
					bool conversion2 = Int32.TryParse(MedicionesClass.medicionesDiccionario[medicionKey][10], out numeroProvar2);
					if (conversion2)
					{
						posicionesRecibir.Add(numeroProvar2);
						diccionarioRecibir.Add(numeroProvar2, medicionKey);
					}
				}
			}
			if (posicionesEnviar.Count != 0)
			{
				maximoEnviar = posicionesEnviar.Max();
			}
			if (posicionesRecibir.Count != 0)
			{
				maximoRecibir = posicionesRecibir.Max();
			}

			//Agregar las posiciones al datagridview
			for (int i = 1; i <= posicionesEnviar.Count; i++)
			{
				List<string> fila;
				if (diccionarioEnviar.ContainsKey(i))
				{
					fila = new List<string> { i.ToString(), diccionarioEnviar[i] };
				}
				else
				{
					fila = new List<string> { i.ToString(), "" };
				}
				string[] filaArray = fila.ToArray();
				dataGridViewEnvian.Rows.Add(filaArray);
			}
			for (int i = 1; i <= posicionesRecibir.Count; i++)
			{
				List<string> fila;
				if (diccionarioRecibir.ContainsKey(i))
				{
					fila = new List<string> { i.ToString(), diccionarioRecibir[i] };
				}
				else
				{
					fila = new List<string> { i.ToString(), "" };
				}
				string[] filaArray = fila.ToArray();
				dataGridViewReciben.Rows.Add(filaArray);
			}

			// Rellenar los TextBoxes
			// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar
			//                                               :     0    ,           1          ,            2            ,           3         ,           4        

			if (ArduinoConexion.diccionarioConfigsCOM.ContainsKey(puertoSeleccionadoActual + "-Recibir"))
			{
				textBoxComandoRecibir.Text = ArduinoConexion.diccionarioConfigsCOM[puertoSeleccionadoActual + "-Recibir"][1];
				textBoxCharSeparatorRecibir.Text = ArduinoConexion.diccionarioConfigsCOM[puertoSeleccionadoActual + "-Recibir"][2];
			}
			if (ArduinoConexion.diccionarioConfigsCOM.ContainsKey(puertoSeleccionadoActual + "-Enviar"))
			{
				textBoxComandoEnviar.Text = ArduinoConexion.diccionarioConfigsCOM[puertoSeleccionadoActual + "-Enviar"][3];
				textBoxCharSeparatorEnviar.Text = ArduinoConexion.diccionarioConfigsCOM[puertoSeleccionadoActual + "-Enviar"][4];
			}

		}


		private void comboBoxProtocolo_SelectedValueChanged_1(object sender, EventArgs e)
		{
			seHizoUnCambio = 4;
			textBoxComandoEnviar.Enabled = false;
			textBoxCharSeparatorEnviar.Enabled = false;
			textBoxComandoRecibir.Enabled = false;
			textBoxCharSeparatorRecibir.Enabled = false;

			if (comboBoxProtocolo.Text == "Personalizado")
			{
				textBoxComandoEnviar.Enabled = true;
				textBoxCharSeparatorEnviar.Enabled = true;
				textBoxComandoRecibir.Enabled = true;
				textBoxCharSeparatorRecibir.Enabled = true;
				button1.Enabled = false;
			}
			else if(comboBoxProtocolo.Text == "ModBus")
			{
				button1.Enabled = true;
			}
		}

		// Boton Aceptar
		private void buttonAceptarCOM_Click(object sender, EventArgs e)
		{
			// verificar que se selecciono un puerto
			if (listBoxPuertos.SelectedItem == null)
			{
				MessageBox.Show("Seleccione un Puerto Primero!");
				return;
			}
			// Verifica que se seleccione un protocolo
			if (comboBoxProtocolo.Text == "")
			{
				MessageBox.Show("Seleccione un Protocolo para Seguir.");
				return;
			}
			// verifica que los comandos enviar de entrada sean de 4 letras
			if (comboBoxProtocolo.Text != "ModBus")
			{
				if (textBoxComandoEnviar.Text.Length != 4)
				{
					MessageBox.Show("El comando para enviar la informacion debe ser de 4 caracteres.");
					return;
				}
			}


			string nombrePuerto = listBoxPuertos.SelectedItem.ToString();

			if (comboBoxProtocolo.Text != "ModBus")
			{
				// Verifica que el caracter separator exista cuando hay informacion para enviar o recibir
				if (dataGridViewReciben.Rows.Count != 0)
				{
					if (textBoxCharSeparatorRecibir.Text == "")
					{
						MessageBox.Show("Debe Seleccionar un caracter para Separar la Informacion que se Recibe.");
						return;
					}
				}
				if (dataGridViewAumentar.Rows.Count != 0)
				{
					if (textBoxCharSeparatorEnviar.Text == "")
					{
						MessageBox.Show("Debe Seleccionar un caracter para Separar la Informacion que se Envia.");
						return;
					}
				}

			}

			// Agregar las configuraciones al diccionario
			if(comboBoxProtocolo.Text != "ModBus")
			{
				ActualizarDiccionarioCOM(nombrePuerto, "", "", "", "", "");
			}
			else//si es modbus
			{
				if (ArduinoConexion.diccionarioConfigsCOM.ContainsKey(puertoSeleccionado + "-Enviar"))
				{
					if(ArduinoConexion.diccionarioConfigsCOM[puertoSeleccionado + "-Enviar"][9] == "")
					{
						MessageBox.Show("Por favor terminar de configurar el Puerto" + puertoSeleccionado + " con el protocolo Modbus");
					}
				}
				else
				{
					MessageBox.Show("Por favor terminar de configurar el Puerto" + puertoSeleccionado + " con el protocolo Modbus");
				}
			}
			
			

			// Agrega la informacion por cada galeria al diccionario Form1.propiedadesDinamicasComponentes

			//Elimina todos los puertos que no existan actualmente
			ArduinoConexion.obtenerPuertoCOM();
			List<string> eliminarCOMS = new List<string>();
			foreach (string puertoEnviarRecibirKey in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				if (ArduinoConexion.PuertosDisponibles.ContainsKey(puertoEnviarRecibirKey) == false)
				{
					eliminarCOMS.Add(puertoEnviarRecibirKey);
				}
			}
			foreach (string eliminarCOMSItemKey in eliminarCOMS)
			{
				ArduinoConexion.diccionarioConfigsCOM.Remove(eliminarCOMSItemKey);
			}


			// Elimina todas las configuraciones existentes en el diccionario Form1.propiedadesDinamicasComponentes realcionadas con los puertos COM
			if (Form1.propiedadesDinamicasComponentes.ContainsKey("COMPORTS"))
			{
				Form1.propiedadesDinamicasComponentes["COMPORTS"] = new List<string>();
			}
			else
			{
				Form1.propiedadesDinamicasComponentes.Add("COMPORTS", new List<string>());
			}


			//Agrega las configuraciones de los Puertos al diccionario ArduinoConexion.diccionarioConfigsCOM al diccionario Form1.propiedadesDinamicasComponentes, para que luego se puedan guardar en el txt
			foreach (string nombrePuertoKey in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				string configuracionMedicion = "COMPORT-" + nombrePuertoKey + " ; " + string.Join(" ; ", ArduinoConexion.diccionarioConfigsCOM[nombrePuertoKey].ToArray());
				Form1.propiedadesDinamicasComponentes["COMPORTS"].Add(configuracionMedicion);
			}

			mostrarOcultarLabel(labelActualizarCOM);
			seHizoUnCambio = 0;
			estadoControlesInicial();
			//this.Close();
		}

		public void ActualizarDiccionarioCOM(string nombrePuerto, string idEsclavoModBus, string direccionModBus, string valor1, string direccion2, string valor2)
		{
			// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar
			//                                               :     0    ,           1          ,            2            ,           3         ,           4        
			string protocolo = comboBoxProtocolo.Text;
			string comandoIncialRecibir = textBoxComandoRecibir.Text;
			string caracterRecibir = textBoxCharSeparatorRecibir.Text;
			string comandoIncialEnviar = textBoxComandoEnviar.Text;
			string caracterEnviar = textBoxCharSeparatorEnviar.Text;

			List<string> listaPropCOMs = new List<string> { protocolo, comandoIncialRecibir, caracterRecibir, comandoIncialEnviar, caracterEnviar, idEsclavoModBus, direccionModBus, valor1, direccion2, valor2 };
			if (ArduinoConexion.diccionarioConfigsCOM.ContainsKey(nombrePuerto + "-Enviar") && ArduinoConexion.diccionarioConfigsCOM.ContainsKey(nombrePuerto + "-Recibir"))
			{
				ArduinoConexion.diccionarioConfigsCOM[nombrePuerto + "-Enviar"] = listaPropCOMs;
				ArduinoConexion.diccionarioConfigsCOM[nombrePuerto + "-Recibir"] = listaPropCOMs;
			}
			else
			{
				ArduinoConexion.diccionarioConfigsCOM.Add(nombrePuerto + "-Enviar", listaPropCOMs);
				ArduinoConexion.diccionarioConfigsCOM.Add(nombrePuerto + "-Recibir", listaPropCOMs);
			}
			

		}


		private void buttonActualizarRecibir_Click(object sender, EventArgs e)
		{
			buttonAceptarCOM_Click(sender, e);
		}

		private void buttonActualizarEnviar_Click(object sender, EventArgs e)
		{
			buttonAceptarCOM_Click(sender, e);
		}

		private void buttonIdentificarCOM_Click(object sender, EventArgs e)
		{
			Form_Identificar_COM formCOMIdentificar = new Form_Identificar_COM();
			formCOMIdentificar.Show();
		}
		Form configMOd = null;
		private void button1_Click(object sender, EventArgs e)
		{
			string protocoloSeleccionado = comboBoxProtocolo.Text;
			if (protocoloSeleccionado == "ModBus")
			{
				string puertoActual = listBoxPuertos.SelectedItem.ToString();

				configMOd = new ModBusConfig(this, puertoActual);
				configMOd.Show();
			}
			else
			{
				MessageBox.Show("Para configurar Puerto Modbus debe seleccionar dicho protocolo");
			}
			


		}

	}

}
