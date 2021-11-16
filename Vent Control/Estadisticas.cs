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
    public partial class Estadisticas : Form
    {
        

        public Estadisticas()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.IconoGraficos;
            Graficos MyGraficos = Owner as Graficos;
            


        }

    }
}
