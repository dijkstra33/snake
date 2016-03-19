using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        const int n = 5;
        int[,] board = new int[n, n];
        int[] index1 = new int[n*n];
        int[] index2 = new int[n*n];
        int SnakeLength = 1;
        Random random = new Random();
        Timer timer1 = new Timer();
        int foodindex1, foodindex2;
        int x = 20, y = 20, size = 30;
        int dx = 0, dy = 0;
        string direction = "Right";
        //bool test = false;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            timer1.Start();
            timer1.Enabled = true;
            pictureBox1.Size = new Size(40 + n * size, 40 + n * size);
            ClientSize = new Size(40 + n * size, 40 + n * size);
            
            //Змейка
            board[n/2, n/2] = 1;
            for (int i = 0; i < n*n; i++)
            {
                index1[i] = -1;
                index2[i] = -1;
            }
            index1[0] = n/2;
            index2[0] = n/2;
            foodindex1 = random.Next(0, n);
            foodindex2 = random.Next(0, n);
            board[foodindex2, foodindex1] = 2;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Рисование линий
            Pen MyPen = Pens.Black;
            Brush MyBrush = Brushes.Blue;
            Brush FoodBrush = Brushes.Red;
            dx = dy = 0;

            for (int i = 0; i < n + 1; i++)
            {
                e.Graphics.DrawLine(MyPen, x, y + dy, x + n * size, y + dy);
                e.Graphics.DrawLine(MyPen, x + dx, y, x + dx, y + n * size);
                dx += size;
                dy += size;
            }
            for (int i = 0; i < SnakeLength; i++)
                board[index2[i], index1[i]] = 1;

            for (int i = 0; i < SnakeLength; i++)
            {
                if((index1[i] != -1) && (index2[i] != -1))
                    e.Graphics.FillRectangle(MyBrush, x + size * index1[i] + 1, y + size * index2[i] + 1, size - 1, size - 1);
            }

            if ((index1[0] == foodindex1) && (index2[0] == foodindex2))
            {
                SnakeLength++;
                index1[SnakeLength - 1] = index1[SnakeLength - 2];
                index2[SnakeLength - 1] = index2[SnakeLength - 2];
                foodindex1 = random.Next(0, n);
                foodindex2 = random.Next(0, n);

                while (board[foodindex1,foodindex2] == 1)
                {
                    foodindex1 = random.Next(0, n);
                    foodindex2 = random.Next(0, n);
                }
                board[foodindex2, foodindex1] = 2;
            }
            //Рисование еды
            e.Graphics.FillRectangle(FoodBrush, x + size * foodindex1 + 1, y + size * foodindex2 + 1, size - 1, size - 1);
                 
        }

        private void InitializeTimer()
        {
            timer1.Interval = 500;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_Tick);
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Moving(direction);
            pictureBox1.Refresh();
        }

        private void PressedKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                direction = "Up";
            if (e.KeyCode == Keys.Down)
                direction = "Down";
            if (e.KeyCode == Keys.Left)
                direction = "Left";
            if (e.KeyCode == Keys.Right)
                direction = "Right";
        }

        private void OnPress(KeyEventArgs e)
        {
            Moving(direction);
            pictureBox1.Refresh();
        }

        private void Moving(string direction) //Перемещение
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine();
            }
            if (direction == "Up")
            {
                board[index2[SnakeLength - 1], index1[SnakeLength - 1]] = 0;
                for (int i = SnakeLength - 1; i > 0; i--)
                {
                    if((index2[i] == index2[0] - 1) && (index1[i] == index1[0]))
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                        return;
                    }
                    index1[i] = index1[i - 1];
                    index2[i] = index2[i - 1];
                }
                if (index2[0] - 1 < 0)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                    return;
                }
                index2[0]--;
                board[index2[0], index1[0]] = 1;
                index1[SnakeLength] = -1;
                index2[SnakeLength] = -1;
            }
            if (direction == "Down")
            {
                board[index2[SnakeLength - 1], index1[SnakeLength - 1]] = 0;
                for (int i = SnakeLength; i > 0; i--)
                {
                    if ((index2[i] == index2[0] + 1) && (index1[i] == index1[0]))
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                        return;
                    }
                    index1[i] = index1[i - 1];
                    index2[i] = index2[i - 1];
                }
                if (index2[0] + 1 > n - 1)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                    return;
                }
            index2[0]++;
            board[index2[0], index1[0]] = 1;
            index1[SnakeLength] = -1;
            index2[SnakeLength] = -1;
            }
            if (direction == "Left")
            {
                board[index2[SnakeLength - 1], index1[SnakeLength - 1]] = 0;
                for (int i = SnakeLength; i > 0; i--)
                {
                    if ((index2[i] == index2[0]) && (index1[i] == index1[0] - 1))
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                        return;
                    }
                    index1[i] = index1[i - 1];
                    index2[i] = index2[i - 1];
                }
                if (index1[0] - 1 < 0)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                    return;
                }
                index1[0]--;
                board[index2[0], index1[0]] = 1;
                index1[SnakeLength] = -1;
                index2[SnakeLength] = -1;
            }
            if (direction == "Right")
            {
                board[index2[SnakeLength - 1], index1[SnakeLength - 1]] = 0;
                for (int i = SnakeLength; i > 0; i--)
                {
                    if ((index2[i] == index2[0]) && (index1[i] == index1[0] + 1))
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                        return;
                    }
                    index1[i] = index1[i - 1];
                    index2[i] = index2[i - 1];
                }
                if (index1[0] + 1 > n - 1)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                    return;
                }
                index1[0]++;
                board[index2[0], index1[0]] = 1;
                index1[SnakeLength] = -1;
                index2[SnakeLength] = -1;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) direction = "Up";
            if (e.KeyCode == Keys.Down) direction = "Down";
            if (e.KeyCode == Keys.Left) direction = "Left";
            if (e.KeyCode == Keys.Right) direction = "Right";
        }
    }
}
