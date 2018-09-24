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

namespace Scripter
{
	public partial class DialogGoto : Form
	{
		private readonly string m_fileHistory = "goto_history.txt";
		private List<string> m_history = new List<string>();

		public DialogGoto() {
			InitializeComponent();
		}

		public static String ShowDialogGoto(string defaultURL = "") {
			var dlg = new DialogGoto();
			dlg.textBox1.Text = defaultURL;
			if (dlg.ShowDialog() != DialogResult.OK) { return null; }
			return dlg.textBox1.Text;
		}

		private void DialogGoto_Load(object sender, EventArgs e) {
			if (File.Exists(m_fileHistory)) {
				m_history.AddRange(File.ReadLines(m_fileHistory).Take(10));
				textBox1.Items.AddRange(m_history.ToArray());
			}
		}

		private void btn_ok_Click(object sender, EventArgs e) {
			var url = textBox1.Text.Trim();
			if (!string.IsNullOrEmpty(url)) {
				if (m_history.Contains(url)) {
					if (m_history[0] == url) { return; }
					m_history.Remove(url);
				}
				m_history.Insert(0, url);
				File.WriteAllLines(m_fileHistory, m_history.ToArray());
			}
		}

		private void DialogGoto_Shown(object sender, EventArgs e) {
			if (textBox1.Items.Count > 0) {
				textBox1.SelectedIndex = 0;
			}
		}
	}
}
