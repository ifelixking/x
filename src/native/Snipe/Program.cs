using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snipe
{
	static class Program
	{
		public static int MainThreadID { get; internal set; }

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
			Application.Run(new Form2());
		}
	}
}
