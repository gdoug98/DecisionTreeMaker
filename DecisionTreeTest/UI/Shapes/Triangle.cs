using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace DecisionTreeTest.UI.Shapes
{
    public class Triangle : Shape
    {
        //public Point Origin { get; set; }
        protected override Geometry DefiningGeometry
        {
            get
            {
                Point[] points = new Point[3];
                PathSegment[] segments = new PathSegment[3];
                segments[0] = new LineSegment { Point = RenderTransformOrigin };
                segments[1] = new LineSegment { Point = new Point(RenderTransformOrigin.X + Width, RenderTransformOrigin.Y) };
                segments[2] = new LineSegment { Point = new Point(RenderTransformOrigin.X + Width / 2, RenderTransformOrigin.Y + Height) };
                PathGeometry geometry = new PathGeometry();
                geometry.Figures.Add(new PathFigure { Segments = new PathSegmentCollection(segments) });
                return geometry;
                
            }
        }
    }
}
