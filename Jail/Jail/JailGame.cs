using MazeGame;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Jail.Chest;

namespace Jail
{
    public partial class Jail : Form
    {
        // Utilisation de winAPI pour avoir la position du curseur même en dehors de la fenêtre
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        // Utilisation de winAPI pour changer le registre de la sensibilité de la souris
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, UIntPtr pvParam, uint fWinIni);

        const uint SPI_SETMOUSESPEED = 0x0071;
        const uint SPIF_UPDATEINIFILE = 0x01;
        const uint SPIF_SENDCHANGE = 0x02;

        private int mouse_sensitivity = 0;

        private Point? safePointLocal = null;


        // Global
        private Maze maze;
        private bool cancel = true;
        private bool pause_detection = false;
        private const int SHOW_CURSOR = 2000;

        // Bonus/Malus
        private Point previous_position;
        private Point current_position;
        private const int NEUTRAL_SHOW_CURSOR = 1000;
        private const int BONUS_SHOW_CURSOR = 7000;
        private const int MALUS_INVERT_CURSOR = 15000;
        private const int MALUS_LOW_SENSIBILITY = 15000;

        // Timer du curseur
        private bool show_ok = true;
        private const double REMAINING_TIME = 5.0;
        private double remaining_time = REMAINING_TIME; // en secondes
        private const double interval = 0.05; // 50 ms
        private bool cancel_request = false;

        // Gestionnaire des tâches
        private int block_count = 0;
        private int max_blocks = 5; // Nombre de suppression du gestionnaire voulu



        // +----------------+
        // | MÉTHODES START |
        // +----------------+
        public Jail()
        {
            InitializeComponent();
            this.CenterToScreen();
            panelGame.Visible = false;
            mouse_sensitivity = GetMouseSensitivity();
        }

