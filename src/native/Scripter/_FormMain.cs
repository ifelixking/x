using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebKit;

namespace Scripter
{
	public partial class _FormMain : Form
	{
		private WebKitBrowser m_webKitBrowser;
		private Form m_webKitBrowserForm;

		public _FormMain()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (m_webKitBrowser == null) {
				m_webKitBrowserForm = new Form() { StartPosition = FormStartPosition.CenterParent, Width = 800, Height = 600 };
				m_webKitBrowserForm.FormClosed += M_webKitBrowserForm_FormClosed;
				m_webKitBrowser = new WebKitBrowser() { Dock = DockStyle.Fill };
				m_webKitBrowserForm.Controls.Add(m_webKitBrowser);
				m_webKitBrowser.ProgressChanged += M_webKitBrowser_ProgressChanged;
				m_webKitBrowser.DocumentCompleted += M_webKitBrowser_DocumentCompleted;
				m_webKitBrowserForm.Show(this);
			}

			toolStripProgressBar1.Value = 0;
			toolStripProgressBar1.Visible = true;
			lab_status.Text = "navigating...";
			Application.DoEvents();
			m_webKitBrowser.Navigate(textBox1.Text);
			m_webKitBrowserForm.BringToFront();
		}

		private void M_webKitBrowserForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			m_webKitBrowserForm = null;
			m_webKitBrowser = null;
		}

		private void M_webKitBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			toolStripProgressBar1.Visible = false;

			// 注入 jquery
			lab_status.Text = "injection jquery...";
			Application.DoEvents();
			for (;;) {
				try {
					m_webKitBrowser.StringByEvaluatingJavaScriptFromString("(function(){var _x_script = document.createElement('script'); _x_script.src='https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(_x_script);})()");
					break;
				} catch (Exception) { }
				Application.DoEvents();
			}

			lab_status.Text = "Ready";
		}

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);

		private void M_webKitBrowser_ProgressChanged(object sender, ProgressChangesEventArgs e)
		{
			toolStripProgressBar1.Value = e.Percent;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			if (dlg.ShowDialog() != DialogResult.OK) { return; }
			textBox2.Text = dlg.FileName;
		}

		private void btn_run_Click(object sender, EventArgs e)
		{
			listView1.Clear();
			if (m_webKitBrowser == null) { return; }
			var script = File.ReadAllText(textBox2.Text);
			object result;
			try {
				var strResult = m_webKitBrowser.StringByEvaluatingJavaScriptFromString(script);
				result = JsonConvert.DeserializeObject(strResult);
			} catch (Exception ex) {
				lab_status.Text = ex.Message;
				return;
			}
			if (result is JArray) {
				List<string> cols = new List<string>();
				listView1.BeginUpdate();
				foreach (var item in result as JArray) {
					List<string> vals = new List<string>(cols.ToArray());
					if (item.Type == JTokenType.Object) {
						var obj = item as JObject;
						foreach (var prop in obj.Properties()) {
							var idx = cols.IndexOf(prop.Name);
							if (idx == -1) {
								cols.Add(prop.Name);
								listView1.Columns.Add(prop.Name);
								vals.Add(prop.Value.ToString());
							} else {
								vals[idx] = prop.Value.ToString();
							}
						}
					} else {
						var idx = cols.IndexOf(string.Empty);
						if (idx == -1) {
							cols.Add(string.Empty);
							listView1.Columns.Add("-");
							vals.Add(item.ToString());
						} else {
							vals[idx] = item.ToString();
						}
					}
					listView1.Items.Add(new ListViewItem(vals.ToArray()));
				}
				listView1.EndUpdate();
			} else if (result is JObject) {
				List<string> vals = new List<string>();
				foreach (var prop in (result as JObject).Properties()) {
					listView1.Columns.Add(prop.Name);
					vals.Add(prop.Value.ToString());
				}
				listView1.Items.Add(new ListViewItem(vals.ToArray()));
			} else {
				listView1.Columns.Add("-");
				listView1.Items.Add(result.ToString());
			}
			listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			listView1.EndUpdate();
			lab_status.Text = string.Format("{0}条", listView1.Items.Count);
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{
			var result = WritePrivateProfileString("default", "script", textBox2.Text, getIniFile());
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			var result = WritePrivateProfileString("default", "url", textBox1.Text, getIniFile());
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			StringBuilder temp = new StringBuilder(4096);
			GetPrivateProfileString("default", "url", string.Empty, temp, 4096, getIniFile());
			textBox1.Text = temp.ToString();
			GetPrivateProfileString("default", "script", string.Empty, temp, 4096, getIniFile());
			textBox2.Text = temp.ToString();
		}

		private string getIniFile(){
			return Path.Combine(Path.GetDirectoryName( System.Windows.Forms.Application.ExecutablePath), "Scripter.ini");
		}
	}
}
