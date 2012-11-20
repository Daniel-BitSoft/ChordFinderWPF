using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ChordFinderWPF
{
    class TreeNode
    {
        private ArrayList Children = null;
        public TreeNode LeftChild;
        public TreeNode RightChild;
        public TreeNode Parent;
        public List<int[]> Values;
        public int CompareValue, Order;

        public TreeNode(TreeNode parent, int Comparevalue, int order) 
        {
            Parent = parent;
            CompareValue = Comparevalue;
            Order = order;
        }

        public void SetChildren(ArrayList children)
        {
            Children = children;
        }
    }
}
