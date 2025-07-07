using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Jail
{
    public partial class Rules : Form
    {
        // Appel à la winAPI pour cacher le caret (curseur texte)
        [DllImport("user32.dll")]
        private static extern int HideCaret(IntPtr hwnd);

        private bool cancel = false;

        public Rules()
        {
            InitializeComponent();
            this.CenterToScreen();

            richTextBoxInfo.Clear();

            // Titre "Objectif"
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 14, FontStyle.Bold);
            richTextBoxInfo.SelectionColor = Color.LimeGreen;
            richTextBoxInfo.AppendText("🎯 Objectif : ");

            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 12, FontStyle.Italic);
            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText("Trouver la bonne sortie du labyrinthe.\n");
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 7, FontStyle.Regular);
            richTextBoxInfo.AppendText("\n\n");

            // Titre "Règles"
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 13, FontStyle.Bold);
            richTextBoxInfo.SelectionColor = Color.Orange;
            richTextBoxInfo.AppendText("📜 Règles :\n");
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 5, FontStyle.Regular);
            richTextBoxInfo.AppendText("\n");

            // Règle 1 : Curseur invisible
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
            richTextBoxInfo.SelectionColor = Color.Red;
            richTextBoxInfo.AppendText("🔴 ");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText("La souris est invisible, mais commence au centre du labyrinthe (le point rouge).\n");
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 5, FontStyle.Regular);
            richTextBoxInfo.AppendText("\n");

            // Règle 2 : Affichage temporaire
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
            richTextBoxInfo.SelectionColor = Color.DeepSkyBlue;
            richTextBoxInfo.AppendText("⏱️ ");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText("Toutes les ");

            richTextBoxInfo.SelectionColor = Color.DeepSkyBlue;
            richTextBoxInfo.AppendText("5 secondes");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText(", vous pouvez afficher le curseur pendant ");

            richTextBoxInfo.SelectionColor = Color.DeepSkyBlue;
            richTextBoxInfo.AppendText("2 secondes");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText(" en effectuant un ");

            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 11, FontStyle.Bold);
            richTextBoxInfo.SelectionColor = Color.LightBlue;
            richTextBoxInfo.AppendText("clic souris");

            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText(" (droit ou gauche). ");

            richTextBoxInfo.AppendText("Le timer est affiché en ");

            richTextBoxInfo.SelectionColor = Color.DeepSkyBlue;
            richTextBoxInfo.AppendText("bleu");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText(" en bas de l’écran.\n");
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 5, FontStyle.Regular);
            richTextBoxInfo.AppendText("\n");

            // Règle 3 : Coffres
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
            richTextBoxInfo.SelectionColor = Color.Gold;
            richTextBoxInfo.AppendText("💰 ");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText("Des coffres sont placés sur la map : ils donnent ");

            richTextBoxInfo.SelectionColor = Color.LimeGreen;
            richTextBoxInfo.AppendText("bonus");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText(" ou ");

            richTextBoxInfo.SelectionColor = Color.IndianRed;
            richTextBoxInfo.AppendText("malus");

            richTextBoxInfo.SelectionColor = Color.White;
            richTextBoxInfo.AppendText(", de façon aléatoire.\n");
            richTextBoxInfo.SelectionFont = new Font("Segoe UI", 5, FontStyle.Regular);
            richTextBoxInfo.AppendText("\n");

            // Empêche le curseur clignotant (caret)
            richTextBoxInfo.GotFocus += (s, e) => HideCaret(richTextBoxInfo.Handle);
            richTextBoxInfo.MouseDown += (s, e) => HideCaret(richTextBoxInfo.Handle);
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        // Empêche le alt + tab
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        private void Rules_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!cancel) e.Cancel = true;
        }
    }
}
