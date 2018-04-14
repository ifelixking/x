using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using mshtml;
using System.Diagnostics;

namespace Snipe
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//var urlTpl = txt_url.Text;
			//var start = int.Parse(txt_pageStart.Text);
			//var end = int.Parse(txt_pageEnd.Text);
			//var interval = int.Parse(txt_msInterval.Text);
			//for (var i = start; i <= end; ++i) {
			//	var url = urlTpl.Replace("${PAGE}", i.ToString());
			//	processDocument(getDocument(url));
			//	System.Threading.Thread.Sleep(interval);
			//}
		}

		private void processDocument(HTMLDocumentClass doc)
		{
			//var eleContainer = getElementItor(doc, txt_eleContainer.Text, txt_eleItor.Text);
			//var paList = new PathAttribute[]{
			//	new PathAttribute(){ pathElement = txt_eleText.Text.Split('/'), attributeName = txt_attrText.Text },
			//	new PathAttribute(){ pathElement = txt_eleURL.Text.Split('/'), attributeName = txt_attrURL.Text }
			//};
			//var result = getElementAttr(doc, eleContainer, paList);
			//listView1.BeginUpdate(); int i = 0;
			//foreach (var item in result) {
			//	listView1.Items.Add(new ListViewItem(new string[] { (++i).ToString(), item[0], item[1] }));
			//}
			//listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			//listView1.EndUpdate();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//Storage s = new Storage();
			//s.Open();
			//var result = s.GetAllUnprocessPage(1);
			//s.Close();

			var sn = new Sniper_list_xp1024();
			sn.OnProgress += Sn_OnProgress;
			//sn.OnProcessSite += Sn_OnProcessSite;
			//sn.OnProcessPage += Sn_OnProcessPage;
			sn.Run();
		}

		private void Sn_OnProgress(int value, int max)
		{
			
		}

		private void Sn_OnProcessPage(int i, int total, string text)
		{
			txt_log.AppendText(string.Format("{0}: {1}\r\n", DateTime.Now, text));
			progressBar2.Maximum = total;
			progressBar2.Value = i;
			lab2.Text = string.Format("{0}/{1}", i, total);
			Application.DoEvents();
		}

		private void Sn_OnProcessSite(int i, int total, string text)
		{
			txt_log.AppendText(string.Format("{0}: {1}\r\n", DateTime.Now, text));
			progressBar1.Maximum = total;
			progressBar1.Value = i;
			lab1.Text = string.Format("{0}/{1}", i, total);
			Application.DoEvents();
		}
	}
}
