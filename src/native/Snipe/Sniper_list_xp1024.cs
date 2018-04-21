using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snipe
{
	class Sniper_list_xp1024 : Sniper
	{
		class EntryPage
		{
			public string url;
			public string tags;
			public int sniperCode;
			public string pathPage;
		}
		class TaskInfo
		{
			public string url;
			public string tags;
			public int sniperCode;
		}

		private EntryPage[] m_entryList = new EntryPage[] {
			new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=3",  sniperCode = 1001, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "" },
			new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=5",  sniperCode = 1002, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "亚洲;无码" },
			new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=22", sniperCode = 1003, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "日本;有码" },
			new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=7",  sniperCode = 1004, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "欧美;无码" },
			// new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=30", sniperCode = 1005, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "日本" },
			// new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=37", sniperCode = 1006, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "" },
			new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=79", sniperCode = 1007, pathPage="div:#main/div:@4/table/tbody/tr/td:@0/div/div/a:@-1", tags = "" },
			new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=81", sniperCode = 1008, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "" },
			// new EntryPage(){ url ="http://1024.917rbb.pw/pw/thread.php?fid=83", sniperCode = 1009, pathPage="div:#main/div:@3/table/tbody/tr/td:@0/div/div/a:@-1", tags = "" },
		};
		private List<TaskInfo> m_taskList = new List<TaskInfo>();

		public override void Run()
		{
			prepareTask();
			base.Run();
		}

		private void prepareTask()
		{
			int _i = 0;
			EMIT_PROGRESS(0, m_entryList.Length);
			EMIT_LOG("get page count from entry...");
			const string pageVar = "page";
			foreach (var entry in m_entryList) {
				EMIT_LOG("open {0}", entry.url);
				var doc = Utils.retryGetDocument(entry.url);
				var elePage = Utils.stepToElement(doc, null, entry.pathPage.Split('/'));
				IHTMLDOMNode node = ((IHTMLDOMNode)elePage).nextSibling;
				var pageText = node.nodeValue as string;
				var a = pageText.Substring(pageText.IndexOf('/') + 1);
				var b = a.Substring(0, a.IndexOf(' '));
				int pageCount = int.Parse(b);
				EMIT_LOG("page count {0}", pageCount);
				for (var i = 1; i < pageCount; ++i) {
					m_taskList.Add(new TaskInfo() { url = Utils.mergeUrlParam(entry.url, pageVar, i.ToString()), sniperCode = entry.sniperCode, tags = entry.tags });
				}
				EMIT_PROGRESS(++_i, m_entryList.Length);
			}
			EMIT_LOG("got total page count {0}", m_taskList.Count);
		}

		protected override IEnumerable<UrlTask> GetUrlTaskList()
		{
			return m_taskList.Select(a => new UrlTask() { url = a.url, param = a });
		}

		protected override void processDoc(HTMLDocumentClass doc, Storage storage, object param)
		{
			var taskInfo = param as TaskInfo;

			const string strContainer = "table:#ajaxtable/tbody:@1";
			const string strItor = "tr";
			var attrList = new Utils.PathAttribute[]{
				new Utils.PathAttribute(){ pathElement = "td:@1/h3/a".Split('/'), attributeName = "innerText" },
				new Utils.PathAttribute(){ pathElement = "td:@1/h3/a".Split('/'), attributeName = "href" },
				new Utils.PathAttribute(){ pathElement = "td:@4/a".Split('/'), attributeName = "innerText" }
			};

			var eleContainer = Utils.getElementItor(doc, strContainer, strItor);
			var result = Utils.getElementAttr(doc, eleContainer, attrList);
			storage.AddPage(result.Select((item) => {
				DateTime date; DateTime.TryParse(item[2], out date);
				return new Storage.Page() { title = item[0], url = item[1], tags = taskInfo.tags, date = date, sniper = taskInfo.sniperCode };
			}));
		}
	}
}