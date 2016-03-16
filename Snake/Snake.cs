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
        const int n = 10;
        //enum State { head, tail, body, food, empty };
        int[,] board = new int[n, n];
        int[] index1 = new int[n*n];
        int[] index2 = new int[n*n];
        int SnakeLength = 1;
        Random random = new Random();
        int foodindex1, foodindex2;
        int x = 20, y = 20, size = 30;
        int dx = 0, dy = 0;
        string direction = "Up";

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(40 + n * size, 40 + n * size);
            ClientSize = new Size(300 + n * size, 40 + n * size);
            
            //Змейка
            board[5, 5] = 1;
            for (int i = 0; i < n*n; i++)
            {
                index1[i] = -1;
                index2[i] = -1;
            }
            index1[0] = 5;
            index2[0] = 5;
            foodindex1 = random.Next(0, 10);
            foodindex2 = random.Next(0, 10);
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
            {
                if((index1[i] != -1) && (index2[i] != -1))
                    e.Graphics.FillRectangle(MyBrush, x + size * index1[i] + 1, y + size * index2[i] + 1, size - 1, size - 1);
            }

            if ((index1[0] == foodindex1) && (index2[0] == foodindex2))
            {
                SnakeLength++;
                foodindex1 = random.Next(0, 10);
                foodindex2 = random.Next(0, 10);
            }
            //Рисование еды
            e.Graphics.FillRectangle(FoodBrush, x + size * foodindex1 + 1, y + size * foodindex2 + 1, size - 1, size - 1);
                 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Moving(direction);
            pictureBox1.Refresh();
        }

        private void InitializeTimer()
        {
            timer1.Interval = 500;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
            button1.Text = "Stop";
            button1.Click += new EventHandler(button1_Click);
        }

        private void Moving(string direction) //Перемещение
        {
            if (direction == "Up")
            {
                for (int i = SnakeLength; i > 0; i--)
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
                index1[SnakeLength] = -1;
                index2[SnakeLength] = -1;
            }
            if (direction == "Down")
            {
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
                if (index2[0] + 1 > 9)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                    return;
                }
                index2[0]++;
                index1[SnakeLength] = -1;
                index2[SnakeLength] = -1;
            }
            if (direction == "Left")
            {
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
                    index1[SnakeLength] = -1;
                    index2[SnakeLength] = -1;
            }
            if (direction == "Right")
            {
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
                if (index1[0] + 1 > 9)
                {
                    timer1.Enabled = false;
                    MessageBox.Show("You lost! =( Your Score: " + SnakeLength);
                    return;
                }
                    index1[0]++;
                    index1[SnakeLength] = -1;
                    index2[SnakeLength] = -1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Stop")
            {
                button1.Text = "Start";
                timer1.Enabled = false;
            }
            else
            {
                button1.Text = "Stop";
                timer1.Enabled = true;
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            direction = "Up";
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            direction = "Down";
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            direction = "Left";
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            direction = "Right";
        }
    }
}
