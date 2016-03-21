using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{ 
    public struct Point
    {
        public int X;
        public int Y;
    }

    public partial class Form1 : Form
    {
        private const int N = 10;

        private enum CellType                          
        {                                              
            Empty,                                     
            Body,                                      
            Food                                       
        }                                              
                                                       
        private CellType[,] board = new CellType[N, N];
        private Point[] snakeSegments = new Point[N*N];
        private Point[] freeCells = new Point[N*N];
        private int snakeLength = 1;                   
        private int offsetX;                           
        private int offsetY;                           
        private Random random = new Random();          
        private Timer timer1 = new Timer();            
        private Point food;                            
        private int x = 0;                             
        private int y = 0;                             
        private int size = 30;                         
                                                       
        private string direction = "Right";            
                                                       
        public Form1()                                 
        {                                              
            InitializeComponent();
            InitializeTimer();
            timer1.Enabled = false;
            ClientSize = new Size(N*size + 2*pictureBox1.Location.X, N*size + 2*pictureBox1.Location.Y);
            pictureBox1.Size = new Size(N*size + 1, N*size + 1);
            //Змейка
            for (int i = 0; i < N*N; i++)
            {
                snakeSegments[i].X = -1;
                snakeSegments[i].Y = -1;
            }
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    board[i, j] = CellType.Empty;
            snakeSegments[0].X = N/2;
            snakeSegments[0].Y = N/2;
            board[snakeSegments[0].Y, snakeSegments[0].X] = CellType.Body;
            //Еда
            do
            {
                food.X = random.Next(0, N);
                food.Y = random.Next(0, N);
            } while (board[food.Y, food.X] == CellType.Body);
            board[food.Y, food.X] = CellType.Food;
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen linePen = Pens.Black;
            Brush snakeBodyBrush = Brushes.DarkViolet;
            Brush foodBrush = Brushes.Red;
            Brush snakeHeadBrush = Brushes.Blue;
            int dx = 0;
            int dy = 0;

            //Рисование линий
            for (int i = 0; i < N + 1; i++)
            {
                e.Graphics.DrawLine(linePen, x, y + dy, x + N*size, y + dy);
                e.Graphics.DrawLine(linePen, x + dx, y, x + dx, y + N*size);
                dx += size;
                dy += size;
            }
            //Рисование змейки
            e.Graphics.FillRectangle(snakeHeadBrush, x + size*snakeSegments[0].X + 1, y + size*snakeSegments[0].Y + 1,
                size - 1, size - 1);
            for (int i = 1; i < snakeLength; i++)
                if ((snakeSegments[i].X != -1) && (snakeSegments[i].Y != -1))
                    e.Graphics.FillRectangle(snakeBodyBrush, x + size*snakeSegments[i].X + 1,
                        y + size*snakeSegments[i].Y + 1, size - 1, size - 1);

            //Рисование еды
            if(food.X != -1 && food.Y != -1)
                e.Graphics.FillRectangle(foodBrush, x + size*food.X + 1, y + size*food.Y + 1, size - 1, size - 1);
        }

        private void InitializeTimer()
        {
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Moving(direction);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            timer1.Enabled = true;
            if (e.KeyCode == Keys.Up)
            {
                direction = "Up";
            }
            if (e.KeyCode == Keys.Down)
            {
                direction = "Down";
            }
            if (e.KeyCode == Keys.Left)
            {
                direction = "Left";
            }
            if (e.KeyCode == Keys.Right)
            {
                direction = "Right";
            }
        }

        private void Restart()
        {
            timer1.Enabled = false;
            timer1.Interval = 500;
            ScoreLabel.Text = "0";
            for (int i = 0; i < snakeLength; i++)
            {
                snakeSegments[i].X = -1;
                snakeSegments[i].Y = -1;
            }
            snakeLength = 1;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    board[i, j] = CellType.Empty;
            snakeSegments[0].X = N/2;
            snakeSegments[0].Y = N/2;
            board[snakeSegments[0].Y, snakeSegments[0].X] = CellType.Body;
            do
            {
                food.X = random.Next(0, N);
                food.Y = random.Next(0, N);
            } while (board[food.Y, food.X] == CellType.Body);
            board[food.Y, food.X] = CellType.Food;
            pictureBox1.Refresh();
            
        }

        private void Moving(string dir)
        {
            offsetX = 0;
            offsetY = 0;

            if (dir == "Up")
                offsetY = -1;
            if (dir == "Down")
                offsetY = 1;
            if (dir == "Left")
                offsetX = -1;
            if (dir == "Right")
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
                (newHeadPositionY > N - 1) ||
                (newHeadPositionX > N - 1) ||
                (board[newHeadPositionY, newHeadPositionX] == CellType.Body))
            {
                timer1.Enabled = false;
                MessageBox.Show("You lost! =( Your Score: " + (snakeLength - 1));
                Restart();
                return;
            }

            //if ((snakeSegments[0].X + offsetX == food.X) && (snakeSegments[0].Y + offsetY == food.Y))
            if (board[snakeSegments[0].Y + offsetY,snakeSegments[0].X + offsetX] == CellType.Food)
            {
                snakeLength++;
                ScoreLabel.Text = (snakeLength-1).ToString();
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
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
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
                else
                {
                    int freeCell = random.Next(0, k);
                    food.X = freeCells[freeCell].X;
                    food.Y = freeCells[freeCell].Y;
                    board[food.Y, food.X] = CellType.Food;
                }
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