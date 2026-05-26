using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NarutoFighters.Interfaz;

namespace NarutoFighters
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            // este boton te llevara al form Seleccion
            Seleccion seleccion = new Seleccion();
            seleccion.Show();
            this.Hide();
        }
    }
}
