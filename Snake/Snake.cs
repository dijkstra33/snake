using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        const int N = 10;
        int[,] board = new int[N, N];
        int[] indexX = new int[N * N];
        int[] indexY = new int[N * N];
        int SnakeLength = 1;
        int offsetX;
        int offsetY;
        Random random = new Random();
        Timer timer1 = new Timer();
        int foodIndexX, foodIndexY;
        int x = 0, y = 0, size = 30;
        int dx = 0, dy = 0;
        string direction = "Right", previousDirection;
        //bool test = false;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            timer1.Enabled = false;
            ClientSize = new Size(N * size + 2 * pictureBox1.Location.X, N * size + 2 * pictureBox1.Location.Y);
            pictureBox1.Size = new Size(N * size + 1, N * size + 1);
            //Змейка
            for (int i = 0; i < N * N; i++)
            {
                indexX[i] = -1;
                indexY[i] = -1;
            }
            indexX[0] = N / 2;
            indexY[0] = N / 2;
            board[indexY[0], indexX[0]] = 1;
            //Еда
            do
            {
                foodIndexX = random.Next(0, N);
                foodIndexY = random.Next(0, N);
            } while (board[foodIndexY, foodIndexX] == 1);
            board[foodIndexY, foodIndexX] = 2;
        }

        

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Рисование линий
            Pen linePen = Pens.Black;
            Brush SnakeBodyBrush = Brushes.DarkViolet;
            Brush FoodBrush = Brushes.Red;
            Brush SnakeHeadBrush = Brushes.Blue;
            dx = dy = 0;

            /*for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine();
            }*/

            for (int i = 0; i < N + 1; i++)
            {
                e.Graphics.DrawLine(linePen, x, y + dy, x + N * size, y + dy);
                e.Graphics.DrawLine(linePen, x + dx, y, x + dx, y + N * size);
                dx += size;
                dy += size;
            }

            e.Graphics.FillRectangle(SnakeHeadBrush, x + size * indexX[0] + 1, y + size * indexY[0] + 1, size - 1, size - 1);

            for (int i = 1; i < SnakeLength; i++)
            {
                if ((indexX[i] != -1) && (indexY[i] != -1))
                    e.Graphics.FillRectangle(SnakeBodyBrush, x + size * indexX[i] + 1, y + size * indexY[i] + 1, size - 1, size - 1);
            }


            //Рисование еды
            e.Graphics.FillRectangle(FoodBrush, x + size * foodIndexX + 1, y + size * foodIndexY + 1, size - 1, size - 1);

        }

        private void InitializeTimer()
        {
            timer1.Interval = 500;
            //timer1.Enabled = true;
            timer1.Tick += timer1_Tick;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Moving(direction);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            timer1.Enabled = true;
            if (e.KeyCode == Keys.Up) { previousDirection = direction; direction = "Up"; }
            if (e.KeyCode == Keys.Down) { previousDirection = direction; direction = "Down"; }
            if (e.KeyCode == Keys.Left) { previousDirection = direction; direction = "Left"; }
            if (e.KeyCode == Keys.Right) { previousDirection = direction; direction = "Right"; }
        }

        private void Restart()
        {
            timer1.Enabled = false;
            timer1.Interval = 500;
            for (int i = 0; i < SnakeLength; i++)
            {
                indexX[i] = 0;
                indexY[i] = 0;
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i, j] = 0;
                }
            }
            indexX[0] = N / 2;
            indexY[0] = N / 2;
            board[indexY[0], indexX[0]] = 1;
            SnakeLength = 1;
            pictureBox1.Refresh();
        }

        private void Moving(string direction)
        {
            offsetX = 0;
            offsetY = 0;

            if (direction == "Up") 
                offsetY = -1;
            if (direction == "Down")
                offsetY = 1;
            if (direction == "Left")
                offsetX = -1;
            if (direction == "Right")
                offsetX = 1;
            if ((indexX[0] + offsetX == indexX[1]) && (indexY[0] + offsetY == indexY[1]))
            {
                direction = previousDirection;
                if (direction == "Up")
                    offsetY = -1;
                if (direction == "Down")
                    offsetY = 1;
                if (direction == "Left")
                    offsetX = -1;
                if (direction == "Right")
                    offsetX = 1;
            }

            Console.WriteLine(direction + " " + previousDirection);

            int newHeadPositionX = indexX[0] + offsetX;
            int newHeadPositionY = indexY[0] + offsetY;

            if ((newHeadPositionY < 0) ||
                (newHeadPositionX < 0) ||
                (newHeadPositionY > N - 1) ||
                (newHeadPositionX > N - 1) ||
                (board[newHeadPositionY, newHeadPositionX] == 1))
            {
                timer1.Enabled = false;
                MessageBox.Show("You lost! =( Your Score: " + (SnakeLength - 1));
                Restart();
                return;
            }

            if ((indexX[0] + offsetX == foodIndexX) && (indexY[0] + offsetY == foodIndexY))
            {
                SnakeLength++;
                timer1.Interval -= 10;
                for (int i = SnakeLength - 1; i > 0; i--)
                {
                    indexX[i] = indexX[i - 1];
                    indexY[i] = indexY[i - 1];
                }
                indexX[0] = foodIndexX;
                indexY[0] = foodIndexY;
                board[foodIndexY, foodIndexX] = 1;
                do
                {
                    foodIndexX = random.Next(0, N);
                    foodIndexY = random.Next(0, N);
                }
                while (board[foodIndexY, foodIndexX] == 1);

                board[foodIndexY, foodIndexX] = 2;
                pictureBox1.Refresh();
            }
            else
            {
                board[indexY[SnakeLength - 1], indexX[SnakeLength - 1]] = 0;
                for (int i = SnakeLength - 1; i > 0; i--)
                {
                    indexX[i] = indexX[i - 1];
                    indexY[i] = indexY[i - 1];
                }
                
                indexY[0] += offsetY;
                indexX[0] += offsetX;
                board[indexY[0], indexX[0]] = 1;
                board[indexY[SnakeLength - 1], indexX[SnakeLength - 1]] = 0;
                //indexX[SnakeLength - 1] = -1;
                //indexY[SnakeLength - 1] = -1;
                for (int i = 0; i < SnakeLength; i++)
                    board[indexY[i], indexX[i]] = 1;
                pictureBox1.Refresh();
            }
        }
    }
}
