using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snipe
{
	class Tick
	{
		public delegate void ProgressHandler(bool mainThreadRequest, long value, long max);
		public event ProgressHandler OnProgress;

		public delegate void LogHandler(bool mainThreadRequest, string text);
		public event LogHandler OnLog;

		public delegate void FinishHandler(bool mainThreadRequest);
		public event FinishHandler OnFinish;

		public void EMIT_PROGRESS(long value, long max)
		{
			if (OnProgress != null) {
				if (Thread.CurrentThread.ManagedThreadId == Program.MainThreadID) {
					Application.OpenForms.Cast<Control>().First().Invoke(new ProgressHandler(OnProgress), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID, value, max);
				} else {
					Application.OpenForms.Cast<Control>().First().BeginInvoke(new ProgressHandler(OnProgress), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID, value, max);
				}
			}
		}

		public void EMIT_LOG(string format, params object[] args)
		{
			if (OnLog != null) {
				if (Thread.CurrentThread.ManagedThreadId == Program.MainThreadID) {
					Application.OpenForms.Cast<Control>().First().Invoke(new LogHandler(OnLog), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID, string.Format("{0}: {1}\r\n", DateTime.Now, string.Format(format, args)));
				} else {
					Application.OpenForms.Cast<Control>().First().BeginInvoke(new LogHandler(OnLog), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID, string.Format("{0}: {1}\r\n", DateTime.Now, string.Format(format, args)));
				}
			}
		}

		public void EMIT_FINISH()
		{
			if (OnFinish != null) {
				if (Thread.CurrentThread.ManagedThreadId == Program.MainThreadID) {
					Application.OpenForms.Cast<Control>().First().Invoke(new FinishHandler(OnFinish), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID);
				} else {
					Application.OpenForms.Cast<Control>().First().BeginInvoke(new FinishHandler(OnFinish), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID);
				}
			}
		}
	}
}
