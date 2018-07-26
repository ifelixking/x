using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebKit;

namespace Collecter
{
	static class Core
	{
		private static DbConnection m_conn;
		private static Dictionary<String, long> m_dicTag = new Dictionary<string, long>();
		internal static FormMain m_formMain;

		public class Art
		{
			public string text;
			public string downloads;
			public string images;
			public DateTime date;
			public string source;
			public string[] tags;
		}

		public static void Init()
		{
			string connStr = "server=localhost;user=root;database=x;port=3306;password=000000";
			m_conn = new MySqlConnection(connStr);
			m_conn.Open();
			m_dicTag = getAllTag();
		}

		public static void Destroy()
		{
			if (m_conn == null) { return; }
			m_conn.Close();
			m_conn = null;
		}

		// 获取
		public static T Fetch<T>(string url, string script, WebKitBrowser wkb = null)
		{
			var wkbTemp = wkb;
			if (wkbTemp == null) { wkbTemp = new WebKitBrowser(); m_formMain.InvisibleContainer.Controls.Add(wkbTemp); }

			// navigate to url
			if (wkbTemp.Url == null || wkbTemp.Url.ToString() != url) {
				wkbTemp.Navigate("about:blank");
				var oldTag = wkbTemp.Tag; wkbTemp.Tag = 1;
				wkbTemp.DocumentCompleted += wkb_DocumentCompleted;
				wkbTemp.Navigate(url);
				{
					DateTime start = DateTime.Now;
					while (wkbTemp.Tag != null) {
						if ((DateTime.Now - start).TotalSeconds > 30) {
							wkbTemp.Navigate(url);
							// wkbTemp.Reload();
							start = DateTime.Now;
						}
						Application.DoEvents();
					}
				}
				wkbTemp.DocumentCompleted -= wkb_DocumentCompleted;
				wkbTemp.Tag = oldTag;

				// 注入 jquery
				for (;;) {
					try {
						wkbTemp.StringByEvaluatingJavaScriptFromString("(function(){var _x_script = document.createElement('script'); _x_script.src='https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(_x_script);})()");
						break;
					} catch (Exception ex) { Core.Log(ex.ToString()); }
					Thread.Sleep(1000);
					Application.DoEvents();
				}
			}

			// 执行 script
			string result = null;
			for (;;) {
				try {
					result = wkbTemp.StringByEvaluatingJavaScriptFromString(script);
					break;
				} catch (Exception ex) { Core.Log(ex.ToString()); }
				Thread.Sleep(1000);
				Application.DoEvents();
			}

			//
			if (wkb == null) {
				wkbTemp.Parent.Controls.Remove(wkbTemp);
				wkbTemp.Dispose();
			}

			return JsonConvert.DeserializeObject<T>(result);
		}

		// 收集
		public static void CollectArt(IEnumerable<Art> arts)
		{
			using (var trans = m_conn.BeginTransaction()) {
				using (var cmd = m_conn.CreateCommand()) using (var cmd2 = m_conn.CreateCommand()) {
					cmd.Transaction = trans;

					cmd.CommandText = "INSERT INTO art(text, downloads, images, date, source) VALUES(@text, @downloads, @images, @date, @source)";
					var pText = cmd.CreateParameter(); pText.ParameterName = "@text"; cmd.Parameters.Add(pText);
					var pDownloads = cmd.CreateParameter(); pDownloads.ParameterName = "@downloads"; cmd.Parameters.Add(pDownloads);
					var pImages = cmd.CreateParameter(); pImages.ParameterName = "@images"; cmd.Parameters.Add(pImages);
					var pDate = cmd.CreateParameter(); pDate.ParameterName = "@date"; cmd.Parameters.Add(pDate);
					var pSource = cmd.CreateParameter(); pSource.ParameterName = "@source"; cmd.Parameters.Add(pSource);

					cmd2.CommandText = "INSERT INTO rel_art_tag(artID, tagID) VALUES(@artID, @tagID)";
					var pArtID = cmd2.CreateParameter(); pArtID.ParameterName = "@artID"; cmd2.Parameters.Add(pArtID);
					var pTagID = cmd2.CreateParameter(); pTagID.ParameterName = "@tagID"; cmd2.Parameters.Add(pTagID);

					foreach (var art in arts) {
						pText.Value = art.text; pDownloads.Value = art.downloads; pImages.Value = art.images; pDate.Value = art.date; pSource.Value = art.source;
						// insert art
						cmd.ExecuteNonQuery();
						var artID = ((MySqlCommand)cmd).LastInsertedId;
						pArtID.Value = artID;
						// insert rel_art_tag
						foreach (var tag in art.tags) {
							var tagID = m_dicTag[tag];
							pTagID.Value = tagID;
							cmd2.ExecuteNonQuery();
						}
					}
				}
				trans.Commit();
			}
		}

		// 进度
		public delegate void ProgressHandler(object sender, string info, int prograss);
		public static void SetPrograss(object sender, string info, int prograss)
		{
			if (m_formMain != null) {
				m_formMain.Invoke(new ProgressHandler(m_formMain.SetPrograss), new object[] { sender, info, prograss });
			}
		}

		// log
		public static void Log(string msg)
		{
			string filename = string.Format("log-{0}.txt", Process.GetCurrentProcess().Id);
			File.AppendAllText(filename, string.Format("{0} {1}\r\n", DateTime.Now, msg));
		}

		// ================================================================================================================
		private static Dictionary<String, long> getAllTag()
		{
			Dictionary<String, long> result = new Dictionary<string, long>();
			using (var comm = m_conn.CreateCommand()) {
				comm.CommandText = "SELECT id, name FROM tag";
				using (var reader = comm.ExecuteReader()) {
					var idxID = reader.GetOrdinal("id");
					var idxName = reader.GetOrdinal("name");
					while (reader.Read()) {
						var id = reader.GetInt64(idxID);
						var name = reader.GetString(idxName);
						result.Add(name, id);
					}
				}
			}
			return result;
		}

		private static void wkb_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
		{
			var wkb = sender as WebKitBrowser;
			wkb.Tag = null;
		}


	}
}

