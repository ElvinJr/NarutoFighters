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
    public partial class Seleccion : Form
    {
        private string[] allCharacters = { "Naruto", "Sasuke" };
        public Seleccion()
        {
            InitializeComponent();
        }

        private void SelectCharacter(string selectedCharacter)
        {
            Random rand = new Random();
            string enemyCharacter;

            // Elegir enemigo aleatorio que no sea el personaje seleccionado
            do
            {
                enemyCharacter = allCharacters[rand.Next(allCharacters.Length)];
            } while (enemyCharacter == selectedCharacter);

            // Abrir pantalla de batalla pasando los personajes
            Combate combate = new Combate(selectedCharacter, enemyCharacter);
            combate.Show();
            this.Hide();
        }

        private void btnSasuke_Click(object sender, EventArgs e)
        {
            //Selecionar al personaje Sasuke
            SelectCharacter("Sasuke");
        }

        private void btnNaruto_Click(object sender, EventArgs e)
        {
            //Selecionar al personaje Naruto
            SelectCharacter("Naruto");
        }
    }
}
