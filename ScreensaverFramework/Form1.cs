using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ScreensaverFramework
{
    public partial class Form1 : Form
    {
        private bool isActive;

        private Point mouseLocation;
        ScreenSaverIterator screensaver;
        Bitmap currentImage;
        private Font frameFont;
        private SolidBrush frameSolidBrush;
        DateTime preIterate;
        DateTime postIterate;
        TimeSpan timeToWait;

        public Form1()
        {
            InitializeComponent();
            SetupScreenSaver();
        }

        private void SetupScreenSaver()
        {
            //Set application to paint itself
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            this.Capture = true;

            Cursor.Hide();
            Bounds = Screen.PrimaryScreen.Bounds;
            WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            //TopMost = true;

            ShowInTaskbar = false;
            DoubleBuffered = true;
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isActive)
            {
                mouseLocation = MousePosition;
                isActive = true;
            }
            else
            {
                if (Math.Abs(MousePosition.X - mouseLocation.X) > 10 || (Math.Abs(MousePosition.Y - mouseLocation.Y)) > 10)
                {
                    Close();
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            DrawScreen(e.Graphics);
        }
        private void DrawScreen(Graphics g)
        {
            g.Clear(Color.Black);
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            if (currentImage != null)
            {
                g.DrawImage(currentImage, 0f, 0f);
            }
            
            if (screensaver != null)
            {
                g.DrawString("" + screensaver.getCurrentIteration(), frameFont, frameSolidBrush, 20, Screen.PrimaryScreen.Bounds.Height - frameFont.GetHeight(g) - 20);
            }
        }
        private void UpdateScreen()
        {

            if (screensaver.getCurrentIteration() == screensaver.getMaxIteration())
            {
                setUpIterator();
            }
            preIterate = DateTime.Now;
            screensaver.iterate();
            currentImage = screensaver.draw();
            int framerate = screensaver.getFrameRate();
            if(framerate > 0)
            {
                postIterate = DateTime.Now;
                timeToWait = (TimeSpan.FromSeconds(1.0 / (double)framerate).Subtract(postIterate.Subtract(preIterate)));
                if(timeToWait.CompareTo(TimeSpan.Zero) >= 0)
                {
                    Thread.Sleep(timeToWait);
                }
            }
        }

        private void setUpIterator()
        {
            screensaver.setUp();
        }

        private async void runScreenSaver()
        {
            chooseScreensaver();
            setUpIterator();
            while (true)
            {
                Task t = new Task(UpdateScreen);
                t.Start();
                await t.ConfigureAwait(true);
                this.Refresh();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            runScreenSaver();
            
            FontFamily fontFamily = new FontFamily("Arial");
            frameFont = new Font(
               fontFamily,
               30,
               FontStyle.Regular,
               GraphicsUnit.Pixel);
            frameSolidBrush = new SolidBrush(Color.White);
            

        }

        public void chooseScreensaver()
        {
            screensaver = ScreenSaverManager.getNewScreensaver(Screen.PrimaryScreen.Bounds.Width,Screen.PrimaryScreen.Bounds.Height);
        }

    }
}
