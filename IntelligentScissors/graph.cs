﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace IntelligentScissors
{
    public  class graph_
    {
       
       
        public static double [,] calculateWeights(RGBPixel[,]  ImageMatrix)
        {
            double[,] weights = new double[1000, 1000];
            int height = ImageOperations.GetHeight(ImageMatrix);
            int width = ImageOperations.GetWidth(ImageMatrix);
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {

                    Vector2D ee;
                    ee = ImageOperations.CalculatePixelEnergies(x, y, ImageMatrix);
                    weights[y + 1, x] = 1 / ee.X;
                    weights[y, x + 1] = 1 / ee.Y;


                }
            }
            Vector2D e;
            e = ImageOperations.CalculatePixelEnergies(2, 6 ,ImageMatrix);
            MessageBox.Show(e.X.ToString());
            e = ImageOperations.CalculatePixelEnergies(1, 6, ImageMatrix);
            MessageBox.Show(e.Y.ToString());

            return weights;
        }

        const int N = (1 << 22), M = (1 << 18), OO = 0x3f3f3f3f;

        List<Pair<int, int>> adj = new List<Pair<int, int>>(N);
        public static bool valid ( int x, int y)
        {
            if (x >= 0 && y >= 0 && x < 5 && y < 5) return true;
            return false;
        }
        public static double[,] Dijkstra(double [,] graph,int x , int y , int destinationX , int destinationY, int[,] fromx, int[,] fromy)
        {
            double [,] dis = new double[1000,1000];
           // int[,] fromx = new int[1000, 1000];
            //int[,] fromy = new int[1000, 1000];
            for (int i = 0;i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                dis[i, j] = int.MaxValue;
            }
            elPriorityQueuebta3khadiga pq = new elPriorityQueuebta3khadiga(x,y,0.0);
            dis[x,y] = 0;
            while (!pq.Empty())
            {
                double d = pq.Top().weight;
                int xx = pq.Top().qx.Peek();
                int yy = pq.Top().qy.Peek();
                pq.Pop();
                
                if (d > dis[xx, yy]) continue;
                if (valid(xx+1,yy)&&dis[xx+1,yy] > d + graph[xx+1,yy] )
                {
                    dis[xx + 1, yy] = d + graph[xx + 1, yy];
                    fromx[xx+1,yy]=xx;
                    fromy[xx+1, yy] = yy;
                    if (xx+1 == destinationX && yy == destinationY)
                    {
                        pq = null;
                        return dis;
                    }
                    pq.push(xx+1,yy,dis[xx+1,yy]);
                }

                if (valid(xx,yy+1)&&dis[xx, yy+1] > d + graph[xx, yy+1])
                {
                    dis[xx, yy + 1] = d + graph[xx, yy + 1];
                    pq.push(xx, yy+1, dis[xx , yy+1]);
                    fromx[xx , yy+1] = xx;
                    fromy[xx, yy+1] = yy;
                    if (xx == destinationX && yy + 1 == destinationY)
                    {
                        pq = null;
                        return dis;
                    }
                }
                if (valid(xx-1, yy) && dis[xx - 1, yy] > d + graph[xx - 1, yy])
                {
                    dis[xx - 1, yy] = d + graph[xx - 1, yy];
                    pq.push(xx - 1, yy, dis[xx - 1, yy]);
                    fromx[xx-1, yy] = xx;
                    fromy[xx-1, yy] = yy;
                    if (xx-1 == destinationX && yy == destinationY)
                    {
                        pq = null;
                        return dis;
                    }
                }
                if (valid(xx, yy-1) && dis[xx, yy - 1] > d + graph[xx, yy - 1])
                {
                    dis[xx, yy - 1] = d + graph[xx, yy - 1];
                    pq.push(xx, yy-1, dis[xx, yy-1]);
                    fromx[xx, yy-1] = xx;
                    fromy[xx, yy-1] = yy;
                    if (xx == destinationX && yy-1 == destinationY)
                    {
                        pq = null;
                        return dis;
                    }
                }
            }
            return dis;
        }
        public static void printpath(int x, int y, int srcx , int srcy, int[,] fromx, int[,] fromy,double [,] dis)
        {
            if (x == srcx && y == srcy) return;
           // MessageBox.Show(x + " " + y);
            printpath(fromx[x,y],fromy[x,y],srcx,srcy,fromx,fromy, dis);
        }
      
    }
}
