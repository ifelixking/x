using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebViewerDemo
{
	public partial class Form1 : Form
	{
		public Form1() {
			InitializeComponent();
		}

		WebViewer.WebView a;

		private void Form1_Load(object sender, EventArgs e) {
			a = new WebViewer.WebView() { Dock = DockStyle.Fill };			
			this.Controls.Add(a);
			a.BringToFront();
		}
		
		private void toolStripButton1_Click(object sender, EventArgs e) {
			a.SetUrl("http://www.sina.com.cn");
		}

		private void cb(string msg){
			MessageBox.Show(msg);
		}

		private void toolStripButton2_Click(object sender, EventArgs e) {
			a.RunJavaScript("[1,2,3].length", new WebViewer.ScriptResultHandler(cb));
		}
	}
}
