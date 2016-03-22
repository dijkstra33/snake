using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public enum CellType
    {
        Empty,
        Body,
        Food
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public partial class Form1 : Form
    {
        private const int CellsAmount = 10;
        private const int CellSize = 30;

        private CellType[,] board = new CellType[CellsAmount, CellsAmount];
        private Point[] snakeSegments = new Point[CellsAmount*CellsAmount];
        private Point[] freeCells = new Point[CellsAmount*CellsAmount];
        private int snakeLength = 1;
        private int offsetX;
        private int offsetY;
        private Random random = new Random();
        private Timer timer1 = new Timer();
        private Point food;
        private Direction currentSnakeDirection;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            ClientSize = new Size(CellsAmount*CellSize + 2*pictureBox1.Location.X, CellsAmount*CellSize + 2*pictureBox1.Location.Y);
            pictureBox1.Size = new Size(CellsAmount*CellSize + 1, CellsAmount*CellSize + 1);
            Restart();
        }

        private void DrawSnake(PaintEventArgs e)
        {
            Brush snakeHeadBrush = Brushes.Blue;
            Brush snakeBodyBrush = Brushes.DarkViolet;
            e.Graphics.FillRectangle(snakeHeadBrush, CellSize*snakeSegments[0].X + 1, CellSize*snakeSegments[0].Y + 1,
                CellSize - 1, CellSize - 1);
            for (int i = 1; i < snakeLength; i++)
                if ((snakeSegments[i].X != -1) && (snakeSegments[i].Y != -1))
                    e.Graphics.FillRectangle(snakeBodyBrush, CellSize*snakeSegments[i].X + 1,
                        CellSize*snakeSegments[i].Y + 1, CellSize - 1, CellSize - 1);
        }

        private void DrawLines(PaintEventArgs e)
        {
            int dx = 0;
            int dy = 0;
            Pen linePen = Pens.Black;
            for (int i = 0; i < CellsAmount + 1; i++)
            {
                e.Graphics.DrawLine(linePen, 0, dy, CellsAmount*CellSize, dy);
                e.Graphics.DrawLine(linePen, dx, 0, dx, CellsAmount*CellSize);
                dx += CellSize;
                dy += CellSize;
            }
        }

        private void DrawFood(PaintEventArgs e)
        {
            Brush foodBrush = Brushes.Red;
            e.Graphics.FillRectangle(foodBrush, CellSize*food.X + 1, CellSize*food.Y + 1, CellSize - 1, CellSize - 1);
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            DrawLines(e);
            DrawSnake(e);
            DrawFood(e);
        }

        private void InitializeTimer()
        {
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Moving(currentSnakeDirection);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            timer1.Enabled = true;
            if (e.KeyCode == Keys.Up)
            {
                currentSnakeDirection = Direction.Up;
            }
            if (e.KeyCode == Keys.Down)
            {
                currentSnakeDirection = Direction.Down;
            }
            if (e.KeyCode == Keys.Left)
            {
                currentSnakeDirection = Direction.Left;
            }
            if (e.KeyCode == Keys.Right)
            {
                currentSnakeDirection = Direction.Right;
            }
        }

        private void Restart()
        {
            timer1.Enabled = false;
            timer1.Interval = 500;
            ScoreLabel.Text = "0";
            for (int i = 0; i < CellsAmount*CellsAmount; i++)
            {
                snakeSegments[i].X = -1;
                snakeSegments[i].Y = -1;
            }
            for (int i = 0; i < CellsAmount; i++)
                for (int j = 0; j < CellsAmount; j++)
                    board[i, j] = CellType.Empty;
            snakeLength = 1;
            snakeSegments[0].X = CellsAmount/2;
            snakeSegments[0].Y = CellsAmount/2;
            board[snakeSegments[0].Y, snakeSegments[0].X] = CellType.Body;
            do
            {
                food.X = random.Next(0, CellsAmount);
                food.Y = random.Next(0, CellsAmount);
            } while (board[food.Y, food.X] == CellType.Body);
            board[food.Y, food.X] = CellType.Food;
            currentSnakeDirection = Direction.Right;
            pictureBox1.Refresh();
        }

        private void Moving(Direction dir)
        {
            offsetX = 0;
            offsetY = 0;

            if (dir == Direction.Up)
                offsetY = -1;
            if (dir == Direction.Down)
                offsetY = 1;
            if (dir == Direction.Left)
                offsetX = -1;
            if (dir == Direction.Right)
                offsetX = 1;
            if ((snakeSegments[0].X + offsetX == snakeSegments[1].X) &&
                (snakeSegments[0].Y + offsetY == snakeSegments[1].Y))
            {
                offsetX = -offsetX;
                offsetY = -offsetY;
            }

            int newHeadPositionX = snakeSegments[0].X + offsetX;
            int newHeadPositionY = snakeSegments[0].Y + offsetY;

            if ((newHeadPositionY < 0) ||
                (newHeadPositionX < 0) ||
                (newHeadPositionY > CellsAmount - 1) ||
                (newHeadPositionX > CellsAmount - 1) ||
                (board[newHeadPositionY, newHeadPositionX] == CellType.Body))
            {
                timer1.Enabled = false;
                MessageBox.Show("You lost! =( Your Score: " + (snakeLength - 1));
                Restart();
                return;
            }

            if (board[snakeSegments[0].Y + offsetY, snakeSegments[0].X + offsetX] == CellType.Food)
            {
                snakeLength++;
                ScoreLabel.Text = (snakeLength - 1).ToString();
                timer1.Interval -= 5;
                for (int i = snakeLength - 1; i > 0; i--)
                {
                    snakeSegments[i].X = snakeSegments[i - 1].X;
                    snakeSegments[i].Y = snakeSegments[i - 1].Y;
                }
                snakeSegments[0].X = food.X;
                snakeSegments[0].Y = food.Y;

                board[food.Y, food.X] = CellType.Body;
                food.X = -1;
                food.Y = -1;

                int k = 0;
                for (int i = 0; i < CellsAmount; i++)
                {
                    for (int j = 0; j < CellsAmount; j++)
                    {
                        if (board[i, j] == CellType.Empty)
                        {
                            freeCells[k].X = j;
                            freeCells[k].Y = i;
                            k++;
                        }
                    }
                }
                if (k == 0)
                {
                    timer1.Stop();
                    pictureBox1.Refresh();
                    MessageBox.Show("You win! =) Your score: " + (snakeLength - 1));
                    Restart();
                    return;
                }
                int freeCell = random.Next(0, k);
                food.X = freeCells[freeCell].X;
                food.Y = freeCells[freeCell].Y;
                board[food.Y, food.X] = CellType.Food;
                pictureBox1.Refresh();
            }
            else
            {
                board[snakeSegments[snakeLength - 1].Y, snakeSegments[snakeLength - 1].X] = 0;
                for (int i = snakeLength - 1; i > 0; i--)
                {
                    snakeSegments[i].X = snakeSegments[i - 1].X;
                    snakeSegments[i].Y = snakeSegments[i - 1].Y;
                }

                snakeSegments[0].Y += offsetY;
                snakeSegments[0].X += offsetX;
                board[snakeSegments[0].Y, snakeSegments[0].X] = CellType.Body;
                board[snakeSegments[snakeLength - 1].Y, snakeSegments[snakeLength - 1].X] = 0;
                for (int i = 0; i < snakeLength; i++)
                    board[snakeSegments[i].Y, snakeSegments[i].X] = CellType.Body;
                pictureBox1.Refresh();
            }
        }
    }
}