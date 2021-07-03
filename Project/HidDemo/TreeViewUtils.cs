using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HidDemo
{
    class TreeViewUtils
    {
        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        public static void HideCheckBox(TreeView aTreeView, TreeNode aNode)
        {
            TVITEM tvi = new TVITEM();
            tvi.hItem = aNode.Handle;
            tvi.mask = TVIF_STATE;
            tvi.stateMask = TVIS_STATEIMAGEMASK;
            tvi.state = 0;
            SendMessage(aTreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }

        private static string TreeNodeToText(TreeNode aTreeNode, uint aDepth)
        {
            // Print the node.
            string res = "\r\n";

            uint depth = aDepth;
            while (depth > 1)
            {
                depth--;
                res += "  ";
            }

            res += "|-";

            if (aTreeNode.Nodes.Count > 0)
            {
                res += "+";
            }
            else
            {
                res += "-";
            }


            res += aTreeNode.Text;

            // Print each node recursively.
            foreach (TreeNode tn in aTreeNode.Nodes)
            {
                res += TreeNodeToText(tn, aDepth + 1);
            }

            return res;
        }

        /// <summary>
        /// Dumps a Tree View control into a string.
        /// </summary>
        /// <param name="aTreeView"></param>
        /// <returns></returns>
        public static string TreeViewToText(TreeView aTreeView)
        {
            // Print each node recursively.
            string res = "--------------------------------------------------------------------------------------------------------------------------\r\n";
            res += "+" + aTreeView.Name.Replace("treeView", "").Replace("iTreeView", "");
            TreeNodeCollection nodes = aTreeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                res += TreeNodeToText(n, 1);
            }
            res += "\r\n--------------------------------------------------------------------------------------------------------------------------\r\n";
            return res;
        }


    }
}
