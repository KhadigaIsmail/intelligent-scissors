using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IntelligentScissors
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public RGBPixel[,] ImageMatrix;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }

            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
           saving_distance();
        }

        public static void saving_distance()
        {
            double[,] g = new double[100, 100];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                    g[i, j] = i + j + 2.0;

            }
            int[,] fromx = new int[1000, 1000];
            int[,] fromy = new int[1000, 1000];
            double[,] dist = graph_.Dijkstra(g, 0, 0,3,2,fromx,fromy);
            using (StreamWriter writer = new StreamWriter("output1.txt"))
            {
                writer.WriteLine("this is the distance");
                for (int i = 0; i < 5; ++i)
                {
                    for (int j = 0; j < 5; ++j)
                    {
                        writer.Write(g[i, j]);
                        writer.Write(" ");

                    }
                    writer.WriteLine(" ");
                }
                for (int i = 0; i < 5; ++i)
                {
                    for (int j = 0; j < 5; ++j)
                    {
                        writer.Write(dist[i, j]);
                        writer.Write(" ");

                    }
                    writer.WriteLine(" ");
                }
            }
            graph_.printpath(3, 2, 0, 0, fromx, fromy, dist);
        }
        
        double[,] energy = new double[1000, 1000];
        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            energy = graph_.calculateWeights(ImageMatrix);

            MouseEventArgs me = (MouseEventArgs)e;
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        // The "size" of an object for mouse over purposes.
        private const int object_radius = 3;

        // We're over an object if the distance squared
        // between the mouse and the object is less than this.
        private const int over_dist_squared = object_radius * object_radius;

        // The points that make up the line segments.
        private List<Point> Pt1 = new List<Point>();

        // Points for the new line.
        private bool IsDrawing = false;
        private Point NewPt1, NewPt2;


        // See what we're over and start doing whatever is appropriate.
        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {

            // Start drawing a new segment.
            pictureBox1.MouseMove += picCanvas_MouseMove_Drawing;
            pictureBox1.MouseUp += picCanvas_MouseUp_Drawing;

            IsDrawing = true;
            NewPt1 = new Point(e.X, e.Y);
            NewPt2 = new Point(e.X, e.Y);

        }

        #region "Drawing"

        // We're drawing a new segment.
        private void picCanvas_MouseMove_Drawing(object sender, MouseEventArgs e)
        {
            // Save the new point.
            NewPt2 = new Point(e.X, e.Y);

            // Redraw.
            pictureBox1.Invalidate();
        }

        // Stop drawing.
        private void picCanvas_MouseUp_Drawing(object sender, MouseEventArgs e)
        {
            IsDrawing = false;

            // Reset the event handlers.
            pictureBox1.MouseMove -= picCanvas_MouseMove_Drawing;
            pictureBox1.MouseUp -= picCanvas_MouseUp_Drawing;

            // Create the new segment.
            Pt1.Add(NewPt1);
            Pt1.Add(NewPt2);

            // Redraw.
            pictureBox1.Invalidate();
        }

        #endregion // Drawing

        // Draw the lines.
        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            // Draw the segments.
            for (int i = 0; i < Pt1.Count - 1; i++)
            {
                // Draw the segment.
                e.Graphics.DrawLine(Pens.PowderBlue, Pt1[i], Pt1[i + 1]);
            }

            // Draw the end points.
            foreach (Point pt in Pt1)
            {
                Rectangle rect = new Rectangle(
                    pt.X - object_radius, pt.Y - object_radius,
                    2 * object_radius + 1, 2 * object_radius + 1);
                e.Graphics.FillEllipse(Brushes.Red, rect);
                e.Graphics.DrawEllipse(Pens.Black, rect);
            }


            // If there's a new segment under constructions, draw it.
            if (IsDrawing)
            {
                e.Graphics.DrawLine(Pens.Red, NewPt1, NewPt2);
            }
        }
    }
}