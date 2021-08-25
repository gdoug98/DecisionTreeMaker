using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using DecisionTreeTest.UI.Shapes;

namespace DecisionTreeTest.UI
{
    public class ShapeFactory
    {
        public static Shape GetShape(ShapeType shapeType)
        {
            //Point[] points;
            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    return new Rectangle();
                case ShapeType.Ellipse:
                    return new Ellipse();
                case ShapeType.Triangle:
                    return new Triangle();
                case ShapeType.Pentagon:
                    return new Pentagon();
                case ShapeType.Hexagon:
                    return new Hexagon();
                default:
                    return null;
            }
        }
    }

    public enum ShapeType { Rectangle = 1, Ellipse, Triangle, Pentagon, Hexagon }
}
