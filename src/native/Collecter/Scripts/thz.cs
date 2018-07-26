﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collecter.Scripts
{
	class Class11 : IScript
	{
		private Policy11 m_policy;
		public Class11()
		{
			m_policy = new Policy11(
				this,
				"http://thz4.com/forum-220-1.html",
				new string[] { "亚洲有码" }
			);
		}

		public override string ToString()
		{
			return "thz.com - 亚洲有碼原創";
		}

		public void Run()
		{
			m_policy.Run();
		}
	}

	class Class12 : IScript
	{
		private Policy11 m_policy;
		public Class12()
		{
			m_policy = new Policy11(
				this,
				"http://thz4.com/forum-181-1.html",
				new string[] { "亚洲无码" }
			);
		}

		public override string ToString()
		{
			return "thz.com - 亚洲無碼原創";
		}

		public void Run()
		{
			m_policy.Run();
		}
	}

	class Class13 : IScript
	{
		private Policy11 m_policy;
		public Class13()
		{
			m_policy = new Policy11(
				this,
				"http://thz4.com/forum-182-1.html",
				new string[] { "欧美无码" }
			);
		}

		public override string ToString()
		{
			return "thz.com - 欧美無碼";
		}

		public void Run()
		{
			m_policy.Run();
		}
	}

	class Policy11
	{
		private readonly string m_url;
		private readonly string m_script_getArtURLList = @"
			JSON.stringify($('table#threadlisttableid tbody tr')
				.filter(function () {
					return $(this).children('th').children('a.s.xst').length > 0
						&& $(this).children('td.by').length > 0
						&& $(this).children('th').children('a.showhide').length == 0
				})
				.map(function (i, a) {
					var item = $(a).children('th').children('a.s.xst')[0]
					return item && { text: item.innerText, url: item.href, date: $('td.by:first em span span', a).attr('title') }
				}
			).toArray())
		";
		private readonly string m_script_getArt = @"
			JSON.stringify(
				{
					text: $('td.t_f')[0].innerText,
					downloads: $('dl.tattl dd p.attnm a').map(function (i, a) { return a.href }).toArray(),
					images: JSON.stringify($('td.t_f img').map(function (ii, aa) { return aa.src }).toArray())
				}
			)
		";
		private readonly string m_script_next = @"
			JSON.stringify($('#fd_page_bottom div.pg a.nxt').length ? $('#fd_page_bottom div.pg a.nxt')[0].href : '')
		";
		private readonly string m_script_download = "JSON.stringify($('a[onclick=\"hideWindow(\\'imc_attachad\\')\"]')[0].href)";

		private readonly string[] m_tags;
		private object m_host;

		public Policy11(object host, string startURL, string[] tags)
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
			public string[] downloads { get; set; }
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
				ContentResult content;
				for (;;) {
					content = Core.Fetch<ContentResult>(artUrl.url, m_script_getArt, Core.m_formMain.CommonWebKitBrowser);
					if (content != null) { break; }
				}

				List<string> downloads = new List<string>();
				foreach (var dl in content.downloads) {
					downloads.Add(Core.Fetch<string>(dl, m_script_download, Core.m_formMain.CommonWebKitBrowser));
				}

				result.Add(new Core.Art() {
					date = artUrl.date,
					downloads = JsonConvert.SerializeObject(downloads.ToArray()),
					images = content.images,
					source = artUrl.url,
					text = string.Format("{0}\r\n{1}", artUrl.text, content.text),
					tags = m_tags
				});
			}
			Core.CollectArt(result);
			return nextURL;
		}
	}
}


