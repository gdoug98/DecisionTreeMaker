using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DecisionTree.Entities.Classes;
using DecisionTreeTest.Controls;
using DecisionTreeTest.UI;
using System.Xml.Serialization;
using System.Configuration;
using System.Windows.Media;

namespace DecisionTreeTest
{
    [Serializable]
    public class SerialisableNode
    {
        [XmlAttribute("Root")] public bool IsRoot { get; set; }
        public Point Origin { get; set; }
        [XmlAttribute("NodeTitle")] public string Title { get; set; }
        [XmlAttribute("ShapeType")] public ShapeType Type { get; set; }
        public string Description { get; set; }
        [XmlIgnore] public DecisionAction[] Actions { get; private set; } // for use in data binding with XAML forms - doesn't need to be serialised
        [XmlArray("Actions")] public string[] ActionStrings { get; set; }
        [XmlIgnore] public SerialisableNode Parent { get; private set; }
        public SerialisableNode FirstChild { get; set; }
        public SerialisableNode NextSibling { get; set; }
        public Color BodyColour { get; set; }

        [XmlIgnore]
        public List<SerialisableNode> Children
        {
            get
            {
                List<SerialisableNode> ret = new List<SerialisableNode>();
                SerialisableNode current = FirstChild;
                while(current != null)
                {
                    ret.Add(current);
                    current = current.NextSibling;
                }
                return ret;
            }
        }

        [XmlIgnore]
        public SolidColorBrush Fill
        {
            get
            {
                return new SolidColorBrush(BodyColour);
            }
        }

        public SerialisableNode()
        {

        }

        //public SerialisableNode(DTNode<DecisionNode> node)
        //{
        //    if(node == null)
        //    {
        //        return;
        //    }
        //    IsRoot = node.Parent == null;
        //    Origin = node.Value.Origin;
        //    Title = node.Value.DecisionData.Title;
        //    Description = node.Value.DecisionData.Description;
        //    Actions = node.Value.DecisionData.Actions.Select(x => x.Action).ToArray();
        //    Type = node.Value.BodyType;
        //    FirstChild = new SerialisableNode(node.LeftChild);
        //    if(FirstChild != null)
        //    {
        //        FirstChild.Parent = this;
        //    }
        //    NextSibling = new SerialisableNode(node.NextSibling);
        //    if(NextSibling != null)
        //    {
        //        NextSibling.Parent = Parent;
        //    }
        //}

        public static SerialisableNode Create(DTNode<DecisionNode> node)
        {
            SerialisableNode ret = null;
            if (node == null)
            {
                return ret;
            }
            ret = new SerialisableNode();
            ret.IsRoot = node.Parent == null;
            ret.Origin = node.Value.Origin;
            ret.Title = node.Value.DecisionData.Title;
            ret.Description = node.Value.DecisionData.Description;
            ret.Actions = node.Value.DecisionData.Actions.ToArray();
            ret.ActionStrings = ret.Actions.Select(x => x.Action).ToArray();
            ret.Type = node.Value.BodyType;
            SolidColorBrush brh = node.Value.BodyFill as SolidColorBrush;
            ret.BodyColour = brh?.Color ?? Colors.Wheat;
            ret.FirstChild = SerialisableNode.Create(node.LeftChild);
            if (ret.FirstChild != null)
            {
                ret.FirstChild.Parent = ret;
            }
            ret.NextSibling = SerialisableNode.Create(node.NextSibling);
            if (ret.NextSibling != null)
            {
                ret.NextSibling.Parent = ret.Parent;
            }
            return ret;
        }

        //public static SerialisableNode LoadNode(string fileName, string dir)
        //{
        //    return FileIO.ReadFromFile<SerialisableNode>(fileName, dir);
        //}

        //public bool SaveNode(string fileName, string dir)
        //{
        //    return FileIO.WriteToFile(this, fileName, dir);
        //}
    }
}
