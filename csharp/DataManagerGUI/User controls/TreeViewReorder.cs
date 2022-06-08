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
        string NodeMap = string.Empty;
        //StringBuilder NewNodeMap = new StringBuilder();
        bool LineIsShown;

        public TreeViewReorder()
        {
            this.DragOver += TreeViewReorder_DragOver;
            this.ItemDrag += TreeViewReorder_ItemDrag;
            this.DragDrop += TreeViewReorder_DragDrop;
            LineIsShown = false;
        }

        private void TreeViewReorder_DragDrop(object sender, DragEventArgs e)
        {
            _treeView.Refresh();
        }

        private void TreeViewReorder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            NodeMap = string.Empty;
            LineIsShown = false;
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
                var nodePosition = GetNodePosition(NodeOver, pt);
                DrawLine(nodePosition, NodeMoving, NodeOver);
            }
            else if (NodeOver == null && LineIsShown == true)//So the Line dissapears when we are at the ends of the TreeView
            {
                this.Invalidate();
                LineIsShown = false;
            }
        }

        public NodePosition GetNodePosition(TreeNode NodeOver, Point pt)
        {
            int OffsetY = pt.Y - NodeOver.Bounds.Top;
            bool hasChild = NodeOver.Nodes.Count > 0 || NodeOver is TreeNodeGroup;

            if (hasChild)//What about an empty folder?
                return NodePosition.In;

            if (OffsetY < (NodeOver.Bounds.Height / 2))
                return NodePosition.Above;
            else
                return NodePosition.Below;
        }

        private void DrawLine(NodePosition position, TreeNode NodeMoving, TreeNode NodeOver)
        {
            // If NodeOver is a child then cancel
            TreeNode tnParadox = NodeOver;
            while (tnParadox.Parent != null)
            {
                if (tnParadox.Parent == NodeMoving)
                {
                    this.NodeMap = "";
                    return;
                }

                tnParadox = tnParadox.Parent;
            }

            // Store the placeholder info into a pipe delimited string
            bool isBelow = position == NodePosition.Below ? true : false;
            string newNodeMap = SetNodeMap(NodeOver, isBelow);

            //Checks for the lines that are above and below itself
            string invalidNodeMapAbove = SetNodeMap(NodeMoving, false);
            string invalidNodeMapBelow = SetNodeMap(NodeMoving, true);
            bool IsInvalid = invalidNodeMapAbove == newNodeMap || invalidNodeMapBelow == newNodeMap;

            //Don't redraw the line if the line is at the same index OR
            //Don't draw the line if the line is in a invalid position (same position)
            //Don't Draw the Line when the Node that is Moving is a Group and the position is anyhting else other than an other Group
            if (AreNodeMapsEqual(newNodeMap) || IsInvalid || (NodeMoving is TreeNodeGroup && position != NodePosition.In))
                return;

            //Clear placeholders above and below
            this.Refresh();

            #region Draw Lines
            int NodeOverImageWidth = this.ImageList.Images[NodeOver.ImageIndex].Size.Width + 24;
            int LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
            int RightPos = _treeView.Width - 8;
            int boundary = position == NodePosition.Below ? NodeOver.Bounds.Bottom :
                position == NodePosition.Above ? NodeOver.Bounds.Top : NodeOver.Bounds.Top + (NodeOver.Bounds.Height / 2);

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
            if (position == NodePosition.In)
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
            #endregion
        }

        private string SetNodeMap(TreeNode tnNode, bool boolBelowNode)
        {
            StringBuilder NewNodeMap = new StringBuilder();

            if (boolBelowNode)
                NewNodeMap.Insert(0, (int)tnNode.Index + 1);
            else
                NewNodeMap.Insert(0, (int)tnNode.Index);
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
