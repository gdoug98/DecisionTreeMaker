using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Entities.Classes
{
    public class DTNode<T> where T: IComparable<T>
    {
        public T Value { get; set; }
        public DTNode<T> Parent { get; private set; }
        public DTNode<T> LeftChild { get; private set; }
        public DTNode<T> NextSibling { get; private set; }

        public bool IsLeaf => LeftChild == null;

        public DTNode(T value)
        {
            Value = value;
            Parent = null;
            LeftChild = null;
            NextSibling = null;
        }

        public DTNode(T value, DTNode<T> parent, DTNode<T> child)
        {
            Value = value;
            // safe to assume child will be null, since we're in the constructor
            LeftChild = child;
            Parent = parent;
            if (Parent != null)
            {
                // Making sure new child is included in hierarchy - not needed if it's the root node
                Parent.InsertChild(child);
            }
        }

        private void InsertSibling(DTNode<T> sibling)
        {
            if(sibling == null)
            {
                return;
            }
            if (NextSibling == null)
            {
                NextSibling = sibling;
                // Making a hefty assumption here - that node being inserted is a descendant of root (which has no siblings, only children).
                NextSibling.Parent = Parent;
            }
            else
            {
                NextSibling.InsertSibling(sibling);
            }
        }

        public void InsertChild(DTNode<T> child)
        {
            if(child == null)
            {
                return;
            }
            if (LeftChild == null)
            {
                LeftChild = child;
                LeftChild.Parent = this;
            }
            else
            {
                LeftChild.InsertSibling(child);
            }
        }

        //public static DTNode<T> SetChild(DTNode<T> node, DTNode<T> child)
        //{
        //    node.LeftChild = child;
        //    if(node.LeftChild != null)
        //    {
        //        node.LeftChild.Parent = node;
        //    }            
        //    return node;
        //}

        //public static DTNode<T> SetSibling(DTNode<T> node, DTNode<T> sibling)
        //{
        //    node.NextSibling = sibling;
        //    if(node.NextSibling != null)
        //    {
        //        node.NextSibling.Parent = node.Parent;
        //    }            
        //    return node;
        //}

        public DTNode<T> FindNode(DTNode<T> root, T value)
        {
            DTNode<T> ret = null;
            if(root == null || root.Value.CompareTo(value) == 0)
            {
                return root;
            }
            if(root != null && !root.IsLeaf)
            {
                ret = FindNode(root.LeftChild, value);
            }
            if(ret == null && root.NextSibling != null)
            {
                ret = FindNode(root.NextSibling, value);
            }
            return ret;
        }

        public static void RemoveNode(DTNode<T> node)
        {
            DTNode<T> currentNode;
            if (node.Parent == null)
            {
                // in this case the node is root, replace with left child and add siblings as children.                 
                node = node.LeftChild;
                currentNode = node;
                while(currentNode.NextSibling != null)
                {
                    DTNode<T> temp = currentNode.NextSibling;
                    currentNode.NextSibling = null;
                    node.InsertChild(temp);
                    currentNode = temp;
                }
                node.Parent = null;
                return;
            }
            // Otherwise remove node as child of parent node or sibling of another node, depending on relationship.
            currentNode = node.Parent;
            if(currentNode.LeftChild.Value.CompareTo(node.Value) == 0)
            {
                currentNode.RemoveLeftChild();
            }
            else
            {
                currentNode = currentNode.LeftChild;
                while (currentNode.NextSibling != null && currentNode.NextSibling.Value.CompareTo(node.Value) != 0)
                {
                    currentNode = currentNode.NextSibling;
                }
                currentNode.RemoveSibling();
            }
        }
       
        /// <summary>
        /// Remove left child node with respect to the current node
        /// </summary>
        /// <param name="root"></param>
        private void RemoveLeftChild()
        {
            if(LeftChild == null)
            {
                return;
            }
            DTNode<T> temp = LeftChild;
            Collection<DTNode<T>> tempChildren = temp.GetChildren();
            LeftChild = LeftChild.NextSibling;
            foreach(var child in tempChildren)
            {
                child.NextSibling = null;
                InsertChild(child);
            }
        }

        /// <summary>
        /// Remove sibling node with respect to the current node
        /// </summary>
        private void RemoveSibling()
        {
            if(NextSibling == null)
            {
                return;
            }
            DTNode<T> temp = NextSibling;
            Collection<DTNode<T>> tempChildren = temp.GetChildren();
            NextSibling = NextSibling.NextSibling;
            foreach (var child in tempChildren)
            {
                child.NextSibling = null;
                Parent.InsertChild(child);
            }
        }
        public int GetChildCount()
        {
            int ret = 0;
            if (IsLeaf)
            {
                return ret;
            }
            DTNode<T> current = LeftChild;
            while (current != null)
            {
                current = current.NextSibling;
                ret++;
            }
            return ret;
        }

        public int GetChildIndex(T value)
        {
            int ret = 0;
            if (IsLeaf)
            {
                return ret;
            }
            DTNode<T> current = LeftChild;
            while (current != null && current.Value.CompareTo(value) != 0)
            {
                current = current.NextSibling;
                ret++;
            }
            return ret;
        }

        public Collection<DTNode<T>> GetChildren()
        {
            Collection<DTNode<T>> ret = new Collection<DTNode<T>>();
            if (IsLeaf)
            {
                return ret;
            }
            DTNode<T> current = LeftChild;
            while (current != null)
            {
                ret.Add(current);
                current = current.NextSibling;
            }
            return ret;
        }

        public DTNode<T> GetChild(int inx)
        {
            int count = 0;
            DTNode<T> currNode = LeftChild;
            if(LeftChild == null)
            {
                return null;
            }
            while(count != inx && currNode != null)
            {
                currNode = currNode.NextSibling;
                count++;
            }
            return currNode;
        }
    }
}
