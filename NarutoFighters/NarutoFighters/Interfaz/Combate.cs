using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NarutoFighters.Interfaz
{
    public partial class Combate : Form
    {
        // ─── Personajes ───────────────────────────────────────────
        private string playerCharacter;
        private string enemyCharacter;

        // ─── Stats ────────────────────────────────────────────────
        private int playerHP = 1000;
        private int enemyHP = 1000;
        private const int maxHP = 1000;
        private const int minDamage = 10;
        private const int maxDamage = 100;

        // ─── Probabilidades ───────────────────────────────────────
        private const double dodgeChance = 0.20;      // 20% esquivo
        private const double critChance = 0.15;       // 15% critico
        private const double missChance = 0.10;       // 10% fallo

        // ─── Control de turno ─────────────────────────────────────
        private int turnNumber = 1;
        private bool playerSkipTurn = false;
        private bool enemySkipTurn = false;
        private bool battleRunning = false;
        private Random rand = new Random();

        // ─── Historial para Resultado ─────────────────────────────
        private List<string> battleLog = new List<string>();

        public Combate()
        {
            InitializeComponent();
        }

        public Combate(string player, string enemy) : this()
        {
            playerCharacter = player;
            enemyCharacter = enemy;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Nombres
            lblPlayerName.Text = playerCharacter.ToUpper();
            lblEnemyName.Text = enemyCharacter.ToUpper();

            // Sprites Standing
            LoadSprite(pbPlayer, playerCharacter, "Standing", false);
            LoadSprite(pbEnemy, enemyCharacter, "Standing", true);

            // HP inicial
            UpdateHP();
        }

        private void LoadSprite(PictureBox pb, string character, string state, bool flip = false)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filename = "";
            if (character == "Naruto")
            {
                if (state == "Standing") filename = "Naruto.png";
                else if (state == "Attack") filename = "Golpe.png";
                else if (state == "Damage") filename = "Daño.png";
            }
            else if (character == "Sasuke")
            {
                if (state == "Standing") filename = "Sasuke.png";
                else if (state == "Attack") filename = "sGolpe.png";
                else if (state == "Damage") filename = "sDaño.png";
            }

            string path = Path.Combine(basePath, "Imagenes", character, filename);
            if (File.Exists(path))
            {
                Image img = Image.FromFile(path);
                if (flip)
                {
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                pb.Image = img;
            }
        }

        // ─── Actualizar barras de vida ────────────────────────────
        private void UpdateHP()
        {
            // Player
            int pHP = Math.Max(playerHP, 0);
            pbPlayerHP.Value = pHP;
            lblPlayerHP.Text = pHP + " / " + maxHP;

            // Enemy
            int eHP = Math.Max(enemyHP, 0);
            pbEnemyHP.Value = eHP;
            lblEnemyHP.Text = eHP + " / " + maxHP;
        }

        // ─── Escribir en el log ───────────────────────────────────
        private void Log(string message, Color color)
        {
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = color;
            rtbLog.AppendText(message + "\n");
            rtbLog.ScrollToCaret();
            battleLog.Add(message);
        }

        // ─── Botón INICIAR BATALLA ────────────────────────────────
        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (battleRunning) return;
            battleRunning = true;
            btnStart.Enabled = false;

            await RunBattle();
        }

        // ─── Loop principal de batalla ────────────────────────────
        private async Task RunBattle()
        {
            while (playerHP > 0 && enemyHP > 0)
            {
                Log("══════════════════════════════", Color.Gray);
                Log("⚔  TURNO " + turnNumber, Color.Yellow);

                //── Turno del jugador ──
                if (playerSkipTurn)
                {
                    Log("💫 " + playerCharacter + " está recuperándose, pierde su turno.", Color.Orange);
                    playerSkipTurn = false;
                }
                else
                {
                    await ProcessAttack(isPlayer: true);
                }

                if (enemyHP <= 0) break;
                await Task.Delay(500);

                //── Turno del enemigo ──
                if (enemySkipTurn)
                {
                    Log("💫 " + enemyCharacter + " está recuperándose, pierde su turno.", Color.Orange);
                    enemySkipTurn = false;
                }
                else
                {
                    await ProcessAttack(isPlayer: false);
                }

                UpdateHP();
                turnNumber++;
                await Task.Delay(1000);
            }

            UpdateHP();
            await Task.Delay(500);
            EndBattle();
        }

        private double GetDodgeChance(string character)
        {
            if (character == "Sasuke")
                return 0.25;
            else
                return 0.20;
        }

        // ─── Procesar un ataque ───────────────────────────────────
        private async Task ProcessAttack(bool isPlayer)
        {
            string attacker = isPlayer ? playerCharacter : enemyCharacter;
            string defender = isPlayer ? enemyCharacter : playerCharacter;

            // ── Fallo ──
            if (rand.NextDouble() < missChance)
            {
                Log("❌ " + attacker + " falló el ataque!", Color.Gray);
                if (isPlayer) LoadSprite(pbPlayer, playerCharacter, "Attack", false);
                else LoadSprite(pbEnemy, enemyCharacter, "Attack", true);
                await Task.Delay(400);
                LoadSprite(pbPlayer, playerCharacter, "Standing", false);
                LoadSprite(pbEnemy, enemyCharacter, "Standing", true);
                return;
            }

            // ── Calcular daño base ──
            int damage = rand.Next(minDamage, maxDamage + 1);
            bool isCrit = rand.NextDouble() < critChance;
            if (isCrit) damage *= 2;

            // ── Esquivo del defensor ──
            double dodge = GetDodgeChance(defender);
            if (rand.NextDouble() < dodge)
            {
                Log("🌀 " + defender + " esquivó el ataque de " + attacker + "!", Color.Cyan);
                if (isPlayer) LoadSprite(pbEnemy, enemyCharacter, "Attack", true);
                else LoadSprite(pbPlayer, playerCharacter, "Attack", false);
                await Task.Delay(400);
                LoadSprite(pbPlayer, playerCharacter, "Standing", false);
                LoadSprite(pbEnemy, enemyCharacter, "Standing", true);
                return;
            }

            // ── Aplicar daño ──
            string critText = isCrit ? " 💥 CRÍTICO x2!" : "";
            if (isPlayer)
            {
                enemyHP -= damage;

                // Sprites
                LoadSprite(pbPlayer, playerCharacter, "Attack", false);
                LoadSprite(pbEnemy, enemyCharacter, "Damage", true);
                await Task.Delay(400);
                LoadSprite(pbPlayer, playerCharacter, "Standing", false);

                Log("⚡ " + attacker + " atacó a " + defender + " por " + damage + " pts." + critText, Color.LimeGreen);

                // Regeneración 5%
                int regen = (int)(damage * 0.05);
                playerHP = Math.Min(playerHP + regen, maxHP);
                Log("💚 " + attacker + " se regenera " + regen + " pts.", Color.Green);

                // Turno perdido si daño máximo
                if (damage >= maxDamage * 2 || damage == maxDamage)
                {
                    int bonusRegen = (int)(damage * 0.10);
                    enemyHP = Math.Min(enemyHP + bonusRegen, maxHP);
                    enemySkipTurn = true;
                    Log("😵 " + defender + " está mareado! Pierde el siguiente turno y recupera " + bonusRegen + " pts.", Color.Orange);
                }
            }
            else
            {
                playerHP -= damage;

                // Sprites
                LoadSprite(pbEnemy, enemyCharacter, "Attack", true);
                LoadSprite(pbPlayer, playerCharacter, "Damage", false);
                await Task.Delay(400);
                LoadSprite(pbEnemy, enemyCharacter, "Standing", true);

                Log("🔥 " + attacker + " atacó a " + defender + " por " + damage + " pts." + critText, Color.Red);

                // Regeneración 5%
                int regen = (int)(damage * 0.05);
                enemyHP = Math.Min(enemyHP + regen, maxHP);
                Log("❤ " + attacker + " se regenera " + regen + " pts.", Color.IndianRed);

                // Turno perdido si daño máximo
                if (damage >= maxDamage * 2 || damage == maxDamage)
                {
                    int bonusRegen = (int)(damage * 0.10);
                    playerHP = Math.Min(playerHP + bonusRegen, maxHP);
                    playerSkipTurn = true;
                    Log("😵 " + defender + " está mareado! Pierde el siguiente turno y recupera " + bonusRegen + " pts.", Color.Orange);
                }
            }

            await Task.Delay(200);
            LoadSprite(pbPlayer, playerCharacter, "Standing", false);
            LoadSprite(pbEnemy, enemyCharacter, "Standing", true);
        }

        // ─── Fin de batalla ───────────────────────────────────────
        private void EndBattle()
        {
            string winner = playerHP > 0 ? playerCharacter : enemyCharacter;
            string loser = playerHP > 0 ? enemyCharacter : playerCharacter;

            Log("══════════════════════════════", Color.Gray);
            Log("🏆 " + winner.ToUpper() + " GANA LA BATALLA!", Color.Gold);

            // Abrir Resultado
            Resultado resultado = new Resultado(winner, loser, battleLog);
            resultado.Show();
            this.Hide();
        }

        private void rtbLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void pbEnemy_Click(object sender, EventArgs e)
        {

        }

        private void pbPlayer_Click(object sender, EventArgs e)
        {

        }

        private void lblEnemyHP_Click(object sender, EventArgs e)
        {

        }

        private void pbEnemyHP_Click(object sender, EventArgs e)
        {

        }

        private void lblEnemyName_Click(object sender, EventArgs e)
        {

        }

        private void lblPlayerHP_Click(object sender, EventArgs e)
        {

        }

        private void pbPlayerHP_Click(object sender, EventArgs e)
        {

        }

        private void lblPlayerName_Click(object sender, EventArgs e)
        {

        }
    }
}
