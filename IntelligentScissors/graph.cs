using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public  class graph_
    {
       
       
        public static double [,] calculateEnergy(RGBPixel[,]  ImageMatrix)
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
    
    }
}
