using System;
using System.Drawing;
using System.Windows.Forms;


namespace GameOfLife
{

    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private GameEngine gameEngine;
        public Form1()
        {
            InitializeComponent();
        }

        private void DrawNextGeneration()
        {
            graphics.Clear(Color.White);
            var field = gameEngine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0);x++)
            {   
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                    { graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution - 1, resolution - 1); }
                }
            }
            
            
            pictureBox1.Refresh();
            gameEngine.NextGeneration();
        }

       
        public void StartGame()
        {
            if (timer1.Enabled)
                return;

            Resolution.Enabled = false;
            Dencity.Enabled = false;

            resolution = (int)Resolution.Value;

            gameEngine = new GameEngine
                (
                    rows: pictureBox1.Height / resolution,
                    cols: pictureBox1.Width / resolution,
                    density:(int)Dencity.Minimum+(int)Dencity.Maximum-(int)Dencity.Value
                );

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        public void StopGame()
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();
            Resolution.Enabled = true;
            Dencity.Enabled = true;
        }
        public void timer1_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        public void BtnStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        public void BtnStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        public void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
            {
                return;
            }

            

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.AddCell(x, y);

            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.RemoveCell(x, y);
            }

        }

    }
}
