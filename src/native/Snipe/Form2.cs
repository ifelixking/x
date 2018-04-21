using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snipe
{
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			button1.Enabled = false;
			var sn = new Sniper_list_xp1024();
			sn.OnProgress += Sn_OnProgress;
			sn.OnLog += Sn_OnLog;
			sn.OnFinish += Sn_OnFinish;
			m_sn = sn;
			sn.Run();
		}

		private void Sn_OnFinish(bool mainThreadRequest)
		{
			MessageBox.Show("Finish");
			button1.Enabled = true;
			if (mainThreadRequest) { Application.DoEvents(); }
		}

		private void Sn_OnLog(bool mainThreadRequest, string text)
		{
			richTextBox1.AppendText(text);
			if (mainThreadRequest) { Application.DoEvents(); }
		}

		private void Sn_OnProgress(bool mainThreadRequest, int value, int max)
		{
			progressBar1.Maximum = max;
			progressBar1.Value = value;
			label1.Text = string.Format("{0}/{1}", value, max);
			if (mainThreadRequest) { Application.DoEvents(); }
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Storage storage = new Storage();
			storage.Open();
			var count = storage.DistinctPageByURL();
			storage.Close();
			MessageBox.Show(string.Format("删除了 {0} 条重复 Page", count));
		}

		Sniper m_sn = null;

		private void button3_Click(object sender, EventArgs e)
		{
			var sn = new Sniper_detail_xp1024();
			sn.OnProgress += Sn_OnProgress;
			sn.OnLog += Sn_OnLog;
			sn.OnFinish += Sn_OnFinish;
			sn.Run();
			m_sn = sn;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (m_sn != null){
				m_sn.AddThread();
			}
		}
	}
}
