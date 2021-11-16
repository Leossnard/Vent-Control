using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Vent_Control
{
	public partial class FormMonitoreo : Form
	{
		Dictionary<string, int> numeroFilasPorTABDiccionario = new Dictionary<string, int>();
		Form1 formulario1_ok;
		public FormMonitoreo(Form1 formulario1)
		{
			InitializeComponent();
			formulario1_ok = formulario1;
		}

		public void agregarColumnas(string IDColumna, string encabezadoColumna, string llaveDatagridView)
		{
			bool stop = false;
			foreach (TabPage tab in tabControl1.TabPages)
			{
				foreach (Control control in tab.Controls)
				{
					if (control.Name == llaveDatagridView.Split('-')[0] && control is DataGridView)
					{
						(control as DataGridView).Columns.Add(IDColumna, encabezadoColumna);
						if(IDColumna == "ColumnaDate")
						{
							(control as DataGridView).Columns[IDColumna].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
						}
						stop = true;
						break;
					}
				}
				if (stop)
				{
					break;
				}
			}
		}
		public void agregarFilaDataGridview(string[] stringArray, string llaveDatagridView)
		{
			bool stop = false;
			foreach (TabPage tab in tabControl1.TabPages)
			{
				foreach (Control control in tab.Controls)
				{
					if (control.Name == llaveDatagridView.Split('-')[0] && control is DataGridView)
					{
						(control as DataGridView).Rows.Add(stringArray);
						if(checkBoxAutoScroll.Checked == true)
						{
							(control as DataGridView).FirstDisplayedScrollingRowIndex = (control as DataGridView).RowCount - 1;
						}
						stop = true;
						break;
					}
				}
				if (stop)
				{
					break;
				}
			}

		}

		public async Task<Tuple<int,DataGridView>> agregarFilaVacia(string llaveDatagridView)
		{
			int numFila = -1;
			foreach (TabPage tab in tabControl1.TabPages)
			{
				foreach (Control control in tab.Controls)
				{
					if (control.Name == llaveDatagridView.Split('-')[0] && control is DataGridView)
					{
						numFila = (control as DataGridView).Rows.Add();
						numeroFilasPorTABDiccionario[control.Name] = numFila + 1;
						if (tabControl1.SelectedTab.Name == llaveDatagridView.Split('-')[0])
						{
							labelNumMediciones.Text = "Medicion Numero: " + (numFila + 1).ToString();
						}
						if (checkBoxAutoScroll.Checked == true)
						{
							(control as DataGridView).FirstDisplayedScrollingRowIndex = (control as DataGridView).RowCount - 1;
						}
						return new Tuple<int,DataGridView>(numFila, (control as DataGridView));
					}
				}
			}
			return new Tuple<int, DataGridView>(numFila, null); ;
		}


		public int numeroFilas(string llaveDatagridView)
		{
			int numeroFilas = 0;
			bool stop = false;
			foreach (TabPage tab in tabControl1.TabPages)
			{
				foreach (Control control in tab.Controls)
				{
					if (control.Name == llaveDatagridView.Split('-')[0] && control is DataGridView)
					{
						numeroFilas = (control as DataGridView).Rows.Count;
						stop = true;
						break;
					}
				}
				if (stop)
				{
					break;
				}
			}
			return numeroFilas;
		}


		public string ValorCelda(string llaveDatagridViewPuerto, string nombreColumna)
		{
			string valorCelda = "ERROR";
			foreach (TabPage tab in tabControl1.TabPages)
			{
				foreach (Control control in tab.Controls)
				{
					if (control.Name == llaveDatagridViewPuerto.Split('-')[0] && control is DataGridView)
					{
						if((control as DataGridView).RowCount > 0)
						{
							//valorCelda = (control as DataGridView).Rows[(control as DataGridView).RowCount - 1].Cells[nombreColumna].Value.ToString();
							valorCelda = (control as DataGridView).Rows[numeroFilasPorTABDiccionario[control.Name]-1].Cells[nombreColumna].Value.ToString();

							return valorCelda;
						}
						else
						{
							return valorCelda;
						}
					}
				}
			}
			return valorCelda;
		}



		//agrega tantas TAB por cada Puerto y un datagridview por TAB
		public void agregarTABconDatasGridViewsporCOM()
		{
			// Ordena los puertos de menor a mayor
			List<int> listaNumerosCOM = new List<int>();
			List<string> COMutilizados = new List<string>();
			foreach (string COMkey in ArduinoConexion.PuertosDisponibles.Keys)
			{
				if (ArduinoConexion.diccionarioConfigsCOM.ContainsKey(COMkey))
				{
					if (COMutilizados.Contains(COMkey.Split('-')[0]) == false)
					{
						bool puertoRecibir = false;
                        // si el puerto esta configurado como Enviar no se agrega ( y si es modbus tampoco)
                        foreach (string KeyMedicion in MedicionesClass.medicionesDiccionario.Keys)
                        {
							// Key:nombreMedicion, Orden: nombreMedicion,magnitudFisica,unidadMedida,galeriaContenedora,componente1,componente2,restriccionMinima,restriccionMaxima,puertoCOM,posicionCOM1,posicionCOM2,funcionMatematica,valorInicial, vacioPorBotonEditar, incremento/decremento Acumulador, optimizacionSalida, tiempoIntervaloOPtimizacion
							//      Key          , Orden:        0      ,      1       ,     2      ,        3         ,     4     ,     5     ,       6         ,         7       ,    8    ,      9     ,    10      ,        11       ,     12     ,         13         ,                 14              ,          15       ,             16
							string puertoConfigurado = MedicionesClass.medicionesDiccionario[KeyMedicion][8];
							string protocolo = ArduinoConexion.diccionarioConfigsCOM[puertoConfigurado][0];

							if (puertoConfigurado == COMkey & puertoConfigurado.Contains("Recibir") & protocolo != "ModBus")
                            {
								puertoRecibir = true;
								break;

							}
                        }

                        if (puertoRecibir)
                        {
							COMutilizados.Add(COMkey.Split('-')[0]);
							int numeroCOM = int.Parse(COMkey.Split('-')[0].Substring(3));
							listaNumerosCOM.Add(numeroCOM);
						}

					}
				}

			}
			listaNumerosCOM.Sort();
			//le da el nombre a la primera TAB y al datagridview (que ya existen)
			if(listaNumerosCOM.Count != 0)
			{
				string IdTAB = "COM" + listaNumerosCOM[0].ToString();
				tabPage1.Name = IdTAB;
				tabPage1.Text = IdTAB;
				dataGridViewMonitor.Name = IdTAB;
				numeroFilasPorTABDiccionario.Add(IdTAB, 0);
			}
			//agrega los demas TAB y les da un nombre, ademas agrega los datagridview por cada TAB nueva
			for (int i = 1; i < listaNumerosCOM.Count; i++)
			{
				TabPage tp = new TabPage("COM" + listaNumerosCOM[i].ToString());
				tp.Name = "COM" + listaNumerosCOM[i].ToString();
				numeroFilasPorTABDiccionario.Add(tp.Name, 0);
				tabControl1.TabPages.Add(tp);
				DataGridView dgv = new DataGridView();
				tp.Controls.Add(dgv);
				dgv.Name = "COM" + listaNumerosCOM[i].ToString();
				dgv.Dock = DockStyle.Fill;
				dgv.AllowUserToAddRows = false;
				dgv.AllowUserToDeleteRows = false;
				dgv.AllowUserToResizeRows = false;
				dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
				dgv.ReadOnly = true;
				dgv.RowHeadersVisible = false;
				dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			}
		}


		private void FormMonitoreo_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

		private void buttonAceptar_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		// Exportar a Excel, para ello se agrega la refencia Microsoft Excel 16.0 object library.
		private void button1_Click(object sender, EventArgs e)
		{
			//parar medicion
			bool seParoLaMedicion = false;
			if (Form1.comenzarLecturas)
			{
				seParoLaMedicion = true;
				formulario1_ok.buttonStop_Click(sender, e);
			}
			// Verificar que existan datos para exportar
			List<DataGridView> listaDatasGridViews = new List<DataGridView>();
			foreach (TabPage tab in tabControl1.TabPages)
			{
				foreach (Control control in tab.Controls.OfType<DataGridView>())
				{
					listaDatasGridViews.Add((control as DataGridView));
				}
			}
			if(listaDatasGridViews.Count == 0)
			{
				MessageBox.Show("No hay datos que exportar","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Information);
				if (seParoLaMedicion)
				{
					formulario1_ok.buttonComenzar_Click(sender, e);
				}
				return;
			}

			// Path para guardar 
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "Excel files(*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx";
			saveFileDialog1.FilterIndex = 2;
			if (saveFileDialog1.ShowDialog() != DialogResult.OK)
			{
				if (seParoLaMedicion)
				{
					formulario1_ok.buttonComenzar_Click(sender, e);
				}
				return;
			}
			string selectedFileName = saveFileDialog1.FileName;
			System.IO.FileInfo fi = new System.IO.FileInfo(selectedFileName);
			string NombreArchivo = fi.Name;

			// Verificar que Excel este Instalado
			Excel.Application xlApp;
			Excel.Workbook xlWorkBook;
			Excel.Worksheet xlWorkSheet;
			Excel.Range xlWorkSheetRange = null;
			xlApp = new Microsoft.Office.Interop.Excel.Application();
			if (xlApp == null)
			{
				
				xlApp = null;
				xlWorkBook = null;
				xlWorkSheet = null;
				if (seParoLaMedicion)
				{
					formulario1_ok.buttonComenzar_Click(sender, e);
				}
				MessageBox.Show("Excel no esta instalado!!");
				return;
			}
			else
			{
				//Codigo nuevo desde aca
				int hWndExcelApp = xlApp.Application.Hwnd;
				object misValue = System.Reflection.Missing.Value;//Representa valores por defecto
				xlWorkBook = xlApp.Workbooks.Add(misValue);
				xlApp.Visible = false;

				xlWorkSheet = xlWorkBook.Worksheets.get_Item(1);

				// crear hojas por cada dataGridView
				if (listaDatasGridViews.Count > 1)
				{
					xlWorkBook.Worksheets.Add(After: xlWorkSheet, Count: listaDatasGridViews.Count - 1);
				}
				//Cambiar nombre a las hojas agregadas
				for (int i = 0; i < listaDatasGridViews.Count; i++)
				{
					xlWorkBook.Worksheets.get_Item(i + 1).Name = listaDatasGridViews[i].Name;
				}

				//Rellenar Datos por hoja
				for (int i = 0; i < listaDatasGridViews.Count; i++)
				{
					
					// Encabezados de las columnas
					// Extraer nombre de las columnas
					List<string> nombreEncabezados = new List<string>();
					foreach (DataGridViewColumn columna in listaDatasGridViews[i].Columns)
					{
						nombreEncabezados.Add(columna.HeaderText.ToString());
					}

					//ingresar lo demas a la hoja
					//Copiar informacion al copia papeles y luego pegarlo al excel
					DataObject dataObj = null;
					Clipboard.Clear();
					listaDatasGridViews[i].ClearSelection();

					listaDatasGridViews[i].SelectAll();
					dataObj = listaDatasGridViews[i].GetClipboardContent();
					if (dataObj != null)
						Clipboard.SetDataObject(dataObj);

					//en excel
					xlWorkSheet = (Excel.Worksheet)xlApp.Worksheets[i + 1];
					xlWorkSheet.Select();

					xlWorkSheetRange = xlWorkSheet.Range["A1"];
					xlWorkSheetRange.Select();
					xlWorkSheet.PasteSpecial(xlWorkSheetRange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
					//xlWorkSheetRange.Cells[1,1].PasteSpecial(xlWorkSheetRange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
					//.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
					//xlWorkSheetRange.PasteSpecial(Excel.XlPasteType.xlPasteValues, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);



					//ingresar nombre de columnas al Excel
					for (int j = 0; j < nombreEncabezados.Count; j++)
					{
						xlWorkSheet.Cells[1, j + 1] = nombreEncabezados[j];
					}
				}

				xlWorkBook.SaveAs(selectedFileName);//, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
				xlWorkBook.Close(true, misValue, misValue);
				xlApp.Quit();

				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlWorkSheetRange);
				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlWorkSheet);
				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlWorkBook);
				System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);

				xlWorkSheet = null;
				xlWorkBook = null;
				xlApp = null;

				KillProcessByMainWindowHwnd(hWndExcelApp);
				if (seParoLaMedicion)
				{
					formulario1_ok.buttonComenzar_Click(sender, e);
				}
				MessageBox.Show("Se Guardo " + NombreArchivo + " Correctamente!", "Exito",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}



		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		public static void KillProcessByMainWindowHwnd(int hWnd)
		{
			uint processID;
			GetWindowThreadProcessId((IntPtr)hWnd, out processID);
			if (processID == 0)
				throw new ArgumentException("Process has not been found by the given main window handle.", "hWnd");
			System.Diagnostics.Process.GetProcessById((int)processID).Kill();
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			labelNumMediciones.Text = "";
		}

		private void FormMonitoreo_Load(object sender, EventArgs e)
		{
			checkBoxAutoScroll.Checked = true;
		}
	}
}
