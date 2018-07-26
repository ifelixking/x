using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sniper
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// webKitBrowser1.Navigate("http://www.qq.com");

		}

		private void webKitBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

		}

		private void webKitBrowser1_ProgressChanged(object sender, WebKit.ProgressChangesEventArgs e)
		{
			toolStripProgressBar1.Visible = e.Percent != 100;
			toolStripProgressBar1.Value = e.Percent;
			toolStripStatusLabel1.Text = e.Percent.ToString();
			Application.DoEvents();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			webKitBrowser1.ShowDownloader();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			webKitBrowser1.ShowInspector();
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			webKitBrowser1.ShowPageSetupDialog();
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			webKitBrowser1.StringByEvaluatingJavaScriptFromString("var _x_script = document.createElement('script'); _x_script.src='https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(_x_script);");
		}

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			var obj = webKitBrowser1.StringByEvaluatingJavaScriptFromString("$('#su').val()");
			MessageBox.Show(obj);
		}

		private void toolStripButton6_Click(object sender, EventArgs e)
		{
			//Plan p = new Plan();
			//// p.Starts.Add(new Action(Action.ActionType.Add));

			//var a1 = new Action(Action.ActionType.OpenWeb);
			//// var a2 = new Action(Action.ActionType.FatchScaler) { Config = JsonConvert.SerializeObject(new ExeFatchScaler.Configure() { script = "$('#su').val()" }) };
			//// var a2 = new Action(Action.ActionType.FatchTable) { Config = JsonConvert.SerializeObject(new ExeFatchScaler.Configure() { script = "JSON.stringify($('div.result h3.t a').toArray().map((i)=>{return i.innerText}))" }) };
			//var a2 = new Action(Action.ActionType.FatchTable) { Config = JsonConvert.SerializeObject(new ExeFatchScaler.Configure() { script = "JSON.stringify($('div.result h3.t a').toArray().map(function(i){return i.innerText}))" }) };
			//a1.Nexts.Add(a2);

			//p.Starts.Add(a1);
			//p.Param = JsonConvert.SerializeObject(new ExeOpenWeb.Parameter() { URL = "https://www.baidu.com/s?wd=javascript&rsv_spt=1&rsv_iqid=0xf56da80e00015adb&issp=1&f=8&rsv_bp=0&rsv_idx=2&ie=utf-8&tn=baiduhome_pg&rsv_enter=1&rsv_sug3=11&rsv_sug1=8&rsv_sug7=100&sug=javascript%25E5%25AD%25A6%25E4%25B9%25A0%25E6%258C%2587%25E5%258D%2597&rsv_n=1&rsv_sug2=0&inputT=2132&rsv_sug4=3019",
			//	WebKitBrowserHandle = webKitBrowser1.Handle.ToInt64() });
			//p.Run();
		}
	}
}
