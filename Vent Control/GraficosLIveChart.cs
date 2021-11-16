using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Reflection;
using System.Windows.Controls;
using Separator = LiveCharts.Wpf.Separator;
using LiveCharts.Wpf.Charts.Base;
using System.Data.Common;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Drawing.Text;

namespace Vent_Control
{
    //Falta exportar informacion a Excel y hacer que se coloque el nombre de la hora actual y fecha.    
    
    public partial class Graficos : Form
    {
        Estadisticas MyEstadisticas = new Estadisticas();
        
        Form1 Formulario1;
        List<string> soloIDcomponentes;

        public Graficos(Form1 Formulario1, List<string> soloIDcomponentes)
        {
            InitializeComponent();
            dgDatosSectores.DoubleBuffered(true);
            this.Icon = Properties.Resources.IconoGraficos;
            this.Formulario1 = Formulario1;
            this.soloIDcomponentes = soloIDcomponentes;
            CantidadSectores(Componentes());
            CondicionesIniciales();
            metodoInicialGraficos(10);            
            Graficar_Componentes();
            CambiarTamañoInicial();

        }
        public void CondicionesIniciales()
        {

            dgDatosSectores.RowHeadersVisible = false;
            cartesianChart1.Visible = false;
            cGlobal1.Visible = false;            
            cGlobal4.Visible = false;            
            cGlobal3.Visible = false;
            cGlobal2.Visible = false;
            dgDatosSectores.Columns.Add("Hora", "Hora");
            dgDatosSectores.Columns[0].Visible = false; //ACA HACERLA INVISIBLE*********************************************
            toolStripButton2.Enabled = false; //Lo dejo inutil jejeje
            toolStripButton1.Enabled = false; //Lo dejo inutil jejeje
            btnIniciarCaptura.Enabled = false;

        }

