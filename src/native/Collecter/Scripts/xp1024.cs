using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// (function(){var _x_script = document.createElement('script'); _x_script.src='https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(_x_script);})()
namespace Collecter.Scripts
{
	class xp1024_日本騎兵 : IScript
	{
		private Policy1 m_policy;
		public xp1024_日本騎兵()
		{
			m_policy = new Policy1(this, "http://w3.afulyu.pw/pw/thread.php?fid=22", new string[] { "日本有码" });
		}
		
		public void Run(bool reset) { m_policy.Run(reset); }
		public string GetProgressString() { return m_policy.GetProgressString(); }
		public void ResetProgress() { m_policy.ResetProgress(); }
	}

	class xp1024_亚洲无码 : IScript
	{

		private Policy1 m_policy;
		public xp1024_亚洲无码()
		{
			m_policy = new Policy1(this, "http://w3.afulyu.pw/pw/thread.php?fid=5", new string[] { "日本无码" });
		}
		
		public void Run(bool reset) { m_policy.Run(reset); }
		public string GetProgressString() { return m_policy.GetProgressString(); }
		public void ResetProgress() { m_policy.ResetProgress(); }
	}

	class xp1024_歐美新片 : IScript
	{

		private Policy1 m_policy;
		public xp1024_歐美新片()
		{
			m_policy = new Policy1(this, "http://w3.afulyu.pw/pw/thread.php?fid=7", new string[] { "欧美无码" });
		}
		
		public void Run(bool reset) { m_policy.Run(reset); }
		public string GetProgressString() { return m_policy.GetProgressString(); }
		public void ResetProgress() { m_policy.ResetProgress(); }
	}

	class xp1024_三級寫真 : IScript
	{

		private Policy1 m_policy;
		public xp1024_三級寫真()
		{
			m_policy = new Policy1(this, "http://w3.afulyu.pw/pw/thread.php?fid=18", new string[] { "三級" });
		}
		
		public void Run(bool reset) { m_policy.Run(reset); }
		public string GetProgressString() { return m_policy.GetProgressString(); }
		public void ResetProgress() { m_policy.ResetProgress(); }
	}

	class Policy1
	{
		private readonly string m_url;
		private readonly string m_script_getArtURLList = @"
			JSON.stringify($('table#ajaxtable tbody tr')
				.filter(function(){return $(this).find('td:nth-child(2) img[title=""置顶帖标志""]').length == 0 && $(this).find('td:nth-child(2) h3 a').length!=0})
				.map(function(){ return {
					text:$(this).find('td:nth-child(2) h3 a')[0].innerText,
					url:$(this).find('td:nth-child(2) h3 a')[0].href,
					date:$(this).find('td:nth-child(5) a')[0].innerText}}).toArray())
		";
		private readonly string m_script_getArt = @"
			JSON.stringify([
				{
					text:$('#read_tpc').text(),
					downloads: JSON.stringify($('#read_tpc a').filter(function () { return $(this).find('img').length == 0 }).map(function () { return this.href }).toArray()),
					images: JSON.stringify($('#read_tpc img').map(function () { return this.src }).toArray())
				}
			])
		";
		private readonly string m_script_next = @"
			JSON.stringify($('div.pages.cc>b').next()[0].className == 'b' ? '' : $('div.pages.cc>b').next()[0].href)
		";
		private readonly string[] m_tags;
		private IScript m_host;
		public Policy1(IScript host, string startURL, string[] tags)
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

		public void Run(bool reset)
		{
			string nextURL = m_url;
			if (!reset) {
				var progress = GetProgressString();
				if (string.IsNullOrEmpty(progress) && progress != "finish") {
					nextURL = progress;
				}
			}

			int i = 0;
			for (;;) {
				SetProgressString(nextURL);
				Core.SetPrograss(m_host, i++.ToString(), 0);
				nextURL = pageRun(nextURL);
				if (string.IsNullOrEmpty(nextURL)) {
					SetProgressString("finish");
					break;
				}
			}

			Core.SetPrograss(m_host, "Finish", 100);
		}

		public string pageRun(string url)
		{
			var result = new List<Core.Art>();
			Item[] listResult;
			for (;;) {
				listResult = Core.Fetch<Item[]>(url, m_script_getArtURLList);
				if (listResult.Length > 0) { break; }
				Core.WaitRandom();
			}
			var nextURL = Core.Fetch<string>(url, m_script_next);
			int i = 0;
			foreach (var artUrl in listResult) {
				Core.SetPrograss(m_host, null, (int)(++i * 100.0f / listResult.Length));
				ContentResult[] contentList;
				for (;;) {
					Core.WaitRandom();
					contentList = Core.Fetch<ContentResult[]>(artUrl.url, m_script_getArt);
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

		private string progressFilename {
			get { return string.Format("progress.{0}.txt", m_host); }
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
	}

}