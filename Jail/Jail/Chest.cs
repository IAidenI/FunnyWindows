using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jail
{
    public partial class Chest : Form
    {
        // Appel à la winAPI pour cacher le caret (curseur texte)
        [DllImport("user32.dll")]
        private static extern int HideCaret(IntPtr hwnd);
        private Random rng = new Random();

        public enum LootType
        {
            BONUS1,
            BONUS2,
            BONUS_MALUS1,
            BONUS_MALUS2,
            MALUS1,
            MALUS2,
        }

        public Dictionary<string, LootType> lootTypes = new Dictionary<string, LootType>
        {
            { "Afficher la souris durant 7 secondes", LootType.BONUS1 },
            { "Affiche la bonne sortie", LootType.BONUS2 },
            { "Mouvement inversé durant 15 secondes", LootType.MALUS1 },
            { "Souris lente pendant 15 secondes", LootType.MALUS2 },
            { "Téléporte la souris à un endroit random", LootType.BONUS_MALUS1 },
            { "Checkpoint au coffre", LootType.BONUS_MALUS2 }
        };

        private Dictionary<string, int> lootWeights = new Dictionary<string, int>
        {
            // 50% de chance d'avoir un malus
            // 20% de chance d'avoir la tp
            // 15% de chance d'avoir la souris d'affiché
            // 10% de chance d'avoir la sortie d'affiché
            { "Mouvement inversé durant 15 secondes", 25 },
            { "Souris lente pendant 15 secondes", 25 },
            { "Téléporte la souris à un endroit random", 15 },
            { "Checkpoint au coffre", 7 },
            { "Afficher la souris durant 7 secondes", 5 },
            { "Affiche la bonne sortie", 1 }
        };

        private string[] emojis = { "🎲", "🎰", "🎯", "🎮", "💥", "✨" };

        private int ticks = 0;
        private string final_loot = "";

        public string SelectedLoot { get; private set; }

        private bool is_pressed = false;
        private bool cancel = false;

        public Chest()
        {
            InitializeComponent();
            this.CenterToScreen();

            // Empêche le curseur clignotant (caret)
            richTextBoxLoot.GotFocus += (s, e) => HideCaret(richTextBoxLoot.Handle);
            richTextBoxLoot.MouseDown += (s, e) => HideCaret(richTextBoxLoot.Handle);

            richTextBoxDrop.GotFocus += (s, e) => HideCaret(richTextBoxDrop.Handle);
            richTextBoxDrop.MouseDown += (s, e) => HideCaret(richTextBoxDrop.Handle);
        }

        private void Chest_Load(object sender, EventArgs e)
        {
            ticks = 0;
            final_loot = RandomLoot();
            DisplayDropRates();
        }

        private void Chest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!cancel) e.Cancel = true;
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            panelChest.Visible = false;
            panelDrop.Visible = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            panelDrop.Visible = false;
            panelChest.Visible = true;
        }

        // Récupère un loot random
        private string RandomLoot()
        {
            int totalWeight = lootWeights.Values.Sum();
            int roll = rng.Next(totalWeight); // entre 0 et totalWeight - 1
            int cumulative = 0;

            foreach (var kvp in lootWeights)
            {
                cumulative += kvp.Value;
                if (roll < cumulative)
                    return kvp.Key;
            }

            return lootWeights.Keys.First();
        }

        // Lance la roue de la chance
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!is_pressed)
            {
                timerLoot.Start();
                is_pressed = true;
            }
        }

        // Fait défiller les récompenses
        private void timerLoot_Tick(object sender, EventArgs e)
        {
            ticks++;

            Random rng = new Random();
            List<string> allLoots = lootTypes.Keys.ToList();
            string loot = allLoots[rng.Next(allLoots.Count)];
            string emoji = emojis[rng.Next(emojis.Length)];

            Color[] colors = { Color.DeepSkyBlue, Color.Orange, Color.LimeGreen, Color.Fuchsia, Color.Red };
            Color color = colors[rng.Next(colors.Length)];

            richTextBoxLoot.Clear();
            richTextBoxLoot.SelectionAlignment = HorizontalAlignment.Center;
            richTextBoxLoot.AppendText("\n\n\n\n\n\n\n\n\n");
            richTextBoxLoot.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
            richTextBoxLoot.SelectionColor = color;
            richTextBoxLoot.AppendText($"{emoji} {loot}");

            if (ticks >= 20) // après ~2 secondes
            {
                timerLoot.Stop();
                Showfinal_loot(final_loot);
            }
        }

        // Affiche et renvoie en code de retour le loot choisi
        private async void Showfinal_loot(string loot)
        {
            richTextBoxLoot.Clear();
            richTextBoxLoot.SelectionFont = new Font("Segoe UI", 11, FontStyle.Regular);
            richTextBoxLoot.AppendText("\n\n\n\n\n");

            string typeText = "🎲 Bonus ou malus";
            Color typeColor = Color.MediumPurple;

            if (lootTypes.TryGetValue(loot, out LootType type))
            {
                switch (type)
                {
                    case LootType.BONUS1:
                    case LootType.BONUS2:
                        typeText = "✅ Bonus";
                        typeColor = Color.LimeGreen;
                        break;

                    case LootType.MALUS1:
                    case LootType.MALUS2:
                        typeText = "❌ Malus";
                        typeColor = Color.IndianRed;
                        break;

                    case LootType.BONUS_MALUS1:
                    case LootType.BONUS_MALUS2:
                        typeText = "🎲 Effet variable";
                        typeColor = Color.MediumPurple;
                        break;
                }
            }

            // Type
            richTextBoxLoot.SelectionAlignment = HorizontalAlignment.Center;
            richTextBoxLoot.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
            richTextBoxLoot.SelectionColor = typeColor;
            richTextBoxLoot.AppendText(typeText + "\n\n");

            // Titre
            richTextBoxLoot.SelectionAlignment = HorizontalAlignment.Center;
            richTextBoxLoot.SelectionFont = new Font("Segoe UI", 13, FontStyle.Bold);
            richTextBoxLoot.SelectionColor = Color.Gold;
            richTextBoxLoot.AppendText("🎉 Récompense :\n\n");

            // Contenu
            richTextBoxLoot.SelectionAlignment = HorizontalAlignment.Center;
            richTextBoxLoot.SelectionFont = new Font("Segoe UI", 12, FontStyle.Italic);
            richTextBoxLoot.SelectionColor = Color.White;
            richTextBoxLoot.AppendText(loot);

            // ⏱ Attend 2.5 secondes puis ferme
            SelectedLoot = loot;
            await Task.Delay(2500);
            this.DialogResult = DialogResult.OK;
            cancel = true;
            this.Close();
        }

        private void DisplayDropRates()
        {
            richTextBoxDrop.Clear();
            richTextBoxDrop.SelectionAlignment = HorizontalAlignment.Left;

            // Titre
            richTextBoxDrop.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
            richTextBoxDrop.SelectionColor = Color.Gold;
            richTextBoxDrop.AppendText("🎁 Taux de drop :\n");
            richTextBoxDrop.SelectionFont = new Font("Segoe UI", 5, FontStyle.Regular);
            richTextBoxDrop.AppendText("\n");

            // Corps
            foreach (var kvp in lootWeights.OrderByDescending(x => x.Value))
            {
                string lootName = kvp.Key;
                int weight = kvp.Value;
                int total = lootWeights.Values.Sum();
                double percentage = 100.0 * weight / total;

                // Définir la couleur selon le type
                Color color = Color.White;
                if (lootTypes.TryGetValue(lootName, out LootType type))
                {
                    switch (type)
                    {
                        case LootType.BONUS1:
                        case LootType.BONUS2:
                            color = Color.LimeGreen;
                            break;
                        case LootType.MALUS1:
                        case LootType.MALUS2:
                            color = Color.IndianRed;
                            break;
                        case LootType.BONUS_MALUS1:
                        case LootType.BONUS_MALUS2:
                            color = Color.MediumPurple;
                            break;
                    }
                }

                // Affichage
                richTextBoxDrop.SelectionFont = new Font("Segoe UI", 10, FontStyle.Regular);
                richTextBoxDrop.SelectionColor = color;
                richTextBoxDrop.AppendText($"• {lootName} : {percentage:0.##}%\n");
            }

            // Légende
            richTextBoxDrop.AppendText("\n");
            richTextBoxDrop.SelectionAlignment = HorizontalAlignment.Center;
            richTextBoxDrop.SelectionFont = new Font("Segoe UI", 9, FontStyle.Italic);

            // Bonus
            richTextBoxDrop.SelectionColor = Color.LimeGreen;
            richTextBoxDrop.AppendText("🟩 Bonus  ");

            // Malus
            richTextBoxDrop.SelectionColor = Color.IndianRed;
            richTextBoxDrop.AppendText("🟥 Malus  ");

            // Bonus/Malus
            richTextBoxDrop.SelectionColor = Color.MediumPurple;
            richTextBoxDrop.AppendText("🟪 Effet variable");
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

        private void TestDropRates(int simulations = 1000)
        {
            Dictionary<string, int> counts = lootWeights.Keys.ToDictionary(k => k, v => 0);

            for (int i = 0; i < simulations; i++)
            {
                string loot = RandomLoot();
                if (counts.ContainsKey(loot))
                    counts[loot]++;
            }

            Console.WriteLine($"\n🎯 Simulation de {simulations} loots :\n");

            foreach (var kvp in counts.OrderByDescending(k => k.Value))
            {
                double percentage = 100.0 * kvp.Value / simulations;
                Console.WriteLine($"• {kvp.Key.PadRight(40)} : {kvp.Value} fois ({percentage:0.00}%)");
            }

            Console.WriteLine();
        }
    }
}
