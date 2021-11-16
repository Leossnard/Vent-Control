using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vent_Control
{
	public partial class Form_Identificar_COM : Form
	{
		bool comenzar = false;
		public Form_Identificar_COM()
		{
			InitializeComponent();
			label1.Text = "Conecte el dispositivo a un puerto USB, enciendalo y luego presione el boton identificar puerto.\nSe le mostrará el puerto de donde proviene la informacion.";
			textBox1.ReadOnly = true;
		}

		SerialPort puertoActivo;

		private async void button1_Click(object sender, EventArgs e)
		{
			ArduinoConexion.obtenerPuertoCOM();
			string puerto = "";
			puerto = await ArduinoConexion.puertoActivoDataAsync();
			if(puerto != "")
			{
				puertoActivo = new SerialPort(puerto, 9600, Parity.None, 8, StopBits.One);
				puertoActivo.Open();//en ese orden
				comenzar = true;
				textBox1.Text = puerto;
				recibirInfoUSB();

			}
		}

		private async void recibirInfoUSB()
		{
			while (comenzar)
			{
				string stringFromUSB = "";
				stringFromUSB = await Task.Run(() => leerUSB());
				listBox1.Items.Insert(0, stringFromUSB);
			}
		}

		private string leerUSB()
		{
			try
			{
				return puertoActivo.ReadLine();
			}
			catch
			{
				return "Arduino Desconectadox!\n";
			}
		}


		private void button2_Click(object sender, EventArgs e)
		{
			comenzar = false;
		}

		private void Form_Identificar_COM_FormClosed(object sender, FormClosedEventArgs e)
		{

			//Cerrar el puerto
			if (comenzar)
			{
				if (puertoActivo.IsOpen)
				{
					puertoActivo.Close();
				}
			}

			comenzar = false;
		}
	}
}
