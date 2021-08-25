using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Data;
using DecisionTree.Entities.Classes;
using DecisionTreeTest.UI;
using System.Configuration;
using DecisionTreeTest.UI.Geometries;

namespace DecisionTreeTest.Controls
{
    public delegate void ParentChangeHandler(DecisionNode parentNode, Canvas canvas);

    public class DecisionNode : FrameworkElement, IComparable<DecisionNode>
    {
        private static int _nextNodeNum = 1;
        public int NodeNum { get; private set; }

        public event ParentChangeHandler ParentChanged;

        private int _childDirection = -1;

        public ShapeType BodyType { get; private set; }
        public ShapeType ButtonType { get; private set; }

        public Shape MainBody { get; private set; }
        public Shape AddButton { get; private set; }
        public Shape RemoveButton { get; private set; }
        public Geometry BodyGeometry { get; private set; }
        public Geometry AddButtonGeometry { get; private set; }
        public Geometry RemoveButtonGeometry { get; private set; }
        public Line ParentConnector { get; private set; }
        public TextBlock Text { get; private set; }
        public List<TextBlock> ActionTextHolders { get; private set; }

        public Rect BodyRect { get; private set; }
        public Size Padding { get; set; }

        public Point Origin { get; private set; }
        public Point Centre => new Point(Origin.X + BodyRect.Width / 2, Origin.Y + BodyRect.Height / 2);

        public bool IsRoot { get; private set; }
        public DecisionData DecisionData { get; private set; }

        public Brush BodyFill { get; set; }

        private Brush _addButtonFill;
        private Brush _removeButtonFill;

        #region Constructors        