        public void metodoInicialGraficos(int stepInicial)
        {
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                DisableAnimations = true,
                Title = "Horas",
                LabelFormatter = value => new System.DateTime((long)value).ToString("hh:mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(stepInicial).Ticks
                }
            });
        }

        public ChartValues<valoresSensores> ChartValues { get; set; }
        public ChartValues<valoresSensores> ChartValues1 { get; set; }
        public ChartValues<valoresSensores> ChartValues2 { get; set; }
        public ChartValues<valoresSensores> ChartValues3 { get; set; }    

        public void SetAxisLimits(System.DateTime now,long y) //Funcion que setea los limites en el eje x de la grafica en tiempo real
        {
            try
            {
                cartesianChart1.AxisX[0].MaxValue = now.Ticks; //+ TimeSpan.FromSeconds(2).Ticks; lo borre porque no me gustaba visualmente
                cartesianChart1.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(y).Ticks;
            }
            catch{ }
        }     

        public List<string[]> Componentes() //Manipula Lista de soloIDcomponentes y devuelve una lista sin el -
        {
            List<string[]> ComponentePorSector = new List<string[]>();
            for (int i = 0; i < soloIDcomponentes.Count; i++)
            {
                soloIDcomponentes[i] = soloIDcomponentes[i].Replace("COMPONENT-", "");
            }
            soloIDcomponentes = soloIDcomponentes.OrderBy(x => x).ToList();
            for (int i = 0; i < soloIDcomponentes.Count; i++)
            {
                if (soloIDcomponentes[i].Contains("VENT") || soloIDcomponentes[i].Contains("PRESSE") || soloIDcomponentes[i].Contains("PRESST") ||
                    soloIDcomponentes[i].Contains("GASCO2") || soloIDcomponentes[i].Contains("GASCO") || soloIDcomponentes[i].Contains("HUMID") ||
                    soloIDcomponentes[i].Contains("TEMP")||soloIDcomponentes[i].Contains("CUSTOM"))
                    ComponentePorSector.Add(soloIDcomponentes[i].Split('-'));
            }
            return ComponentePorSector;
        }
        public void CantidadSectores(List<string[]> Componentes) //solucionado lo del custom 
        {
            try 
            {
                List<int> sectorComponente = new List<int>();
                foreach(string[] instrumento in Componentes)
                {
                    sectorComponente.Add(Convert.ToInt32(instrumento[instrumento.Length-1]));                     
                }
                int maximoSector = sectorComponente.Max(x => Convert.ToInt32(x));
                for (int x = 0; x < maximoSector; x++)
                {
                    cbSectores.Items.Add("Sector"+"-" + Convert.ToString(x + 1));
                }
            }
            catch
            {
                MessageBox.Show("No se han agregado componentes", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public string seleccionIndiceSectores { get; set; }
        public void EscribirComponentes_Sectores_DataGrid(List<string[]> Componentes, System.Windows.Forms.ComboBox Sector) //arreglarlo y eliminar boton
        {
            List<string> Lista_KeyInstrumentos = new List<string>();
            if (cbSectores.SelectedItem.ToString().Contains('*') == false & MedicionesClass.medicionesDiccionario.Keys.Count > 0)
            {
                foreach (string Key_Instrumentos in MedicionesClass.medicionesDiccionario.Keys)
                {
                    if (Lista_KeyInstrumentos.Contains(Key_Instrumentos) == false)
                        Lista_KeyInstrumentos.Add(Key_Instrumentos);
                }
                Lista_KeyInstrumentos = Lista_KeyInstrumentos.OrderBy(x => x).ToList();
                foreach (string Key_Instrumentos in Lista_KeyInstrumentos)
                {
                    string Header_UltimaColumna = Key_Instrumentos + " - " + cbSectores.SelectedItem.ToString()[cbSectores.SelectedItem.ToString().Length - 1];
                    if (dgDatosSectores.Columns.Contains(Key_Instrumentos) == false & (MedicionesClass.medicionesDiccionario[Key_Instrumentos][4])[MedicionesClass.medicionesDiccionario[Key_Instrumentos][4].Length - 1] == cbSectores.SelectedItem.ToString()[cbSectores.SelectedItem.ToString().Length - 1])
                    {
                        dgDatosSectores.Columns.Add(Key_Instrumentos, Header_UltimaColumna);
                    }
                    else if (dgDatosSectores.Columns.Contains(Key_Instrumentos) == true & (MedicionesClass.medicionesDiccionario[Key_Instrumentos][4])[MedicionesClass.medicionesDiccionario[Key_Instrumentos][4].Length - 1] == cbSectores.SelectedItem.ToString()[cbSectores.SelectedItem.ToString().Length - 1])
                        dgDatosSectores.Columns[Key_Instrumentos].Visible = true;
                }
                Sector.Items[cbSectores.SelectedIndex] = Convert.ToString(Sector.Items[cbSectores.SelectedIndex]) + '*'; //no agregar el * si no hay elementos en el datagrid de ese sector
            }
            else if (cbSectores.SelectedItem.ToString().Contains('*') == true)
            {                
                foreach (DataGridViewColumn columna in dgDatosSectores.Columns)
                {                    
                    if (cbSectores.SelectedItem.ToString()[cbSectores.SelectedItem.ToString().Length - 2] == columna.HeaderText[columna.HeaderText.Length - 1]) //es menos dos por el *               
                    {
                        try
                        {
                            for(int x=0;x<cartesianChart1.Series.Count;x++)
                            {
                                MessageBox.Show(cartesianChart1.Series[x].Title);
                                if (cartesianChart1.Series[x].Title == columna.HeaderText) 
                                {
                                    cartesianChart1.Series.RemoveAt(x);
                                    doble_click = (x+1);
                                }                                      
                            }
                            columna.Visible = false;                              
                        }
                        catch
                        { }
                    }
                }
                Sector.Items[cbSectores.SelectedIndex] = cbSectores.SelectedItem.ToString().Replace("*", "");
                int cantidad_visibles = 0;
                foreach(string item in cbSectores.Items)
                {
                    if (item.Contains("*"))
                        cantidad_visibles += 1;                    
                }
                if (cantidad_visibles == 0)
                {
                    btnIniciarCaptura.Enabled = false;
                    btnIniciarCaptura.Text = "Iniciar";
                    tmAgregarDatos.Stop();
                    timer1.Interval = 1000000000; //Lo cambiare aca 
                    tmrGlobales.Interval = 1000000000;
                    toolStripButton2.Enabled = false;
                    toolStripButton1.Enabled = false;
                }

                if (cartesianChart1.Series.Count == 0)
                {                    
                    cartesianChart1.Visible = false;
                    cartesianChart1.AxisY.Clear();
                }                                    
            }
            else if (MedicionesClass.medicionesDiccionario.Keys.Count == 0)
            {
                MessageBox.Show("No se han agregado componentes en ninguna sección");
            }
        }
        public void DatosArduino(List<string[]> Componentes, System.Windows.Forms.ComboBox Sector)
        {
            //Hay que revisar a conectar sensores para ver si todo va bien
            if (MedicionesClass.medicionesDiccionario.Keys.Count > 0)
            {
                foreach (string Key_InstrumentosActuales in MedicionesClass.medicionesDiccionario.Keys)
                {
                    if (dgDatosSectores.Columns.Contains(Key_InstrumentosActuales))
                    {
                        int posicion_Coincidencia = dgDatosSectores.Columns[Key_InstrumentosActuales].Index;
                        dgDatosSectores.Rows[0].Cells[posicion_Coincidencia].Value = Convert.ToString(Formulario1.medicionActual(Key_InstrumentosActuales));
                    }
                }
            }
        }
        public void Graficar_Componentes() //Esta funcion se esta inciiando al abrir el formulario actual 
        {
            var mapper = Mappers.Xy<valoresSensores>()
                .X(model => model.DateTime.Ticks) //Puede que aca este el problema, escribe los valores en x en funcion dE LA WEA 
                .Y(model => model.Value);      
            Charting.For<valoresSensores>(mapper); 
            ChartValues = new ChartValues<valoresSensores>(); //Valores para el grafico1_1
            ChartValues1 = new ChartValues<valoresSensores>(); //Valores para el grafico2_2
            ChartValues2 = new ChartValues<valoresSensores>(); // Valores para el grafico3_3
            ChartValues3 = new ChartValues<valoresSensores>(); //Valores para el grafico4_4
            cartesianChart1.LegendLocation = LiveCharts.LegendLocation.Bottom;
            timer1.Interval = 2000; //cambie aca 
            timer1.Tick += TimerOnTick2;//ACA SE INGRESOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOSKEEEEEEEEEEEEEERE
            timer1.Start(); //El timer 1 es el que realiza todo el cuento cada vez que hace tick 
            tmrGlobales.Start(); //Echa a andar el timer de los globales (estaticos) 
        }

        public void Grafico1_1() //Funcion que Agrega Grafico 1_1 En tiempo Real
        {
            cartesianChart1.Series.Add(new LineSeries //añade la siguiente serie 
            {
                Title = dgDatosSectores.Columns[Grafico_1].HeaderText, //El titulo del grafico sera el headerText del datagridview
                Values = ChartValues, //Losvalores seran los de chartvalues
                PointGeometry = DefaultGeometries.Square,
                PointGeometrySize = 10,
                StrokeThickness = 3
              
            });            
        }

        public void Grafico2_2()
        {
            cartesianChart1.Series.Add(new LineSeries
            {
                Title = dgDatosSectores.Columns[Grafico_2].HeaderText,
                Values = ChartValues1,
                PointGeometry = DefaultGeometries.Cross,
                PointGeometrySize = 10,
                StrokeThickness = 3
            });
        }

        public void Grafico3_3()
        {
            cartesianChart1.Series.Add(new LineSeries
            {
                Title = dgDatosSectores.Columns[Grafico_3].HeaderText,
                Values = ChartValues2,
                PointGeometry = DefaultGeometries.Diamond,
                PointGeometrySize = 10,
                StrokeThickness = 3
            });
        }

        public void Grafico4_4()
        {
            cartesianChart1.Series.Add(new LineSeries
            {
                Title = dgDatosSectores.Columns[Grafico_4].HeaderText,
                Values = ChartValues3,
                PointGeometry = DefaultGeometries.None,
                PointGeometrySize = 10,
                StrokeThickness = 3
            });
        }
        //ACA TERMINAN LAS FUNCIONES PARA LOS GRAFICOS EN TIEMPO REAL

        private void tmAgregarDatos_Tick(object sender, EventArgs e)//timer que agrega los datos al dataGridView
        { //Cada vez que este timer haga tick agrega datos al datagridview
            try
            {
                dgDatosSectores.Rows.Insert(0);
                DatosArduino(Componentes(), cbSectores);
            }
            catch (Exception)
            {
                tmAgregarDatos.Stop(); //Si no puede agregarlos lo detiene 
                MessageBox.Show("No existen sectores añadidos al Data Grid View", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIniciarCaptura.Text = "Iniciar";
            }
        }

        private void btnVaciarDataGrid_Click(object sender, EventArgs e) //Boton que vacia todo lo del datagridView
        {
            tmAgregarDatos.Stop();
            timer1.Interval = 1000000000; //Lo cambiare aca 
            tmrGlobales.Interval = 1000000000;//estaba en 1000000000
            btnIniciarCaptura.Text = "Iniciar";
            btnIniciarCaptura.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton1.Enabled = false;
            doble_click = 1;
            GraficosDisponibles_Globales.Clear();
            dgDatosSectores.Rows.Clear();
            dgDatosSectores.Columns.Clear();
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisY.Clear();
            PrimeroGlobal5 = "vacio";
            PrimeroGlobal6 = "vacio";
            PrimeroGlobal7 = "vacio";
            PrimeroGlobal8 = "vacio";
            //Sector para ocultar graficos
            cartesianChart1.Visible = false;
            cGlobal1.Visible = false;
            cGlobal4.Visible = false;
            cGlobal3.Visible = false;
            cGlobal2.Visible = false;
            //Fin sector ocultar graficos

            for (int x = 0; x < cbSectores.Items.Count; x++) //Les quito el * para que deseleccione todo 
            {
                cbSectores.Items[x] = cbSectores.Items[x].ToString().Replace("*", "");
            }                         
        }

        private void btnIniciarCaptura_Click(object sender, EventArgs e) 
        {

            if (btnIniciarCaptura.Text == "Iniciar")
            {
                tmAgregarDatos.Interval = 1000; //timer que agrega los datos al dataGridView               
                tmAgregarDatos.Start(); //Lo echa a andar 
                timer1.Interval = 2000; //Timer de los estaticos ( esta funcionando bien) 
                tmrGlobales.Interval = 1000; 
                btnIniciarCaptura.Text = "Detener";
            }

            else if (btnIniciarCaptura.Text == "Detener")
            {
                tmAgregarDatos.Stop();
                timer1.Interval = 1000000000; //Lo cambiare aca 
                tmrGlobales.Interval = 1000000000;//estaba en 1000000000
                btnIniciarCaptura.Text = "Iniciar";
            }
            toolStripButton2.Enabled = true; //Lo Habilito
            toolStripButton1.Enabled = true;
        }

        private void btnSeleccionar_Click(object sender, EventArgs e) //Solo escribe en el data grid view los componentes inscritos
        {
            if (dgDatosSectores.Columns.Count == 0) //Si no hay columnas en el data grid producto de vaciar entonces al seleccionar se agregar inmediatamente la columna hora antes de todo
            {
                dgDatosSectores.Columns.Add("Hora", "Hora");
            }
            dgDatosSectores.Columns[0].Visible = false;
            EscribirComponentes_Sectores_DataGrid(Componentes(), cbSectores); //se escriben los componentes en el data grid; nada mas 
            foreach(DataGridViewColumn columna in dgDatosSectores.Columns)
            {
                if (columna.Visible == true &&columna.Name!= "Hora") //se activa el boton iniciar solo si hay una columna visible y que la columna no sea la columna hora
                {
                    btnIniciarCaptura.Enabled = true;
                    break;
                }               
            }
        }
        
        public void TimerOnTick2(object sender, EventArgs eventArgs ) 
        {
            List<ChartValues<valoresSensores>> ListaCharts = new List<ChartValues<valoresSensores>>();            
            System.DateTime now = System.DateTime.Now;//.ToString(); //Aca esta el error segun entiendo
            try
            {

                ChartValues.Add(new valoresSensores
                {
                    DateTime = now,
                    Value = Convert.ToDouble(dgDatosSectores.Rows[0].Cells[Grafico_1].Value),
                });

                ChartValues1.Add(new valoresSensores
                {
                    DateTime = now,
                    Value = Convert.ToDouble(dgDatosSectores.Rows[0].Cells[Grafico_2].Value)
                });
                ChartValues2.Add(new valoresSensores
                {
                    DateTime = now,
                    Value = Convert.ToDouble(dgDatosSectores.Rows[0].Cells[Grafico_3].Value)
                });
                ChartValues3.Add(new valoresSensores
                {
                    DateTime = now,
                    Value = Convert.ToDouble(dgDatosSectores.Rows[0].Cells[Grafico_4].Value)
                });
                ListaCharts.Add(ChartValues);
                ListaCharts.Add(ChartValues1);
                ListaCharts.Add(ChartValues2);
                ListaCharts.Add(ChartValues3);
                foreach (ChartValues<valoresSensores> charts in ListaCharts) //algo aca me esta borrando cosas que no debe 
                {
                    if (charts.Count > (y / 2)+1)
                    {
                        int x = 0;
                        while(charts.Count > (y / 2)+1)
                        {
                            charts.RemoveAt(x); //creo que funciona bien                             
                        }
                    }       
                }
                SetAxisLimits(now, y); //lo puse aca asi no reestablece como enfermo el eje X;
            }
            catch  {}
        }

        public string Eliminar { get; set; } = "Indiferente";

        private void dgDatosSectores_MouseEnter(object sender, EventArgs e)
        {
            if (Eliminar == "Desactivado")
                dgDatosSectores.Cursor = Cursors.Hand;
            else if (Eliminar == "Activado")
            {
                dgDatosSectores.Cursor = new System.Windows.Forms.Cursor(Properties.Resources.deleteCursor.Handle);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Eliminar = "Activado";
        }

        private void dgDatosSectores_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            dgDatosSectores.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        List<int> ListaEliminados = new List<int>();
        public int Grafico_1 { get; set; }
        public int Grafico_2 { get; set; }
        public int Grafico_3 { get; set; }
        public int Grafico_4 { get; set; }
        public int doble_click { get; set; } = 1;
        public int posicion_eliminar { get; set; }

        private void dgDatosSectores_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) 
        {
            List<string> GraficosPresentes = new List<string>(); //Me crea una lista para determinar los graficos presentes 
            string ejey="";
            if (dgDatosSectores.RowCount > 1) // Si existe mas de una fila empieza 
            {
                if (dgDatosSectores.Cursor == Cursors.Hand) // si el cursor es igual a la mano entonces entra 
                {
                    if (ListaEliminados.Count > 0) // Si hay mas de uno en la lista de eliminados, la lista de eliminados agrega el numero de click 
                    {
                        for (int x = 0; x < ListaEliminados.Count; x++) //Revisa los componentes de la lista eliminados 
                        {
                            if (ListaEliminados[x] == 1) //Si el item es igual a 1 quiere decir que es el primero grafico
                            {
                                doble_click = 1; //Defien a doble click en 1 
                                ListaEliminados.RemoveAt(x); //Remueve el x seleccionado
                                break; //Sale
                            }
                            else if (ListaEliminados[x] == 2)
                            {
                                doble_click = 2;
                                ListaEliminados.RemoveAt(x);
                                break;
                            }
                            else if (ListaEliminados[x] == 3)
                            {
                                doble_click = 3;
                                ListaEliminados.RemoveAt(x);
                                break;
                            }
                            else if (ListaEliminados[x] == 4)
                            {
                                doble_click = 4;
                                ListaEliminados.RemoveAt(x);
                                break;
                            }
                        }
                        //Termina funcion para eliminar 
                    }

                    for (int x = 0; x < cartesianChart1.Series.Count; x++) //para cada serie en el liveChart
                    {                        
                        GraficosPresentes.Add(cartesianChart1.Series[x].Title); //Agrega el titulo de cada serie en la lista, el titulo es el headertext                         
                    }
                    cartesianChart1.Visible = true; //Visualiza el cartesianChart1 de tiempo real
                    //lblTiempoReal.Visible = true; //Visualiza el nombre de los graficos en tiempo real segun lo de abajo

                    if (doble_click == 1 && cartesianChart1.Series.Count < 4) //Define el nombre del grafico en funcion del primer doble_click
                    {
                        Grafico_1 = dgDatosSectores.CurrentCell.ColumnIndex;
                        if (MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Contains("VENT"))//dgDatosSectores.Columns[Grafico_1].Name.Contains("VENT")) //No lo va a contener porque no son del mismo nombre, seria entrar en class mediciones 
                            ejey = "Velocidad en ";
                        else if (MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Contains("PRESST"))//dgDatosSectores.Columns[Grafico_1].Name.Contains("PRESST"))
                            ejey = "Presión en ";
                        else if (MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Contains("GASO2") || MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Contains("GASCO2"))
                            ejey= "Concentración en ";
                        else if (MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Contains("TEMP"))
                            ejey = "Temperatura en ";
                        else if (MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Contains("HUMID"))
                            ejey = "Humedad en "; //Hasta aca Funciona
                        //DEFINIR GRAFICOS PARA CAUDAL Y VELOCIDAD 

                        if (GraficosPresentes.Contains(dgDatosSectores.Columns[Grafico_1].HeaderText) == false)
                        {
                            Grafico1_1(); //Ejecuta la funcion para el primer grafico real
                            string key = dgDatosSectores.Columns[Grafico_1].HeaderText.Split('-')[0];
                            key = key.Remove(key.Length - 1);
                            ejey = ejey+MedicionesClass.medicionesDiccionario[key][2];
                            cartesianChart1.AxisY.Add(new LiveCharts.Wpf.Axis
                            {
                                Title = ejey //aca insertar la weaita en funcion de la unidad 
                            });                            
                            
                            GraficosDisponibles_Globales.Add(Grafico_1); //Añade el indice 
                            GraficosGlobales(); //Ejecuta GraficosGlobales
                            doble_click = 2;
                        }
                        else
                            ListaEliminados.Add(1);
                    }
                    else if (doble_click == 2 && cartesianChart1.Series.Count < 4)
                    {
                        Grafico_2 = dgDatosSectores.CurrentCell.ColumnIndex; //posicion del grafico 2 en el data grid view 
                        if (Grafico_2 != Grafico_1 && GraficosPresentes.Contains(dgDatosSectores.Columns[Grafico_2].HeaderText) == false && MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_2].Name][4].Split('-')[1]== MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Split('-')[1])//dgDatosSectores.Columns[Grafico_2].Name.Split(' '))[0]== (dgDatosSectores.Columns[Grafico_1].Name.Split(' '))[0])
                        {
                            Grafico2_2();
                            GraficosDisponibles_Globales.Add(Grafico_2);
                            GraficosGlobales();
                            doble_click = 3;
                        }
                        else
                            ListaEliminados.Add(2);
                    }
                    else if (doble_click == 3 && cartesianChart1.Series.Count < 4)
                    {
                        Grafico_3 = dgDatosSectores.CurrentCell.ColumnIndex;
                        if (Grafico_3 != Grafico_2 && Grafico_3 != Grafico_1 && GraficosPresentes.Contains(dgDatosSectores.Columns[Grafico_3].HeaderText) == false && MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_3].Name][4].Split('-')[1] == MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Split('-')[1])
                        {
                            Grafico3_3();
                            GraficosDisponibles_Globales.Add(Grafico_3);
                            GraficosGlobales();
                            doble_click =4;
                        }
                        else
                            ListaEliminados.Add(3);
                    }
                    else if (doble_click == 4 && cartesianChart1.Series.Count < 4)
                    {
                        Grafico_4 = dgDatosSectores.CurrentCell.ColumnIndex;
                        if (Grafico_4 != Grafico_3 && Grafico_4 != Grafico_2 && Grafico_4 != Grafico_1 && GraficosPresentes.Contains(dgDatosSectores.Columns[Grafico_4].HeaderText) == false && MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_3].Name][4].Split('-')[1] == MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Grafico_1].Name][4].Split('-')[1])
                        {
                            Grafico4_4();
                            GraficosDisponibles_Globales.Add(Grafico_4);
                            GraficosGlobales();
                            doble_click = 5;
                        }
                        else
                            ListaEliminados.Add(4);
                    }
                    else if (cartesianChart1.Series.Count >= 4)
                        MessageBox.Show("Se alcanzo el limite de gráficos en simultaneo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (dgDatosSectores.Cursor != Cursors.Hand)
                {
                    posicion_eliminar = dgDatosSectores.CurrentCell.ColumnIndex;
                    if (posicion_eliminar == Grafico_1)
                    {
                        for (int x = 0; x < cartesianChart1.Series.Count; x++)
                        {
                            if (cartesianChart1.Series[x].Title == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                            {                                
                                cartesianChart1.Series.RemoveAt(x);
                                ListaEliminados.Add(1);
                                if (cartesianChart1.Series.Count == 0)
                                {
                                    cartesianChart1.Visible = false;
                                    cartesianChart1.AxisY.Clear();
                                    doble_click = 1;
                                }
                            }
                            try
                            {
                                if (cGlobal1.Series[x].Name == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                                {
                                    cGlobal1.Series.RemoveAt(x); 
                                }
                            }
                            catch { }
                        }
                        for (int y = 0; y < GraficosDisponibles_Globales.Count; y++)
                        {
                            if (posicion_eliminar == GraficosDisponibles_Globales[y])
                            {
                                GraficosDisponibles_Globales.RemoveAt(y);
                                break;
                            }
                        }
                        GraficosGlobales();
                    }

                    else if (posicion_eliminar == Grafico_2)
                    {
                        for (int x = 0; x < cartesianChart1.Series.Count; x++)
                        {
                            if (cartesianChart1.Series[x].Title == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                            {
                                cartesianChart1.Series.RemoveAt(x);
                                ListaEliminados.Add(2);
                                if (cartesianChart1.Series.Count == 0)
                                {
                                    cartesianChart1.Visible = false;
                                    cartesianChart1.AxisY.Clear();                                    
                                    doble_click = 2;
                                }
                            }
                            try
                            {
                                if (cGlobal1.Series[x].Name == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                                {
                                    cGlobal1.Series.RemoveAt(x); 
                                }
                            }
                            catch { }
                            
                        }
                        for (int y = 0; y < GraficosDisponibles_Globales.Count; y++)
                        {
                            if (posicion_eliminar == GraficosDisponibles_Globales[y])
                            {
                                GraficosDisponibles_Globales.RemoveAt(y);
                                break;
                            }

                        }
                        GraficosGlobales();
                    }

                    else if (posicion_eliminar == Grafico_3)
                    {
                        for (int x = 0; x < cartesianChart1.Series.Count; x++)
                        {
                            if (cartesianChart1.Series[x].Title == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                            {
                                cartesianChart1.Series.RemoveAt(x);
                                ListaEliminados.Add(3);
                                if (cartesianChart1.Series.Count == 0)
                                {
                                    cartesianChart1.Visible = false;
                                    cartesianChart1.AxisY.Clear();
                                    doble_click = 3;

                                }
                            }
                            try
                            {
                                if (cGlobal1.Series[x].Name == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                                {
                                    cGlobal1.Series.RemoveAt(x); 
                                }
                            }
                            catch { }                            
                        }
                        for (int y = 0; y < GraficosDisponibles_Globales.Count; y++)
                        {
                            if (posicion_eliminar == GraficosDisponibles_Globales[y])
                            {
                                GraficosDisponibles_Globales.RemoveAt(y);
                                MessageBox.Show(Convert.ToString(GraficosDisponibles_Globales.Count));
                                break;
                            }
                        }
                        GraficosGlobales();
                    }

                    else if (posicion_eliminar == Grafico_4)
                    {
                        for (int x = 0; x < cartesianChart1.Series.Count; x++)
                        {
                            if (cartesianChart1.Series[x].Title == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                            {
                                cartesianChart1.Series.RemoveAt(x);
                                ListaEliminados.Add(4);
                                if (cartesianChart1.Series.Count == 0)
                                {
                                    cartesianChart1.Visible = false;
                                    cartesianChart1.AxisY.Clear();
                                    doble_click = 4;
                                }
                            }
                            try
                            {
                                if (cGlobal1.Series[x].Name == dgDatosSectores.Columns[posicion_eliminar].HeaderText)
                                {
                                    cGlobal1.Series.RemoveAt(x); 
                                }
                            }
                            catch { }
                            
                        }
                        for (int y = 0; y < GraficosDisponibles_Globales.Count; y++)
                        {
                            if (posicion_eliminar == GraficosDisponibles_Globales[y])
                            {
                                GraficosDisponibles_Globales.RemoveAt(y);
                                MessageBox.Show(Convert.ToString(GraficosDisponibles_Globales.Count));
                                break;
                            }
                        }
                        GraficosGlobales();
                    }
                }
            }
            else
                MessageBox.Show("No existen datos a graficar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            VerificarVisibilidad_GraficosEstaticos(tecla_down, tecla_up);
        }
       

        public string PrimeroGlobal1 { get; set; } //Define el tipo de grafico que almacenara el Global 1
        public string PrimeroGlobal2 { get; set; } //Define el tipo de grafico que almacenara el Global 2
        public string PrimeroGlobal3 { get; set; } //Define el tipo de grafico que almacenara el Global 3
        public string PrimeroGlobal4 { get; set; } //Define el tipo de grafico que almacenara el Global 4

        //***************************************ACA CODIGO DE GRAFICOS ESTATICOS******************************************************************
        public List<string> nombreGrafico (int Indice) //Esta función solo define el nombre de los distintos graficos estáticos presentes
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];            
            string unidadMedida= MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][2];
            List <string>nombreGrafico = new List<string>();
            if (componentePosicionGaleria == "GASCO2")
            {
                nombreGrafico.Add("Gráfico Global de Dióxido de Carbono");
                nombreGrafico.Add("Concentración en "+unidadMedida);
            }
            else if (componentePosicionGaleria == "GASO2") //Hay que cambiar este porque sera monoxido de carbono
            {
                nombreGrafico.Add("Gráfico Global de Monóxido de Carbono");
                nombreGrafico.Add("Concentración en "+unidadMedida);
            }
            else if (componentePosicionGaleria == "TEMP")
            {
                nombreGrafico.Add("Gráfico Global de Temperatura");
                nombreGrafico.Add("Temperatura en "+unidadMedida);
            }
            else if (componentePosicionGaleria == "PRESST")
            {
                nombreGrafico.Add("Gráfico Global de Presión Total");
                nombreGrafico.Add("Presión en " + unidadMedida);
            }
            else if (componentePosicionGaleria == "PRESSE")
            {
                nombreGrafico.Add("Gráfico Global de Presión Estática");
                nombreGrafico.Add("Presión en " + unidadMedida);
            }                
            else if (componentePosicionGaleria == "HUMID")
            {
                nombreGrafico.Add("Gráfico Global de Humedad");
                nombreGrafico.Add("Humedad en " + unidadMedida);
            }                
            return nombreGrafico;
        }

        public void graficoGlobal1( int Indice,List<string> horas)
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if (cGlobal1.Series.Count < 4)
            {
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext
                    
                    if (cGlobal1.Series.Count == 0 ) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        if (tecla_up == true)
                        {
                            PrimeroGlobal1 = (componentePosicionGaleria);
                            PrimeroGlobal1 += ("-" + Convert.ToString(Indice));
                        }
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal1.Series.Add(Global);
                        ser1.ChartArea = cGlobal1.ChartAreas[0].Name;                        
                        ser1.Name = Global;
                        cGlobal1.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal1.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal1.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if(cGlobal1.Series.Count>0 && cGlobal1.Series.Count<4 && PrimeroGlobal1.Split('-')[0] == componentePosicionGaleria.Split('-')[0])//Vuelvo e definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal1.Series.Add(Global);
                        ser1.ChartArea = cGlobal1.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }
                }
                catch {}                             
            }
        }        
        public void graficoGlobal2(int Indice, List<string> horas) 
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if (cGlobal2.Series.Count < 4 && componentePosicionGaleria!=PrimeroGlobal1.Split('-')[0]) //Limito altiro que no debe ser igual al parametro del primer global
            {                                
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext

                    if (cGlobal2.Series.Count == 0) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        if (tecla_up == true)
                        {
                            PrimeroGlobal2 = (componentePosicionGaleria);
                            PrimeroGlobal2 += ("-" + Convert.ToString(Indice));
                        }
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal2.Series.Add(Global);
                        ser1.ChartArea = cGlobal2.ChartAreas[0].Name;
                        ser1.Name = Global;
                        cGlobal2.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal2.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal2.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if (cGlobal2.Series.Count > 0 && cGlobal2.Series.Count < 4 && PrimeroGlobal2.Split('-')[0] == componentePosicionGaleria.Split('-')[0])//Vuelvo a definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal2.Series.Add(Global);
                        ser1.ChartArea = cGlobal2.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }
                }
                catch { }
            }
        }
        public void graficoGlobal3(int Indice, List<string> horas) 
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if (cGlobal3.Series.Count < 4 && (componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0])) //Limito altiro que no debe ser igual al parametro del primer global
            {
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext
                    if (cGlobal3.Series.Count == 0 ) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        if (tecla_up == true)
                        {
                            PrimeroGlobal3 = (componentePosicionGaleria);
                            PrimeroGlobal3 += ("-" + Convert.ToString(Indice));
                        }
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal3.Series.Add(Global);
                        ser1.ChartArea = cGlobal3.ChartAreas[0].Name;
                        ser1.Name = Global;
                        cGlobal3.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal3.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal3.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if (cGlobal3.Series.Count > 0 && cGlobal3.Series.Count < 4 && PrimeroGlobal3.Split('-')[0] == componentePosicionGaleria.Split('-')[0])//Vuelvo a definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal3.Series.Add(Global);
                        ser1.ChartArea = cGlobal3.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }                   
                }
                catch { }
            }
        }
        public void graficoGlobal4(int Indice, List<string> horas) 
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if (cGlobal4.Series.Count < 4 && (componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0])) //Limito altiro que no debe ser igual al parametro del primer global
            {
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext

                    if (cGlobal4.Series.Count == 0 ) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        if (tecla_up == true)
                        {
                            PrimeroGlobal4 = (componentePosicionGaleria);
                            PrimeroGlobal4 += ("-" + Convert.ToString(Indice));
                        }                     
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal4.Series.Add(Global);
                        ser1.ChartArea = cGlobal4.ChartAreas[0].Name;
                        ser1.Name = Global;
                        cGlobal4.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal4.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal4.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if (cGlobal4.Series.Count > 0 && cGlobal4.Series.Count < 4&&PrimeroGlobal4.Split('-')[0]==componentePosicionGaleria.Split('-')[0])//Vuelvo a definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal4.Series.Add(Global);
                        ser1.ChartArea = cGlobal4.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }
                }
                catch { }
            }
        }

        //*******************************************************************GRÁFICOS ALTERNOS*************************************************************************************** 
        public void graficoGlobal5(int Indice, List<string> horas)
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if (cGlobal1.Series.Count < 4 && (componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0]))
            {
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext

                    if (cGlobal1.Series.Count == 0) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal1.Series.Add(Global);
                        ser1.ChartArea = cGlobal1.ChartAreas[0].Name;
                        ser1.Name = Global;
                        cGlobal1.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal1.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;

                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal1.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if (cGlobal1.Series.Count > 0 && cGlobal1.Series.Count < 4 && PrimeroGlobal5.Split('-')[0] == componentePosicionGaleria.Split('-')[0] && tecla_up == false)//Vuelvo e definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal1.Series.Add(Global);
                        ser1.ChartArea = cGlobal1.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }
                }
                catch { }
            }
        }
        public void graficoGlobal6(int Indice, List<string> horas)
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if (cGlobal2.Series.Count < 4 && (componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal5.Split('-')[0])) //Limito altiro que no debe ser igual al parametro del primer global
            {
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext

                    if (cGlobal2.Series.Count == 0) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal2.Series.Add(Global);
                        ser1.ChartArea = cGlobal2.ChartAreas[0].Name;
                        ser1.Name = Global;
                        cGlobal2.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal2.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal2.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if (cGlobal2.Series.Count > 0 && cGlobal2.Series.Count < 4 && PrimeroGlobal6.Split('-')[0] == componentePosicionGaleria.Split('-')[0] && tecla_up == false)//Vuelvo a definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal2.Series.Add(Global);
                        ser1.ChartArea = cGlobal2.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }
                }
                catch { }
            }
        }

        public void graficoGlobal7(int Indice, List<string> horas)
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if (cGlobal3.Series.Count < 4 && (componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal5.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal6.Split('-')[0]))
            {
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext

                    if (cGlobal3.Series.Count == 0) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal3.Series.Add(Global);
                        ser1.ChartArea = cGlobal3.ChartAreas[0].Name;
                        ser1.Name = Global;
                        cGlobal3.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal3.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal3.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if (cGlobal3.Series.Count > 0 && cGlobal3.Series.Count < 4 && PrimeroGlobal7.Split('-')[0] == componentePosicionGaleria.Split('-')[0] && tecla_up == false)//Vuelvo a definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal3.Series.Add(Global);
                        ser1.ChartArea = cGlobal3.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }
                }
                catch { }
            }
        }

        public void graficoGlobal8(int Indice, List<string> horas)
        {
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];            
            if (cGlobal4.Series.Count < 4 && (componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal5.Split('-')[0]) 
                && (componentePosicionGaleria != PrimeroGlobal6.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal7.Split('-')[0]))
            {
                List<double> DatosGlobales = new List<double>();
                for (int x = 0; x < dgDatosSectores.Rows.Count; x++)
                {
                    DatosGlobales.Add(Convert.ToDouble(dgDatosSectores.Rows[x].Cells[(Indice)].Value));
                }
                DatosGlobales.Reverse();
                try
                {
                    string Global = dgDatosSectores.Columns[Indice].HeaderText; // Estaba en Name pero lo dejare en headertext

                    if (cGlobal4.Series.Count == 0) //Si no hay ningun serie entonces defino el nombre y hago que coincida con los demas
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal4.Series.Add(Global);
                        ser1.ChartArea = cGlobal4.ChartAreas[0].Name;
                        ser1.Name = Global;
                        cGlobal4.Titles.Clear(); //Borro los nombres anteriores
                        cGlobal4.Titles.Add(nombreGrafico(Indice)[0]); //Defino el titulo del grafico en funcion del parametro ingresado
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                        cGlobal4.ChartAreas[0].AxisY.Title = nombreGrafico(Indice)[1];
                    }
                    else if (cGlobal4.Series.Count > 0 && cGlobal4.Series.Count < 4 && PrimeroGlobal8.Split('-')[0] == componentePosicionGaleria.Split('-')[0] && tecla_up == false)//Vuelvo a definir que debe ser menor a 4 por seguridad
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series ser1 = cGlobal4.Series.Add(Global);
                        ser1.ChartArea = cGlobal4.ChartAreas[0].Name;
                        ser1.Name = Global;
                        ser1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        ser1.BorderWidth = 2;
                        for (int x = 0; x < (horas.Count); x++)
                        {
                            ser1.Points.AddXY(horas[x], DatosGlobales[x]);
                        }
                    }
                }
                catch { }
            }
        }

        public string PrimeroGlobal5 { get; set; } = "vacio";
        public string PrimeroGlobal6 { get; set; } = "vacio";
        public string PrimeroGlobal7 { get; set; } = "vacio";
        public string PrimeroGlobal8 { get; set; } = "vacio";

        public void GraficosReserva(int Indice)
        {            
            string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[Indice].Name][4].Split('-')[1];
            if ((componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0])&&PrimeroGlobal5=="vacio")
            {
                PrimeroGlobal5 = componentePosicionGaleria;
                PrimeroGlobal5 += "-" + Convert.ToString(Indice);                
            }    
            else if((componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0]) 
                && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0])&&(componentePosicionGaleria != PrimeroGlobal5.Split('-')[0]) && PrimeroGlobal6 == "vacio")
            {
                PrimeroGlobal6 = componentePosicionGaleria;
                PrimeroGlobal6 += "-" + Convert.ToString(Indice);
            }
            else if ((componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal5.Split('-')[0])&& (componentePosicionGaleria != PrimeroGlobal6.Split('-')[0])&& PrimeroGlobal7 == "vacio")
            {
                PrimeroGlobal7 = componentePosicionGaleria;
                PrimeroGlobal7 += "-" + Convert.ToString(Indice);
            }
            else if ((componentePosicionGaleria != PrimeroGlobal1.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal2.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal3.Split('-')[0])
                && (componentePosicionGaleria != PrimeroGlobal4.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal5.Split('-')[0]) && (componentePosicionGaleria != PrimeroGlobal6.Split('-')[0]) 
                && (componentePosicionGaleria != PrimeroGlobal6.Split('-')[0]) && PrimeroGlobal8 == "vacio")
            {
                PrimeroGlobal8 = componentePosicionGaleria;
                PrimeroGlobal8 += "-" + Convert.ToString(Indice);
            }
        }

        //****************************************************************************Inicio Estadisticas******************************************************************************************** 
        public void Estadisticas(System.Windows.Forms.DataVisualization.Charting.Chart grafico)
        {
            //la key la tengo del cGlobal1.series[x].Name
            //El Yvalues es un dataPoint y me entrega valor por valor de la weaita men, confia en Dios, lo mas grande wacho, mas que maradona
            //La lista horas es una lista global que se creo hace cualquier tiempo y es lo mejor que pude haber hecho 
            
            MyEstadisticas.tbEstadisticas.Clear(); //Limpio la ventana para cada vez que entre este limpia y no muestre informacion anterior 
            foreach(System.Windows.Forms.DataVisualization.Charting.Series serie in grafico.Series)
            {
                List<Double> DatosY = new List<Double>();
                string nombre = serie.Name.Split('-')[0]; //La serie depende del for y asi recorre cada serie ingresada en el gráfico 
                nombre = nombre.Remove(nombre.Length - 1); //Le removi el espacio al final porque se le agregaba uno sin querer.                                                           //MessageBox.Show("Este es el nombre: "+nombre);
                string unidad = MedicionesClass.medicionesDiccionario[nombre][2];//obtengo la unidad desde el diccionario
                foreach (DataPoint punto in serie.Points)
                {
                    DatosY.Add(Convert.ToDouble(punto.YValues[0])); //añado punto por punto lo que necesito que se visualice 
                }
                double media = Math.Round(DatosY.Average(), 2); //Calculo la media 
                double maximo = DatosY.Max(); //Calculo el maximo 
                int pos_maximo = DatosY.IndexOf(DatosY.Max()); //Ponerme en el caso que exista mas veces el maximo
                double minimo = DatosY.Min();
                int pos_minimo = DatosY.IndexOf(DatosY.Min()); //Ponerme en el caso que exista mas veces el minimo
                double moda = DatosY.GroupBy(v => v) //Ponerme en el caso que sea bimodal o multimodal
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;
                double sumaCuadradosDiferencia = DatosY.Select(val => (val - media) * (val - media)).Sum();
                double desviacion = Math.Round(Math.Sqrt(sumaCuadradosDiferencia / DatosY.Count()), 2);
                AddOwnedForm(MyEstadisticas); //La declaro asi porque me da mas poder sobre el formulario 
                MyEstadisticas.tbEstadisticas.AppendLine("SENSOR: "+serie.Name.ToUpper()+ Environment.NewLine);
                MyEstadisticas.tbEstadisticas.AppendLine("La media es: " + Convert.ToString(media) + " " + unidad + Environment.NewLine + "El valor máximo es: " + Convert.ToString(maximo) + " " + unidad + " registrado a la hora: "
                + " " + horas[pos_maximo] + Environment.NewLine + "El valor mínimo es: " + Convert.ToString(minimo) + " " + unidad + " registrado a la hora: " + " " + horas[pos_minimo] + Environment.NewLine + "El valor modal es: " + Convert.ToString(moda) + " " + unidad + Environment.NewLine +
                "La desviación estandar es: " + Convert.ToString(desviacion) + " " + unidad);
                MyEstadisticas.tbEstadisticas.AppendLine("\r\n");                
            }
            MyEstadisticas.ShowDialog();
        }      
        //***********************************************************FIN ESTADISTICAS ***************************************************************************************************************************************************************************************************

        List<int> GraficosDisponibles_Globales = new List<int>();
        List<string> horas = new List<string>(); //Lista re importante que contiene todas las horas para poder graficar los charts

        //*****************************************GRAFICOS GLOBALES ***************************************************************************************************        
        public void GraficosGlobales() 
        {
            horas.Clear(); //limpia Lista con horas 
            for (int x = 0; x < dgDatosSectores.Rows.Count; x++) //Me agrega todas las horas existentes a una lista 
            {
                if (Convert.ToString(dgDatosSectores.Rows[x].Cells[0].Value)!=string.Empty)
                     horas.Add(Convert.ToString(dgDatosSectores.Rows[x].Cells[0].Value));
            }                        
            horas.Reverse(); //La da vuelta para que vayan del mas viejo al mas nuevo
            cGlobal1.Series.Clear(); //Limpia el cGlobal1
            cGlobal4.Series.Clear(); //Limpia el cGlobal2 
            cGlobal3.Series.Clear(); //Limpia el cGlobal3
            cGlobal2.Series.Clear(); //Limpia el cGlobal4
            if (tecla_up == true && tecla_down == false) //Restringo que quiero que ocurra en funcion de si ha apretado o no la tecla up y down
            {
                PrimeroGlobal5 = "vacio"; //Los vuelvo vacio para que no se sobreescriban 
                PrimeroGlobal6 = "vacio"; //Los vuelvo vacio para que no se, tengo que arreglarlo tambien al momento de limpiar las cosas en el boton de limpiar  
                PrimeroGlobal7 = "vacio"; //Los vuelvo vacio para que no se 
                PrimeroGlobal8 = "vacio"; //Los vuelvo vacio para que no se 
                foreach (int indicePresente in GraficosDisponibles_Globales)
                {
                    for (int z = 1; z < dgDatosSectores.Columns.Count; z++)
                    {
                        if (dgDatosSectores.Columns[indicePresente].HeaderText.Split('-')[1] == dgDatosSectores.Columns[z].HeaderText.Split('-')[1])
                        {
                            try
                            {
                                graficoGlobal1(z, horas);
                                graficoGlobal2(z, horas);
                                graficoGlobal3(z, horas);
                                graficoGlobal4(z, horas);
                                GraficosReserva(z);
                            }
                            catch { }
                        }
                    }
                }
            }
            if (tecla_up == false && tecla_down == true && PrimeroGlobal5!="vacio")
            {
                if (PrimeroGlobal5 != "vacio" && PrimeroGlobal6 == "vacio")
                {
                    foreach (int indicePresente in GraficosDisponibles_Globales)
                    {
                        for (int z = 1; z < dgDatosSectores.Columns.Count; z++)
                        {
                            if (dgDatosSectores.Columns[indicePresente].HeaderText.Split('-')[1] == dgDatosSectores.Columns[z].HeaderText.Split('-')[1])
                            {
                                try
                                {
                                    graficoGlobal5(z, horas);
                                    graficoGlobal2(z, horas);
                                    graficoGlobal3(z, horas);
                                    graficoGlobal4(z, horas);
                                }
                                catch { }
                            }
                        }
                    }
                }
                else if (PrimeroGlobal6 != "vacio" && PrimeroGlobal7 == "vacio")
                {
                    foreach (int indicePresente in GraficosDisponibles_Globales)
                    {
                        for (int z = 1; z < dgDatosSectores.Columns.Count; z++)
                        {
                            if (dgDatosSectores.Columns[indicePresente].HeaderText.Split('-')[1] == dgDatosSectores.Columns[z].HeaderText.Split('-')[1])
                            {
                                try
                                {
                                    string componentePosicionGaleria = MedicionesClass.medicionesDiccionario[dgDatosSectores.Columns[z].Name][4].Split('-')[1];
                                    graficoGlobal5(z, horas);
                                    graficoGlobal6(z, horas);
                                    graficoGlobal3(z, horas);
                                    graficoGlobal4(z, horas);

                                }
                                catch { }
                            }
                        }
                    }
                }
                else if (PrimeroGlobal7 != "vacio" && PrimeroGlobal8 == "vacio")
                {
                    foreach (int indicePresente in GraficosDisponibles_Globales)
                    {
                        for (int z = 1; z < dgDatosSectores.Columns.Count; z++)
                        {
                            if (dgDatosSectores.Columns[indicePresente].HeaderText.Split('-')[1] == dgDatosSectores.Columns[z].HeaderText.Split('-')[1])
                            {
                                try
                                {
                                    graficoGlobal5(z, horas);
                                    graficoGlobal6(z, horas);
                                    graficoGlobal7(z, horas);
                                    graficoGlobal4(z, horas);
                                }
                                catch { }
                            }
                        }
                    }
                }
                else if (PrimeroGlobal8 != "vacio" && PrimeroGlobal7!="vacio")
                {
                    foreach (int indicePresente in GraficosDisponibles_Globales)
                    {
                        for (int z = 1; z < dgDatosSectores.Columns.Count; z++)
                        {
                            if (dgDatosSectores.Columns[indicePresente].HeaderText.Split('-')[1] == dgDatosSectores.Columns[z].HeaderText.Split('-')[1])
                            {
                                try
                                {
                                    graficoGlobal5(z, horas);
                                    graficoGlobal6(z, horas);
                                    graficoGlobal7(z, horas);
                                    graficoGlobal8(z, horas);
                                }
                                catch { }
                            }
                        }
                    }
                }                
            }
        }

        private void tmrGlobales_Tick(object sender, EventArgs e) //Funcion que solo anota la hora en la columna cero del datagridview
        {
            try
            {
                dgDatosSectores.Rows[0].Cells[0].Value = DateTime.Now.ToString("HH:mm:ss");
            }
            catch { }
        }

        public long y { get; set; }
        public int Click_Graficar { get; set; }
        public bool tiempo_DefinidoPorUsuario { get; set; }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Eliminar = "Desactivado";
            if (Click_Graficar == 0 && tiempo_DefinidoPorUsuario==false) //Si no se ha definido un tiempo por usuario 
            {
                Click_Graficar += 1; // le suma uno indicando que ua se esta eligiendo tiempo
                y = 120; // lo define en 2 minutos
                Separator separadorx = new Separator { Step = TimeSpan.FromSeconds(20).Ticks }; //Define un separador de 20 segundos
                SetAxisLimits(System.DateTime.Now, y); //Hora y limite de tiempo visualizado  
                //timer1.Tick += TimerOnTick2;  //ACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAASECOMEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEENTOOOOOOOOOOOO
                foreach(LiveCharts.Wpf.Axis eje in cartesianChart1.AxisX) 
                {
                    eje.Separator = new Separator { Step = TimeSpan.FromSeconds(Math.Ceiling(y / 12.0)).Ticks };
                }
                MessageBox.Show("Se ha establecido un tiempo de visualización por defecto de: " + Convert.ToString(y) + " segundos");
            } 
            else
                foreach (LiveCharts.Wpf.Axis eje in cartesianChart1.AxisX)
                {
                    eje.Separator = new Separator { Step = TimeSpan.FromSeconds(Math.Ceiling(y / 12.0)).Ticks }; //ocurre problema de que pasa si son mas de 12
                }
        }

        private void tstrpTiempoVisualizacion_Click(object sender, EventArgs e)
        {            
            if (MessageBox.Show("¿Desea Especificar el Tiempo de Visualización?", "Tiempo Visualización", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FTiempoVisualizacion MyFTiempoVisualizacion = new FTiempoVisualizacion();
                AddOwnedForm(MyFTiempoVisualizacion);                
                MyFTiempoVisualizacion.ShowDialog();
                tiempo_DefinidoPorUsuario = true;
            }
            else
                y = 120;
        }

        private void tsRefrescarGraficos_Click(object sender, EventArgs e)
        {
            GraficosGlobales();
        }       

        //******************************************************************* KEYS ON tabControl1 *************************************************************************************************
        public bool tecla_down { get; set; } = false;
        public bool tecla_up { get; set; } = true;
        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down && tecla_down==false &&PrimeroGlobal5!="vacio")
            {
                tecla_down = true;
                tecla_up = false;
                VerificarVisibilidad_GraficosEstaticos(tecla_down,tecla_up) ;
            }
            else if (e.KeyData == Keys.Up && tecla_up==false)
            {
                tecla_down = false;
                tecla_up = true;
                VerificarVisibilidad_GraficosEstaticos(tecla_down, tecla_up);
            }
            else if(e.KeyData==Keys.R)

                GraficosGlobales();
        }

        //************************Funcion que define cual gráfico esta visible y cual no *********************************************
        private void VerificarVisibilidad_GraficosEstaticos(bool tecla_down,bool tecla_up)
        {
            List<string> IndiceGraficosTeclas = new List<string>();
            List<System.Windows.Forms.DataVisualization.Charting.Chart> Lista_Charts = new List<System.Windows.Forms.DataVisualization.Charting.Chart>();
            Lista_Charts.Add(cGlobal1);
            Lista_Charts.Add(cGlobal2);
            Lista_Charts.Add(cGlobal3);
            Lista_Charts.Add(cGlobal4);
            if (tecla_up == true && tecla_down==false) //que se muestren los primeros cuatro que encontro con visibilidad no mas pos 
            {
                foreach (System.Windows.Forms.DataVisualization.Charting.Chart Grafico in Lista_Charts)
                {

                    Grafico.Visible = true;
                }
                GraficosGlobales();
            }
            else if(tecla_down==true &&tecla_up ==false &&PrimeroGlobal5!="vacio")
            {
                foreach (System.Windows.Forms.DataVisualization.Charting.Chart Grafico in Lista_Charts)
                {
                    
                    Grafico.Visible = true;
                }
                GraficosGlobales();         
            }            
        }

        private void cbSectores_SelectedIndexChanged(object sender, EventArgs e)
        {
            seleccionIndiceSectores = Convert.ToString(cbSectores.SelectedItem);            
        }

        //****************************************************************Aca se ejecutara la clase de Estadisticas***************************************************************************************************** 

        private void cGlobal1_DoubleClick(object sender, EventArgs e)
        {
            Estadisticas(cGlobal1);
        }

        private void cGlobal2_DoubleClick(object sender, EventArgs e)
        {
            Estadisticas(cGlobal2);
        }

        private void cGlobal3_DoubleClick(object sender, EventArgs e)
        {
            Estadisticas(cGlobal3);
        }

        private void cGlobal4_DoubleClick(object sender, EventArgs e)
        {
            Estadisticas(cGlobal4);
        }
        //************************************ACA SE EXPORTARA A EXCEL EL CONTENIDO DEL DATAGRIDVIEW****************************************************
        private async void tsExcel_Click(object sender, EventArgs e)
        {
            string i= await exportarExcel(dgDatosSectores);
            
        }

        public async Task<string> exportarExcel(DataGridView tablilla) //Falta el releaseObject y ejecutar esto en un thread
        {            
            await Task.Run(() =>
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook libro = null;
                Microsoft.Office.Interop.Excel._Worksheet hoja = null;
                libro = (Microsoft.Office.Interop.Excel._Workbook)excel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                hoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.Worksheets.Add();
                hoja.Name = "Datos VentControl";
                ((Microsoft.Office.Interop.Excel.Worksheet)excel.ActiveWorkbook.Sheets["Hoja1"]).Delete();
                int IndiceColumna = 0;
                foreach (DataGridViewColumn col in tablilla.Columns)
                {
                    IndiceColumna++;
                    excel.Cells[1, IndiceColumna] = col.Name;
                }
                int indiceFila = 0;
                foreach (DataGridViewRow row in tablilla.Rows)
                {
                    indiceFila++;
                    IndiceColumna = 0;
                    foreach (DataGridViewColumn col in tablilla.Columns)
                    {
                        IndiceColumna++;
                        excel.Cells[indiceFila + 1, IndiceColumna] = row.Cells[col.Name].Value;
                    }
                }
                libro.Saved = true;
                libro.SaveAs(DateTime.Now.ToString("yyyy-MM-dd")); //aca la wea no tira la hora y segundos 
                excel.Visible = true;                
                releaseObject(excel);
            });
            return "Listo";
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Error mientras liberaba objeto " + ex.ToString());                
            }
            finally
            {
                GC.Collect();
            }
        }

        //***************************************************************** Modelo de VENTSIM ***********************************************************************************
        //****************************************************************** Falta Limitar a Solo Archivos .vsm ******************
        private void tsVentsim_Click(object sender, EventArgs e)
        {
            Clase_Ventsim MyVentsim = new Clase_Ventsim();
            MyVentsim.Show();
        }

        private void Graficos_SizeChanged(object sender, EventArgs e)
        {
			if(this.WindowState != FormWindowState.Minimized)
			{
				CambiarTamañoInicial();
			}
			
        }

        public void CambiarTamañoInicial()
        {
            //******************************Cglobal1************************************************************************           
            int x= tabControl1.Width;
            int y= tabControl1.Height;
            cGlobal1.Size = new Size((x/2)-5, (y/2)-7);
            //******************************Cglobal 2************************************************************************
            cGlobal2.Left = cGlobal1.Right;
            cGlobal2.Width = cGlobal1.Width;
            cGlobal2.Height = cGlobal1.Height;
            //******************************Cglobal3************************************************************************
            cGlobal3.Top = cGlobal1.Bottom;
            cGlobal3.Height = cGlobal1.Height;
            cGlobal3.Width = cGlobal1.Width;
            //*******************************Cglobal4************************************************************************
            cGlobal4.Top = cGlobal1.Bottom;
            cGlobal4.Left = cGlobal3.Right;
            cGlobal4.Height = cGlobal3.Height;
            cGlobal4.Width = cGlobal2.Width;
            //********************************Fin************************************************************************ 
        }
    }    
}

public static class ExtensionMethods //doble buffer al datagrid , asi no colapsa y se ve fluido  
{
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
        Type dgvType = dgv.GetType();
        PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        pi.SetValue(dgv, setting, null);
    }
}
public static class WinFormsExtensions  //permite agregar lineas al text box multilinea
{
    public static void AppendLine(this System.Windows.Forms.TextBox source, string value) 
    {
        try
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
        catch
        {  }

    }
}


