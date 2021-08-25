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
    public abstract class CustomGeometry
    {
        public Point Origin { get; set; }
        public Size Bounds { get; set; }
        protected abstract Geometry geometryInternal { get; }

        public abstract Geometry Geometry { get; }

        public CustomGeometry()
        {
            Origin = new Point(0, 0);
            Bounds = new Size(0, 0);
        }

        public CustomGeometry(Point _origin, Size _size)
        {
            Origin = _origin;
            Bounds = _size;
        }

        public CustomGeometry(Point _origin, double width, double height)
        {
            Origin = _origin;
            Bounds = new Size(width, height);
        }
    }
}