        private void Jail_Load(object sender, EventArgs e)
        {
            // Centre la souris au form au démarage
            Cursor.Position = this.PointToScreen(new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2));
            monitor_taskmgr(); // Ferme les gestionnaires des tâches si y en a
        }

        // Empêche de fermer la fenêtre lors de l'appuie sur la croix u
        private void Jail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancel) {
                // Message
                MessageBox.Show("Non non non", "Interdit !", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Annule la fermeture
                e.Cancel = cancel;
            }
        }

        // Lance le jeu
        private void btnStart_Click(object sender, EventArgs e)
        {
            panelStart.Visible = false;
            panelGame.Visible = true;

            using (Rules rules_form = new Rules())
            {
                rules_form.ShowDialog();
            }

            // Centre le curseur
            Cursor.Position = this.PointToScreen(new Point(pictureBoxMaze.ClientSize.Width / 2, pictureBoxMaze.ClientSize.Height / 2));
            maze = new Maze(pictureBoxMaze.PointToClient(Cursor.Position), pictureBoxMaze.Size);

            this.Cursor = CreateTransparentCursor(); // Cache le curseur
        }

        // Si le curseur est en dehors de la fenêtre alors le replacer au centre
        // Possibilité de le faire avec Cursor.Clip = this.Bounds et Cursor.Clip = Rectangle.Empty mais bug avec la touche windows, ça perd le focus
        private void timerCheckCursor_Tick(object sender, EventArgs e)
        {
            // Si la souris se trouve pour une quelconque raison en dehors de la fenêtre, alors le remettre dedans
            GetCursorPos(out Point globalPos);
            Rectangle clientScreenRect = this.RectangleToScreen(this.ClientRectangle);

            if (!clientScreenRect.Contains(globalPos))
            {
                // Replace le curseur au centre du form
                Point center = new Point(
                    clientScreenRect.Left + clientScreenRect.Width / 2,
                    clientScreenRect.Top + clientScreenRect.Height / 2
                );
                Cursor.Position = center;
            }
        }



        // +---------------+
        // | MÉTHODES MAZE |
        // +---------------+

        // Affiche la souris au clic gauche si c'est autoriser
        private async void pictureBoxMaze_MouseClickAsync(object sender, MouseEventArgs e)
        {
            if (show_ok)
            {
                timerCursor.Start();
                show_ok = false;
                this.Cursor = Cursors.Default; // Affiche le curseur
                await Task.Delay(SHOW_CURSOR);

                if (!cancel_request)
                {
                    this.Cursor = CreateTransparentCursor(); // Cache le curseur
                }
            }
        }

        // A chaque mouvement de la souris vérifie si l'utilisateur à le droit ou si il a trouvé une sortie
        private void pictureBoxMaze_MouseMove(object sender, MouseEventArgs e)
        {
            if (maze != null && !pause_detection)
            {
                // Vérifie si le chemin empreinté est correcte
                var ret = maze.isInPath(pictureBoxMaze.PointToClient(Cursor.Position));
                if (ret == RetCode.FALSE)
                {
                    labelError.Visible = true;
                    timerError.Start();
                    Reset();
                }

                if (ret == RetCode.LOOT) GetLoot();
                
                // Vérifie si il l'utilisateur à trouvé la sortie
                if (ret == RetCode.EXIT) Exit();
            }
        }

        // Récupere un item aléatoire
        private async void GetLoot()
        {
            // Mets en pause le jeu
            pause_detection = true;
            cancel_request = true;
            this.Cursor = Cursors.Default; // Affiche le curseur
            timerCursor.Stop();

            int display_cursor = NEUTRAL_SHOW_CURSOR;
            Point position = Cursor.Position;
            double remaining_time_backup = remaining_time;

            // Ouvre la fenêtre coffre
            using (Chest chest_form = new Chest())
            {
                if (chest_form.ShowDialog() == DialogResult.OK)
                {
                    string loot = chest_form.SelectedLoot;

                    if (chest_form.lootTypes.TryGetValue(loot, out LootType type))
                    {
                        switch (type)
                        {
                            case LootType.BONUS1:
                                display_cursor = BONUS_SHOW_CURSOR;
                                break;
                            case LootType.BONUS2:
                                pictureBoxMaze.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBoxMaze.Image = Image.FromFile("images/maze_exit.png");
                                break;
                            case LootType.MALUS1:
                                Cursor.Position = position;
                                previous_position = position;
                                _ = Task.Run(async () =>
                                {
                                    Invoke((MethodInvoker)(() => timerInvertMouse.Start()));
                                    await Task.Delay(MALUS_INVERT_CURSOR);
                                    Invoke((MethodInvoker)(() => timerInvertMouse.Stop()));
                                });
                                break;
                            case LootType.MALUS2:
                                _ = Task.Run(async () =>
                                {
                                    SetMouseSensitivity(1);
                                    await Task.Delay(MALUS_LOW_SENSIBILITY);
                                    SetMouseSensitivity(mouse_sensitivity);
                                });
                                break;
                            case LootType.BONUS_MALUS1:
                                Point grid_pos = maze.getSafePlace();
                                position = pictureBoxMaze.PointToScreen(grid_pos);
                                
                                safePointLocal = pictureBoxMaze.PointToClient(position);
                                pictureBoxMaze.Invalidate();
                                break;
                            case LootType.BONUS_MALUS2:
                                maze.checkpoint = maze.realToGrid(pictureBoxMaze.PointToClient(position)); // On sauvegarde la position actuelle comme étant le checkpoint
                                break;
                        }
                    }
                }
            }

            // Relance le jeu
            cancel_request = false;
            Cursor.Position = position;
            remaining_time = remaining_time_backup == REMAINING_TIME ? -1 : remaining_time_backup;
            timerCursor.Start();
            pause_detection = false;

            this.Cursor = Cursors.Default;
            await Task.Delay(display_cursor);
            if (!cancel_request) this.Cursor = CreateTransparentCursor();
        }

        private void pictureBoxMaze_Paint(object sender, PaintEventArgs e)
        {
            if (safePointLocal.HasValue)
            {
                Point p = safePointLocal.Value;
                int radius = 5;
                Rectangle circle = new Rectangle(p.X - radius, p.Y - radius, radius * 2, radius * 2);
                using (Brush brush = new SolidBrush(Color.Fuchsia)) // Rose flashy
                {
                    e.Graphics.FillEllipse(brush, circle);
                }
            }
        }

        // Génère un curseur invisible
        private Cursor CreateTransparentCursor()
        {
            Bitmap bmp = new Bitmap(1, 1);
            bmp.MakeTransparent();
            return new Cursor(bmp.GetHicon());
        }

        // Replace le curseur au centre
        private void Reset()
        {
            // Replace le curseur au centre
            Point position = maze.gridToReal(maze.checkpoint);
            Cursor.Position = pictureBoxMaze.PointToScreen(position);
            maze.reset(pictureBoxMaze.PointToClient(Cursor.Position));

            // Arret les malus/bonus si il y en a
            this.Cursor = CreateTransparentCursor(); // Bonus 1 
            pictureBoxMaze.Image = Image.FromFile("images/maze.png"); // Bonus 2
            if (timerInvertMouse.Enabled) timerInvertMouse.Stop(); // Malus 1
            SetMouseSensitivity(mouse_sensitivity); // Malus 2
            safePointLocal = null;       //
            pictureBoxMaze.Invalidate(); // Bonus/malus1
            // Bonus/Malus 2 il ne s'enlève pas car c'est le but d'un checkpoint

            // Réinitialise le timer du curseur
            show_ok = true;
            timerCursor.Stop();
            remaining_time = REMAINING_TIME;
            labelCursor.Text = "Prêt";

        }

        // Libère l'utilisateur de la fenêtre
        private void Exit()
        {
            maze = null;
            cancel = false; // Autorise la fermeture de la fenêtre
            timerCheckCursor.Stop();
            timerCursor.Stop();
            cancel_request = true;
            this.Cursor = Cursors.Default; // Affiche le curseur
            labelInstruction.Text = "Objectif : GAGNÉ";
        }

        // Autorise l'affichage du curseur toutes les X secondes et affiche le temps restant dans un label
        private void timerCursor_Tick(object sender, EventArgs e)
        {
            remaining_time -= interval;

            // Affiche le temps restant toutes les 50ms
            if (remaining_time <= 0)
            {
                // Une fois le temps écoulé, autoriser l'affichage du curseur
                labelCursor.Text = "Prêt";
                show_ok = true;
                timerCursor.Stop();
                remaining_time = REMAINING_TIME;
            }
            else
            {
                labelCursor.Text = remaining_time.ToString("0.00"); // Format XX.XX
            }
        }

        // Déaffiche le label erreur toutes les X secondes
        private void timerError_Tick(object sender, EventArgs e)
        {
            // Affiche le label error si il y a une erreur
            if (labelError.Visible)
            {
                labelError.Visible = false;
                timerError.Stop();
            }
        }

        private void timerInvertMouse_Tick(object sender, EventArgs e)
        {
            GetCursorPos(out current_position);

            int dx = current_position.X - previous_position.X;
            int dy = current_position.Y - previous_position.Y;

            if (dx != 0 || dy != 0)
            {
                Rectangle bounds = Screen.PrimaryScreen.Bounds;

                // Tente de calculer la nouvelle position inversée
                int newX = current_position.X - 2 * dx;
                int newY = current_position.Y - 2 * dy;

                // Si la nouvelle position est hors des bornes, ignore l'inversion pour cet axe
                if (newX < bounds.Left || newX >= bounds.Right)
                    newX = current_position.X; // pas d'inversion horizontale

                if (newY < bounds.Top || newY >= bounds.Bottom)
                    newY = current_position.Y; // pas d'inversion verticale

                SetCursorPos(newX, newY);

                previous_position = new Point(newX, newY);
            }
        }



        // +------------------+
        // | MÉTHODES SYSTÈME |
        // +------------------+

        // Pour récupèrer la sensibilité de la souris
        public int GetMouseSensitivity()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Mouse", writable: false))
            {
                string value = key.GetValue("MouseSensitivity", "10").ToString();
                if (int.TryParse(value, out int sensitivity))
                {
                    return Math.Max(1, Math.Min(20, sensitivity)); // clamp au cas où
                }
                return 10; // défaut si conversion impossible
            }
        }

        // Pour changer la sensibilité de la souris
        public void SetMouseSensitivity(int sensitivity)
        {
            // Clamp entre 1 et 20
            sensitivity = Math.Max(1, Math.Min(20, sensitivity));

            // Écrit dans le registre
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Mouse", writable: true))
            {
                key.SetValue("MouseSensitivity", sensitivity.ToString());
            }

            // Applique immédiatement
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, (UIntPtr)(uint)sensitivity, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        // Pour pas bouger la fenêtre https://stackoverflow.com/questions/907830/how-do-you-prevent-a-windows-from-being-moved
        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
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

        // Boucle de surveillance des processus
        private void monitor_taskmgr()
        {
            Thread monitorThread = new Thread(() => {
                while (true)
                {
                    Process[] processes = Process.GetProcessesByName("Taskmgr");
                    foreach (Process proc in processes)
                    {
                        block_count++;
                        try
                        {
                            proc.Kill();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erreur : " + ex.Message);
                        }

                        if (block_count >= max_blocks)
                        {
                            return;
                        }
                        break;
                    }
                    Thread.Sleep(500);
                }
            });

            monitorThread.IsBackground = true;
            monitorThread.Start();
        }
    }
}
