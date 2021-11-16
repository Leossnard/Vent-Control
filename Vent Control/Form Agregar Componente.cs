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
	public partial class Form_Agregar_Componente : Form
	{
		Form1 formulario1;
		bool rotar90 = false;
		bool contenedorSensores = false;
		public Form_Agregar_Componente(Form1 formulario1)
		{
			InitializeComponent();
			this.formulario1 = formulario1;
			pictureBoxVistaPrevia.SizeMode = PictureBoxSizeMode.Zoom;
		}

		//-------------------------- Funciones ----------------------------------------------------------


		//-------------------------- Eventos ----------------------------------------------------------

		private void Form_Agregar_Componente_Load(object sender, EventArgs e)
		{
			// Borrar todos los text box
			labelNombreSensor.Visible = false;
			textBoxNombreSensor.Visible = false;
			ButtonAgregarComp.Enabled = false;
			checkBoxContenedor.Visible = false;
			comboBoxComponentes.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBoxComponentes.Items.Add("Ventilador");
			comboBoxComponentes.Items.Add("Galeria Abierta");
			comboBoxComponentes.Items.Add("Galeria Abierta Izquierda o Abajo");
			comboBoxComponentes.Items.Add("Galeria Abierta Derecha o Arriba");
			comboBoxComponentes.Items.Add("Galeria Cerrada");
			comboBoxComponentes.Items.Add("Sensor de Presion Estatica");
			comboBoxComponentes.Items.Add("Sensor de Presion Total");
			comboBoxComponentes.Items.Add("Sensor de Presion Diferencial");
			comboBoxComponentes.Items.Add("Sensor de Gas CO2");
			comboBoxComponentes.Items.Add("Sensor de Gas CO");
			comboBoxComponentes.Items.Add("Sensor de Humedad");
			comboBoxComponentes.Items.Add("Sensor de Temperatura");
			comboBoxComponentes.Items.Add("Compuerta");
			comboBoxComponentes.Items.Add("Personalizado");
		}
		Bitmap imagenSeleccionada;
		private void comboBoxComponentes_SelectedValueChanged(object sender, EventArgs e)
		{
			if(comboBoxComponentes.Text == "Personalizado")
			{
				labelNombreSensor.Visible = true;
				textBoxNombreSensor.Visible = true;
			}
			else
			{
				labelNombreSensor.Visible = false;
				textBoxNombreSensor.Visible = false;
			}

			if (comboBoxComponentes.Text.Contains("Galeria"))
			{
				checkBoxContenedor.Visible = true;
			}
			else
			{
				checkBoxContenedor.Visible = false;
			}

			if (comboBoxComponentes.Text != "")
			{
				ButtonAgregarComp.Enabled = true;
			}
			string valorCombobox = comboBoxComponentes.Text;
			
			switch (valorCombobox)
			{
				case "Ventilador":
					imagenSeleccionada = Properties.Resources.Ventilador1;
					break;
				case "Galeria Abierta":
					imagenSeleccionada = Properties.Resources.galeria_abierta;
					break;
				case "Galeria Abierta Izquierda o Abajo":
					imagenSeleccionada = Properties.Resources.galeria_abierta_izq_abajo;
					break;
				case "Galeria Abierta Derecha o Arriba":
					imagenSeleccionada = Properties.Resources.galeria_abierta_der_arriba;
					break;
				case "Galeria Cerrada":
					imagenSeleccionada = Properties.Resources.galeria_cerrada;
					break;
				case "Sensor de Presion Estatica":
					imagenSeleccionada = Properties.Resources.presion_estatica;
					break;
				case "Sensor de Presion Total":
					imagenSeleccionada = Properties.Resources.Presion_total;
					break;
				case "Sensor de Presion Diferencial":
					imagenSeleccionada = Properties.Resources.sensor_Presion_diferencial;
					break;
				case "Sensor de Gas CO2":
					imagenSeleccionada = Properties.Resources.sensor_gas_CO2;
					break;
				case "Sensor de Gas CO":
					imagenSeleccionada = Properties.Resources.sensor_gas_CO;
					break;
				case "Sensor de Humedad":
					imagenSeleccionada = Properties.Resources.sensor_humedad;
					break;
				case "Sensor de Temperatura":
					imagenSeleccionada = Properties.Resources.Sensor_Temp;
					break;
				case "Compuerta":
					imagenSeleccionada = Properties.Resources.compuerta;
					break;
				case "Personalizado":
					imagenSeleccionada = abrirFoto();
					break;
				default:
					imagenSeleccionada = null;
					break;
			}

			// Ingresar la imagen personalizada siempre como horizontal
			if(valorCombobox == "Personalizado")
			{
				radioButtonHor.Checked = true;
				if(imagenSeleccionada == null)
				{
					comboBoxComponentes.SelectedIndex = 0;
				}
			}

			//Rotar la imagen los grados especificados antes de agregar la imagen
			if (imagenSeleccionada != null)
			{
				if (radioButtonVert.Checked)
				{
					imagenSeleccionada.RotateFlip(RotateFlipType.Rotate90FlipY);
				}
			}


			//actualizar el picturebox
			pictureBoxVistaPrevia.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBoxVistaPrevia.Image = imagenSeleccionada;
			
		}

		// Abrir ventana para seleccionar la foto
		string pathImagenSeleccionada = "";
		string NombreImagen = "";

		private Bitmap abrirFoto()
		{
			//deja al usuario seleccionar el archivo
			OpenFileDialog openArchivo = new OpenFileDialog();
			openArchivo.Title = "Selecciona una Imagen para asociar al sensor";
			openArchivo.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.tif;...";
			openArchivo.Multiselect = false;

			if (openArchivo.ShowDialog() == DialogResult.OK)
			{
				pathImagenSeleccionada = openArchivo.FileName;
				System.IO.FileInfo fi = new System.IO.FileInfo(pathImagenSeleccionada);
				NombreImagen = fi.Name;
				return new Bitmap(pathImagenSeleccionada);
			}
			else
			{
				MessageBox.Show("Error al Seleccionar la Imagen.");
				return null;
			}
		}

		private void ButtonAgregarComp_Click(object sender, EventArgs e)
		{
			if(comboBoxComponentes.Text == "Personalizado")
			{
				if (textBoxNombreSensor.Text == "")
				{
					MessageBox.Show("Por favor Ingresar el nombre del Sensor");
					return;
				}
			}
			
			// Agregar el componente seleccionado
			//"VENT","GALO","GALC","PRESSE","PRESST,"GASCO2","GASCO","HUMID","TEMP","COMPUERTA"
			
			if (radioButtonVert.Checked)
			{
				rotar90 = true;
			}
			if (checkBoxContenedor.Checked)
			{
				contenedorSensores = true;
			}
			string valorCombobox = comboBoxComponentes.Text;
			switch (valorCombobox)
			{
				case "Ventilador":
					formulario1.AgregarComponentes("VENT");
					break;
				case "Galeria Abierta":
					formulario1.AgregarComponentes("GALO", rotar90, contenedorSensores);
					break;
				case "Galeria Abierta Izquierda o Abajo":
					formulario1.AgregarComponentes("GALOIA", rotar90, contenedorSensores);
					break;
				case "Galeria Abierta Derecha o Arriba":
					formulario1.AgregarComponentes("GALODA", rotar90, contenedorSensores);
					break;
				case "Galeria Cerrada":
					formulario1.AgregarComponentes("GALC", rotar90, contenedorSensores);
					break;
				case "Sensor de Presion Estatica":
					formulario1.AgregarComponentes("PRESSE");
					break;
				case "Sensor de Presion Total":
					formulario1.AgregarComponentes("PRESST");
					break;
				case "Sensor de Presion Diferencial":
					formulario1.AgregarComponentes("PRESDIF");
					break;
				case "Sensor de Gas CO2":
					formulario1.AgregarComponentes("GASCO2");
					break;
				case "Sensor de Gas CO":
					formulario1.AgregarComponentes("GASCO");
					break;
				case "Sensor de Humedad":
					formulario1.AgregarComponentes("HUMID");
					break;
				case "Sensor de Temperatura":
					formulario1.AgregarComponentes("TEMP");
					break;
				case "Compuerta":
					formulario1.AgregarComponentes("COMPUERTA");
					break;
				case "Personalizado":
					formulario1.AgregarComponentes("CUSTOM-" + textBoxNombreSensor.Text, rotar90,false,false, pathImagenSeleccionada);
					break;
				default:
					MessageBox.Show("No se encontro el componente a agregar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}
			this.Hide();

		}

		private void radioButtonVert_CheckedChanged(object sender, EventArgs e)
		{
			if (imagenSeleccionada != null)
			{
				if(comboBoxComponentes.Text == "Personalizado")
				{
					if (radioButtonVert.Checked)
					{
						imagenSeleccionada.RotateFlip(RotateFlipType.Rotate90FlipY);
					}
					else
					{
						imagenSeleccionada.RotateFlip(RotateFlipType.Rotate90FlipY);
					}
					pictureBoxVistaPrevia.Image = imagenSeleccionada;
				}
				else
				{
					comboBoxComponentes_SelectedValueChanged(sender, e);
				}
			}
		}
	}
}
