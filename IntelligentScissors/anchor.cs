using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace IntelligentScissors
{
   /* public class form1
    {
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
                picCanvas.MouseMove += picCanvas_MouseMove_Drawing;
                picCanvas.MouseUp += picCanvas_MouseUp_Drawing;
                
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
            picCanvas.Invalidate();
        }

        // Stop drawing.
        private void picCanvas_MouseUp_Drawing(object sender, MouseEventArgs e)
        {
            IsDrawing = false;

            // Reset the event handlers.
            picCanvas.MouseMove -= picCanvas_MouseMove_Drawing;
            picCanvas.MouseUp -= picCanvas_MouseUp_Drawing;

            // Create the new segment.
            Pt1.Add(NewPt1);
            Pt1.Add(NewPt2);

            // Redraw.
            picCanvas.Invalidate();
        }

        #endregion // Drawing
       
        // Draw the lines.
        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            // Draw the segments.
            for (int i = 0; i < Pt1.Count-1; i++)
            {
                // Draw the segment.
                e.Graphics.DrawLine(Pens.PowderBlue, Pt1[i], Pt1[i+1]);
            }

            // Draw the end points.
            foreach (Point pt in Pt1)
            {
                Rectangle rect = new Rectangle(
                    pt.X - object_radius, pt.Y - object_radius,
                    2 * object_radius + 1, 2 * object_radius + 1);
                e.Graphics.FillEllipse(Brushes.Red , rect);
                e.Graphics.DrawEllipse(Pens.Black, rect);
            }
            

            // If there's a new segment under constructions, draw it.
            if (IsDrawing)
            {
                e.Graphics.DrawLine(Pens.Red, NewPt1, NewPt2);
            }
        }
    }*/
}
