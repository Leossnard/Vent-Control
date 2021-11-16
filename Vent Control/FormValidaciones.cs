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
	public partial class FormValidaciones : Form
	{
		Form1 fomulario1;
		public FormValidaciones(Form1 form1Entrada)
		{
			InitializeComponent();
			fomulario1 = form1Entrada;
		}

		public void agregarComponentesEntrada()//y salida igual xd
		{
			int contadorColumnas = 0;
			DataGridViewComboBoxColumn columnaCombobox = new DataGridViewComboBoxColumn();
			DataGridViewTextBoxColumn columnaIntervalo = new DataGridViewTextBoxColumn();
			foreach (string keymedicion in MedicionesClass.medicionesDiccionario.Keys)
			{
				if (MedicionesClass.medicionesDiccionario[keymedicion][8].Contains("-Recibir"))
				{
					string min = MedicionesClass.medicionesDiccionario[keymedicion][6];
					string max = MedicionesClass.medicionesDiccionario[keymedicion][7];

					List<string> filaTemp = new List<string> {keymedicion,min,max,"","",""};
					if(filaTemp.Count != 0)
					{
						dataGridViewMonitoreo.Rows.Add(filaTemp.ToArray());
					}
				}
				else
				{
					string[] filaTemp = new string[] { keymedicion };
					int fila = dataGridViewOptimizacion.Rows.Add(filaTemp);
					//dataGridViewMonitoreo.Rows[fila].Cells[1].Value = MedicionesClass.medicionesDiccionario[keymedicion][15];
					if(contadorColumnas == 0)
					{
						
						columnaCombobox.Items.Add("");
						columnaCombobox.Items.Add("Maximizar");
						columnaCombobox.Items.Add("Minimizar");
						columnaCombobox.Name = "Columna-Optimizar";
						columnaCombobox.HeaderText = "Optimizacion";
						dataGridViewOptimizacion.Columns.Add(columnaCombobox);
					}

					dataGridViewOptimizacion.Rows[fila].Cells[columnaCombobox.Name].Value = MedicionesClass.medicionesDiccionario[keymedicion][15];//optmizacion salida
					//agregar la tercera columna
					if(contadorColumnas == 0)
					{
						columnaIntervalo.Name = "Columna-Intervalo";
						columnaIntervalo.HeaderText = "Verificacion Cada Segundos";
						dataGridViewOptimizacion.Columns.Add(columnaIntervalo);
					}
					dataGridViewOptimizacion.Rows[fila].Cells[columnaIntervalo.Name].Value = MedicionesClass.medicionesDiccionario[keymedicion][16];//intervalo de tiempo para actualizacion
					contadorColumnas++;
				}
			}
		}
		
		public void actualizarMedicion(string keymedicion, string valorActual, string AumentarMensaje, string disminuirMensaje)
		{
			for (int i = 0; i < dataGridViewMonitoreo.Rows.Count; i++)
			{
				if(dataGridViewMonitoreo.Rows[i].Cells[0].Value.ToString() == keymedicion)
				{
					dataGridViewMonitoreo.Rows[i].Cells[3].Value = valorActual;
					if(AumentarMensaje != "")
					{
						dataGridViewMonitoreo.Rows[i].Cells[4].Value = AumentarMensaje;
					}
					if(disminuirMensaje != "")
					{
						dataGridViewMonitoreo.Rows[i].Cells[5].Value = disminuirMensaje;
					}
				}
			}
		}

		private void buttonAplicar_Click(object sender, EventArgs e)
		{
			int intTemp;
			for (int i = 0; i < dataGridViewOptimizacion.Rows.Count; i++)
			{
				string llave = dataGridViewOptimizacion.Rows[i].Cells[0].Value.ToString();
				string valorCelda = "";
				if (dataGridViewOptimizacion.Rows[i].Cells[1].Value != null)
				{
					valorCelda = dataGridViewOptimizacion.Rows[i].Cells[1].Value.ToString();
				}
				string valorCelda3 = dataGridViewOptimizacion.Rows[i].Cells[2].Value.ToString();
				//verificar que sean numeros enteros en la fila 3
				if (int.TryParse(valorCelda3,out intTemp) == false)
				{
					MessageBox.Show("Introducir un Intervalo de Espera entero Positivo");
					return;
				}

				MedicionesClass.medicionesDiccionario[llave][15] = valorCelda;
				MedicionesClass.medicionesDiccionario[llave][16] = valorCelda3;

				//identificar posicion en Form1.propiedadesDinamicasComponentes
				string galeriaContenedora = MedicionesClass.medicionesDiccionario[llave][3];
				for (int indice = 0; indice < Form1.propiedadesDinamicasComponentes[galeriaContenedora].Count; indice++)
				{
					if (Form1.propiedadesDinamicasComponentes[galeriaContenedora][indice].Contains("CONFIG-" + llave))
					{
						List<string> listaTemp = Form1.propiedadesDinamicasComponentes[galeriaContenedora][indice].Split(';').ToList();
						listaTemp[15] = valorCelda;
						listaTemp[16] = valorCelda3;
						Form1.propiedadesDinamicasComponentes[galeriaContenedora][indice] = string.Join(";", listaTemp.ToArray());
						break;
					}
				}
			}
			//ejecutar los timers
			fomulario1.crearTimerSalidas();
			if (fomulario1.modoManualButtonEstado())
			{
				fomulario1.activarTimerSalidas(true);
			}

		}

		public Dictionary<string,List<string>> obtenerConfiguraciones()
		{
			Dictionary<string, List<string>> diccionarioTemp = new Dictionary<string, List<string>>();
			for (int i = 0; i < dataGridViewOptimizacion.Rows.Count; i++)
			{
				if(dataGridViewOptimizacion.Rows[i].Cells[1].Value != null)
				{
					diccionarioTemp.Add(dataGridViewOptimizacion.Rows[i].Cells[0].Value.ToString(),new List<string> { dataGridViewOptimizacion.Rows[i].Cells[1].Value.ToString(), dataGridViewOptimizacion.Rows[i].Cells[2].Value.ToString() });
				}
				else
				{
					diccionarioTemp.Add(dataGridViewOptimizacion.Rows[i].Cells[0].Value.ToString(), new List<string> { "", "" });
				}

			}
			return diccionarioTemp;
		}


		private void buttonAceptarVal_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void FormValidaciones_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}
	}


}
