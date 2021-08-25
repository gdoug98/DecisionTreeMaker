using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTreeTest.UI.Geometries;
using System.Windows.Media;
using System.Windows;

namespace DecisionTreeTest.UI.Geometries
{
    public class GeometryUtils
    {
        public static Point ClosestPoint(Geometry geometry, Point p)
        {
            PathGeometry flatGeometry = geometry.GetFlattenedPathGeometry();

            var closestTuple = flatGeometry.Figures.Select(x => closestOnFigure(x, p)).OrderBy(y => y.Item2).First();

            return closestTuple.Item1;
        }

        private static Tuple<Point, double> closestOnFigure(PathFigure figure, Point p)
        {
            List<Tuple<Point, double>> retList = new List<Tuple<Point, double>>();
            Point current = figure.StartPoint;
            foreach(PathSegment segment in figure.Segments)
            {
                LineSegment line = segment as LineSegment;
                PolyLineSegment poly = segment as PolyLineSegment;

                Point[] pts;

                if (line != null)
                {
                    pts = new[] { line.Point };
                }
                else if (poly != null)
                {
                    pts = poly.Points.ToArray();
                }
                else
                {
                    throw new ArgumentException("Unexpected segment type. Please only use Lines or Polylines");
                }

                foreach(Point pt in pts)
                {
                    Point closest = closestPointOnLine(current, pt, p);
                    double distancePow2 = (closest - p).LengthSquared;
                    retList.Add(new Tuple<Point, double>(closest, distancePow2));

                    current = pt;
                }

            }

            return retList.OrderBy(x => x.Item2).First();
        }

        private static Point closestPointOnLine(Point u, Point v, Point p)
        {
            double l2 = (u - v).LengthSquared;
            if (l2 == 0)
            {
                return u;
            }

            Vector dist = u - v;

            double closest = (u - p) * (dist / l2);

            if (closest < 0.0)
            {
                return u;
            }
            else if (closest > 1.0)
            {
                return v;
            }
            else
            {
                return u + (closest * dist);
            }
        }
    }
}
