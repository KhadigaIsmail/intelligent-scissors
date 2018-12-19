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
    }
}