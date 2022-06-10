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
        private class TreeNodeInfo
        {
            public TreeNodeInfo(TreeNode node)
            {
                if (node == null)
                {
                    NumberOfGroups = 0;
                    NumberOfRulesets = 0;
                    return;
                }

                List<TreeNode> groups = new List<TreeNode>();
                List<TreeNode> rulesets = new List<TreeNode>();
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    TreeNode item = node.Nodes[i];
                    if (item.GetType() == typeof(TreeNodeGroup))
                        groups.Add(item);
                    else
                        rulesets.Add(item);
                }

                NumberOfGroups = groups.Count;
                NumberOfRulesets = rulesets.Count;
            }

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

            public int NumberOfGroups { get; set; }
            public int NumberOfRulesets { get; set; }
        }

        TreeView _treeView => this;
        Graphics g => this.CreateGraphics();
        internal protected string NodeMap = string.Empty;
        internal protected NodePosition? nodePosition;
        private bool isLineShown;
        List<Rectangle> rects = new List<Rectangle>();

        public TreeViewReorder()
        {
            isLineShown = false;
            nodePosition = null;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            NodeMap = string.Empty;
            isLineShown = false;
            nodePosition = null;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
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
            else if (NodeOver == null && isLineShown)//Remove the line when at the end of the tree.
            {
                this.Invalidate(true);
                isLineShown = false;
                return;
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
            if (!ValidateLocation(NodeOver, NodeMoving))
                return;

            //Clear placeholders above and below
            if (isLineShown)
                RemoveLines();

            //Draw the line
            DrawLine(NodeOver);
        }

        private bool ValidateLocation(TreeNode NodeOver, TreeNode NodeMoving)
        {
            if (nodePosition == null)
                return false;

            // If NodeOver is a child then cancel
            TreeNode tnParadox = NodeOver;
            while (tnParadox.Parent != null)
            {
                if (tnParadox.Parent == NodeMoving)
                {
                    this.NodeMap = "";
                    nodePosition = null;
                    return false;
                }

                tnParadox = tnParadox.Parent;
            }

            // Store the placeholder info into a pipe delimited string
            bool isBelow = nodePosition == NodePosition.Below ? true : false;
            string newNodeMap = SetNodeMap(NodeOver, isBelow, out int newIndex);

            //Checks for the lines that are above and below itself same location
            string sameLocationAbove = SetNodeMap(NodeMoving, false);
            string sameLocationBelow = SetNodeMap(NodeMoving, true);
            bool IsInvalid = (sameLocationAbove == newNodeMap || sameLocationBelow == newNodeMap);

            //Remove the Lines after the groups are over and at the the end of the list
            TreeNodeInfo tni = new TreeNodeInfo(NodeOver.Parent);
            bool areGroupsOver = !(NodeOver is TreeNodeGroup) && (NodeMoving is TreeNodeGroup) && tni.NumberOfGroups > 0 && NodeOver.Index == tni.NumberOfGroups && (nodePosition == NodePosition.Below);
            bool LastIndex = (NodeMoving is TreeNodeGroup) && nodePosition == NodePosition.Below && NodeOver.Index == NodeOver.Parent?.Nodes.Count - 1 && tni.NumberOfRulesets > 0;
            //No lines for normal nodes inbetween Groups, only In
            bool normalInBetweenGroups = NodeMoving.GetType() != typeof(TreeNodeGroup) && NodeOver.GetType() == typeof(TreeNodeGroup)
                && (nodePosition == NodePosition.Below || nodePosition == NodePosition.Above) && newIndex != tni.NumberOfGroups;
            //Show the line when moving a group only when the groups are over, so the line to insert at the end of the groups, but not between normal nodes
            bool firstNonGroup = (NodeMoving is TreeNodeGroup) && tni.NumberOfGroups > 0 && NodeOver.Index == tni.NumberOfGroups && (nodePosition == NodePosition.Above)
                && NodeOver.GetType() != typeof(TreeNodeGroup);
            bool betweenRootNode = NodeOver.Parent == null && nodePosition != NodePosition.In;

            if (isLineShown && (areGroupsOver || LastIndex || normalInBetweenGroups || betweenRootNode))
            {
                RemoveLines();
                isLineShown = false;
                nodePosition = null;
                NodeMap = newNodeMap;
                return false;
            }

            //Don't redraw the line if the line is at the same index OR
            //Don't redraw the line if the line is in a invalid position (same position)
            //Don't redraw the Line when the Node that is Moving is a Group and the position is anyhting else other than an other Group
            //Unless it's the first regular non Group item after the Last Group
            if (AreNodeMapsEqual(newNodeMap) || IsInvalid || normalInBetweenGroups || (NodeMoving is TreeNodeGroup && !(NodeOver is TreeNodeGroup) && !firstNonGroup))
            {
                return false;
            }

            return true;
        }

        private void DrawLine(TreeNode NodeOver)
        {
            int NodeOverImageWidth = this.ImageList.Images[NodeOver.ImageIndex].Size.Width + 24;
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

            Rectangle rect = new Rectangle(LeftPos, boundary - 4, RightPos, 8);
            rects.Add(rect);
            isLineShown = true;
        }

        private void RemoveLines()
        {
            foreach (Rectangle item in rects)
            {
                this.Invalidate(item);
            }

            rects = new List<Rectangle>();
        }
        #endregion

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
