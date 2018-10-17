using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Scripter
{
	public partial class FormMain : Form
	{
		// private WebKit.WebKitBrowser m_browser;
		private WebViewer.WebView m_browser;
		private TreeView treeViewCE;
		private Scripter.CaptureElement[] m_ceList;
		public FormMain() {
			InitializeComponent();

			treeViewCE = new MyTreeView() { Dock = DockStyle.Fill, FullRowSelect = true, HideSelection = false, CheckBoxes = true, ShowLines = false };
			treeViewCE.BeforeCheck += treeViewCE_BeforeCheck;
			treeViewCE.AfterCheck += treeViewCE_AfterCheck;
			treeViewCE.AfterSelect += treeViewCE_AfterSelect;
			splitContainer3.Panel1.Controls.Add(treeViewCE); treeViewCE.BringToFront();

			m_browser = new WebViewer.WebView() { Dock = DockStyle.Fill };
			m_browser.LoadFinished += M_browser_LoadFinished;
			m_browser.JavaScriptInvoke += M_browser_JavaScriptInvoke;
			//m_browser.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534+ (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
			// m_browser.DocumentCompleted += M_browser_DocumentCompleted;
			// m_browser.Navigating += M_browser_Navigating;
			// m_browser.Navigated += M_browser_Navigated;
			this.splitContainer1.Panel1.Controls.Add(m_browser);
		}

		private void M_browser_JavaScriptInvoke(object sender, string type, string param) {
			if (type == "captureElements") {
				onCaptureElement(param);
			}
		}

		private void M_browser_LoadFinished(object sender, bool ok) {
			Scripter.InjectInitializeScript(this.m_browser);
			label1.Text = "Completed";
			Application.DoEvents();
		}

		#region events
		private void FormMain_Shown(object sender, EventArgs e) {
			gotoToolStripMenuItem.PerformClick();
		}

		//private void M_browser_Navigating(object sender, WebKit.WebKitBrowserNavigatingEventArgs e) {
		//	label1.Text = "Navigating";
		//	Application.DoEvents();
		//}

		//private void M_browser_Navigated(object sender, WebBrowserNavigatedEventArgs e) {
		//	label1.Text = "Navigated";
		//	Application.DoEvents();
		//}

		//private void M_browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
		//	Scripter.InjectInitializeScript(this.m_browser);
		//	m_browser.GetScriptManager.ScriptObject = new JsExternObject(this);
		//	label1.Text = "Completed";
		//	Application.DoEvents();
		//}

		private void inspectorToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.ShowDevTools();
		}

		private void backToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.Back();
		}

		private void forwardToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.Forward();
		}

		private void stopToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.Stop();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
		}
		
		private void reloadToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.Reload();
		}
		#endregion

		private void recBuildTree(TreeNodeCollection parentCollection, Scripter.CaptureElement[] eles) {
			foreach (var ele in eles) {
				var node = new TreeNode(ele.ToString()) { Tag = ele, Checked = true }; node.Expand();
				parentCollection.Add(node);
				recBuildTree(node.Nodes, ele.children);
			}
		}

		private void onCaptureElement(string jsonElements) {
			m_ceList = JsonConvert.DeserializeObject<Scripter.CaptureElement[]>(jsonElements);
			treeViewCE.BeginUpdate(); treeViewCE.Nodes.Clear();
			recBuildTree(treeViewCE.Nodes, m_ceList);
			treeViewCE.ExpandAll(); treeViewCE.EndUpdate();
			treeViewCE_AfterSelect(null, null);
			flushResult();
		}

		private void gotoToolStripMenuItem_Click(object sender, EventArgs e) {
			var url = DialogGoto.ShowDialogGoto();
			if (url != null) {
				if (url.Length >= 4 && url.Substring(0, 4).ToLower() != "http") { url = "http://" + url; }
				if (System.Uri.IsWellFormedUriString(url, UriKind.Absolute)) {
					m_browser.SetUrl(url);
				}
			}
		}

		#region selector

		private void treeViewCE_AfterCheck(object sender, TreeViewEventArgs e) {
			if (e.Node.Checked) { foreach (TreeNode c in e.Node.Nodes) { c.Checked = true; } }
			if (!e.Node.Checked && e.Node.Parent != null) { e.Node.Parent.Checked = false; }
		}

		private void treeViewCE_BeforeCheck(object sender, TreeViewCancelEventArgs e) {
			if (e.Node.Nodes.Count == 0) { e.Cancel = true; }
		}

		private void onCaptureSetting(object sender, EventArgs e) {
			if (treeViewCE.SelectedNode == null) { return; }
			var ce = treeViewCE.SelectedNode.Tag as Scripter.CaptureElement;
			ce.config.tag = ckb_tag.Checked;
			ce.config.classes.Clear(); ce.config.classes.AddRange(ckbPanel_class.Controls.Cast<CheckBox>().SelectMany((a, i) => a.Checked ? new int[] { i } : new int[] { }));
			ce.config.index = ckb_index.Checked;
			ce.config.first = ckb_first.Checked;
			ce.config.last = ckb_last.Checked;
			ce.config.odd = ckb_odd.Checked;
			ce.config.even = ckb_even.Checked;
			ce.config.content = ckb_content.Checked;
			ce.config.attrs.Clear(); ce.config.attrs.AddRange(ckbPanel_attrs.Controls.Cast<CheckBox>().SelectMany((a, i) => a.Checked ? new int[] { i } : new int[] { }));
			if (treeViewCE.SelectedNode.Nodes.Count == 0) {
				ce.config.col_content = (ckbPanel_cols.Controls[0] as CheckBox).Checked;
				ce.config.col_attrs.Clear(); ce.config.col_attrs.AddRange(ckbPanel_cols.Controls.Cast<CheckBox>().Skip(1).SelectMany((a, i) => a.Checked ? new int[] { i } : new int[] { }));
			}
			flushResult();
		}

		private void recFillRawTree(Scripter.SelectItem[] items, TreeNodeCollection nodes, string historyJQ, Dictionary<string, int> dicCol, List<string> tmp, bool topFlag = false) {
			foreach (var item in items) {
				var node = new TreeNode(string.Format("{0} - {1}", item.jq, item.idx)) { Tag = item }; node.Expand();

				var fullJQ = string.Format("{0} > {1}", historyJQ, item.jq);
				if (topFlag) { tmp.Clear(); tmp.AddRange(new string[dicCol.Count]); }

				// 
				if (item.subItems != null) {
					recFillRawTree(item.subItems, node.Nodes, fullJQ, dicCol, tmp);
				}
				if (item.attrs != null) foreach (var attr in item.attrs) {
						var key = string.Format("{0} : {1}", fullJQ, attr.name);
						if (!dicCol.TryGetValue(key, out int idx)) {
							idx = dicCol.Count; dicCol.Add(key, idx); tmp.Add(null);
							listView1.Columns.Add(attr.name);
						}
						// 注: table 显示树状多分枝时, 只取第一个节点, 有点类似 :first, 想看完整的可以在[raw]中看树状显示
						if (tmp[idx] == null) { tmp[idx] = HttpUtility.UrlDecode(attr.value); }
					}

				if (topFlag) {
					// 排除空数据
					if (!tmp.All(i => string.IsNullOrWhiteSpace(i))) {
						listView1.Items.Add(new ListViewItem(tmp.ToArray()));
					}
				}
				nodes.Add(node);
			}
		}
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
			listView2.BeginUpdate(); listView2.Items.Clear();
			if (treeView1.SelectedNode != null) {
				var selectItem = treeView1.SelectedNode.Tag as Scripter.SelectItem;
				if (selectItem.attrs != null && selectItem.attrs.Length > 0) {
					listView2.Items.AddRange(selectItem.attrs.Select(a => new ListViewItem(new string[] { a.name, HttpUtility.UrlDecode(a.value) })).ToArray());
					listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				}
			}
			listView2.EndUpdate();
		}

		private void recBuildQueryTree(Scripter.Query query, TreeNodeCollection nodes) {
			var node = new TreeNode(query.strJQuery);
			foreach (var sub in query.subs) {
				recBuildQueryTree(sub, node.Nodes);
			}
			nodes.Add(node);
		}

		private void flushResult() {
			Scripter.Select(m_browser, m_ceList, new Scripter.SelectResultHandler((result) => {
				listView2.Items.Clear();
				treeView1.BeginUpdate(); treeView1.Nodes.Clear();
				listView1.BeginUpdate(); listView1.Clear();
				Dictionary<string, int> dic = new Dictionary<string, int>();
				List<string> tmp = new List<string>();
				if (result != null) { recFillRawTree(result.data, treeView1.Nodes, string.Empty, dic, tmp, true); }
				treeView1.EndUpdate();
				if (listView1.Items.Count > 0) { listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent); }
				listView1.EndUpdate();

				treeView_query.BeginUpdate(); treeView_query.Nodes.Clear();
				if (result != null) { recBuildQueryTree(result.query, treeView_query.Nodes); }
				treeView_query.ExpandAll();
				treeView_query.EndUpdate();
			}));
		}

		private void treeViewCE_AfterSelect(object sender, TreeViewEventArgs e) {
			splitContainer3.Panel2.SuspendLayout();
			splitContainer3.Panel2.Enabled = false;

			this.ckb_content.CheckedChanged -= new System.EventHandler(this.onCaptureSetting);
			this.ckb_even.CheckedChanged -= new System.EventHandler(this.onCaptureSetting);
			this.ckb_first.CheckedChanged -= new System.EventHandler(this.onCaptureSetting);
			this.ckb_index.CheckedChanged -= new System.EventHandler(this.onCaptureSetting);
			this.ckb_last.CheckedChanged -= new System.EventHandler(this.onCaptureSetting);
			this.ckb_odd.CheckedChanged -= new System.EventHandler(this.onCaptureSetting);
			this.ckb_tag.CheckedChanged -= new System.EventHandler(this.onCaptureSetting);

			ckb_tag.Checked = false; txt_tag.Clear();
			ckbPanel_class.Controls.Clear();
			ckb_index.Checked = false;
			ckb_first.Checked = false; ckb_first.Enabled = false;
			ckb_last.Checked = false; ckb_last.Enabled = false;
			ckb_odd.Checked = false; ckb_odd.Enabled = false;
			ckb_even.Checked = false; ckb_even.Enabled = false;
			ckbPanel_attrs.Controls.Clear();

			if (treeViewCE.SelectedNode != null) {
				var item = treeViewCE.SelectedNode.Tag as Scripter.CaptureElement;

				ckb_tag.Checked = item.config.tag; txt_tag.Text = item.tagName;
				for (var i = 0; i < item.classNames.Length; ++i) { var ckb = new CheckBox() { AutoSize = true, Text = item.classNames[i], Checked = item.config.classes.Contains(i) }; ckb.CheckedChanged += onCaptureSetting; ckbPanel_class.Controls.Add(ckb); }
				ckb_index.Text = string.Format("i[{0}]", item.index != null ? item.index.ToString() : "-"); ckb_index.Enabled = item.index != null; ckb_index.Checked = item.index != null && item.config.index;
				ckb_first.Enabled = item.index != null && item.index == 0; ckb_first.Checked = ckb_first.Enabled && item.config.first;
				ckb_last.Enabled = item.isLast != null && item.isLast.Value; ckb_first.Checked = ckb_first.Enabled && item.config.last;
				ckb_odd.Enabled = item.index != null && item.index % 2 == 1; ckb_odd.Checked = ckb_odd.Enabled && item.config.odd;
				ckb_even.Enabled = item.index != null && item.index % 2 == 0; ckb_even.Checked = ckb_even.Enabled && item.config.even;
				ckb_content.Checked = item.config.content; txt_content.Text = HttpUtility.UrlDecode(item.innerText);
				for (var i = 0; i < item.attributes.Length; ++i) { var ckb = new CheckBox() { AutoSize = true, Text = item.attributes[i].ToString(), Checked = item.config.attrs.Contains(i) }; ckb.CheckedChanged += onCaptureSetting; ckbPanel_attrs.Controls.Add(ckb); }

				splitContainer3.Panel2.Enabled = true;
			}

			this.ckb_content.CheckedChanged += new System.EventHandler(this.onCaptureSetting);
			this.ckb_even.CheckedChanged += new System.EventHandler(this.onCaptureSetting);
			this.ckb_first.CheckedChanged += new System.EventHandler(this.onCaptureSetting);
			this.ckb_index.CheckedChanged += new System.EventHandler(this.onCaptureSetting);
			this.ckb_last.CheckedChanged += new System.EventHandler(this.onCaptureSetting);
			this.ckb_odd.CheckedChanged += new System.EventHandler(this.onCaptureSetting);
			this.ckb_tag.CheckedChanged += new System.EventHandler(this.onCaptureSetting);

			splitContainer3.Panel2.ResumeLayout();

			ckbPanel_cols.SuspendLayout();
			ckbPanel_cols.Visible = false;
			ckbPanel_cols.Controls.Clear();
			if (treeViewCE.SelectedNode != null && treeViewCE.SelectedNode.Nodes.Count == 0) {
				var cei = treeViewCE.SelectedNode.Tag as Scripter.CaptureElement;
				var ckbContent = new CheckBox() { AutoSize = true, Text = "content", Tag = "content", Checked = cei.config.col_content };
				ckbPanel_cols.Controls.Add(ckbContent);
				ckbPanel_cols.Controls.AddRange(cei.attributes.Select((a, i) => { var ckb = new CheckBox() { AutoSize = true, Text = a.name, Tag = a, Checked = cei.config.col_attrs.Contains(i) }; ckb.CheckedChanged += onCaptureSetting; return ckb; }).ToArray());
				ckbPanel_cols.Visible = true;
			}
			ckbPanel_cols.ResumeLayout();
		}

		#endregion

		private void startToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.RunJavaScript("_x_captureStart()", null);
		}

		private void finishToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.RunJavaScript("_x_captureFinish()", new WebViewer.ScriptResultHandler(onCaptureElement));
		}

		private void cancelToolStripMenuItem_Click(object sender, EventArgs e) {
			m_browser.RunJavaScript("_x_captureCancel()", null);
		}
		
		private void treeView_query_AfterSelect(object sender, TreeViewEventArgs e) {
			txt_query.Text = e.Node.Text;
		}


		private void FormMain_Load(object sender, EventArgs e) {

		}
	}
}