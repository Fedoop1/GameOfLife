using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class GameEngine
    {
        private bool[,] field;
        private readonly int cols;
        private readonly int rows;
        
        public uint currGenerations { get; private set; } = 0;

        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];
            Random rnd = new Random();

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next(density) == 0;
                }
            }
        }

        public void NextGeneration()
        {
            var newFileds = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {

                    var neighboursCount = CountNeightbours(x, y);

                    var haslife = field[x, y];

                    if (!haslife && neighboursCount == 3)
                    {
                        newFileds[x, y] = true;
                    }
                    else if (haslife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newFileds[x, y] = false;
                    }
                    else newFileds[x, y] = field[x, y];
                }
            }

            field = newFileds;
            currGenerations++;
        }

        public bool[,] GetCurrentGeneration()
        {
            var result = new bool[cols, rows];
            {
                for (int x = 0; x < cols; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        result[x, y] = field[x, y];
                    }
                }
                return result;
            }
        }

        private int CountNeightbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;

                    var row = (y + j + rows) % rows;

                    var Self = col == x && row == y;

                    var hasLife = field[x, y];

                    if (hasLife && !Self)
                    {
                        count++;
                    }
                }

            }

            return count;
        }

        public bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                field[x, y] = state;
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }
    }
}
