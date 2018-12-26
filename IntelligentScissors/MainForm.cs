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
        int h, w;
        direction[,] energy;//= new direction[1000, 1000];
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                h_w();
                MessageBox.Show("start");
                energy = graph_.calculateWeights(ImageMatrix);
                MessageBox.Show("end");

                // saving_constructed_graph(energy, h, w);
            }


        }
        public void h_w()
        {
            h = ImageOperations.GetWidth(ImageMatrix);
            w = ImageOperations.GetHeight(ImageMatrix);
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }


        private const int object_radius = 3;

        // We're over an object if the distance squared
        // between the mouse and the object is less than this.
        private const int over_dist_squared = object_radius * object_radius;

        // The points that make up the line segments.
        private List<Point> Pt1 = new List<Point>();
        Point[] arrayofpoints = new Point[1];
        // Points for the new line.
        private bool IsDrawing = false;
        private Point NewPt1, NewPt2;

        private List<Point> lololyy = new List<Point>();

        // See what we're over and start doing whatever is appropriate.
        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {

            // Start drawing a new segment.
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += picCanvas_MouseUp_Drawing;

            IsDrawing = true;
            NewPt1 = new Point(e.X, e.Y);
            NewPt2 = new Point(e.X, e.Y);


        }

        #region "Drawing"


        private void picCanvas_MouseUp_Drawing(object sender, MouseEventArgs e)
        {
            //IsDrawing = false;

            // Reset the event handlers.
            pictureBox1.MouseMove -= pictureBox1_MouseMove;
            pictureBox1.MouseUp -= picCanvas_MouseUp_Drawing;

            // Create the new segment.
            Pt1.Add(NewPt1);
            //Pt1.Add(NewPt2);
            if (Pt1.Count > 1)
            {
                List<Point> cl = new List<Point>(); int[,] fromx = new int[h, w]; int[,] fromy = new int[h, w];
                double[,] dis = graph_.Dijkstra(energy, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, Pt1[Pt1.Count - 2].X, Pt1[Pt1.Count - 2].Y, fromx, fromy, h, w);
                graph_.printpath(Pt1[Pt1.Count - 2].X, Pt1[Pt1.Count - 2].Y, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, fromx, fromy, dis, ImageMatrix, cl);
                ////arrayofpoints = new Point[lololyy.Count];
                graph_.color(ImageMatrix, cl);
                //ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            }


            // Redraw.
            pictureBox1.Invalidate();
        }

        private void done_Click(object sender, EventArgs e)
        {
            if (Pt1.Count > 1)
            {
                List<Point> cl = new List<Point>(); int[,] fromx = new int[h, w]; int[,] fromy = new int[h, w];
                double[,] dis = graph_.Dijkstra(energy, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, Pt1[0].X, Pt1[0].Y, fromx, fromy, h, w);
                graph_.printpath(Pt1[0].X, Pt1[0].Y, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, fromx, fromy, dis, ImageMatrix, cl);
                graph_.color(ImageMatrix, cl);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            }
            pictureBox1.Invalidate();
        }

        #endregion // Drawing


        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {

            foreach (Point pt in Pt1)
            {
                Rectangle rect = new Rectangle(
                    pt.X - object_radius, pt.Y - object_radius,
                    2 * object_radius + 1, 2 * object_radius + 1);
                e.Graphics.FillEllipse(Brushes.CornflowerBlue, rect);
                e.Graphics.DrawEllipse(Pens.Black, rect);
            }



            if (IsDrawing && Pt1.Count > 0)
            {
                for (int i = 0; i < lololyy.Count - 1; i++)
                {
                    e.Graphics.DrawLine(Pens.Aqua, lololyy[i], lololyy[i + 1]);
                }

            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            NewPt2 = new Point(e.X, e.Y);
            if (Pt1.Count > 0 && e.X < w && e.Y < h)
            {
                lololyy.Clear();
                int[,] fromx = new int[h, w]; int[,] fromy = new int[h, w];
                double[,] dis = graph_.Dijkstra(energy, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, e.X, e.Y, fromx, fromy, h, w);
                graph_.printpath(e.X, e.Y, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, fromx, fromy, dis, ImageMatrix, lololyy);


            }
            // Redraw.
            pictureBox1.Invalidate();
        }

        public static string saving_constructed_graph_help(int nodeindex, int right, int left, int up, int down, double r, double l, double u, double d)//help saving_constructed_graph
        {

            if ((up == -1) && (down == -1) && (right == -1) && (left == -1))
                return "";
            string a = "", b = "", c = "", dd = "", s = nodeindex.ToString() + "|edges:";
            if (right != -1)
            {
                a = "(" + nodeindex.ToString() + "," + right.ToString() + "," + r.ToString() + ")";
            }//edge from   2  To  3  With Weights  1E+16
            if (down != -1)
            {
                b = "(" + nodeindex.ToString() + "," + down.ToString() + "," + r.ToString() + ")";
            }
            if (up != -1)
            {
                c = "(" + nodeindex.ToString() + "," + up.ToString() + "," + r.ToString() + ")";
            }
            if (left != -1)
            {
                dd = "(" + nodeindex.ToString() + "," + left.ToString() + "," + r.ToString() + ")";
            }

            return s + a + b + c + dd;

        }
        public static void saving_constructed_graph(direction[,] graphh, int N, int M)
        {
            int counter = 0, box;
            string s = "";
            using (StreamWriter writer = new StreamWriter("output2.txt"))
            {
                writer.WriteLine("The constructed graph");
                writer.WriteLine("\n");

                int right, left, up, down;
                right = -1; left = -1; up = -1; down = -1;
                double r = 0, l = 0, u = 0, d = 0;

                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < M; ++j)
                    {

                        if (i != N - 1) //down
                        {
                            down = counter + M;
                            d = graphh[j, i].down;
                            //d = graphh[i + 1, j];
                        }
                        if (j != M - 1)//right
                        {
                            //MessageBox.Show(i.ToString() + " " + j.ToString());
                            right = counter + 1;
                            r = graphh[j, i].right;
                            // r = graphh[i, j + 1];
                        }

                        if (j != 0)//left
                        {


                            left = counter - 1;
                            l = graphh[j - 1, i].right;
                            //  l = graphh[i, j - 1];

                            // MessageBox.Show(graphh[0, 1].ToString() + " " + graphh[1, 0].ToString() + "bbom");
                        }
                        if (i != 0)//up
                        {
                            up = counter - M;
                            u = graphh[j, i - 1].down;
                            //  u = graphh[i - 1, j];
                        }

                        s = saving_constructed_graph_help(counter, right, left, up, down, r, l, u, d);
                        counter++;
                        right = -1; left = -1; up = -1; down = -1;
                        if (s != null)
                            writer.WriteLine(s);
                    }


                }

            }

        }

    }
}