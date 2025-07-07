using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fog
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        Point mouse_position = new Point();
        private const int hole_diameter = 100;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateHole()
        {
            int x = mouse_position.X - (hole_diameter / 2);
            int y = mouse_position.Y - (hole_diameter / 2);

            // Créer une région "pleine" puis en enlever un rond au centre
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new Rectangle(0, 0, this.Width, this.Height));

            path.AddEllipse(x, y, hole_diameter, hole_diameter);

            this.Region = new Region(path);
            this.Region.Exclude(new Region(new Rectangle(x, y, hole_diameter, hole_diameter)));
        }

        private void timerCursor_Tick(object sender, EventArgs e)
        {
            // Récupère la position de la souris
            GetCursorPos(ref mouse_position);
            UpdateHole();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (this.BackgroundImage != null)
            {
                Graphics g = e.Graphics;

                // Facteur de zooma
                float zoom = 1.5f;

                int newWidth = (int)(this.BackgroundImage.Width * zoom);
                int newHeight = (int)(this.BackgroundImage.Height * zoom);

                // Centrer l'image zoomée
                int x = (this.ClientSize.Width - newWidth) / 2;
                int y = (this.ClientSize.Height - newHeight) / 2;

                g.DrawImage(this.BackgroundImage, new Rectangle(x, y, newWidth, newHeight));
            }
        }

        // Empêche le alt + tab (doit être combiner à this.ShowInTaskbar = false; pour marcher)
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
    }
}
