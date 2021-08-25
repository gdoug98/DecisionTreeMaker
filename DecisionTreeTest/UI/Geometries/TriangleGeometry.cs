using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace DecisionTreeTest.UI.Geometries
{
    public class TriangleGeometry : CustomGeometry
    {
        protected override Geometry geometryInternal
        {
            get
            { 
                PathGeometry geometry = new PathGeometry();            
                PathFigure fig = new PathFigure { IsClosed = true };
                fig.StartPoint = new Point(Origin.X, Origin.Y + Bounds.Height);
                geometry.Figures.Add(fig);

                PathSegment[] segments = new PathSegment[3];

                //segments[0] = new LineSegment { Point = new Point(Origin.X, Origin.Y + Bounds.Height) };
                //fig.StartPoint = segments[0].;
                segments[0] = new LineSegment { Point = new Point(Origin.X + Bounds.Width, Origin.Y + Bounds.Height) };
                fig.Segments.Add(segments[0]);
                segments[1] = new LineSegment { Point = new Point(Origin.X + Bounds.Width / 2, Origin.Y) };
                fig.Segments.Add(segments[1]);

                //segments[0] = new LineSegment { Point = new Point(0, 0) };
                //segments[1] = new LineSegment { Point = new Point(0, 0) };
                //segments[2] = new LineSegment { Point = new Point(0, 0) };
                //geometry.Figures.Add(new PathFigure { Segments = new PathSegmentCollection(segments) });
                return geometry;
            }
        }

        public override Geometry Geometry => geometryInternal;

        public TriangleGeometry() : base()
        {

        }

        public TriangleGeometry(Point _origin, Size _bounds) : base(_origin, _bounds)
        {

        }

        public TriangleGeometry(Point _origin, double width, double height) : base(_origin, width, height)
        {

        }
    }
}
