using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collecter
{
	public partial class FormRun : Form
	{
		internal IScript Script { get; set; }

		public FormRun()
		{
			InitializeComponent();
		}

		private void webKitBrowser1_Navigating(object sender, WebKit.WebKitBrowserNavigatingEventArgs e)
		{
			Uri url; try { url = e.Url; } catch (NullReferenceException) { return; }
			labTip.Text = "Navigating:" + e.Url;
			prograss.Visible = true;
		}

		private void webKitBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			Uri url; try { url = e.Url; } catch (NullReferenceException) { return; }
			labTip.Text = "Navigated:" + e.Url;
		}

		private void webKitBrowser1_ProgressChanged(object sender, WebKit.ProgressChangesEventArgs e)
		{
			prograss.Value = e.Percent;
		}

		private void webKitBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			prograss.Visible = false;
			labTip.Text = "Completed:" + e.Url.ToString();
		}

		private void btnReload_Click(object sender, EventArgs e)
		{
			webKitBrowser1.Reload();
		}

		private void btnNavTo_Click(object sender, EventArgs e)
		{
			webKitBrowser1.Navigate("http://www.sina.com.cn");
			//Script.Run(this.webKitBrowser1);
		}

		//bool m_firstShown = true;
		//private void FormRun_VisibleChanged(object sender, EventArgs e)
		//{
		//	if (this.Visible && m_firstShown) {
		//		m_firstShown = false;
		//		Script.Run(this.webKitBrowser1);
		//	}
		//}
	}
}
