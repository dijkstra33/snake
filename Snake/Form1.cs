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
        enum State { head, tail, body, food, empty };
        int[,] board = new int[n, n];
        int[] index1 = new int[n];
        int[] index2 = new int[n];
        int x = 20, y = 20, size = 30;
        int dx = 0, dy = 0;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(40 + n * size, 40 + n * size);
            ClientSize = new Size(40 + n * size, 40 + n * size);
            

            //Brush MyBrush = Brushes.Red;
            //SolidBrush FillBrush = new SolidBrush(Color.Black);
            //e.Graphics.FillRectangle(FillBrush, x, y, size, size);
            //FillBrush = new SolidBrush(Color.Red);
            //e.Graphics.FillRectangle(MyBrush, x + size*5 + 1, y + size*5 + 1, size - 1, size - 1);
            //e.Graphics.FillRectangle(MyBrush, x + size * 6 + 1, y + size * 5 + 1, size - 1, size - 1);


            //Змейка
            board[5, 5] = 1;
            for (int i = 0; i < n; i++)
            {
                index1[i] = -1;
                index2[i] = -1;
            }
            index1[0] = 5;
            index2[0] = 5;
            //MyBrush = Brushes.Blue;
            //e.Graphics.FillRectangle(MyBrush, x + size * 5 + 1, y + size * 5 + 1, size - 1, size - 1);
            Moving("Up");
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Рисование линий
            Pen MyPen = Pens.Black;
            Brush MyBrush = Brushes.Blue;
            for (int i = 0; i < n + 1; i++)
            {
                e.Graphics.DrawLine(MyPen, x, y + dy, x + n * size, y + dy);
                e.Graphics.DrawLine(MyPen, x + dx, y, x + dx, y + n * size);
                dx += size;
                dy += size;
            }

            for (int i = 0; i < n; i++)
            {
                if((index1[i] != -1) || (index2[i] != -1))
                    e.Graphics.FillRectangle(MyBrush, x + size * index1[i] + 1, y + size * index2[i] + 1, size - 1, size - 1);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Moving("Up");
            pictureBox1.Refresh();
        }

        private void InitializeTimer()
        {
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
            button1.Text = "Stop";
            button1.Click += new EventHandler(button1_Click);
        }

        private void Moving(string direction)
        {
            if (direction == "Up")
            {
                for (int i = n - 1; i > 0; i--)
                {
                    index1[i] = index1[i - 1];
                    index2[i] = index2[i - 1];
                }
                index2[0]--;
            }
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Brush MyBrush = Brushes.Blue;
            for (int i = 0; i < n; i++)
            {
                if ((index1[i] != -1) && (index2[i] != -1))
                    e.Graphics.FillRectangle(MyBrush, x + size * index1[i] + 1, y + size * index2[i] + 1, size - 1, size - 1);
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
    }
}
