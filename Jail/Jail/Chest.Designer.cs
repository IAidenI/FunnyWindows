namespace Jail
{
    partial class Chest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBoxLoot = new System.Windows.Forms.RichTextBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.timerLoot = new System.Windows.Forms.Timer(this.components);
            this.panelChest = new System.Windows.Forms.Panel();
            this.btnDrop = new System.Windows.Forms.Button();
            this.panelDrop = new System.Windows.Forms.Panel();
            this.richTextBoxDrop = new System.Windows.Forms.RichTextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panelChest.SuspendLayout();
            this.panelDrop.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLoot
            // 
            this.richTextBoxLoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.richTextBoxLoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLoot.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLoot.Name = "richTextBoxLoot";
            this.richTextBoxLoot.ReadOnly = true;
            this.richTextBoxLoot.Size = new System.Drawing.Size(403, 226);
            this.richTextBoxLoot.TabIndex = 0;
            this.richTextBoxLoot.TabStop = false;
            this.richTextBoxLoot.Text = "";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(58, 23);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(297, 25);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Récupèration d\'un objet aléatoire";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(164, 64);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(76, 33);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Lancer";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timerLoot
            // 
            this.timerLoot.Tick += new System.EventHandler(this.timerLoot_Tick);
            // 
            // panelChest
            // 
            this.panelChest.Controls.Add(this.btnDrop);
            this.panelChest.Controls.Add(this.btnStart);
            this.panelChest.Controls.Add(this.labelTitle);
            this.panelChest.Controls.Add(this.richTextBoxLoot);
            this.panelChest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChest.Location = new System.Drawing.Point(0, 0);
            this.panelChest.Name = "panelChest";
            this.panelChest.Size = new System.Drawing.Size(403, 226);
            this.panelChest.TabIndex = 3;
            // 
            // btnDrop
            // 
            this.btnDrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnDrop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDrop.ForeColor = System.Drawing.Color.White;
            this.btnDrop.Location = new System.Drawing.Point(381, 3);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(19, 21);
            this.btnDrop.TabIndex = 3;
            this.btnDrop.Text = "?";
            this.btnDrop.UseVisualStyleBackColor = false;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // panelDrop
            // 
            this.panelDrop.Controls.Add(this.buttonOK);
            this.panelDrop.Controls.Add(this.richTextBoxDrop);
            this.panelDrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDrop.Location = new System.Drawing.Point(0, 0);
            this.panelDrop.Name = "panelDrop";
            this.panelDrop.Size = new System.Drawing.Size(403, 226);
            this.panelDrop.TabIndex = 4;
            this.panelDrop.Visible = false;
            // 
            // richTextBoxDrop
            // 
            this.richTextBoxDrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.richTextBoxDrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxDrop.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxDrop.Name = "richTextBoxDrop";
            this.richTextBoxDrop.ReadOnly = true;
            this.richTextBoxDrop.Size = new System.Drawing.Size(403, 226);
            this.richTextBoxDrop.TabIndex = 0;
            this.richTextBoxDrop.TabStop = false;
            this.richTextBoxDrop.Text = "";
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.ForeColor = System.Drawing.Color.White;
            this.buttonOK.Location = new System.Drawing.Point(164, 191);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // Chest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(403, 226);
            this.Controls.Add(this.panelDrop);
            this.Controls.Add(this.panelChest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Chest";
            this.ShowInTaskbar = false;
            this.Text = "Chest";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chest_FormClosing);
            this.Load += new System.EventHandler(this.Chest_Load);
            this.panelChest.ResumeLayout(false);
            this.panelChest.PerformLayout();
            this.panelDrop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLoot;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timerLoot;
        private System.Windows.Forms.Panel panelChest;
        private System.Windows.Forms.Button btnDrop;
        private System.Windows.Forms.Panel panelDrop;
        private System.Windows.Forms.RichTextBox richTextBoxDrop;
        private System.Windows.Forms.Button buttonOK;
    }
}