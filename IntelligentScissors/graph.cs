using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IntelligentScissors
{
    public  class graph_
    {
       
       
        public static double [,] calculateWeights(RGBPixel[,]  ImageMatrix)
        {
            double[,] energy = new double[1000, 1000];
            int height = ImageOperations.GetHeight(ImageMatrix);
            int width = ImageOperations.GetWidth(ImageMatrix);

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {

                    Vector2D e;
                    e = ImageOperations.CalculatePixelEnergies(x, y, ImageMatrix);
                    energy[y + 1, x] = 1 / e.X;
                    energy[y, x + 1] = 1 / e.Y;


                }
            }
            return energy;
        }

        const int N = (1 << 22), M = (1 << 18), OO = 0x3f3f3f3f;

        List<Pair<int, int>> adj = new List<Pair<int, int>>(N);
        int n, m, u, v, c, s;
        
        public static void Dijkstra(int src)
        {
            int [] dis ;
            //	priority_queue<pair<int, int>, vector<pair<int, int>>, greater<pair<int, int>>> pq;
            Pair<int, int> pr = new Pair<int, int>();
            PriorityQueue<Pair<int, int>> pq = new PriorityQueue<Pair<int, int>>();
            Pair<Pair<int, int>,int > p = new Pair<Pair<int, int>,int >();
            List<Pair<int, int>> l = new List<Pair<int, int>>();
            pr.First = 0;
            pr.Second = src;
            pq.Enqueue(pr);
            dis[src] = 0;
            while (!pq.Empty())
            {
                int p = pq.Peek().second;
                int d = -pq.Peek().first;
                pq.Dequeue();
                if (d > dis[p]) continue;
                for (Pair<int, int> ch : adj[p])
                {
                    if (dis[ch.second] > d + ch.first)
                    {
                        dis[ch.second] = d + ch.first;
                        pq.Enqueue(make_pair(-dis[ch.second], ch.second));
                    }
                }
            }
        }
    }
}
