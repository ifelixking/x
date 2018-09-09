using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
		private static Dictionary<String, long> m_dicActor = new Dictionary<string, long>();
		internal static FormMain m_formMain;
		private static Random m_random;


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
			m_random = new Random(DateTime.Now.Millisecond);
			string connStr = ConfigurationManager.AppSettings["ConnectionString"];
			m_conn = new MySqlConnection(connStr);
			m_conn.Open();
			m_dicTag = getAllTag();
			m_dicActor = initDicActor();
		}

		public static void Destroy()
		{
			if (m_conn == null) { return; }
			m_conn.Close();
			m_conn = null;
		}

		// 获取
		public static T Fetch<T>(string url, string script)
		{
			var wkb = m_formMain.webKitBrowser1;

			// navigate to url
			if (wkb.Url == null || wkb.Url.ToString() != url) {
				var oldTag = wkb.Tag; wkb.Tag = 1;
				// wkb.DocumentCompleted += wkb_DocumentCompleted;
				wkb.Navigated += Wkb_Navigated;
				wkb.Navigate(url);
				{
					DateTime start = DateTime.Now;
					while (wkb.Tag != null) {
						if ((DateTime.Now - start).TotalSeconds > 30) {
							wkb.Navigate(url);
							// wkbTemp.Reload();
							start = DateTime.Now;
						}
						Application.DoEvents();
					}
				}
				// wkb.DocumentCompleted -= wkb_DocumentCompleted;
				wkb.Navigated -= Wkb_Navigated;
				wkb.Tag = oldTag;

				// 注入 jquery
				for (;;) {
					try {
						wkb.StringByEvaluatingJavaScriptFromString("(function(){var _x_script = document.createElement('script'); _x_script.src='https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(_x_script);})()");
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
					result = wkb.StringByEvaluatingJavaScriptFromString(script);
					break;
				} catch (Exception ex) { Core.Log(ex.ToString()); }
				Thread.Sleep(1000);
				Application.DoEvents();
			}

			return JsonConvert.DeserializeObject<T>(result);
		}

		// 随机等待
		public static void WaitRandom()
		{
			var secs = m_random.Next(3, 10);    // 等待 3 到 10 秒
			DateTime t = DateTime.Now.AddSeconds(secs);
			while (DateTime.Now < t) { Application.DoEvents(); Thread.Sleep(500); }
		}

		// 收集
		public static void CollectArt(IEnumerable<Art> arts)
		{
			using (var trans = m_conn.BeginTransaction()) {
				using (var cmd = m_conn.CreateCommand()) using (var cmd2 = m_conn.CreateCommand()) using (var cmd3 = m_conn.CreateCommand()) {
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

					cmd3.CommandText = "INSERT INTO rel_actor_art(actorID, artID) VALUES(@actorID, artID)";
					var pActorID = cmd3.CreateParameter(); pActorID.ParameterName = "@actorID"; cmd3.Parameters.Add(pActorID);
					var pArtID2 = cmd3.CreateParameter(); pArtID2.ParameterName = "@artID"; cmd3.Parameters.Add(pArtID2);

					foreach (var art in arts) {
						pText.Value = art.text; pDownloads.Value = art.downloads; pImages.Value = art.images; pDate.Value = art.date; pSource.Value = art.source;
						// insert art
						cmd.ExecuteNonQuery();
						var artID = ((MySqlCommand)cmd).LastInsertedId;
						pArtID.Value = artID;
						pArtID2.Value = artID;
						// insert rel_art_tag
						foreach (var tag in art.tags) {
							var tagID = m_dicTag[tag];
							pTagID.Value = tagID;
							cmd2.ExecuteNonQuery();
						}
						// inesrt rel_actor_art
						var text = art.text.ToLower();
						foreach(var mw in m_dicActor){
							if (text.Contains(mw.Key)){
								pActorID.Value = mw.Value;
								cmd3.ExecuteNonQuery();
							}
						}
					}
				}
				trans.Commit();
			}
		}

		public class Actor_Fanhome
		{
			public string name;
			public string pic;
			public int count;
			public string text;
		}

		public static void CollectActor_Fanhome(IEnumerable<Actor_Fanhome> items)
		{
			using (var trans = m_conn.BeginTransaction()) {
				using (var cmd = m_conn.CreateCommand()) using (var cmd2 = m_conn.CreateCommand()) {
					cmd.Transaction = trans;

					cmd.CommandText = "INSERT INTO actor_fanhome(name, pic, count, text) VALUES(@name, @pic, @count, @text)";
					var pName = cmd.CreateParameter(); pName.ParameterName = "@name"; cmd.Parameters.Add(pName);
					var pPic = cmd.CreateParameter(); pPic.ParameterName = "@pic"; cmd.Parameters.Add(pPic);
					var pCount = cmd.CreateParameter(); pCount.ParameterName = "@count"; cmd.Parameters.Add(pCount);
					var pText = cmd.CreateParameter(); pText.ParameterName = "@text"; cmd.Parameters.Add(pText);

					foreach (var item in items) {
						pName.Value = item.name; pPic.Value = item.pic; pCount.Value = item.count; pText.Value = item.text;
						cmd.ExecuteNonQuery();
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

		// ============

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

		private static void Wkb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			var wkb = sender as WebKitBrowser;
			wkb.Tag = null;
		}

		private static Dictionary<String, long> initDicActor()
		{
			Dictionary<String, long> result = new Dictionary<string, long>();
			using (var comm = m_conn.CreateCommand()) {
				comm.CommandText = "SELECT id, name, matchWords FROM actor";
				using (var reader = comm.ExecuteReader()) {
					var idxID = reader.GetOrdinal("id");
					var idxName = reader.GetOrdinal("name");
					var idxMW = reader.GetOrdinal("matchWords");
					while (reader.Read()) {
						var id = reader.GetInt64(idxID);
						var name = reader.GetString(idxName);
						var mw = reader.IsDBNull(idxMW) ? null : reader.GetString(idxMW);
						result[name] = id;
						if (!string.IsNullOrWhiteSpace(mw)) {
							var wList = mw.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
							foreach (var w in wList) {
								result[w.ToLower()] = id;
							}
						}
					}
				}
			}
			return result;
		}

	}

	static class Utils
	{
		[DllImport("kernel32.dll")]
		public static extern int TerminateProcess(IntPtr hProcess, uint uExitCode);
	}
}

