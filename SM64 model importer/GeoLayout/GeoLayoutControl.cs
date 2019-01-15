using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SM64ModelImporter
{
    public partial class GeoLayoutControl : UserControl
    {
        GeoLayoutNode root = new GeoLayoutNode();
        Dictionary<TreeNode, GeoLayoutNode> treeNodeToGeoLayout = new Dictionary<TreeNode, GeoLayoutNode>();

        public GeoLayoutControl()
        {
            InitializeComponent();
            foreach (var a in GeoLayoutCommands.Command.commands)
            {
                var tmp = a;
                var wop = new ToolStripButton("0x" + a.Key.ToString("X2") + ": " + a.Value.Name);
                wop.Click += (_, e) => 
                {
                    GeoLayoutCommands.Command newCmd = (GeoLayoutCommands.Command) Activator.CreateInstance(tmp.Value);

                    var newNode = new TreeNode(wop.Text);
                    var parentNode = treeEntries.SelectedNode == null ? null : treeEntries.SelectedNode.Parent;
                    GeoLayoutNode targetNode = parentNode == null ? root : treeNodeToGeoLayout[parentNode];

                    if (parentNode != null)
                    {
                        int index = Math.Max(0, parentNode.Nodes.IndexOf(treeEntries.SelectedNode));
                        parentNode.Nodes.Insert(index, newNode);
                    }
                    else
                    {
                        int index = Math.Max(0, treeEntries.Nodes.IndexOf(treeEntries.SelectedNode));
                        treeEntries.Nodes.Insert(index, newNode);
                    }
                    targetNode.commands.Add(newCmd);
                };
                tsmAddCommand.DropDownItems.Add(wop );
            }

            treeEntries.MouseDown += (_, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    contextTreeEntries.Show(treeEntries.PointToScreen( e.Location));
                }
            };
        }
    }
}
