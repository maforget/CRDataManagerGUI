using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataManagerGUI
{
    public class TreeViewReorder : TreeView
    {
        TreeView _treeView => this;
        Graphics g => this.CreateGraphics();
        internal string NodeMap = string.Empty;
        internal NodePosition? nodePosition;
        bool LineIsShown;

        public TreeViewReorder()
        {
            this.DragOver += TreeViewReorder_DragOver;
            this.ItemDrag += TreeViewReorder_ItemDrag;
            this.DragDrop += TreeViewReorder_DragDrop;
            LineIsShown = false;
            nodePosition = null;
        }

        private void TreeViewReorder_DragDrop(object sender, DragEventArgs e)
        {
            _treeView.Refresh();
        }

        private void TreeViewReorder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            NodeMap = string.Empty;
            LineIsShown = false;
            nodePosition = null;
        }

        private void TreeViewReorder_DragOver(object sender, DragEventArgs e)
        {
            TreeNode NodeOver = _treeView.GetNodeAt(this.PointToClient(Cursor.Position));
            TreeNode NodeMoving = GetTreeNode(e);

            // A bit long, but to summarize, process the following code only if the nodeover is null
            // and either the nodeover is not the same thing as nodemoving UNLESSS nodeover happens
            if (NodeOver != null && NodeMoving != null && NodeOver != NodeMoving)
            {
                Point pt = this.PointToClient(new Point(e.X, e.Y));
                nodePosition = GetNodePosition(NodeOver, pt);
                ProcessLine(NodeMoving, NodeOver);
            }
            else if (NodeOver == null && LineIsShown)//So the Line dissapears when we are at the ends of the TreeView
            {
                this.Invalidate();
                LineIsShown = false;
            }
        }

        public NodePosition GetNodePosition(TreeNode NodeOver, Point pt)
        {
            int OffsetY = pt.Y - NodeOver.Bounds.Top;
            bool hasChild = NodeOver.Nodes.Count > 0 || NodeOver is TreeNodeGroup;

            if (hasChild)
            {
                if (OffsetY < (NodeOver.Bounds.Height / 3))
                {
                    return NodePosition.Above;
                }
                else if (OffsetY > (NodeOver.Bounds.Height - (NodeOver.Bounds.Height / 3)))
                {
                    return NodePosition.Below;
                }
                else
                {
                    return NodePosition.In;
                }
            }
            else
            {
                if (OffsetY < (NodeOver.Bounds.Height / 2))
                    return NodePosition.Above;
                else
                    return NodePosition.Below;
            }

        }

        #region Draw Lines
        private void ProcessLine(TreeNode NodeMoving, TreeNode NodeOver)
        {
            if (nodePosition == null)
                return;

            // If NodeOver is a child then cancel
            TreeNode tnParadox = NodeOver;
            while (tnParadox.Parent != null)
            {
                if (tnParadox.Parent == NodeMoving)
                {
                    this.NodeMap = "";
                    nodePosition = null;
                    return;
                }

                tnParadox = tnParadox.Parent;
            }

            // Store the placeholder info into a pipe delimited string
            bool isBelow = nodePosition == NodePosition.Below ? true : false;
            string newNodeMap = SetNodeMap(NodeOver, isBelow, out int newIndex);

            //Checks for the lines that are above and below itself
            string invalidNodeMapAbove = SetNodeMap(NodeMoving, false);
            string invalidNodeMapBelow = SetNodeMap(NodeMoving, true);

            bool LastIndex = (NodeMoving is TreeNodeGroup) && nodePosition == NodePosition.Below && NodeOver.Index == NodeOver.Parent?.Nodes.Count - 1;
            bool normalInBetweenGroups = NodeMoving.GetType() != typeof(TreeNodeGroup) && NodeOver.GetType() == typeof(TreeNodeGroup) 
                && (nodePosition == NodePosition.Below || nodePosition == NodePosition.Above);
            int numberOfGroups = GetNumberOfGroups(NodeOver.Parent);
            bool areGroupsOver = (NodeMoving is TreeNodeGroup) && NodeOver.Index == numberOfGroups && (nodePosition == NodePosition.Below);
            bool IsInvalid = nodePosition != NodePosition.In && (invalidNodeMapAbove == newNodeMap || invalidNodeMapBelow == newNodeMap || normalInBetweenGroups);
            if (IsInvalid) nodePosition = null;

            if(LineIsShown && (areGroupsOver || LastIndex))
            {
                this.Invalidate();
                LineIsShown = false;
                return;
            }

            //Don't redraw the line if the line is at the same index OR
            //Don't redraw the line if the line is in a invalid position (same position)
            //Don't redraw the Line when the Node that is Moving is a Group and the position is anyhting else other than an other Group
            //Unless it's the first regular non Group item after the LastGroup
            bool firstNonGroup = (NodeMoving is TreeNodeGroup) && numberOfGroups > 0 && NodeOver.Index == numberOfGroups && (nodePosition == NodePosition.Above)
                && NodeOver.GetType() != typeof(TreeNodeGroup);
            if (AreNodeMapsEqual(newNodeMap) || IsInvalid || (NodeMoving is TreeNodeGroup && !(NodeOver is TreeNodeGroup) && !firstNonGroup))
                return;

            //Clear placeholders above and below
            this.Refresh();

            //Draw the line
            DrawLine(NodeOver);
        }

        private void DrawLine(TreeNode NodeOver)
        {
            int NodeOverImageWidth = this.ImageList.Images[NodeOver.ImageIndex].Size.Width + 30;
            int LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            int RightPos = _treeView.Width - 8;
            int boundary = nodePosition == NodePosition.Below ? NodeOver.Bounds.Bottom :
                nodePosition == NodePosition.Above ? NodeOver.Bounds.Top : NodeOver.Bounds.Top + (NodeOver.Bounds.Height / 2);

            Point[] LeftTriangle = new Point[3]
            {
                new Point(LeftPos, boundary - 4),
                new Point(LeftPos, boundary + 4),
                new Point(LeftPos + 4, boundary)
            };

            Point[] RightTriangle = new Point[3]
            {
                new Point(RightPos, boundary - 4),
                new Point(RightPos, boundary + 4),
                new Point(RightPos - 4, boundary)
            };

            Pen customPen = new Pen(Color.Black, 2);
            if (nodePosition == NodePosition.In)
            {
                //NodeOver.Expand();
                g.FillPolygon(Brushes.Black, LeftTriangle);
            }
            else
            {
                g.FillPolygon(Brushes.Black, LeftTriangle);
                g.FillPolygon(Brushes.Black, RightTriangle);
                g.DrawLine(customPen, new Point(LeftPos, boundary), new Point(RightPos, boundary));
            }

            LineIsShown = true;
        }
        #endregion

        private int GetNumberOfGroups(TreeNode node)
        {
            if (node == null) return 0;

            List<TreeNode> list = new List<TreeNode>();
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode item = node.Nodes[i];
                if (item.GetType() == typeof(TreeNodeGroup))
                    list.Add(item);
            }

            return list.Count;
        }

        private string SetNodeMap(TreeNode tnNode, bool boolBelowNode)
        {
            return SetNodeMap(tnNode, boolBelowNode, out int LastIndex);
        }

        private string SetNodeMap(TreeNode tnNode, bool boolBelowNode, out int newIndex)
        {
            StringBuilder NewNodeMap = new StringBuilder();

            if (boolBelowNode)
                newIndex = tnNode.Index + 1;
            else
                newIndex = tnNode.Index;
            NewNodeMap.Insert(0, newIndex);

            TreeNode tnCurNode = tnNode;
            while (tnCurNode.Parent != null)
            {
                tnCurNode = tnCurNode.Parent;

                if (NewNodeMap.Length == 0 && boolBelowNode == true)
                {
                    NewNodeMap.Insert(0, $"{tnCurNode.Index + 1}|");
                }
                else
                {
                    NewNodeMap.Insert(0, $"{tnCurNode.Index}|");
                }
            }

            return NewNodeMap.ToString();
        }

        private bool AreNodeMapsEqual(string NewNodeMap)
        {
            if (nodePosition == NodePosition.In)
                NewNodeMap = NewNodeMap.Insert(NewNodeMap.Length, "|0");

            if (NewNodeMap == this.NodeMap)
                return true;
            else
            {
                this.NodeMap = NewNodeMap;
                return false;
            }
        }

        internal TreeNode GetTreeNode(DragEventArgs drgevent)
        {
            TreeNode obj = (TreeNode)drgevent.Data.GetData(drgevent.Data.GetFormats()[0]);
            if (typeof(TreeNode).IsAssignableFrom(obj.GetType()))
                return obj;

            return null;
        }
    }

    public class TreeNodeGroup : TreeNode
    {
        public TreeNodeGroup()
            : base()
        {

        }

        public TreeNodeGroup(string Name)
            : base(Name)
        {

        }
    }

    public enum NodePosition
    {
        Above,
        Below,
        In
    }
}
