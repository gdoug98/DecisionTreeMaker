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
    public class Pentagon : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get
            {
                Point[] points = new Point[5];
                PathSegment[] segments = new PathSegment[5];
                segments[0] = new LineSegment { Point = new Point(RenderTransformOrigin.X + (0.15f * Width), RenderTransformOrigin.Y) };
                segments[1] = new LineSegment { Point = new Point(RenderTransformOrigin.X + (0.85f * Width), RenderTransformOrigin.Y) };
                segments[2] = new LineSegment { Point = new Point(RenderTransformOrigin.X + Width, RenderTransformOrigin.Y + (Height / 2)) };
                segments[3] = new LineSegment { Point = new Point(RenderTransformOrigin.X + (Width / 2), RenderTransformOrigin.Y + Height) };
                segments[4] = new LineSegment { Point = new Point(RenderTransformOrigin.X, RenderTransformOrigin.Y + (Height / 2)) };
                PathGeometry geometry = new PathGeometry();
                geometry.Figures.Add(new PathFigure { Segments = new PathSegmentCollection(segments) });
                return geometry;

            }
        }
    }
}
