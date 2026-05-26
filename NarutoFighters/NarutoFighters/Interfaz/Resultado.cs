using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NarutoFighters.Interfaz
{
    public partial class Resultado : Form
    {
        private string winner;
        private string loser;
        private List<string> battleLog;

        public Resultado()
        {
            InitializeComponent();
        }

        public Resultado(string winner, string loser, List<string> log) : this()
        {
            this.winner = winner;
            this.loser = loser;
            this.battleLog = log;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (winner != null && loser != null)
            {
                lblWinner.Text = "🏆 " + winner.ToUpper() + " GANA!";
                lblLoser.Text = loser.ToUpper() + " fue derrotado.";
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            Seleccion seleccion = new Seleccion();
            seleccion.Show();
            this.Close();
        }

        private void lblWinner_Click(object sender, EventArgs e)
        {

        }

        private void lblLoser_Click(object sender, EventArgs e)
        {

        }
    }
}