        public DecisionNode(ShapeType bodyType, ShapeType buttonType, Point origin, Rect rect, Size pad, bool isRoot, Panel parent = null)
        {
            int buttonRatio = Convert.ToInt32(ConfigurationManager.AppSettings["BodyButtonRatio"]);
            //MainBody = ShapeFactory.GetShape(bodyType);
            //MainBody.MouseEnter += OnNodeMouseOver;
            //MainBody.MouseLeave += OnNodeMouseOff;
            //MainBody.MouseDown += OnNodeMouseDown;

            //AddButton = ShapeFactory.GetShape(buttonType);
            //AddButton.MouseEnter += OnAddMouseOver;
            //AddButton.MouseDown += OnAddMouseDown;
            //AddButton.MouseLeave += OnAddMouseOff;

            //RemoveButton = ShapeFactory.GetShape(buttonType);
            //RemoveButton.MouseEnter += OnRemoveMouseOver;
            //RemoveButton.MouseDown += OnRemoveMouseDown;
            //RemoveButton.MouseLeave += OnRemoveMouseOff;

            MouseEnter += NodeMouseOver;
            MouseLeave += NodeMouseOff;
            MouseDown += OnNodeMouseDown;

            // TODO: Finish including geometry loading logic.
            BodyGeometry = GeometryBuilder.GetGeometry(bodyType, new Point(0, 0), rect.Size);
            AddButtonGeometry = GeometryBuilder.GetGeometry(buttonType, new Point(rect.Width + pad.Width, rect.Height + pad.Height), new Size(rect.Width / buttonRatio, rect.Height / buttonRatio));
            RemoveButtonGeometry = GeometryBuilder.GetGeometry(buttonType, new Point(rect.Width + pad.Width, rect.Height), new Size(rect.Width / buttonRatio, rect.Height / buttonRatio));

            BodyType = bodyType;
            ButtonType = buttonType;

            Origin = origin;
            BodyRect = rect;
            Padding = pad;           

            Padding = pad;

            ParentChanged += OnParentChanged;
            NodeNum = _nextNodeNum;
            _nextNodeNum++;

            IsRoot = isRoot;
            DecisionData = new DecisionData();
            ActionTextHolders = new List<TextBlock>();

            BindTitle();
            BodyFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["DefaultColour"].ToString()));
            _addButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["AddButtonColour"].ToString()));
            _removeButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["RemoveButtonColour"].ToString()));

            // Set starting position of element like this - prevents weird shit
            Canvas.SetTop(this, Origin.Y);
            Canvas.SetLeft(this, Origin.X);
            parent.Children.Add(this);
        }

        public DecisionNode(ShapeType bodyType, ShapeType buttonType, Point origin, Rect rect, Size pad, Color? BodyCol, bool isRoot, Panel parent = null)
        {
            MainBody = ShapeFactory.GetShape(bodyType);
            MainBody.MouseEnter += OnNodeMouseOver;
            MainBody.MouseLeave += OnNodeMouseOff;
            MainBody.MouseDown += OnNodeMouseDown;

            AddButton = ShapeFactory.GetShape(buttonType);
            AddButton.MouseEnter += OnAddMouseOver;
            AddButton.MouseDown += OnAddMouseDown;
            AddButton.MouseLeave += OnAddMouseOff;

            RemoveButton = ShapeFactory.GetShape(buttonType);
            RemoveButton.MouseEnter += OnRemoveMouseOver;
            RemoveButton.MouseDown += OnRemoveMouseDown;
            RemoveButton.MouseLeave += OnRemoveMouseOff;

            BodyType = bodyType;
            ButtonType = buttonType;

            Origin = origin;
            BodyRect = rect;
            Padding = pad;

            Padding = pad;

            ParentChanged += OnParentChanged;
            NodeNum = _nextNodeNum;
            _nextNodeNum++;

            IsRoot = isRoot;
            DecisionData = new DecisionData();
            ActionTextHolders = new List<TextBlock>();
            BindTitle();
            BodyFill = new SolidColorBrush(BodyCol == null ? (Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["DefaultColour"].ToString()) : BodyCol.Value);
            _addButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["AddButtonColour"].ToString()));
            _removeButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["RemoveButtonColour"].ToString()));

            // Set starting position of element like this - prevents weird shit
            Canvas.SetTop(this, Origin.Y);
            Canvas.SetLeft(this, Origin.X);
            parent.Children.Add(this);
        }


        public DecisionNode(ShapeType bodyType, ShapeType buttonType, Point origin, Rect rect, Size pad, DecisionData decisionData, Panel parent)
        {
            int buttonRatio = Convert.ToInt32(ConfigurationManager.AppSettings["BodyButtonRatio"]);
            MainBody = ShapeFactory.GetShape(bodyType);
            MainBody.MouseEnter += OnNodeMouseOver;
            MainBody.MouseLeave += OnNodeMouseOff;
            MainBody.MouseDown += OnNodeMouseDown;

            AddButton = ShapeFactory.GetShape(buttonType);
            AddButton.MouseEnter += OnAddMouseOver;
            AddButton.MouseDown += OnAddMouseDown;
            AddButton.MouseLeave += OnAddMouseOff;

            RemoveButton = ShapeFactory.GetShape(buttonType);
            RemoveButton.MouseEnter += OnRemoveMouseOver;
            RemoveButton.MouseDown += OnRemoveMouseDown;
            RemoveButton.MouseLeave += OnRemoveMouseOff;

            MouseEnter += NodeMouseOver;
            MouseLeave += NodeMouseOff;

            BodyGeometry = GeometryBuilder.GetGeometry(bodyType, new Point(0, 0), rect.Size);
            AddButtonGeometry = GeometryBuilder.GetGeometry(buttonType, new Point(rect.Width + pad.Width, rect.Height + pad.Height), new Size(rect.Width / buttonRatio, rect.Height / buttonRatio));
            RemoveButtonGeometry = GeometryBuilder.GetGeometry(buttonType, new Point(rect.Width + pad.Width, rect.Height), new Size(rect.Width / buttonRatio, rect.Height / buttonRatio));

            BodyType = bodyType;
            ButtonType = buttonType;

            Origin = origin;
            BodyRect = rect;
            Padding = pad;

            Padding = pad;

            ParentChanged += OnParentChanged;
            NodeNum = _nextNodeNum;
            _nextNodeNum++;

            DecisionData = decisionData == null ? new DecisionData() : decisionData;
            ActionTextHolders = new List<TextBlock>();
            BindTitle();
            BodyFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["DefaultColour"].ToString()));
            _addButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["AddButtonColour"].ToString()));
            _removeButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["RemoveButtonColour"].ToString()));

            Canvas.SetTop(this, Origin.Y);
            Canvas.SetLeft(this, Origin.X);
            parent.Children.Add(this);
        }

        public DecisionNode(ShapeType bodyType, ShapeType buttonType, Point origin, Rect rect, Size pad, Color? BodyCol, DecisionData decisionData, Panel parent)
        {
            int buttonRatio = Convert.ToInt32(ConfigurationManager.AppSettings["BodyButtonRatio"]);
            //MainBody = ShapeFactory.GetShape(bodyType);
            //MainBody.MouseEnter += OnNodeMouseOver;
            //MainBody.MouseLeave += OnNodeMouseOff;
            //MainBody.MouseDown += OnNodeMouseDown;

            //AddButton = ShapeFactory.GetShape(buttonType);
            //AddButton.MouseEnter += OnAddMouseOver;
            //AddButton.MouseDown += OnAddMouseDown;
            //AddButton.MouseLeave += OnAddMouseOff;

            //RemoveButton = ShapeFactory.GetShape(buttonType);
            //RemoveButton.MouseEnter += OnRemoveMouseOver;
            //RemoveButton.MouseDown += OnRemoveMouseDown;
            //RemoveButton.MouseLeave += OnRemoveMouseOff;

            MouseEnter += NodeMouseOver;
            MouseLeave += NodeMouseOff;
            MouseDown += OnNodeMouseDown;

            // TODO: Finish including geometry loading logic.
            BodyGeometry = GeometryBuilder.GetGeometry(bodyType, new Point(0, 0), rect.Size);
            AddButtonGeometry = GeometryBuilder.GetGeometry(buttonType, new Point(rect.Width + pad.Width, rect.Height + pad.Height), new Size(rect.Width / buttonRatio, rect.Height / buttonRatio));
            RemoveButtonGeometry = GeometryBuilder.GetGeometry(buttonType, new Point(rect.Width + pad.Width, rect.Height), new Size(rect.Width / buttonRatio, rect.Height / buttonRatio));

            BodyType = bodyType;
            ButtonType = buttonType;

            Origin = origin;
            BodyRect = rect;
            Padding = pad;

            Padding = pad;

            ParentChanged += OnParentChanged;
            NodeNum = _nextNodeNum;
            _nextNodeNum++;

            DecisionData = decisionData == null ? new DecisionData() : decisionData;
            ActionTextHolders = new List<TextBlock>();

            BindTitle();
            BodyFill = new SolidColorBrush(BodyCol == null ? (Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["DefaultColour"].ToString()) : BodyCol.Value);
            _addButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["AddButtonColour"].ToString()));
            _removeButtonFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["RemoveButtonColour"].ToString()));

            Canvas.SetTop(this, Origin.Y);
            Canvas.SetLeft(this, Origin.X);
            parent.Children.Add(this);
        }
        #endregion
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Pen rootPen = IsRoot ? new Pen(new SolidColorBrush(Colors.Gold), 2.5) : null;
            drawingContext.DrawGeometry(BodyFill, rootPen, BodyGeometry);
            drawingContext.DrawGeometry(_addButtonFill, null, AddButtonGeometry);
            drawingContext.DrawGeometry(_removeButtonFill, null, RemoveButtonGeometry);
        }
        #region Helper Methods
        private void BindTitle()
        {
            Text = new TextBlock { Foreground = Brushes.DarkGray, FlowDirection = FlowDirection.LeftToRight, Width = BodyRect.Width, Height = BodyRect.Height / 4, TextAlignment = TextAlignment.Center };
            Binding binding = new Binding("Title");
            binding.Source = DecisionData;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(Text, TextBlock.TextProperty, binding);
        }

        private void BindActionName(Canvas canvas, int childInx)
        {
            var window = ((Grid)(canvas.Parent)).Parent as MainWindow;
            DTNode<DecisionNode> dNode = window.Root.FindNode(window.Root, this);


            DTNode<DecisionNode> cNode = dNode.GetChild(childInx);
            TextBlock newAction = new TextBlock { Foreground = Brushes.Gray, FlowDirection = FlowDirection.LeftToRight };
            // Data binding
            Binding binding = new Binding("Action");
            binding.Source = dNode.Value.DecisionData.Actions[childInx];
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(newAction, TextBlock.TextProperty, binding);
            Canvas.SetTop(newAction, (Centre.Y + cNode.Value.Centre.Y) / 2);
            Canvas.SetLeft(newAction, (Centre.X + cNode.Value.Centre.X) / 2);
            dNode.Value.ActionTextHolders.Add(newAction);
        }

        public void DrawNode(Canvas canvas)
        {
            MainBody.Width = BodyRect.Width;
            MainBody.Height = BodyRect.Height;
            MainBody.Fill = BodyFill;
            if(IsRoot)
            {
                MainBody.Stroke = Brushes.Orange;
                MainBody.StrokeThickness = 2.5;
            }

            AddButton.Width = BodyRect.Width / 5;
            AddButton.Height = BodyRect.Height / 5;
            AddButton.Fill = _addButtonFill;

            RemoveButton.Width = BodyRect.Width / 5;
            RemoveButton.Height = BodyRect.Height / 5;
            RemoveButton.Fill = _removeButtonFill;           

            Canvas.SetTop(this, Origin.Y);
            Canvas.SetLeft(this, Origin.X);

            
            canvas.Children.Add(this); // works because DecisionNode is a UIElement descendant(?)
        }

        public void DrawConnector(DecisionNode parentNode, Canvas canvas)
        {
            if(parentNode == null)
            {
                if(ParentConnector != null)
                {
                    canvas.Children.Remove(ParentConnector);
                    ParentConnector = null;
                }                                
                MainBody.Stroke = Brushes.Orange;
                MainBody.StrokeThickness = 2.5;
                IsRoot = true;
                return;
            }
            if (ParentConnector == null)
            {
                ParentConnector = new Line();
                ParentConnector.X1 = 0;
                ParentConnector.Y1 = 0;
                ParentConnector.X2 = Centre.X - parentNode.Centre.X;
                ParentConnector.Y2 = Centre.Y - parentNode.Centre.Y;
                ParentConnector.Stroke = Brushes.Black;
                ParentConnector.StrokeThickness = 2;

                Canvas.SetTop(ParentConnector, parentNode.Centre.Y);
                Canvas.SetLeft(ParentConnector, parentNode.Centre.X);
                Canvas.SetZIndex(ParentConnector, 3);
                canvas.Children.Add(ParentConnector);
            }
            else
            {
                ParentConnector.X1 = 0;
                ParentConnector.Y1 = 0;
                ParentConnector.X2 = Centre.X - parentNode.Centre.X;
                ParentConnector.Y2 = Centre.Y - parentNode.Centre.Y;
                ParentConnector.Stroke = Brushes.Black;
                ParentConnector.StrokeThickness = 2;

                Canvas.SetTop(ParentConnector, parentNode.Centre.Y);
                Canvas.SetLeft(ParentConnector, parentNode.Centre.X);
                Canvas.SetZIndex(ParentConnector, 3);
            }
        }

        // note: to be used with the parent of a node, not the node itself.
        public void DrawActionName(Canvas canvas, int inx)
        {
            BindActionName(canvas, inx);
            if(inx < ActionTextHolders.Count)
            {
                canvas.Children.Add(ActionTextHolders[inx]);
            }            
        }

        public void DestroyNode(Canvas canvas)
        {
            canvas.Children.Remove(ParentConnector);
            canvas.Children.Remove(this);
            MainBody = null;
            AddButton = null;
            RemoveButton = null;
            ParentConnector = null;
            Text = null;
            ActionTextHolders = null;
            DecisionData = null;
        }

        public void UpdateSize(float width, float height)
        {
            BodyRect = new Rect(Origin, new Size(width, height));
            MainBody.Width = BodyRect.Width;
            MainBody.Height = BodyRect.Height;
        }

        public void UpdateColour(Color color)
        {
            BodyFill = new SolidColorBrush(color);
            MainBody.Fill = BodyFill;
        }

        public void UpdateShape(ShapeType shapeType)
        {
            Shape oldShape = MainBody;
            Shape newShape = ShapeFactory.GetShape(shapeType);             
            Brush brh = BodyFill;
            Rect rect = BodyRect;
            BodyType = shapeType;
            MainBody = newShape;
            MainBody.Fill = brh;
            MainBody.Width = rect.Width;
            MainBody.Height = rect.Height;
            MainBody.MouseEnter += OnNodeMouseOver;
            MainBody.MouseLeave += OnNodeMouseOff;
            MainBody.MouseDown += OnNodeMouseDown;
            Canvas.SetTop(MainBody, Origin.Y);
            Canvas.SetLeft(MainBody, Origin.X);
            Canvas.SetZIndex(MainBody, 1);
            // Replace old shape in canvas with new one
            Canvas canvas = (Canvas)LogicalTreeHelper.GetParent(this);
            canvas.Children.Remove(this);
            canvas.Children.Add(this);
        }

        private void CreateContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            var menuItem = new MenuItem { Header = "Format Node" };
            menuItem.Click += (o, i) =>
            {
                NodeFormatWindow newWindow = new NodeFormatWindow(this);
                newWindow.ShowDialog();
            };
            contextMenu.Items.Add(menuItem);
            ContextMenu = contextMenu;
        }
        #endregion

        #region Event Handlers
        private void OnAddMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = ((DecisionNode)e.Source).Parent as Canvas;
            var window = ((Grid)canvas.Parent).Parent as MainWindow;
            // find corresponding tree node for node item
            var dNode = window.Root.FindNode(window.Root, this);
            Point newOrigin = new Point(Origin.X + ((BodyRect.Width / 2 + Padding.Width) * ((dNode.GetChildCount() + 1) * _childDirection)), Origin.Y + BodyRect.Height + Padding.Height);
            _childDirection *= -1;
            DecisionNode newNode = new DecisionNode(BodyType, ButtonType, newOrigin, new Rect(newOrigin, BodyRect.Size), Padding, false, (Canvas)Parent);
            var newDNode = new DTNode<DecisionNode>(newNode);
            dNode.InsertChild(newDNode);

            // Add new entry in action names list, and list of action name textboxes
            DecisionData.Actions.Add(new DecisionAction { Action = string.Empty });

            TextBlock newAction = new TextBlock { Foreground = Brushes.Gray, FlowDirection = FlowDirection.LeftToRight };
            // Data binding
            Binding binding = new Binding("Action");
            binding.Source = DecisionData.Actions[dNode.GetChildIndex(newNode)];
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(newAction, TextBlock.TextProperty, binding);
            Canvas.SetTop(newAction, (Centre.Y + newNode.Centre.Y) / 2);
            Canvas.SetLeft(newAction, (Centre.X + newNode.Centre.X) / 2);

            ActionTextHolders.Add(newAction);
            canvas.Children.Add(newAction);

            
            newDNode.Value.DrawConnector(this, canvas);
        }

        private void OnRemoveMouseDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = ((DecisionNode)e.Source).Parent as Canvas;
            var window = ((Grid)canvas.Parent).Parent as MainWindow;
            // find corresponding tree node for node item
            var dNode = window.Root.FindNode(window.Root, this);
            var dNodeChildren = dNode.GetChildren();

            if (dNode.Parent != null)
            {
                var nodeChildInx = dNode.Parent.GetChildIndex(this);

                dNode.Parent.Value.DecisionData.Actions.RemoveAt(nodeChildInx);
                canvas.Children.Remove(dNode.Parent.Value.ActionTextHolders[nodeChildInx]);
                dNode.Parent.Value.ActionTextHolders.RemoveAt(nodeChildInx);
                
                
                DTNode<DecisionNode>.RemoveNode(dNode);
                for (int x = 0; x < dNodeChildren.Count; x++)
                {
                    DTNode<DecisionNode> child = dNodeChildren[x];
                    child.Value.ParentChanged(child.Parent?.Value, canvas);
                    child.Parent?.Value.DecisionData.Actions.Add(DecisionData.Actions[x]);
                    child.Parent?.Value.ActionTextHolders.Add(ActionTextHolders[x]);

                    
                    Binding binding = new Binding("Action");
                    int newParentInx = child.Parent?.GetChildIndex(child.Value) ?? -1;
                    binding.Source = child.Parent?.Value.DecisionData.Actions[newParentInx];
                    binding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(child.Parent?.Value.ActionTextHolders[newParentInx], TextBox.TextProperty, binding);
                    
                }
                BindingOperations.ClearBinding(Text, TextBox.TextProperty);
            }
            else
            {
                var tempNode = dNode;
                dNode = dNode.LeftChild;
                DTNode<DecisionNode>.RemoveNode(tempNode);
                
                dNode.Value.ParentChanged(dNode.Parent?.Value, canvas);                               
                for(int x = 1; x < tempNode.Value.DecisionData.Actions.Count; x++)
                {
                    var child = dNodeChildren[x];
                    child.Value.ParentChanged(child.Parent?.Value, canvas);

                    dNode.Value.DecisionData.Actions.Add(tempNode.Value.DecisionData.Actions[x]);
                    dNode.Value.ActionTextHolders.Add(tempNode.Value.ActionTextHolders[x]);

                    Binding binding = new Binding("Action");
                    int newParentInx = child.Parent?.GetChildIndex(child.Value) ?? -1;
                    binding.Source = child.Parent?.Value.DecisionData.Actions[newParentInx];
                    binding.Mode = BindingMode.TwoWay;
                    BindingOperations.SetBinding(child.Parent?.Value.ActionTextHolders[newParentInx], TextBox.TextProperty, binding);
                }
                canvas.Children.Remove(tempNode.Value.ActionTextHolders[0]);
                window.ResetRoot(dNode);
            }

            DestroyNode(canvas);
        }

        private void OnNodeMouseDown(object sender, MouseButtonEventArgs e)
        {
            NodeComponent clickedComp = GetClosestComponent(e.GetPosition(this));
            switch(clickedComp)
            {
                case NodeComponent.Body:
                    if (e.ChangedButton == MouseButton.Left)
                    {
                        DecisionEditWindow window = new DecisionEditWindow(DecisionData);
                        window.ShowDialog();
                    }
                    else if (e.ChangedButton == MouseButton.Right)
                    {
                        if (ContextMenu == null)
                        {
                            CreateContextMenu();
                        }
                        ContextMenu.IsOpen = true;
                    }
                    break;
                case NodeComponent.AddButton:
                    OnAddMouseDown(sender, e);
                    break;
                case NodeComponent.RemoveButton:
                    OnRemoveMouseDown(sender, e);
                    break;
            }
            
        }

        private void OnNodeMouseOver(object sender, MouseEventArgs e)
        {
            MainBody.Fill = Brushes.DarkCyan;
        }

        private void OnNodeMouseOff(object sender, MouseEventArgs e)
        {
            MainBody.Fill = BodyFill;
        }

        private void OnAddMouseOver(object sender, MouseEventArgs e)
        {
            AddButton.Fill = Brushes.LightGreen;
        }
        private void OnAddMouseOff(object sender, MouseEventArgs e)
        {
            if(AddButton != null)
                AddButton.Fill = _addButtonFill;
        }

        private void OnRemoveMouseOver(object sender, MouseEventArgs e)
        {
            RemoveButton.Fill = Brushes.Red;
        }
        private void OnRemoveMouseOff(object sender, MouseEventArgs e)
        {
            if(RemoveButton != null)
                RemoveButton.Fill = _removeButtonFill;
        }

        private void OnParentChanged(DecisionNode parentNode, Canvas canvas)
        {
            DrawConnector(parentNode, canvas);
        }

        private void NodeMouseOver(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(this);

            if (BodyGeometry.FillContains(mousePos))
            {
                BodyFill.Opacity = (BodyFill.Opacity / 2) - 0.01;
                
            }
            else if(AddButtonGeometry.FillContains(mousePos))
            {
                _addButtonFill.Opacity = (_addButtonFill.Opacity / 2) - 0.01;
                
                
            }
            else if(RemoveButtonGeometry.FillContains(mousePos))
            {
                _removeButtonFill.Opacity = (_removeButtonFill.Opacity / 2) - 0.01;
            }
            InvalidateVisual();
        }

        private void NodeMouseOff(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(this);

            Point[] closestPoints = new[] { GeometryUtils.ClosestPoint(BodyGeometry, mousePos), GeometryUtils.ClosestPoint(RemoveButtonGeometry, mousePos), GeometryUtils.ClosestPoint(AddButtonGeometry, mousePos) };

            double[] distances = closestPoints.Select(x => (x - mousePos).LengthSquared).ToArray();

            double smallest = double.MaxValue;
            double sInx = -1;
            for (int i = 0; i < distances.Length; i++)
            {

                if(distances[i] < smallest)
                {
                    smallest = distances[i];
                    sInx = i;
                }
            }

            switch(sInx)
            {
                case 0:
                    BodyFill.Opacity = (BodyFill.Opacity + 0.01) * 2;
                    break;
                case 1:
                    _removeButtonFill.Opacity = (_removeButtonFill.Opacity + 0.01) * 2;
                    break;
                case 2:
                    _addButtonFill.Opacity = (_addButtonFill.Opacity + 0.01) * 2;
                    break;
                default:
                    break;
            }

            //var source = sender as Canvas;
            InvalidateVisual();
        }

        private NodeComponent GetClosestComponent(Point mousePos)
        {
            double[] closestPoints = new[] { GeometryUtils.ClosestPoint(BodyGeometry, mousePos), GeometryUtils.ClosestPoint(AddButtonGeometry, mousePos), GeometryUtils.ClosestPoint(RemoveButtonGeometry, mousePos) }.Select(x => (x - mousePos).LengthSquared).ToArray();
            if(closestPoints[0] < closestPoints[1])
            {
                return (NodeComponent)(closestPoints[0] < closestPoints[2] ? 0 : 2);
            }
            else
            {
                return (NodeComponent)(closestPoints[1] < closestPoints[2] ? 1 : 2);
            }
        }
        #endregion

        public int CompareTo(DecisionNode other)
        {
            //Vector distance = new Vector(Centre.X - other.Centre.X, Centre.Y - other.Centre.Y);
            //return (int)distance.Length;
            return NodeNum - other.NodeNum;
        }

        public override string ToString()
        {
            return string.Format("Node #{0}", Text.Text);
        }
    }

    enum NodeComponent { Body, AddButton, RemoveButton }
}
