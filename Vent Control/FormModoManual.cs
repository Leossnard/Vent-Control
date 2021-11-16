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
	public partial class FormModoManual : Form
	{
		Dictionary<string, List<Control>> controlesComponentesSalidaDiccionario = new Dictionary<string, List<Control>>();
		Form1 Formulario1;
		public FormModoManual(Form1 form1Entrada)
		{
			InitializeComponent();
			Formulario1 = form1Entrada;
			agregarComponentesdeSalida();

		}

		private void agregarComponentesdeSalida()
		{
			foreach (string llave in MedicionesClass.medicionesDiccionario.Keys)
			{
				if (MedicionesClass.medicionesDiccionario[llave][8].Contains("-Enviar"))
				{
					agregarHerramientaAlPanel(llave);
				}
			}
		}


		private void agregarHerramientaAlPanel(string keyComponenteSalida)
		{
			//todos los controles que se agregan
			Panel panelContenedor = new Panel();
			flowLayoutPanel1.Controls.Add(panelContenedor);
			// los demas controles
			TrackBar trackBarSalida = new TrackBar();
			trackBarSalida.Width = 600;
			TextBox texboxValorActual = new TextBox();
			Label nombreComponente = new Label();
			Label minComponente = new Label();
			Label maxComponente = new Label();
			Label unidadComponente = new Label();
			// crear lista y agregarla al diccionario global
			List<Control> controlesTemp = new List<Control> {panelContenedor, trackBarSalida, texboxValorActual, nombreComponente,minComponente,maxComponente, unidadComponente };
			controlesComponentesSalidaDiccionario.Add(keyComponenteSalida, controlesTemp);
			//agregar componentes al panel
			for (int i = 1; i < controlesTemp.Count; i++)
			{
				panelContenedor.Controls.Add(controlesTemp[i]);
			}
			//ubicacion componentes
			panelContenedor.Width = flowLayoutPanel1.Width - 10;
			trackBarSalida.Location = new Point(20, 40);
			texboxValorActual.Location = new Point(trackBarSalida.Left +trackBarSalida.Width + 15, trackBarSalida.Top + 5); // ver como se ve
			texboxValorActual.Width = 60;
			nombreComponente.Location = new Point(6, 10);
			minComponente.Location = new Point(trackBarSalida.Left, trackBarSalida.Top + trackBarSalida.Height);
			maxComponente.Location = new Point(trackBarSalida.Left + trackBarSalida.Width -25, trackBarSalida.Top + trackBarSalida.Height);
			unidadComponente.Location = new Point(texboxValorActual.Left + texboxValorActual.Width + 5, texboxValorActual.Top +3);
			//propiedades componentes
			trackBarSalida.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			nombreComponente.Text = keyComponenteSalida;
			nombreComponente.BorderStyle = BorderStyle.FixedSingle;
			unidadComponente.Text = MedicionesClass.medicionesDiccionario[keyComponenteSalida][2];
			string minimo = MedicionesClass.medicionesDiccionario[keyComponenteSalida][6];
			string maximo = MedicionesClass.medicionesDiccionario[keyComponenteSalida][7];
			minComponente.Text = minimo + " " + unidadComponente.Text;
			maxComponente.Text = maximo + " " + unidadComponente.Text;
			trackBarSalida.Minimum = int.Parse(minimo);
			trackBarSalida.Maximum = int.Parse(maximo);
			//Valores Iniciales
			trackBarSalida.Value = int.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][12]) + int.Parse(MedicionesClass.medicionesDiccionario[keyComponenteSalida][14]);
			texboxValorActual.Text = trackBarSalida.Value.ToString();

			trackBarSalida.SmallChange = 10;

			//--------------------------------------       Eventos      -----------------------------------------------------------
			trackBarSalida.Scroll += (sender, e) => 
			{
				texboxValorActual.Text = trackBarSalida.Value.ToString();
				Formulario1.enviarComandoSegunPosicionyPuerto(keyComponenteSalida,double.Parse(texboxValorActual.Text));

			};
			texboxValorActual.KeyDown += (sender, e) => 
			{
				if(e.KeyCode == Keys.Enter)//si se presiona la tecla enter y el textbox tiene el foco
				{
					//verificar que sea un numero entero
					int numeroTemp;
					if(int.TryParse(texboxValorActual.Text, out numeroTemp))
					{
						//verificar que este dentro de los valores min y max
						if(numeroTemp >= int.Parse(minimo) && numeroTemp <= int.Parse(maximo))
						{
							trackBarSalida.Value = int.Parse(texboxValorActual.Text);
							Formulario1.enviarComandoSegunPosicionyPuerto(keyComponenteSalida, double.Parse(texboxValorActual.Text));
						}
						else
						{
							texboxValorActual.Text = trackBarSalida.Value.ToString();
							MessageBox.Show("Ingrese un Numero que se encuentre entre el Minimo y Maximo permitidos para el Componente");
						}
					}
					else
					{
						texboxValorActual.Text = trackBarSalida.Value.ToString();
						MessageBox.Show("Ingrese un Numero Valido!");
					}

				}


			};
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{

		}
	}
}
