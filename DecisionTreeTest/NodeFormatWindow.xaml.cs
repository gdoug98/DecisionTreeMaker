using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DecisionTreeTest.Controls;
using DecisionTreeTest.UI;

namespace DecisionTreeTest
{
    /// <summary>
    /// Interaction logic for NodeFormatWindow.xaml
    /// </summary>
    public partial class NodeFormatWindow : Window
    {
        private List<SystemColour> _systemColours;
        private List<ShapeStruct> _shapeStructs;
        private DecisionNode _node;

        public NodeFormatWindow(DecisionNode node)
        {
            _node = node;
            InitializeComponent();
            InitialiseColours();
            cmbColour.ItemsSource = _systemColours;
            cmbColour.DataContext = _systemColours[0];

            InitialiseShapes();
            cmbShape.ItemsSource = _shapeStructs;
            cmbShape.DataContext = _shapeStructs[0];
        }
        private void InitialiseColours()
        {
            _systemColours = new List<SystemColour>();
            var colorsArray = Enum.GetValues(typeof(KnownColor));
            KnownColor[] allColors = new KnownColor[colorsArray.Length];
            foreach (KnownColor colour in colorsArray)
            {
                var oldCol = System.Drawing.Color.FromKnownColor(colour);
                var sysCol = new SystemColour { Name = colour.ToString(), Color = System.Windows.Media.Color.FromArgb(oldCol.A, oldCol.R, oldCol.G, oldCol.B) };
                sysCol.Fill = new SolidColorBrush(sysCol.Color);
                _systemColours.Add(sysCol);
            }
        }
        private void InitialiseShapes()
        {
            _shapeStructs = new List<ShapeStruct>();
            var shapesArray = Enum.GetValues(typeof(ShapeType));
            foreach(var shapeType in shapesArray)
            {
                _shapeStructs.Add(new ShapeStruct { Name = (ShapeType)shapeType, Shape = ShapeFactory.GetShape((ShapeType)shapeType) });
            }
        }

        private void cmdExit_Click(object sender, RoutedEventArgs e)
        {
            float x, y;
            bool sizeChanged = !string.IsNullOrWhiteSpace(txtWidth.Text) && !string.IsNullOrWhiteSpace(txtHeight.Text);
            try
            {
                x = -1;
                y = -1;
                if (sizeChanged)
                {
                    x = Convert.ToSingle(txtWidth.Text);
                    y = Convert.ToSingle(txtHeight.Text);
                }                
            }
            catch(FormatException)
            {
                MessageBox.Show("Please enter the width and height values in the correct format.");
                return;
            }
            catch(OverflowException)
            {
                MessageBox.Show("Please enter values within reasonable limits.");
                return;
            }

            ShapeType? selectedType = null;
            if (cmbShape.SelectedItem != null)
            {
                selectedType = ((ShapeStruct)cmbShape.SelectedItem).Name;
            }
            System.Windows.Media.Color? selectedColor = null;
            if (cmbColour.SelectedItem != null)
            {
                selectedColor = ((SystemColour)cmbColour.SelectedItem).Color;
            }            

            SolidColorBrush brh = _node.BodyFill as SolidColorBrush;
            if(brh != null && selectedColor != null)
            {
                if(!brh.Color.Equals(selectedColor))
                {
                    _node.UpdateColour(selectedColor ?? Colors.Aqua);
                }
            }

            if (selectedType != null && _node.BodyType != selectedType)
            {
                _node.UpdateShape(selectedType ?? ShapeType.Rectangle);
            }

            if(sizeChanged)
            {
                _node.UpdateSize(x, y);
            }

            Close();
        }
    }

    public struct SystemColour
    {
        public string Name { get; set; }
        public System.Windows.Media.Color Color { get; set; }
        public System.Windows.Media.Brush Fill { get; set; }
    }

    public struct ShapeStruct
    {
        public ShapeType Name { get; set; }
        public Shape Shape { get; set; }
    }
}
