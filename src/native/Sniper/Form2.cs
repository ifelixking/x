using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sniper
{
	public partial class Form2 : Form
	{
		CtrlPlan m_ctrlPlan;
		CtrlOpenWeb m_ctrlActionOpenWeb;

		public Form2()
		{
			InitializeComponent();
			m_ctrlPlan = new CtrlPlan() { Dock = DockStyle.Fill };
			m_ctrlActionOpenWeb = new CtrlOpenWeb() { Dock = DockStyle.Fill };
		}

		private void btn_CreatePlan_Click(object sender, EventArgs e)
		{
			Plan p = new Plan();
			treeView1.Nodes.Add(new TreeNode(p.Name) { Tag = p });
		}

		private void btn_Save_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog() { Filter = "X files (*.x)|*.x|All files (*.*)|*.*" };
			if (dlg.ShowDialog() != DialogResult.OK) { return; }
			var plans = treeView1.Nodes.Cast<TreeNode>().Select(n => n.Tag as Plan);
			var content = Plan.ArchiveOut(plans);
			File.WriteAllText(dlg.FileName, content);
		}

		private void btn_Open_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog() { Filter = "X files (*.x)|*.x|All files (*.*)|*.*" };
			if (dlg.ShowDialog() != DialogResult.OK) { return; }
			var content = File.ReadAllText(dlg.FileName);
			var plans = Plan.ArchiveIn(content);
			treeView1.Nodes.Clear();
			foreach (var p in plans) {
				var nodePlan = new TreeNode(p.Name) { Tag = p };
				treeView1.Nodes.Add(nodePlan);
				rec_buildActionNode(nodePlan, p.Starts);
			}
			treeView1.ExpandAll();
		}

		private void rec_buildActionNode(TreeNode parentNode, List<Action> actions)
		{
			foreach (var action in actions) {
				var node = new TreeNode(action.Name) { Tag = action };
				parentNode.Nodes.Add(node);
				rec_buildActionNode(node, action.Nexts);
			}
		}

		private void btn_Action_OpenWeb_Click(object sender, EventArgs e)
		{
			doCreateAction(Action.ActionType.OpenWeb);
		}

		private void btn_Action_FatchTable_Click(object sender, EventArgs e)
		{
			doCreateAction(Action.ActionType.FatchTable);
		}

		private void doCreateAction(Action.ActionType type)
		{
			if (treeView1.SelectedNode == null) { return; }
			Action newAction = new Action(type);
			treeView1.SelectedNode.Nodes.Add(new TreeNode(newAction.Name) { Tag = newAction });
			treeView1.SelectedNode.Expand();
			if (treeView1.SelectedNode.Tag is Plan plan) {
				plan.Starts.Add(newAction);
				return;
			}
			if (treeView1.SelectedNode.Tag is Action action) {
				action.Nexts.Add(newAction);
				return;
			}
			throw new Exception();
		}

		private void Form2_Shown(object sender, EventArgs e)
		{
			btn_Open.PerformClick();
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node == null) { return; }
			splitContainer1.Panel2.Controls.Clear();
			if (e.Node.Tag is Plan plan) {
				m_ctrlPlan.Plan = plan;
				splitContainer1.Panel2.Controls.Add(m_ctrlPlan);
			} else if (e.Node.Tag is Action action) {
				switch (action.Type) {
					case Action.ActionType.OpenWeb:
					m_ctrlActionOpenWeb.Action = action;
					splitContainer1.Panel2.Controls.Add(m_ctrlActionOpenWeb);
					break;
				}
			}
		}
	}
}
