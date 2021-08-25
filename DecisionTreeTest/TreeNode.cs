using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionTree.Entities.Classes;
using DecisionTreeTest.UI;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

namespace DecisionTreeTest
{
    public delegate void NodeParentChangedHandler(System.Windows.Controls.Canvas canvas, TreeNode node);

    public class TreeNode
    {
        private float _childDirection = 1;

        public DTNode<string> Node { get; private set; }
        // private TreeNodeRenderer _renderer;

        public Point Origin { get; set; }
        public Point Centre => new Point(Origin.X + NodeSize.Width / 2, Origin.Y + NodeSize.Height / 2);
        public string NodeValue => Node.Value;
        public Rectangle MainBody { get; private set; }
        public Rectangle AddButton { get; private set; }
        public Rectangle RemoveButton { get; private set; }
        public Line ParentConnector { get; private set; }

        public Size NodeSize { get; set; }
        public Size ButtonSize { get; set; }
        public Size Padding { get; set; }

        public event NodeParentChangedHandler ParentChanged;

        //public TreeNode(DTNode<string> node, Vector origin, Size nodeSize, Size buttonSize, Size padding)
        //{
        //    _node = node;
        //    _origin = new Point(origin.X, origin.Y);
        //    _nodeSize = nodeSize;
        //    _buttonSize = buttonSize;
        //    _padding = padding;
            
        //}

        private TreeNode()
        {
            Origin = new Point();
            NodeSize = new Size();
            ButtonSize = new Size();
            Padding = new Size();
            Node = new DTNode<string>(string.Empty);
            ParentChanged += OnParentChanged;
        }

        public static TreeNode CreateNode(System.Windows.Controls.Canvas canvas, Point origin, Size nSize, Size bSize, Size pad, bool isRoot = false)
        {
            TreeNode ret = new TreeNode();

            ret.NodeSize = nSize;
            ret.ButtonSize = bSize;
            ret.Padding = pad;
            ret.Origin = origin;

            // draw node
            drawNode(canvas, ret, null, isRoot);

            return ret;
        }

        public static TreeNode CreateNode(System.Windows.Controls.Canvas canvas, Point origin, Size nSize, Size bSize, Size pad, DTNode<string> parent)
        {
            TreeNode ret = new TreeNode();

            ret.NodeSize = nSize;
            ret.ButtonSize = bSize;
            ret.Padding = pad;
            ret.Origin = origin;

            // draw node
            drawNode(canvas, ret, parent, false);

            return ret;
        }

        public void DestroyNode(System.Windows.Controls.Canvas canvas)
        {
            Node = null;
            canvas.Children.Remove(MainBody);
            canvas.Children.Remove(AddButton);
            canvas.Children.Remove(RemoveButton);
            if(ParentConnector != null)
            {
                canvas.Children.Remove(ParentConnector);
            }            
        }

        private static void drawNode(System.Windows.Controls.Canvas canvas, TreeNode node, DTNode<string> parent, bool isRoot = false)
        {
            node.MainBody = new Rectangle { Width = node.NodeSize.Width, Height = node.NodeSize.Height, Fill = Brushes.Blue };
            node.AddButton = new Rectangle { Width = node.ButtonSize.Width, Height = node.ButtonSize.Height, Fill = Brushes.Green };
            node.RemoveButton = new Rectangle { Width = node.ButtonSize.Width, Height = node.ButtonSize.Height, Fill = Brushes.DarkRed };
            

            if (!isRoot)
            {
                parent.InsertChild(node.Node);
                var window = (MainWindow)((System.Windows.Controls.Grid)canvas.Parent).Parent;
                TreeNode parentNode = window.TreeNodes.Find(x => x.Node == node.Node.Parent);
                if(parentNode != null)
                {
                    node.ParentConnector = new Line { X1 = 0, X2 = node.Centre.X - parentNode.Centre.X , Y1 = 0, Y2 = node.Centre.Y - parentNode.Centre.Y, Stroke = Brushes.Black, StrokeThickness = 2 };

                    System.Windows.Controls.Canvas.SetTop(node.ParentConnector, parentNode.Centre.Y);
                    System.Windows.Controls.Canvas.SetLeft(node.ParentConnector, parentNode.Centre.X);

                    canvas.Children.Add(node.ParentConnector);
                }                
            }

            node.AddButton.MouseLeftButtonDown += node.OnAddMouseDown;
            node.AddButton.MouseEnter += node.OnAddMouseOver;
            node.AddButton.MouseLeave += node.OnAddMouseOff;

            node.MainBody.MouseEnter += node.OnNodeMouseOver;
            node.MainBody.MouseLeave += node.OnNodeMouseOff;

            node.RemoveButton.MouseDown += node.OnRemoveMouseDown;
            node.RemoveButton.MouseEnter += node.OnRemoveMouseOver;
            node.RemoveButton.MouseLeave += node.OnRemoveMouseOff;

            System.Windows.Controls.Canvas.SetTop(node.MainBody, node.Origin.Y);
            System.Windows.Controls.Canvas.SetLeft(node.MainBody, node.Origin.X);

            System.Windows.Controls.Canvas.SetTop(node.AddButton, node.Origin.Y + node.NodeSize.Height / 2);
            System.Windows.Controls.Canvas.SetLeft(node.AddButton, node.Origin.X + node.NodeSize.Width + node.Padding.Width);

            System.Windows.Controls.Canvas.SetTop(node.RemoveButton, node.Origin.Y);
            System.Windows.Controls.Canvas.SetLeft(node.RemoveButton, node.Origin.X + node.NodeSize.Width + node.Padding.Width);

            canvas.Children.Add(node.MainBody);
            canvas.Children.Add(node.AddButton);
            canvas.Children.Add(node.RemoveButton);            
        }

