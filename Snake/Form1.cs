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
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Form1 board = new Form1();
            const int n = 10;
            int size = 30;
            pictureBox1.Size = new Size(40 + n * size, 40 + n * size);
            ClientSize = new Size(40 + n * size, 40 + n * size);
            Graphics graphics = e.Graphics;
            Pen MyPen = new Pen(Color.Black, 1);
            int x = 20, y = 20;
            int dx = 0, dy = 0;

            //Рисование линий
            for (int i = 0; i < n+1; i++)
            {
                e.Graphics.DrawLine(MyPen, x, y + dy, x + n * size, y + dy);
                e.Graphics.DrawLine(MyPen, x + dx, y, x + dx, y + n * size);
                dx += size;
                dy += size;
            }

            //Задание прямоугольников
            Rectangle[,] rectangles = new Rectangle[n,n];
            dx = dy = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    rectangles[i, j] = new Rectangle(x + j * dx, y + i * dy, x + (j + 1) * dx, y + (i + 1) * dy);
                    dx += size;
                }
                dy += size;
            }
            Brush MyBrush = Brushes.Red;
            SolidBrush FillBrush = new SolidBrush(Color.Black);
            e.Graphics.FillRectangle(FillBrush, x, y, size, size);
            FillBrush = new SolidBrush(Color.Red);
            e.Graphics.FillRectangle(MyBrush, rectangles[5,5]);
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
