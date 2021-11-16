using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vent_Control
{
    public partial class Clase_Ventsim : Form
    {
        public Clase_Ventsim()
        {
            InitializeComponent();
            btnAbrirModelo.Enabled = false;
        }
        public string direccion { get; set; }
        private void btnExaminar_Click(object sender, EventArgs e)
        {
            OpenFileDialog buscar = new OpenFileDialog();
            if (buscar.ShowDialog() == DialogResult.OK)
            {
                direccion = buscar.FileName;
                btnAbrirModelo.Enabled = true;
            }
        }

        private void btnAbrirModelo_Click(object sender, EventArgs e)
        {
            if (direccion.Contains(".vsm"))
            {
                Process.Start(direccion);
            }
            else
            {
                MessageBox.Show("No es un modelo válido");
            }
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
