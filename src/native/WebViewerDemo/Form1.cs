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
			a.JavaScriptInvoke += A_JavaScriptInvoke;
			this.Controls.Add(a);
			a.BringToFront();
		}

		private void A_JavaScriptInvoke(object sender, string type, string param) {
			int aa = 0;
		}

		private void toolStripButton1_Click(object sender, EventArgs e) {
			a.SetUrl("http://www.baidu.com");
		}

		private void cb(string msg){
			MessageBox.Show(msg);
		}

		private void toolStripButton2_Click(object sender, EventArgs e) {
			a.RunJavaScript("[1,2,3].length", new WebViewer.ScriptResultHandler(cb));
		}

		private void toolStripButton3_Click(object sender, EventArgs e) {
			//string script = @"
			//	var context;
			//	// 初始化
			//	function init()
			//	{
			//		if (typeof qt != 'undefined')
			//		{
			//			new QWebChannel(qt.webChannelTransport, function(channel)
			//			{
			//				context = channel.objects.context;

			//				onBtnSendMsg()
			//			});
			//		}else{
			//			alert('qt对象获取失败！');
			//		}
			//	}
			//	// 向qt发送消息
			//	function sendMessage(msg)
			//	{
			//		if(typeof context == 'undefined')
			//		{
			//			alert('context对象获取失败！');
			//		}
			//		else
			//		{
			//			context.onMsg(msg);
			//		}
			//	}
			//	// 控件控制函数
			//	function onBtnSendMsg()
			//	{
			//		// var cmd = document.getElementById('待发送消息').value;
			//		sendMessage('show me the money');   
			//	}
			//	init();
				
			//";

			string script = @"
				context.jsInvoke('show', 'me');
			";

			a.RunJavaScript(script, new WebViewer.ScriptResultHandler(cb));
		}

		private void toolStripButton4_Click(object sender, EventArgs e) {
			a.ShowDevTools();
		}

		private void toolStripButton5_Click(object sender, EventArgs e) {
			a.RunInit();
		}
	}
}
