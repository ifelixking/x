using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Collecter
{
	public partial class FormMain : Form
	{
		private IScript m_script;

		public FormMain()
		{
			InitializeComponent();
			progress2.Visible = false;			
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			Core.Init();
			Core.m_formMain = this;
		}

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Core.Destroy();
			Utils.TerminateProcess(Process.GetCurrentProcess().Handle, 0);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenScriptDialog dlg = new OpenScriptDialog();
			if (dlg.ShowDialog() != DialogResult.OK) { return; }
			m_script = dlg.SelectedScript;
			this.Text = string.Format("Collecter - {0}", m_script);
			m_script.Run(true);
		}

		private void webKitBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			progress2.Visible = false;
			labTip2.Text = "Completed:" + e.Url.ToString();
		}

		private void webKitBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			Uri url; try { url = e.Url; } catch (NullReferenceException) { return; }
			labTip2.Text = "Navigated:" + e.Url;
		}

		private void webKitBrowser1_Navigating(object sender, WebKit.WebKitBrowserNavigatingEventArgs e)
		{
			Uri url; try { url = e.Url; } catch (NullReferenceException) { return; }
			labTip2.Text = "Navigating:" + e.Url;
			progress2.Visible = true;
		}

		private void webKitBrowser1_ProgressChanged(object sender, WebKit.ProgressChangesEventArgs e)
		{
			progress2.Value = e.Percent;
		}

		private void FormMain_Shown(object sender, EventArgs e)
		{
			openToolStripMenuItem.PerformClick();
		}

		internal void SetPrograss(object sender, string info, int prograss)
		{
			if (info != null) { labTip1.Text = info; }
			progress1.Value = prograss;
		}

		private void labTip_DoubleClick(object sender, EventArgs e)
		{
			MessageBox.Show(labTip2.Text);
		}
	}
}
