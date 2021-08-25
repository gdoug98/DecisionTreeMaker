using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DecisionTree.Entities.Classes;

namespace DecisionTreeTest
{
    /// <summary>
    /// Interaction logic for DecisionEditWindow.xaml
    /// </summary>
    public partial class DecisionEditWindow : Window
    {
        private DecisionData _data;
        

        public DecisionEditWindow()
        {
            InitializeComponent();
        }

        public DecisionEditWindow(DecisionData data)
        {
            InitializeComponent();
            _data = data;
            if(_data != null)
            {
                txtTitle.DataContext = _data.Title;
                txtTitle.Text = _data.Title;

                txtDescription.DataContext = _data.Description;
                txtDescription.Text = _data.Description;

                foreach(var action in _data.Actions)
                {
                    lbActions.Items.Add(new TextBox { Text = action.Action, Height = 60 });
                }
            }
            //InitialiseColours();
            ////cmbColour.ItemsSource = _systemColours;
            ////cmbColour.DataContext = _systemColours[0];
        }

        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _data.Description = txtDescription.Text;
            _data.Title = txtTitle.Text;
            for(int c = 0; c < _data.Actions.Count; c++)
            {
                if (lbActions.Items[c] is TextBox item)
                {
                    _data.Actions[c].Action = item.Text;
                }
            }
        }
    }

    
}
