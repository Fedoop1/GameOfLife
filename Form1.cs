using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GameOfLife
{
    
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int cols;
        private int rows;
        public Form1()
        {
            InitializeComponent();
        }

        public void NextGeneration()
        {
            graphics.Clear(Color.White);

            var newFileds = new bool[cols, rows];

            var i = Convert.ToInt32(textBox1.Text) + 1;

            textBox1.Text = Convert.ToString(i);

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {

                    var neighboursCount = CountNeightbours(x, y);

                    var haslife = field[x, y];

                    if (!haslife && (neighboursCount == 3))
                    {
                        newFileds[x, y] = true;
                    }
                    else if (haslife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newFileds[x, y] = false;
                    }
                    else newFileds[x, y] = field[x, y];


                    if (haslife)
                    {
                        graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution, resolution);
                    } 
                }
            }
            field = newFileds;
            pictureBox1.Refresh();
        }

        public int CountNeightbours(int x, int y)
        {
            int Count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;

                    var row = (y + j + rows) % rows;

                    var Self = col == x && row == y;

                    var hasLife = field[x, y];

                    if (hasLife && (!Self))
                    {
                        Count++;
                    }
                }
                
            }

            return Count;
        }
        public void StartGame()
        {
            if (timer1.Enabled)
                return;

            Resolution.Enabled = false;
            Dencity.Enabled = false;

            resolution = (int)Resolution.Value;

            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;

            field = new bool[cols,rows];

            Random rnd = new Random();

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next((int)Dencity.Value) == 0;
                }
            }

            
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.FillRectangle(Brushes.Black, 0, 0, resolution, resolution);
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
            NextGeneration();
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
            if(!timer1.Enabled)
            {
                return;
            }

            var x = e.Location.X / resolution;

            var y = e.Location.Y / resolution;

            var ValidatePassed = ValidateMousePosition(x, y);

            if(e.Button == MouseButtons.Left)
            {

                if(ValidatePassed) 
                    field[x, y] = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (ValidatePassed) 
                    field[x, y] = false;
            }

        }
        public bool ValidateMousePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

    }
}
