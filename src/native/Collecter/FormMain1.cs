using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collecter
{
	public partial class FormMain1 : Form
	{
		//private WebKit.WebKitBrowser m_wkbCommon;
		//internal WebKit.WebKitBrowser CommonWebKitBrowser { get { return m_wkbCommon; } }

		public FormMain1()
		{
			InitializeComponent();
			// m_wkbCommon = new WebKit.WebKitBrowser();
			// panel1.Controls.Add(m_wkbCommon);
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			// panel1.SendToBack();

			Core.Init();
			Core.m_formMain = this;

			var scriptTypes = getScriptTypes();
			foreach (var t in scriptTypes) {
				var instance = t.GetConstructor(new Type[] { }).Invoke(new object[] { }) as IScript;
				var item = new ListViewItem(new string[] { instance.ToString(), string.Empty, string.Empty }); item.Tag = instance; //item.Checked = true;
				listView1.Items.Add(item);
			}
		}


		[DllImport("kernel32.dll")]
		private static extern int TerminateProcess(IntPtr hProcess, uint uExitCode);

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Core.Destroy();
			TerminateProcess(Process.GetCurrentProcess().Handle, 0);
		}

		private void runScript(object script)
		{
			FormRun form = new FormRun();
			form.Script = script as IScript;
			form.ShowDialog();
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in listView1.Items) {
				if (!item.Checked) { continue; }
				var script = item.Tag as IScript;
				Thread thread = new Thread(new ParameterizedThreadStart(runScript));
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start(script);
			}

			//foreach (ListViewItem item in listView1.Items) {
			//	if (!item.Checked) { continue; }
			//	var script = item.Tag as IScript;
			//	FormRun form = new FormRun();
			//	form.Script = script as IScript;
			//	form.ShowDialog();
			//	break;
			//}


		}

		private Type[] getScriptTypes()
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

		internal void SetPrograss(object sender, string info, int prograss)
		{
			foreach (ListViewItem item in listView1.Items) {
				if (item.Tag == sender) {
					if (info != null) { item.SubItems[1].Text = info; }
					item.SubItems[2].Text = prograss.ToString() + "%";
					Application.DoEvents();
					break;
				}
			}
		}

		// internal Control InvisibleContainer { get { return panel1; } }

		public enum ShowWindowCommands : int
		{

			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,    //用最近的大小和位置显示，激活
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_MAX = 10
		}
		[DllImport("shell32.dll")]
		public static extern IntPtr ShellExecute(
			IntPtr hwnd,
			string lpszOp,
			string lpszFile,
			string lpszParams,
			string lpszDir,
			ShowWindowCommands FsShowCmd
			);

		private void logToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShellExecute(IntPtr.Zero, "open", "NOTEPAD.EXE", "log.txt", null, ShowWindowCommands.SW_SHOWNORMAL);
		}
	}
}
