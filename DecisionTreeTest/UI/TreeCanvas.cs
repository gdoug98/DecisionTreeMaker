using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DecisionTreeTest.UI
{
    public class TreeCanvas: Canvas
    {
        private List<Visual> _nodeVisuals = new List<Visual>();

        protected override Visual GetVisualChild(int index)
        {
            return _nodeVisuals[index];
        }

        protected override int VisualChildrenCount => _nodeVisuals.Count;

        public void AddVisual(Visual vis)
        {
            _nodeVisuals.Add(vis);

            base.AddVisualChild(vis);
            base.AddLogicalChild(vis);
        }

        public void RemoveVisual(Visual vis)
        {
            _nodeVisuals.Remove(vis);

            base.RemoveVisualChild(vis);
            base.RemoveLogicalChild(vis);
        }

        public DrawingVisual GetVisual(Point point)
        {
            var hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }
    }
}
