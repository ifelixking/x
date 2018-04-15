using mshtml;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snipe
{
	abstract class Sniper
	{
		protected struct UrlTask
		{
			public string url;
			public object param;
		}
		
		private ConcurrentQueue<UrlTask> m_queuePage;
		private int m_queueItemTotalCount;
		private int m_threadCount;

		public delegate void ProgressHandler(bool mainThreadRequest, int value, int max);
		public event ProgressHandler OnProgress;

		public delegate void LogHandler(bool mainThreadRequest, string text);
		public event LogHandler OnLog;

		public delegate void FinishHandler(bool mainThreadRequest);
		public event FinishHandler OnFinish;
		
		public virtual void Run()
		{
			int threadCount = 8;
			EMIT_LOG("parallel task, thread count {0}", threadCount);
			m_threadCount = threadCount;
			m_queuePage = new ConcurrentQueue<UrlTask>(this.GetUrlTaskList());
			m_queueItemTotalCount = m_queuePage.Count;
			for (var i = 0; i < threadCount; ++i) {
				Thread thread = new Thread(new ThreadStart(threadProcessTask));
				thread.Name = string.Format("Sniper {0}", i);
				thread.Start();
			}
		}

		private void threadProcessTask()
		{
			EMIT_LOG("thread {0} started", Thread.CurrentThread.Name);
			Storage storage = new Storage(); storage.Open();
			for (;;) {
				if (!m_queuePage.TryDequeue(out UrlTask task)) {
					if (Interlocked.Decrement(ref m_threadCount) == 0) {
						// EMIT_LOG("{0}: storage close", Thread.CurrentThread.Name);
						// m_storage.Close();
						EMIT_FINISH();
					}
					storage.Close();
					EMIT_LOG("thread {0} exiting", Thread.CurrentThread.Name);
					return;
				}
				HTMLDocumentClass doc = null;
				for (var i = 0; i < 10; ++i) {
					try {
						EMIT_LOG("{0}: open doc: {1}", Thread.CurrentThread.Name, task.url);
						doc = Utils.retryGetDocument(task.url);
						break;
					} catch (Exception ex) {
						EMIT_LOG("{0}: error: {1}; retry...", Thread.CurrentThread.Name, ex.Message);
						continue;
					}
				}
				if (doc == null) { EMIT_LOG("{0}: error: doc open failed: {1}", Thread.CurrentThread.Name, task.url); continue; }
				EMIT_LOG("{0}: process doc: {1}", Thread.CurrentThread.Name, task.url);
				processDoc(doc, storage, task.param);
				EMIT_PROGRESS(m_queueItemTotalCount - m_queuePage.Count, m_queueItemTotalCount);
			}

		}

		protected virtual IEnumerable<UrlTask> GetUrlTaskList() { return null; }

		protected virtual void processDoc(HTMLDocumentClass doc, Storage storage, object param) { }

		protected void EMIT_PROGRESS(int value, int max)
		{
			if (OnProgress != null) {
				Application.OpenForms.Cast<Control>().First().Invoke(new ProgressHandler(OnProgress), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID, value, max);
			}
		}

		protected void EMIT_LOG(string format, params object[] args)
		{
			if (OnLog != null) {
				Application.OpenForms.Cast<Control>().First().Invoke(new LogHandler(OnLog), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID, string.Format("{0}: {1}\r\n", DateTime.Now, string.Format(format, args)));
			}
		}

		protected void EMIT_FINISH()
		{
			if (OnFinish != null) {
				Application.OpenForms.Cast<Control>().First().Invoke(new FinishHandler(OnFinish), Thread.CurrentThread.ManagedThreadId == Program.MainThreadID);
			}
		}
	}
}
