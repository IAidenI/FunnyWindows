namespace Jail
{
    partial class Jail
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelGame = new System.Windows.Forms.Panel();
            this.labelCursor = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.labelInstruction = new System.Windows.Forms.Label();
            this.pictureBoxMaze = new System.Windows.Forms.PictureBox();
            this.panelStart = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.timerError = new System.Windows.Forms.Timer(this.components);
            this.timerCheckCursor = new System.Windows.Forms.Timer(this.components);
            this.timerCursor = new System.Windows.Forms.Timer(this.components);
            this.timerInvertMouse = new System.Windows.Forms.Timer(this.components);
            this.panelGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaze)).BeginInit();
            this.panelStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGame
            // 
            this.panelGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panelGame.Controls.Add(this.labelCursor);
            this.panelGame.Controls.Add(this.labelError);
            this.panelGame.Controls.Add(this.labelInstruction);
            this.panelGame.Controls.Add(this.pictureBoxMaze);
            this.panelGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGame.Location = new System.Drawing.Point(0, 0);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(800, 529);
            this.panelGame.TabIndex = 0;
            // 
            // labelCursor
            // 
            this.labelCursor.AutoSize = true;
            this.labelCursor.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCursor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(193)))), ((int)(((byte)(228)))));
            this.labelCursor.Location = new System.Drawing.Point(397, 506);
            this.labelCursor.Name = "labelCursor";
            this.labelCursor.Size = new System.Drawing.Size(40, 18);
            this.labelCursor.TabIndex = 3;
            this.labelCursor.Text = "Prêt";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(588, 505);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(209, 20);
            this.labelError.TabIndex = 2;
            this.labelError.Text = "Vous êtes tombé dans la lave.";
            this.labelError.Visible = false;
            // 
            // labelInstruction
            // 
            this.labelInstruction.AutoSize = true;
            this.labelInstruction.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInstruction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.labelInstruction.Location = new System.Drawing.Point(3, 503);
            this.labelInstruction.Name = "labelInstruction";
            this.labelInstruction.Size = new System.Drawing.Size(347, 21);
            this.labelInstruction.TabIndex = 0;
            this.labelInstruction.Text = "Objectif : Vous devez trouver la bonne sortie";
            // 
            // pictureBoxMaze
            // 
            this.pictureBoxMaze.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxMaze.Image = global::Jail.Properties.Resources.maze;
            this.pictureBoxMaze.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxMaze.Name = "pictureBoxMaze";
            this.pictureBoxMaze.Size = new System.Drawing.Size(800, 500);
            this.pictureBoxMaze.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMaze.TabIndex = 1;
            this.pictureBoxMaze.TabStop = false;
            this.pictureBoxMaze.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxMaze_Paint);
            this.pictureBoxMaze.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMaze_MouseClickAsync);
            this.pictureBoxMaze.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMaze_MouseMove);
            // 
            // panelStart
            // 
            this.panelStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panelStart.Controls.Add(this.labelTitle);
            this.panelStart.Controls.Add(this.btnStart);
            this.panelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStart.Location = new System.Drawing.Point(0, 0);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new System.Drawing.Size(800, 529);
            this.panelStart.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(276, 190);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(242, 32);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "On va jouer à un jeu.";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(344, 253);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(87, 36);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timerError
            // 
            this.timerError.Interval = 2000;
            this.timerError.Tick += new System.EventHandler(this.timerError_Tick);
            // 
            // timerCheckCursor
            // 
            this.timerCheckCursor.Enabled = true;
            this.timerCheckCursor.Interval = 30;
            this.timerCheckCursor.Tick += new System.EventHandler(this.timerCheckCursor_Tick);
            // 
            // timerCursor
            // 
            this.timerCursor.Interval = 50;
            this.timerCursor.Tick += new System.EventHandler(this.timerCursor_Tick);
            // 
            // timerInvertMouse
            // 
            this.timerInvertMouse.Interval = 10;
            this.timerInvertMouse.Tick += new System.EventHandler(this.timerInvertMouse_Tick);
            // 
            // Jail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 529);
            this.Controls.Add(this.panelStart);
            this.Controls.Add(this.panelGame);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Jail";
            this.ShowInTaskbar = false;
            this.Text = "Jail";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Jail_FormClosing);
            this.Load += new System.EventHandler(this.Jail_Load);
            this.panelGame.ResumeLayout(false);
            this.panelGame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaze)).EndInit();
            this.panelStart.ResumeLayout(false);
            this.panelStart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGame;
        private System.Windows.Forms.Panel panelStart;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label labelInstruction;
        private System.Windows.Forms.PictureBox pictureBoxMaze;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Timer timerError;
        private System.Windows.Forms.Timer timerCheckCursor;
        private System.Windows.Forms.Timer timerCursor;
        private System.Windows.Forms.Label labelCursor;
        private System.Windows.Forms.Timer timerInvertMouse;
    }
}

