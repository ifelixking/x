using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collecter.Scripts
{
	class Class1 : IScript
	{
		private Policy1 m_policy;
		public Class1()
		{
			m_policy = new Policy1(this,
				"http://d2.sku117.org/forum.php?mod=forumdisplay&fid=22&page=1",
				new string[] { "日本" }
			);
		}

		public override string ToString()
		{
			return "xp1024.com - 日本";
		}

		public void Run()
		{
			m_policy.Run();
		}
	}

	class Class2 : IScript
	{

		private Policy1 m_policy;
		public Class2()
		{
			m_policy = new Policy1(this,
				"http://d2.sku117.org/forum.php?mod=forumdisplay&fid=5&page=1",
				new string[] { "亚洲无码" }
			);
		}

		public override string ToString()
		{
			return "xp1024.com - 亞洲無碼";
		}

		public void Run()
		{
			m_policy.Run();
		}

	}

	class Class3 : IScript
	{

		private Policy1 m_policy;
		public Class3()
		{
			m_policy = new Policy1(this,
				"http://d2.sku117.org/forum.php?mod=forumdisplay&fid=7&page=1",
				new string[] { "欧美无码" }
			);
		}

		public override string ToString()
		{
			return "xp1024.com - 歐美新片";
		}

		public void Run()
		{
			m_policy.Run();
		}

	}

	class Class4 : IScript
	{

		private Policy1 m_policy;
		public Class4()
		{
			m_policy = new Policy1(this,
				"http://d2.sku117.org/forum.php?mod=forumdisplay&fid=18&page=1",
				new string[] { "三級" }
			);
		}

		public override string ToString()
		{
			return "xp1024.com - 三級寫真";
		}

		public void Run()
		{
			m_policy.Run();
		}

	}

	class Policy1
	{
		private readonly string m_url;
		private readonly string m_script_getArtURLList = @"
			JSON.stringify($('#moderate table tbody tr td:nth-last-child(2).max-td table.article tbody tr.article-title td a[target]').map(function (i, a) {
				return { text: a.innerText, url: a.href, date: new Date((new Date()).getFullYear() + '-' + $(a).children('span').text()) }
			}).toArray())
		";
		private readonly string m_script_getArt = @"
			JSON.stringify($('td.t_f').map(function (i, a) {
				return {
					text: a.innerText,
					downloads: JSON.stringify($(a).children('a').filter(function () { return $(this).find('img').length == 0 }).map(function (ii, aa) { return aa.href }).toArray()),
					images: JSON.stringify($(a).find('img').map(function (ii, aa) { return aa.src }).toArray())
				}
			}).toArray())
		";
		private readonly string m_script_next = @"
			JSON.stringify($('#fd_page_bottom div.pg a.nxt').length ? $('#fd_page_bottom div.pg a.nxt')[0].href : '')
		";
		private readonly string[] m_tags;
		private object m_host;
		public Policy1(object host, string startURL, string[] tags)
		{
			m_host = host;
			m_url = startURL;
			m_tags = tags;
		}

		class Item
		{
			public string text { get; set; }
			public string url { get; set; }
			public DateTime date { get; set; }
		}

		class ContentResult
		{
			public string text { get; set; }
			public string downloads { get; set; }
			public string images { get; set; }
		}

		public void Run()
		{
			string nextURL = m_url;
			for (;;) {
				nextURL = pageRun(nextURL);
				if (string.IsNullOrEmpty(nextURL)) { break; }
			}
			Core.SetPrograss(m_host, "Finish", 100);
		}

		public string pageRun(string url)
		{
			Core.SetPrograss(m_host, url, 0);
			var result = new List<Core.Art>();
			Item[] listResult;
			for (;;) {
				listResult = Core.Fetch<Item[]>(url, m_script_getArtURLList, Core.m_formMain.CommonWebKitBrowser);
				if (listResult.Length > 0) { break; }
			}
			var nextURL = Core.Fetch<string>(url, m_script_next, Core.m_formMain.CommonWebKitBrowser);
			int i = 0;
			foreach (var artUrl in listResult) {
				Core.SetPrograss(m_host, artUrl.url, (int)(++i * 100.0f / listResult.Length));
				ContentResult[] contentList;
				for (;;) {
					contentList = Core.Fetch<ContentResult[]>(artUrl.url, m_script_getArt, Core.m_formMain.CommonWebKitBrowser);
					if (contentList.Length > 0) { break; }
				}

				foreach (var content in contentList) {
					result.Add(new Core.Art() {
						date = artUrl.date,
						downloads = content.downloads,
						images = content.images,
						source = artUrl.url,
						text = string.Format("{0}\r\n{1}", artUrl.text, content.text),
						tags = m_tags
					});
				}
			}
			Core.CollectArt(result);
			return nextURL;
		}
	}

	// ==============================

	//class Class5 : IScript
	//{

	//	private Policy2 m_policy;
	//	public Class5()
	//	{
	//		m_policy = new Policy2(
	//			"http://d2.sku117.org/forum.php?mod=forumdisplay&fid=3&page=1"
	//		);
	//	}

	//	public override string ToString()
	//	{
	//		return "xp1024.com - 最新合集";
	//	}

	//	public void Run()
	//	{
	//		m_policy.Run();
	//	}

	//}

	//class Policy2
	//{
	//	private readonly string m_url;
	//	private readonly string m_script_getArtURLList = @"
	//		JSON.stringify($('#moderate table tbody tr td:nth-last-child(2).max-td table.article tbody tr.article-title td a[target]').map(function (i, a) {
	//			return { text: a.innerText, url: a.href, date: new Date((new Date()).getFullYear() + '-' + $(a).children('span').text()) }
	//		}).toArray())
	//	";
	//	private readonly string m_script_getArt = @"
	//		JSON.stringify($('td.t_f').map(function (i, a) {
	//			return {
	//				text: a.innerText,
	//				downloads: JSON.stringify($(a).children('a').filter(function () { return $(this).find('img').length == 0 }).map(function (ii, aa) { return aa.href }).toArray()),
	//				images: JSON.stringify($(a).find('img').map(function (ii, aa) { return aa.src }).toArray())
	//			}
	//		}).toArray())
	//	";
	//	private readonly string m_script_next = @"
	//		JSON.stringify($('#fd_page_bottom div.pg a.nxt').length ? $('#fd_page_bottom div.pg a.nxt')[0].href : '')
	//	";

	//	public Policy2(string startURL)
	//	{
	//		m_url = startURL;
	//	}

	//	class Item
	//	{
	//		public string text { get; set; }
	//		public string url { get; set; }
	//		public DateTime date { get; set; }
	//	}

	//	class ContentResult
	//	{
	//		public string text { get; set; }
	//		public string downloads { get; set; }
	//		public string images { get; set; }
	//	}

	//	public void Run()
	//	{
	//		string nextURL = m_url;
	//		for (;;) {
	//			nextURL = pageRun(nextURL);
	//			if (string.IsNullOrEmpty(nextURL)) { break; }
	//		}
	//	}

	//	public string pageRun(string url)
	//	{
	//		Core.SetPrograss(this, url, 0);
	//		var result = new List<Core.Art>();
	//		var listResult = Core.Fetch<Item[]>(url, m_script_getArtURLList, Core.m_formMain.CommonWebKitBrowser);
	//		var nextURL = Core.Fetch<string>(url, m_script_next, Core.m_formMain.CommonWebKitBrowser);
	//		int i = 0;
	//		foreach (var artUrl in listResult) {
	//			Core.SetPrograss(this, artUrl.url, (int)(++i * 100.0f / listResult.Length));
	//			ContentResult[] contentList;
	//			for (;;) {
	//				contentList = Core.Fetch<ContentResult[]>(artUrl.url, m_script_getArt, Core.m_formMain.CommonWebKitBrowser);
	//				if (contentList.Length > 0) { break; }
	//			}

	//			foreach (var content in contentList) {
	//				result.Add(new Core.Art() {
	//					date = artUrl.date,
	//					downloads = content.downloads,
	//					images = content.images,
	//					source = artUrl.url,
	//					text = string.Format("{0}\r\n{1}", artUrl.text, content.text),
	//					tags = m_tags
	//				});
	//			}

	//		}
	//		Core.CollectArt(result);
	//		return nextURL;
	//	}
	//}
}