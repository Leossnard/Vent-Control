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
    public partial class FTiempoVisualizacion : Form
    {
        public FTiempoVisualizacion()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icono_reloj;
            cbUnidadTiempo.Items.Add("Segundos");
            cbUnidadTiempo.Items.Add("Minutos");
            cbUnidadTiempo.Items.Add("Horas");
            cbUnidadTiempo.SelectedIndex = 0;
        }

        public bool contiene_Caracter { get; set; }
        public int Multiplicador { get; set; }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            contiene_Caracter = false;
            Graficos MyGraficos= Owner as Graficos;
            foreach(char caracter in txtY.Text)
            {
                if (char.IsDigit(caracter) == false)
                {
                    contiene_Caracter = true;
                    break;
                }
            }
            if (contiene_Caracter == false)
            {
                if(cbUnidadTiempo.Text== "Segundos")
                {
                    Multiplicador = 1;
                }
                else if(cbUnidadTiempo.Text == "Minutos")
                {
                    Multiplicador = 60;
                }
                else if(cbUnidadTiempo.Text == "Horas")
                {
                    Multiplicador = 3600;
                }
                long parametro = Convert.ToInt32(txtY.Text) * Multiplicador;
                MyGraficos.y = parametro;
                MyGraficos.SetAxisLimits(System.DateTime.Now, MyGraficos.y);
                //MyGraficos.timer1.Tick += MyGraficos.TimerOnTick2;
                foreach (LiveCharts.Wpf.Axis eje in MyGraficos.cartesianChart1.AxisX) //aca se soluciono lo de l
                {
                    eje.Separator = new LiveCharts.Wpf.Separator { Step = TimeSpan.FromSeconds(Math.Ceiling(MyGraficos.y / 12.0)).Ticks };
                }
                
                //Aca convertir la weaita 
                //MyGraficos.metodoInicialGraficos(MyGraficos.y/4);
                MessageBox.Show("Se ha establecido un tiempo de visualización de: " + Convert.ToString(txtY.Text) + " "+cbUnidadTiempo.Text);
                this.Hide();
            }
            else
            {
                MessageBox.Show("El tiempo ingresado no es valido","Advertencia");
            }            
        }
        
        private void txtY_TextChanged(object sender, EventArgs e)
        {
            bool error = false;
            foreach (char caracter in txtY.Text)
            {
                if (char.IsDigit(caracter)==false)
                {
                    error = true;
                    break;
                }               
            }
            if (error)
                errorProvider1.SetError(txtY, "No se admiten Letras o Caracteres Extraños");
            else
                errorProvider1.Clear();
        }
    }
}
