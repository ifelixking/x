using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collecter.Scripts
{
	class fanhome : IScript
	{
		private readonly string m_urlStart = "http://www.fanhome.cc/star/1.htm";
		private readonly string m_script_nextList = @"
			JSON.stringify(
				$('ul.pagination li:last-child:not(.active)').length ? $('ul.pagination li:last-child:not(.active) a')[0].href : ''
			)
		";
		private readonly string m_script_list = @"
			JSON.stringify(
				$('div.col-xs-6.col-sm-2.placeholder h4 a').map(function(i,a){return a.href}).toArray()
			)
		";
		private readonly string m_script_detail = @"
			JSON.stringify(	
				(function(){
					var result = {
						name:$('h2.page-header a:last-child').text(),
						image:$('div.col-xs-6.col-md-2 img')[0].src,
						text:$('div.col-xs-6.col-md-10 p').map(function(i,a){return a.innerText}).toArray().join('\n'),
					}
					if ($('ul.pagination li').length){
						result.pageCount = parseInt($('ul.pagination li:nth-last-child(2)').text().replace('... ',''))
						result.urlLastPage = $('ul.pagination li:nth-last-child(2) a')[0].href
					}else{
						result.count = $('table.table.table-striped tbody tr').length
					}
					return result
				})()
			)
		";
		private readonly string m_script_itemCount = @"
			JSON.stringify(	
				$('table.table.table-striped tbody tr').length
			)
		";
		private readonly int m_pageSize = 40;

		public override string ToString()
		{
			return "番号女优列表";
		}

		private string progressFilename {
			get { return string.Format("progress.{0}.txt", this); }
		}

		public string GetProgressString()
		{
			return File.Exists(progressFilename) ? File.ReadAllText(progressFilename) : string.Empty;
		}

		private void SetProgressString(string value)
		{
			File.WriteAllText(progressFilename, value);
		}

		public void ResetProgress()
		{
			File.Delete(progressFilename);
		}

		public void Run(bool reset)
		{
			string nextURL = m_urlStart;
			if (!reset) {
				var progress = GetProgressString();
				if (string.IsNullOrEmpty(progress) && progress != "finish") {
					nextURL = progress;
				}
			}

			int i = 0;
			for (;;) {
				SetProgressString(nextURL);
				Core.SetPrograss(this, i++.ToString(), 0);
				nextURL = pageRun(nextURL);
				if (string.IsNullOrEmpty(nextURL)) {
					SetProgressString("finish");
					break;
				}
			}

			Core.SetPrograss(this, "Finish", 100);
		}

		class Detail
		{
			public string name { get; set; }
			public string image { get; set; }
			public string text { get; set; }
			public int count { get; set; }
			public int pageCount { get; set; }
			public string urlLastPage { get; set; }
		}

		public string pageRun(string url)
		{
			string[] listResult;
			for (;;) {
				listResult = Core.Fetch<string[]>(url, m_script_list);
				if (listResult.Length > 0) { break; }
				Core.WaitRandom();
			}
			var nextURL = Core.Fetch<string>(url, m_script_nextList);
			int i = 0;
			List<Core.Actor_Fanhome> items = new List<Core.Actor_Fanhome>();
			foreach (var artUrl in listResult) {
				Core.SetPrograss(this, null, (int)(++i * 100.0f / listResult.Length));


				Detail detail;
				for (;;) {
					Core.WaitRandom();
					detail = Core.Fetch<Detail>(artUrl, m_script_detail);
					if (detail != null) { break; }
				}

				int? lastPageItemCount;
				if (!string.IsNullOrEmpty(detail.urlLastPage)) {
					for (;;) {
						Core.WaitRandom();
						lastPageItemCount = Core.Fetch<int?>(detail.urlLastPage, m_script_itemCount);
						if (lastPageItemCount != null) { break; }
					}
					detail.count = lastPageItemCount.Value + detail.pageCount * m_pageSize;
				}

				items.Add(new Core.Actor_Fanhome() { count = detail.count, name = detail.name, pic = detail.image, text = detail.text });
			}

			Core.CollectActor_Fanhome(items);

			return nextURL;
		}

	}
}
