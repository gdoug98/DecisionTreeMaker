using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using DecisionTreeTest.Controls;
using DecisionTreeTest.UI;
using DecisionTree.Entities.Classes;
using System.IO;

namespace DecisionTreeTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private const string _ImageDirectory = @"./../../../../../../"
        private string _defaultSaveDirectory;
        public DTNode<DecisionNode> Root { get; private set; }
        public List<TreeNode> TreeNodes { get; private set; }

        public MainWindow()
        {          

            InitializeComponent();
            _defaultSaveDirectory = ConfigurationManager.AppSettings["DefaultTreeSaveDirectory"].ToString();
            
            DecisionNode newNode = new DecisionNode(ShapeType.Ellipse, ShapeType.Rectangle, new Point(Width / 2, 25), new Rect(new Point(Width / 2, Height / 2), new Size(180, 72)), new Size(20, 20), true, _canvas);
            Root = new DTNode<DecisionNode>(newNode);
            //Root.Value.DrawNode(_canvas);
        }

        public void ResetRoot(DTNode<DecisionNode> root)
        {
            // Guard condition - prevents root from being initialised to null
            if (root == null)
            {
                return;
            }
            Root = root;
        }

        private void _saveButton_Click(object sender, RoutedEventArgs e)
        {
            SerialisableNode sNode = SerialisableNode.Create(Root);
            SaveFileDialog sfg = new SaveFileDialog();
            if(sfg.ShowDialog() == true)
            {
                bool status = FileIO.WriteToFile(sNode, sfg.FileName);
                MessageBox.Show(status ? "Tree data saved successfully" : "Failed to save tree data.");
                _saveTreeDiagram(System.IO.Path.GetFileName(sfg.FileName), System.IO.Path.GetDirectoryName(sfg.FileName));
            }
        }

        private void _saveTreeDiagram(string filePath, string _dir)
        {
            var target = new RenderTargetBitmap((int)(_canvas.RenderSize.Width), (int)(_canvas.RenderSize.Height), 96, 96, PixelFormats.Pbgra32);
            var visual = new DrawingVisual();
            var bounds = VisualTreeHelper.GetDescendantBounds(_canvas);
            using (DrawingContext dc = visual.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(_canvas);
                dc.DrawRectangle(brush, null, new Rect(new Point(), bounds.Size));
                //foreach (UIElement child in _canvas.Children)
                //{
                //    //VisualBrush visB = new VisualBrush(child);
                //    if(child is DecisionNode)
                //    {
                //        DecisionNode n = child as DecisionNode;
                //        dc.DrawGeometry(n.BodyFill, null, n.BodyGeometry);
                //    }
                //    else
                //    {
                //        // only ever lines if not a node, ignoring brush
                //        //Brush brh = child.GetType().GetProperty("Background").GetValue(child) as Brush;
                //        //Pen pen = child.GetType().GetProperty("Stroke").GetValue(child) as Pen;
                //        ////dc.DrawGeometry(null, pen ?? new Pen(new SolidColorBrush(Colors.Black), 2.5), Line);
                //        //dc.DrawLine(pen ?? new Pen(new SolidColorBrush(Colors.Black), 2.5), child.)

                //        Line l = child as Line;
                //        dc.DrawLine(new Pen(l.Stroke, 2.5), new Point(l.X1, l.Y1), new Point(l.X2, l.Y2));
                //    }
                    
                //}
            }

            target.Render(visual);

            string dir = _dir ?? _defaultSaveDirectory;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(target);
            encoder.Frames.Add(outputFrame);
            FileIO.WritePngToFile(encoder, filePath, dir);
        }

        private void _loadButton_Click(object sender, RoutedEventArgs e)
        {
            
            SerialisableNode sNode;
            OpenFileDialog lfg = new OpenFileDialog();
            if(lfg.ShowDialog() == true)
            {
                sNode = FileIO.ReadFromFile<SerialisableNode>(lfg.FileName);
                Root.Value.DestroyNode(_canvas);
                _canvas.Children.Clear();
                MessageBox.Show(sNode == null ? $"Failed to load tree data from {lfg.FileName}" : $"Tree data in {lfg.FileName} loaded successfully.");
                Root = buildTree(sNode);
                //drawTree(Root);
            }
        }

        private DTNode<DecisionNode> buildTree(SerialisableNode root)
        {
            if(root == null)
            {
                return null;
            }
            DTNode<DecisionNode> ret = new DTNode<DecisionNode>(new DecisionNode(root.Type, ShapeType.Triangle, root.Origin, new Rect(root.Origin, new Size(180, 72)), new Size(20, 20), root.BodyColour, new DecisionData(root.ActionStrings, root.Title, root.Description), _canvas));
            SerialisableNode currNode = root.FirstChild;
            while(currNode != null)
            {
                DTNode<DecisionNode> newNode = buildTree(currNode);
                ret.InsertChild(newNode);
                newNode.Value.DrawConnector(newNode.Parent.Value, _canvas);
                currNode = currNode.NextSibling;
            }
            //ret.InsertChild(buildTree(root.FirstChild));
            // ret = DTNode<DecisionNode>.SetChild(ret, buildTree(root.FirstChild));
            return ret;
        }

        private void drawTree(DTNode<DecisionNode> root)
        {
            if(root == null)
            {
                return;
            }
            root.Value.DrawNode(_canvas);
            root.Value.DrawConnector(root.Parent?.Value, _canvas);
            if (root.Parent != null)
            {
                root.Parent.Value.DrawActionName(_canvas, root.Parent.GetChildIndex(root.Value));
            }
            drawTree(root.LeftChild);
            drawTree(root.NextSibling);
            // int childCount = root.GetChildCount();
                       
        }

        private void CmdWalkthrough_Click(object sender, RoutedEventArgs e)
        {
            if(Root.Value.DecisionData.Actions.Count < 1)
            {
                MessageBox.Show("Root Node has no children, and cannot be displayed!");
                return;
            }
            WalkthroughWindow newWindow = new WalkthroughWindow(SerialisableNode.Create(Root));
            newWindow.Show();
        }
    }
}