        private void OnAddMouseDown(object sender, MouseButtonEventArgs e)
        {
            _childDirection *= -1;
            Point newOrigin = new Point(Centre.X + ((Node.GetChildCount() + 1) * (NodeSize.Width + (2 * Padding.Width)) * _childDirection), Origin.Y + NodeSize.Height + 25);
            // combining 2 types of casting on the same line? Dangerous.
            var canvas = ((Rectangle)e.Source).Parent as System.Windows.Controls.Canvas;
            var window = (MainWindow)((System.Windows.Controls.Grid)canvas.Parent).Parent;

            TreeNode newNode = TreeNode.CreateNode(canvas, newOrigin, NodeSize, ButtonSize, Padding, Node);
            //Node.InsertChild(newNode.Node);
            window.TreeNodes.Add(newNode);
        }

        private void OnRemoveMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = ((Rectangle)e.Source).Parent as System.Windows.Controls.Canvas;
            var window = (MainWindow)((System.Windows.Controls.Grid)canvas.Parent).Parent;            
            var children = window.TreeNodes.FindAll(x => x.Node.Parent == Node);
            DTNode<string>.RemoveNode(Node);
            foreach (TreeNode child in children)
            {
                var parentNode = window.TreeNodes.Find(x => x.Node == child.Node.Parent);
                child.ParentChanged(canvas, parentNode);
            }
            DestroyNode(canvas);
            window.TreeNodes.Remove(this);
        }

        private void OnNodeMouseOver(object sender, MouseEventArgs e)
        {
            MainBody.Fill = Brushes.DarkCyan;
        }

        private void OnNodeMouseOff(object sender, MouseEventArgs e)
        {
            MainBody.Fill = Brushes.Blue;
        }

        private void OnAddMouseOver(object sender, MouseEventArgs e)
        {
            AddButton.Fill = Brushes.LightGreen;
        }
        private void OnAddMouseOff(object sender, MouseEventArgs e)
        {
            AddButton.Fill = Brushes.Green;
        }

        private void OnRemoveMouseOver(object sender, MouseEventArgs e)
        {
            RemoveButton.Fill = Brushes.Red;
        }
        private void OnRemoveMouseOff(object sender, MouseEventArgs e)
        {
            RemoveButton.Fill = Brushes.DarkRed;
        }

        private void OnParentChanged(System.Windows.Controls.Canvas canvas, TreeNode parentNode)
        {
            canvas.Children.Remove(ParentConnector);
            if (parentNode != null)
            {
                ParentConnector = new Line { X1 = 0, X2 = Centre.X - parentNode.Centre.X, Y1 = 0, Y2 = Centre.Y - parentNode.Centre.Y, Stroke = Brushes.Black, StrokeThickness = 2 };

                System.Windows.Controls.Canvas.SetTop(ParentConnector, parentNode.Centre.Y);
                System.Windows.Controls.Canvas.SetLeft(ParentConnector, parentNode.Centre.X);

                canvas.Children.Add(ParentConnector);
            }
        }
    }
}
