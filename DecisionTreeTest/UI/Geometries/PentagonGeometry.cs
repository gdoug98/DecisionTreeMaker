using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace DecisionTreeTest.UI.Geometries
{
    public class PentagonGeometry : CustomGeometry
    {
        protected override Geometry geometryInternal
        {
            get
            {
                Point[] points = new Point[5];
                PathSegment[] segments = new PathSegment[5];
                segments[0] = new LineSegment { Point = new Point(Origin.X + (0.15f * Bounds.Width), Origin.Y) };
                segments[1] = new LineSegment { Point = new Point(Origin.X + (0.85f * Bounds.Width), Origin.Y) };
                segments[2] = new LineSegment { Point = new Point(Origin.X + Bounds.Width, Origin.Y + (Bounds.Height / 2)) };
                segments[3] = new LineSegment { Point = new Point(Origin.X + (Bounds.Width / 2), Origin.Y + Bounds.Height) };
                segments[4] = new LineSegment { Point = new Point(Origin.X, Origin.Y + (Bounds.Height / 2)) };
                PathGeometry geometry = new PathGeometry();
                geometry.Figures.Add(new PathFigure { Segments = new PathSegmentCollection(segments) });
                return geometry;
            }
        }

        public override Geometry Geometry => geometryInternal;

        public PentagonGeometry() : base()
        {

        }

        public PentagonGeometry(Point _origin, Size _bounds) : base(_origin, _bounds)
        {

        }

        public PentagonGeometry(Point _origin, double width, double height) : base(_origin, width, height)
        {

        }
    }
}
