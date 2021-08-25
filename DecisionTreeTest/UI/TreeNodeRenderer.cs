using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DecisionTree.Entities.Classes;

namespace DecisionTreeTest.UI
{
    public class TreeNodeRenderer
    {
        public static void DrawNode(string nodeText, Point origin, Vector nodeSize, Vector padding, Vector buttonSize, DrawingVisual[] visuals)
        {
            drawNodeBody(visuals[0], new Rect(origin, new Size(nodeSize.X, nodeSize.Y)), nodeText);

            drawAddButton(visuals[1], new Rect(new Point(origin.X + nodeSize.X + padding.X, origin.Y), new Size(buttonSize.X, buttonSize.Y)));

            drawRemoveButton(visuals[2], new Rect(new Point(origin.X + nodeSize.X + padding.X, origin.Y + nodeSize.Y / 2), new Size(buttonSize.X, buttonSize.Y)));
        }

        private static void drawNodeBody(DrawingVisual body, Rect bodyRect, string text)
        {
            using (DrawingContext dc = body.RenderOpen())
            {
                Brush bsh = Brushes.Blue;

                dc.DrawRectangle(bsh, new Pen(Brushes.Black, 2.75), bodyRect);
                System.Globalization.CultureInfo info = new System.Globalization.CultureInfo("en-GB");
                FormattedText fText = new FormattedText(text, info, FlowDirection.LeftToRight, new Typeface("Arial"), 35, Brushes.Black);
                dc.DrawText(fText, bodyRect.BottomLeft);
            }
        }

        private static void drawAddButton(DrawingVisual addBtn, Rect buttonRect)
        {
            using (DrawingContext dc = addBtn.RenderOpen())
            {
                Brush bsh = Brushes.Green;

                dc.DrawRectangle(bsh, new Pen(Brushes.Black, 2.75), buttonRect);
            }
        }        
                 
        private static void drawRemoveButton(DrawingVisual remBtn, Rect buttonRect)
        {
            using (DrawingContext dc = remBtn.RenderOpen())
            {
                Brush bsh = Brushes.DarkRed;

                dc.DrawRectangle(bsh, new Pen(Brushes.Black, 2.75), buttonRect);
            }
        }
    }
}
