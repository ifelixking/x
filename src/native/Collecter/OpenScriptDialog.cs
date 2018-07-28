using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collecter
{
	public partial class OpenScriptDialog : Form
	{
		public OpenScriptDialog()
		{
			InitializeComponent();
		}

		private void listView1_ItemActivate(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		internal IScript SelectedScript {
			get {
				if (listView1.SelectedItems.Count == 0) { return null; }
				return listView1.SelectedItems[0].Tag as IScript;
			}
		}

		private void OpenScriptDialog_Load(object sender, EventArgs e)
		{
			reloadScript();
		}

		private void reloadScript()
		{
			var scriptTypes = getScriptTypes();
			listView1.BeginUpdate();
			listView1.Items.Clear();
			foreach (var t in scriptTypes) {
				var instance = t.GetConstructor(new Type[] { }).Invoke(new object[] { }) as IScript;
				var progress = instance.GetProgressString();
				var item = new ListViewItem(new string[] { instance.ToString(), progress }) { Tag = instance };
				listView1.Items.Add(item);
			}
			listView1.EndUpdate();
		}

		private static Type[] getScriptTypes()
		{
			var types = Assembly.GetExecutingAssembly().GetTypes();
			var result = new List<Type>();
			foreach (var type in types) {
				if (!type.IsInterface && typeof(IScript).IsAssignableFrom(type)) {
					result.Add(type);
				}
			}
			return result.ToArray();
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count == 0) { return; }
			var script =  listView1.SelectedItems[0].Tag as IScript;
			script.ResetProgress();
			reloadScript();
		}
	}
}
