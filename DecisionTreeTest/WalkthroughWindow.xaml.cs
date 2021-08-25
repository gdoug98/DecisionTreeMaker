using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using DecisionTree.Entities.Classes;
using DecisionTreeTest.Controls;

namespace DecisionTreeTest
{
    /// <summary>
    /// Interaction logic for WalkthroughWindow.xaml
    /// </summary>
    public partial class WalkthroughWindow : Window
    {
        //private ObservableCollection<DTNode<DecisionNode>> _nodes;
        //private DTNode<DecisionNode> _activeNode;

        //public WalkthroughWindow(DTNode<DecisionNode> root)
        //{
        //    InitializeComponent();
        //    _nodes = new ObservableCollection<DTNode<DecisionNode>>();
        //    _nodes.Add(root);
        //    lstNodes.ItemsSource = _nodes;
        //    lstNodes.DataContext = _nodes[0];

        //    _activeNode = _nodes[0];

        //    lstActions.ItemsSource = _activeNode.Value.DecisionData.Actions;
        //    lstActions.DataContext = _nodes[0].Value.DecisionData.Actions[0];
        //}

        //private void cmdAction_Click(object sender, RoutedEventArgs e)
        //{
        //    int selectInx = lstActions.SelectedIndex;
        //    if(selectInx != -1)
        //    {
        //        _activeNode = _activeNode.GetChild(selectInx);
        //        _nodes.Add(_activeNode);
        //        lstActions.ItemsSource = _activeNode.Value.DecisionData.Actions;
        //    }
        //}

        private ObservableCollection<SerialisableNode> _nodes;
        private SerialisableNode _activeNode;

        public WalkthroughWindow(SerialisableNode root)
        {
            InitializeComponent();
            _nodes = new ObservableCollection<SerialisableNode>();
            _nodes.Add(root);
            lstNodes.ItemsSource = _nodes;
            lstNodes.DataContext = _nodes[0];
            _activeNode = _nodes[0];

            lstActions.ItemsSource = _activeNode.Actions;
            lstActions.DataContext = _activeNode.Actions[0];
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            int selectInx = lstActions.SelectedIndex;
            if (selectInx != -1)
            {
                _activeNode = _activeNode.Children[selectInx];
                _nodes.Add(_activeNode);
                lstActions.ItemsSource = _activeNode.Actions;
            }
        }
    }
}
