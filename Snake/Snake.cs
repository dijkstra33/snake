using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        const int N = 10;
        enum CellType {empty, body, food};
        CellType [,] board = new CellType[N, N];
        struct Point
        {
            public int X;
            public int Y;
        }
        Point[] SnakeSegments = new Point[N * N];
        int SnakeLength = 1;
        int offsetX;
        int offsetY;
        Random random = new Random();
        Timer timer1 = new Timer();
        int foodIndexX, foodIndexY;
        int x = 0, y = 0, size = 30;
        int dx = 0, dy = 0;
        string direction = "Right";
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
                SnakeSegments[i].X = -1;
                SnakeSegments[i].Y = -1;
            }
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    board[i, j] = CellType.empty;
            SnakeSegments[0].X = N / 2;
            SnakeSegments[0].Y = N / 2;
            board[SnakeSegments[0].Y, SnakeSegments[0].X] = CellType.body;
            //Еда
            do
            {
                foodIndexX = random.Next(0, N);
                foodIndexY = random.Next(0, N);
            } while (board[foodIndexY, foodIndexX] == CellType.body);
            board[foodIndexY, foodIndexX] = CellType.food;
        }

        

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen linePen = Pens.Black;
            Brush SnakeBodyBrush = Brushes.DarkViolet;
            Brush FoodBrush = Brushes.Red;
            Brush SnakeHeadBrush = Brushes.Blue;
            dx = dy = 0;
            
            //Рисование линий
            for (int i = 0; i < N + 1; i++)
            {
                e.Graphics.DrawLine(linePen, x, y + dy, x + N * size, y + dy);
                e.Graphics.DrawLine(linePen, x + dx, y, x + dx, y + N * size);
                dx += size;
                dy += size;
            }

            //Рисование змейки
            e.Graphics.FillRectangle(SnakeHeadBrush, x + size * SnakeSegments[0].X + 1, y + size * SnakeSegments[0].Y + 1, size - 1, size - 1);
            for (int i = 1; i < SnakeLength; i++)
                if ((SnakeSegments[i].X != -1) && (SnakeSegments[i].Y != -1))
                    e.Graphics.FillRectangle(SnakeBodyBrush, x + size * SnakeSegments[i].X + 1, y + size * SnakeSegments[i].Y + 1, size - 1, size - 1);

            //Рисование еды
            e.Graphics.FillRectangle(FoodBrush, x + size * foodIndexX + 1, y + size * foodIndexY + 1, size - 1, size - 1);

        }

        private void InitializeTimer()
        {
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //previousDirection = direction;
            Moving(direction);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            timer1.Enabled = true;
            if (e.KeyCode == Keys.Up) { direction = "Up"; }
            if (e.KeyCode == Keys.Down) { direction = "Down"; }
            if (e.KeyCode == Keys.Left) {  direction = "Left"; }
            if (e.KeyCode == Keys.Right) { direction = "Right"; }
        }

        private void Restart()
        {
            timer1.Enabled = false;
            timer1.Interval = 500;
            for (int i = 0; i < SnakeLength; i++)
            {
                SnakeSegments[i].X = 0;
                SnakeSegments[i].Y = 0;
            }
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    board[i, j] = CellType.empty;
            SnakeSegments[0].X = N / 2;
            SnakeSegments[0].Y = N / 2;
            board[SnakeSegments[0].Y, SnakeSegments[0].X] = CellType.body;
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
            if ((SnakeSegments[0].X + offsetX == SnakeSegments[1].X) && (SnakeSegments[0].Y + offsetY == SnakeSegments[1].Y))
            {
                offsetX = -offsetX;
                offsetY = -offsetY;
            }

            int newHeadPositionX = SnakeSegments[0].X + offsetX;
            int newHeadPositionY = SnakeSegments[0].Y + offsetY;

            if ((newHeadPositionY < 0) ||
                (newHeadPositionX < 0) ||
                (newHeadPositionY > N - 1) ||
                (newHeadPositionX > N - 1) ||
                (board[newHeadPositionY, newHeadPositionX] == CellType.body))
            {
                timer1.Enabled = false;
                MessageBox.Show("You lost! =( Your Score: " + (SnakeLength - 1));
                Restart();
                return;
            }

            if ((SnakeSegments[0].X + offsetX == foodIndexX) && (SnakeSegments[0].Y + offsetY == foodIndexY))
            {
                SnakeLength++;
                timer1.Interval -= 5;
                for (int i = SnakeLength - 1; i > 0; i--)
                {
                    SnakeSegments[i].X = SnakeSegments[i - 1].X;
                    SnakeSegments[i].Y = SnakeSegments[i - 1].Y;
                }
                SnakeSegments[0].X = foodIndexX;
                SnakeSegments[0].Y = foodIndexY;
                board[foodIndexY, foodIndexX] = CellType.body;
                do
                {
                    foodIndexX = random.Next(0, N);
                    foodIndexY = random.Next(0, N);
                }
                while (board[foodIndexY, foodIndexX] == CellType.body);

                board[foodIndexY, foodIndexX] = CellType.food;
                pictureBox1.Refresh();
            }
            else
            {
                board[SnakeSegments[SnakeLength - 1].Y, SnakeSegments[SnakeLength - 1].X] = 0;
                for (int i = SnakeLength - 1; i > 0; i--)
                {
                    SnakeSegments[i].X = SnakeSegments[i - 1].X;
                    SnakeSegments[i].Y = SnakeSegments[i - 1].Y;
                }
                
                SnakeSegments[0].Y += offsetY;
                SnakeSegments[0].X += offsetX;
                board[SnakeSegments[0].Y, SnakeSegments[0].X] = CellType.body;
                board[SnakeSegments[SnakeLength - 1].Y, SnakeSegments[SnakeLength - 1].X] = 0;
                for (int i = 0; i < SnakeLength; i++)
                    board[SnakeSegments[i].Y, SnakeSegments[i].X] = CellType.body;
                pictureBox1.Refresh();
            }
        }
    }
}
