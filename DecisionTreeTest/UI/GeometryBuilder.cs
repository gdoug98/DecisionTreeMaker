using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTreeTest.UI.Geometries;
using System.Windows.Media;
using System.Windows;

namespace DecisionTreeTest.UI
{
    public class GeometryBuilder
    {
        public static Geometry GetGeometry(ShapeType type)
        {
            switch(type)
            {
                case ShapeType.Ellipse:
                    return new EllipseGeometry();
                case ShapeType.Rectangle:
                    return new RectangleGeometry();
                case ShapeType.Triangle:
                    return new TriangleGeometry().Geometry;
                case ShapeType.Pentagon:
                    return new PentagonGeometry().Geometry;
                case ShapeType.Hexagon:
                    return new HexagonGeometry().Geometry;
                default:
                    return null;
            }
        }

        public static Geometry GetGeometry(ShapeType type, Point origin, Size bounds)
        {
            switch (type)
            {
                case ShapeType.Ellipse:
                    EllipseGeometry ep = new EllipseGeometry
                    {
                        Center = new Point(origin.X + bounds.Width / 2, origin.Y + bounds.Height / 2),
                        RadiusX = bounds.Width / 2,
                        RadiusY = bounds.Height / 2

                        //Center = new Point(0, 0),
                        //RadiusX = bounds.Width / 2,
                        //RadiusY = bounds.Height / 2
                    };
                    return ep;
                case ShapeType.Rectangle:
                    RectangleGeometry rp = new RectangleGeometry
                    {
                        Rect = new Rect(origin, bounds)
                    };
                    return rp;
                case ShapeType.Triangle:
                    return new TriangleGeometry(origin, bounds).Geometry;
                case ShapeType.Pentagon:
                    return new PentagonGeometry(origin, bounds).Geometry;
                case ShapeType.Hexagon:
                    return new HexagonGeometry(origin, bounds).Geometry;
                default:
                    return null;
            }
        }
    }
    
}
