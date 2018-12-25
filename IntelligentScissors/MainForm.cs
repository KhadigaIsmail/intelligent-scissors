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

            h_w();
           saving_distance();
        }
        public void h_w ()
        {
            h = ImageOperations.GetWidth(ImageMatrix);
            w = ImageOperations.GetHeight(ImageMatrix);
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
            /*double[,] dist = graph_.Dijkstra(g, 0, 0,3,2,fromx,fromy);
                 {
               MessageBox.Show("L");
                for (int i = 0; i < 100; ++i)
                {
                    for (int j = 0; j < 100; ++j)
                    {
                        writer.WriteLine("x y "+i+" "+j+" "+fromx[i, j]+" "+ fromy[i, j]);
                        
                     
                    }
                }
               
            }
             */
            // graph_.printpath(3, 2, 0, 0, fromx, fromy, dist);
        }

        double[,] energy = new double[1000, 1000];
        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            energy = graph_.calculateWeights(ImageMatrix);
            saving_constructed_graph(energy, h, w);
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
        Point[] arrayofpoints = new Point[1];
        // Points for the new line.
        private bool IsDrawing = false;
        private Point NewPt1, NewPt2;

              private  List<Point> lololyy = new List<Point>();

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
           // MessageBox.Show("L");
            
            // Redraw.
            pictureBox1.Invalidate();
        }

        // Stop drawing.
        private void picCanvas_MouseUp_Drawing(object sender, MouseEventArgs e)
        {
            //IsDrawing = false;

            // Reset the event handlers.
            pictureBox1.MouseMove -= picCanvas_MouseMove_Drawing;
            pictureBox1.MouseUp -= picCanvas_MouseUp_Drawing;

            // Create the new segment.
            Pt1.Add(NewPt1);
            //Pt1.Add(NewPt2);
            
            // Redraw.
            pictureBox1.Invalidate();
        }

        private void done_Click(object sender, EventArgs e)
        {

        }

        #endregion // Drawing

        // Draw the lines.
        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            // Draw the segments.
            /*for (int i = 0; i < Pt1.Count - 1; i++)
            {
                // Draw the segment.
                e.Graphics.DrawLine(Pens.PowderBlue, Pt1[i], Pt1[i + 1]);
            }*/

            // Draw the end points.
            foreach (Point pt in Pt1)
            {
                Rectangle rect = new Rectangle(
                    pt.X - object_radius, pt.Y - object_radius,
                    2 * object_radius + 1, 2 * object_radius + 1);
                e.Graphics.FillEllipse(Brushes.CornflowerBlue, rect);
                e.Graphics.DrawEllipse(Pens.Black, rect);
            }


            // If there's a new segment under constructions, draw it.
            if (IsDrawing && Pt1.Count > 0)
            {
                //MessageBox.Show("L");
                for (int i = 0; i < lololyy.Count-1; i++)
                {
                    e.Graphics.DrawLine(Pens.DarkSalmon, lololyy[i], lololyy[i+1]);
                }
                //Pen p = new Pen(Color.Red ,3 );
                //e.Graphics.DrawLines(p, arrayofpoints);
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            NewPt2 = new Point(e.X, e.Y);
            if (Pt1.Count > 0)
            {
                lololyy.Clear();
                int[,] fromx = new int[1000, 1000]; int[,] fromy = new int[1000, 1000];
                double[,] dis = graph_.Dijkstra(energy, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, e.X, e.Y, fromx, fromy, h, w);
                graph_.printpath(e.X, e.Y, Pt1[Pt1.Count - 1].X, Pt1[Pt1.Count - 1].Y, fromx, fromy, dis, ImageMatrix,lololyy);
                //arrayofpoints = new Point[lololyy.Count];
                
                //ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                //MessageBox.Show("P");
            }
            // Redraw.
            pictureBox1.Invalidate();
        }

        public static string saving_constructed_graph_help(int nodeindex, int right, int left, int up, int down, double r, double l, double u, double d)//help saving_constructed_graph
        {
            if ((up == -1) && (down == -1) && (right == -1) && (left == -1))
                return "";
            string a = "", b = "", c = "", dd = "", s = "The  index node" + nodeindex.ToString() + "\n" + "Edges" + '\n';
            if (right != -1)
            {
                a = "edge from   " + nodeindex.ToString() + "  To  " + right.ToString() + "  With Weights  " + r.ToString() + "\n";
            }//edge from   2  To  3  With Weights  1E+16
            if (down != -1)
            {
                b = "edge from   " + nodeindex.ToString() + "  To  " + down.ToString() + "  With Weights  " + d.ToString() + "\n";
            }
            if (up != -1)
            {
                c = "edge from   " + nodeindex.ToString() + "  To  " + up.ToString() + "  With Weights  " + u.ToString() + "\n";
            }
            if (left != -1)
            {
                dd = "edge from   " + nodeindex.ToString() + "  To  " + left.ToString() + "  With Weights  " + l.ToString() + "\n";
            }

            return s + Environment.NewLine + a + Environment.NewLine + b + Environment.NewLine + c + Environment.NewLine + dd + Environment.NewLine;

        }
        public static void saving_constructed_graph(double[,] graphh, int N, int M)
        {
            int counter = 0, box;
            string s = "";
           // MessageBox.Show("P");
            using (StreamWriter writer = new StreamWriter("output1.txt"))
            {//double[,] energy = new double[1000, 1000];
                writer.WriteLine("The constructed graph");
                writer.WriteLine("\n");
                //display index 0
                int right, left, up, down;
                right = -1; left = -1; up = -1; down = -1;
                double r = 0, l = 0, u = 0, d = 0;
                //edge from   0  To  11  With Weights  1E+16
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < M; ++j)
                    {

                        if (i != N - 1) //down
                        {
                            down = counter + M;
                            d = graphh[i + 1, j];
                        }
                        if (j != M - 1)//right
                        {
                            //MessageBox.Show(i.ToString() + " " + j.ToString());
                            right = counter + 1;
                            r = graphh[i, j + 1];
                        }

                        if (j != 0)//left
                        {
                            left = counter - 1;
                            l = graphh[i, j - 1];
                        }
                        if (i != 0)//up
                        {
                            up = counter - M;
                            u = graphh[i - 1, j];
                        }
                        // MessageBox.Show(counter.ToString() + " " + down.ToString());
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