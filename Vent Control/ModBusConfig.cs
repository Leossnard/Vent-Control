using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Vent_Control
{
	public partial class ModBusConfig : Form
	{
		Configuraciones FormConfiguraciones;
		ModBusRTU libreriaModBus;
		string puertoActualAca;


		public ModBusConfig(Configuraciones FormConfiguracionesInput, string puertoActual)
		{
			InitializeComponent();
			FormConfiguraciones = FormConfiguracionesInput;
			puertoActualAca = puertoActual;
			comboBoxPuerto.Items.Add(puertoActualAca);
			libreriaModBus = new ModBusRTU();
			textBoxValor2.Enabled = false;


		}

		// boton aceptar
		private void buttonAceptarMOD_Click(object sender, EventArgs e)
		{
			if(dataGridResumenMOD.Rows.Count == 0)
			{
				MessageBox.Show("Primero Actualizar los valores!");
				return;
			}
			// Actualizar la informacion
			// obtener el puerto actual desde el combobox
			// string puertoActual = comboBoxPuerto.Text;

			foreach (DataGridViewRow filadatagrid in dataGridResumenMOD.Rows)
			{
				// columnas: Actuador, Puerto COM, ID Esclavo, Direccion, Valor Actual
				string puerto = filadatagrid.Cells[1].Value.ToString();
				string idEsclavo = filadatagrid.Cells[2].Value.ToString();
				string direccion1 = filadatagrid.Cells[3].Value.ToString();
				string valor1 = filadatagrid.Cells[4].Value.ToString();
				string dir2 = filadatagrid.Cells[5].Value.ToString();
				string val2 = filadatagrid.Cells[6].Value.ToString();

				FormConfiguraciones.ActualizarDiccionarioCOM(puerto, idEsclavo, direccion1,valor1, dir2, val2);
			}

			this.Close();




		}
		//boton actualizar
		private void buttonEscribir_Click(object sender, EventArgs e)
		{
			// Actualiza el datagridview segun la informacion de los textboxes actuales y agrega la info actual si es que el datagrid esta vacio

			//convertir a int y si no es posible mandar error.
			List<TextBox> texboxesActuales = new List<TextBox>() { textBoxDireccion, textBoxEsclavo, textBoxDirre2 };
			foreach (TextBox textboxelemento in texboxesActuales)
			{
				int numero;
				if(Int32.TryParse(textboxelemento.Text, out numero) == false)
				{
					MessageBox.Show("Ingrese solo valores enteros");
					return;
				}
			}

	
			//buscar si el el puerto existe en el datagridview, si existe lo modifica
			foreach (DataGridViewRow filaDataGrid in dataGridResumenMOD.Rows)
			{
				
				// columnas: Actuador, Puerto COM, ID Esclavo, Direccion power, Valor Actual power, direccion velocidad, valor actual velocidad
				if (filaDataGrid.Cells[1].Value.ToString() == comboBoxPuerto.Text)
				{
					filaDataGrid.Cells[2].Value = textBoxEsclavo.Text;
					filaDataGrid.Cells[3].Value = textBoxDireccion.Text;
					filaDataGrid.Cells[4].Value = textBoxValor.Text;
					filaDataGrid.Cells[5].Value = textBoxDirre2.Text;
					filaDataGrid.Cells[6].Value = textBoxValor2.Text;
				}
			}
			//actualizar datagridview
			ActualizarDatagrid();

		}

		//--------------------------------------------------- evento al cargar el form -----------------------------
		private void ModBusConfig_Load(object sender, EventArgs e)
		{
			comboBoxPuerto.Text = puertoActualAca;
			comboBoxPuerto.Enabled = false;
			textBoxValor.Enabled = false;
			//string puertoActual = puertoActualAca;

			//rellena la infromacion actual a las textboxes
			foreach (string puertoKey in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				// Diccionario con las configuraciones
				// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion, valor,dir2,val2
				//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6    ,   7  ,  8 ,  9
				if(puertoKey == puertoActualAca + "-Enviar")
				{
					textBoxEsclavo.Text = ArduinoConexion.diccionarioConfigsCOM[puertoKey][5];
					textBoxDireccion.Text = ArduinoConexion.diccionarioConfigsCOM[puertoKey][6];
					textBoxValor.Text = ArduinoConexion.diccionarioConfigsCOM[puertoKey][7];//actualizar en un futuro
					textBoxDirre2.Text = ArduinoConexion.diccionarioConfigsCOM[puertoKey][8];
					textBoxValor2.Text = ArduinoConexion.diccionarioConfigsCOM[puertoKey][9];//actualizar en un futuro
				}

				
			}


			
			//agregrar la informacion de los actuadores modbus YA CONFIGURADOS al datagridview
			ActualizarDatagrid();

		}

		//funcion que actualiza el datagridview segun el valor en la clase 
		async void ActualizarDatagrid()
		{
			bool existe = false;
			foreach (string comKey in ArduinoConexion.diccionarioConfigsCOM.Keys)
			{
				if(comKey == puertoActualAca + "-Enviar")
				{
					// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion
					//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6    
					//si es distinto de vacio es por que es un puerto configurado como modbus
					if (ArduinoConexion.diccionarioConfigsCOM[comKey][5] != "")
					{
						existe = true;
					}
				}
			}


			foreach (string medicionKey in MedicionesClass.medicionesDiccionario.Keys)
			{

				// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2,funcionMatematica,valorInicial, vacioPorBotonEditar, incremento/decremento Acumulador, optimizacionSalida, tiempoIntervaloOPtimizacion
				//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10      ,        11       ,     12     ,         13         ,                 14              ,          15       ,             16
				if (MedicionesClass.medicionesDiccionario[medicionKey][8] == puertoActualAca + "-Enviar")
				{
					// columnas: Actuador, Puerto COM, ID Esclavo, Direccion, Valor Actual
					string nombreMedicion = MedicionesClass.medicionesDiccionario[medicionKey][0];
					
					string IDEsclavo;
					string direccionTabla;
					string valorActualTabla;
					string direccion2;
					string valor2;


					if (existe)
					{
						IDEsclavo = ArduinoConexion.diccionarioConfigsCOM[puertoActualAca + "-Enviar"][5];
						direccionTabla = ArduinoConexion.diccionarioConfigsCOM[puertoActualAca + "-Enviar"][6];
						direccion2 = ArduinoConexion.diccionarioConfigsCOM[puertoActualAca + "-Enviar"][8];
					}
					else
					{
						IDEsclavo =  textBoxEsclavo.Text;
						direccionTabla = textBoxDireccion.Text;
						valorActualTabla = textBoxValor.Text;
						direccion2 = textBoxDirre2.Text;
						valor2 = textBoxValor2.Text;
						
					}
					//si no hay valor en direccion no se puede obtener ninguna respuesta por lo que se sale
					if(textBoxDireccion.Text == "")
					{
						return;
					}
				
					SerialPort serialPortCOM = new SerialPort(puertoActualAca, 19200, Parity.None, 8, StopBits.One);
					serialPortCOM.Open();

					//funcion arroja dos arrays de bytes el primero con la peticion enviada al dispositivo y el segundo con la respuesta de este
					Tuple<byte[], byte[]> tuplaRespuesta = await libreriaModBus.ReadHoldingRegisters(int.Parse(IDEsclavo), int.Parse(direccionTabla), 1, serialPortCOM);//(IDEsclavo, direccionTabla, 1, 180, serialPortCOM);
					Tuple<byte[], byte[]> tuplaRespuesta2 = await libreriaModBus.ReadHoldingRegisters(int.Parse(IDEsclavo), int.Parse(direccion2), 1, serialPortCOM);//(IDEsclavo, direccionTabla, 1, 180, serialPortCOM);

					serialPortCOM.Close();
					string respuesta = libreriaModBus.leerRegistros(tuplaRespuesta)[0];
					string respuesta2 = libreriaModBus.leerRegistros(tuplaRespuesta2)[0];

					if (respuesta == "-99")
					{
						textBoxValor.Text = "";
					}
					else
					{
						textBoxValor.Text = respuesta;
					}

					if(respuesta2 == "-99")
					{
						textBoxValor2.Text = "";
					}
					else
					{
						textBoxValor2.Text = respuesta2;
					}


					//si no hay filas se agrega, si hay se modifica
					if (dataGridResumenMOD.RowCount == 0)
					{
						//actualiza al valor actual del dispositivo modbus
						string[] filaModBus = new string[7] { nombreMedicion, puertoActualAca, IDEsclavo, direccionTabla, respuesta, direccion2, respuesta2 };
						dataGridResumenMOD.Rows.Add(filaModBus);
					}
					else//se modifica
					{
						foreach (DataGridViewRow filaDataGrid in dataGridResumenMOD.Rows)
						{
							// Diccionario con las configuraciones
							// Key: NombrePuertoCOM (enviar o recibir); Lista: protocolo, comandoInicialRecibir, caracterSeparadorRecibir, comandoInicialEnviar, caracterSeparadorEnviar, id_esclavo, direccion, valor,dir2,val2
							//                                               :     0    ,           1          ,            2            ,           3         ,           4            ,      5    ,     6    ,   7  ,  8 ,  9

							// columnas: Actuador, Puerto COM, ID Esclavo, Direccion power, Valor Actual power, dir2, val2
							if (filaDataGrid.Cells[1].Value.ToString() == comboBoxPuerto.Text)
							{

								filaDataGrid.Cells[2].Value = textBoxEsclavo.Text;
								filaDataGrid.Cells[3].Value = textBoxDireccion.Text;
								filaDataGrid.Cells[4].Value = respuesta;
								filaDataGrid.Cells[5].Value = textBoxDirre2.Text;
								filaDataGrid.Cells[6].Value = respuesta2;

							}
						}
					}
					

					
					
				}

			}
		}


	}
}
