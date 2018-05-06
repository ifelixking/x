using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snipe
{
	class Storage
	{
		public class Tag
		{
			public long id;
			public string name;
		}

		public class Page
		{
			public long id;
			public string title;
			public string url;
			public DateTime date;
			public string tags;
			public int sniper;
		}

		public class Art
		{
			public long id;
			public long pageID;
			public string text;
			public string downloads;
			public string images;
		}

		public struct URL
		{
			public string text;
			public string href;
		}

		private DbConnection m_conn;

		public Storage()
		{
			m_conn = null;
		}

		private Dictionary<String, long> m_dicTag = new Dictionary<string, long>();

		public void Open()
		{
			if (m_conn != null) { Close(); }
			string connStr = "server=localhost;user=root;database=x;port=3306;password=000000";
			m_conn = new MySqlConnection(connStr);
			m_conn.Open();
			m_dicTag = GetAllTag();
		}

		public void Close()
		{
			m_conn.Close();
			m_conn = null;
		}

		public Dictionary<String, long> GetAllTag()
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

		public Tag NewTag(string tagName)
		{
			using (var cmd = m_conn.CreateCommand()) {
				cmd.CommandText = "INSERT INTO tag(name) VALUES(@name)";
				var param = cmd.CreateParameter(); param.ParameterName = "@name"; param.Value = tagName;
				cmd.Parameters.Add(param);
				var id = ((MySqlCommand)cmd).LastInsertedId;
				return new Tag() { id = id, name = tagName };
			}
		}

		public void AddPage(IEnumerable<Page> pageList)
		{
			using (var trans = m_conn.BeginTransaction()) {
				using (var cmd = m_conn.CreateCommand()) {
					cmd.Transaction = trans;
					cmd.CommandText = "INSERT INTO page(title, url, `date`, tags, sniper) VALUES(@title, @url, @date, @tags, @sniper)";
					var pTitle = cmd.CreateParameter(); cmd.Parameters.Add(pTitle); pTitle.ParameterName = "@title";
					var pUrl = cmd.CreateParameter(); cmd.Parameters.Add(pUrl); pUrl.ParameterName = "@url";
					var pDate = cmd.CreateParameter(); cmd.Parameters.Add(pDate); pDate.ParameterName = "@date";
					var pTags = cmd.CreateParameter(); cmd.Parameters.Add(pTags); pTags.ParameterName = "@tags";
					var pSniper = cmd.CreateParameter(); cmd.Parameters.Add(pSniper); pSniper.ParameterName = "@sniper";
					foreach (var page in pageList) {
						pTitle.Value = page.title; pUrl.Value = page.url; pDate.Value = page.date; pTags.Value = page.tags; pSniper.Value = page.sniper;
						cmd.ExecuteNonQuery();
						page.id = ((MySqlCommand)cmd).LastInsertedId;
					}
				}
				trans.Commit();
			}
		}

		public List<Page> GetAllUnprocessPage(int[] sniperCodes)
		{
			if (sniperCodes == null || sniperCodes.Length == 0) { return new List<Page>(); }
			List<Page> result = new List<Page>();
			using (var cmd = m_conn.CreateCommand()) {
				cmd.CommandText = sniperCodes.Length == 1 ?
					string.Format("SELECT id, url, tags, sniper FROM page WHERE processed=0 AND sniper={0}", sniperCodes[0]) :
					string.Format("SELECT id, url, tags, sniper FROM page WHERE processed=0 AND sniper in ({0})", string.Join(",", sniperCodes));
				using (var reader = cmd.ExecuteReader()) {
					int idxID = reader.GetOrdinal("id");
					int idxURL = reader.GetOrdinal("url");
					int idxTags = reader.GetOrdinal("tags");
					int idxSniper = reader.GetOrdinal("sniper");
					while (reader.Read()) { result.Add(new Page() { id = reader.GetInt64(idxID), url = reader.GetString(idxURL), tags = reader.GetString(idxTags), sniper = reader.GetInt32(idxSniper) }); }
					reader.Close();
				}
			}
			return result;
		}

		// 可子线程调用
		public void AddPageArt(Page page, List<Art> arts)
		{
			using (var trans = m_conn.BeginTransaction()) {
				using (var cmd = m_conn.CreateCommand()) {
					cmd.Transaction = trans;
					if (arts.Count > 0) {
						cmd.CommandText = "INSERT INTO art(pageID, text, downloads, images) VALUES(@pageID, @text, @downloads, @images)";
						var pPageID = cmd.CreateParameter(); pPageID.ParameterName = "@pageID"; pPageID.Value = page.id; cmd.Parameters.Add(pPageID);
						var pText = cmd.CreateParameter(); pText.ParameterName = "@text"; cmd.Parameters.Add(pText);
						var pDownloads = cmd.CreateParameter(); pDownloads.ParameterName = "@downloads"; cmd.Parameters.Add(pDownloads);
						var pImages = cmd.CreateParameter(); pImages.ParameterName = "@images"; cmd.Parameters.Add(pImages);
						foreach (var art in arts) {
							pText.Value = art.text; pDownloads.Value = art.downloads; pImages.Value = art.images;
							cmd.ExecuteNonQuery();
						}
					}
					cmd.Parameters.Clear();
					{
						cmd.CommandText = "UPDATE page SET processed=1, artCount=@artCount WHERE id=@id";
						var pArtCount = cmd.CreateParameter(); pArtCount.ParameterName = "@artCount"; cmd.Parameters.Add(pArtCount); pArtCount.Value = arts.Count;
						var pID = cmd.CreateParameter(); pID.ParameterName = "@id"; cmd.Parameters.Add(pID); pID.Value = page.id;
						cmd.ExecuteNonQuery();
					}
				}
				trans.Commit();
			}
		}

		// 去重
		public int DistinctPageByURL()
		{
			List<long> deleteIDs;
			using (var cmd = m_conn.CreateCommand()) {
				// 找到重复的 url
				cmd.CommandText = "SELECT url, COUNT(*) AS c FROM page GROUP BY url ORDER BY c DESC";
				List<string> duplicateURLs = new List<string>();
				int duplicateCount = 0;
				using (var reader = cmd.ExecuteReader()) {
					int idxURL = reader.GetOrdinal("url");
					int idxC = reader.GetOrdinal("c");
					while (reader.Read()) {
						int count = reader.GetInt32(idxC);
						if (count == 1) { break; }
						duplicateURLs.Add(reader.GetString(idxURL));
						duplicateCount += (count - 1);
					}
				}
				// 计算出需要删除的 ID 列表
				deleteIDs = new List<long>(duplicateCount);
				cmd.CommandText = "SELECT id FROM page WHERE url=@url";
				var pURL = cmd.CreateParameter(); pURL.ParameterName = "@url"; cmd.Parameters.Add(pURL);
				foreach (var url in duplicateURLs) {
					pURL.Value = url;
					using (var reader = cmd.ExecuteReader()) {
						int idxID = reader.GetOrdinal("id");
						reader.Read();      // 排除第一个
						while (reader.Read()) {
							deleteIDs.Add(reader.GetInt64(idxID));
						}
					}
				}
			}
			// 执行删除
			using (var trans = m_conn.BeginTransaction()) {
				using (var cmd = m_conn.CreateCommand()) {
					cmd.CommandText = "DELETE FROM page WHERE id=@id";
					var pID = cmd.CreateParameter(); pID.ParameterName = "@id"; cmd.Parameters.Add(pID);
					foreach (var id in deleteIDs) {
						pID.Value = id;
						cmd.ExecuteNonQuery();
					}
				}
				trans.Commit();
			}
			return deleteIDs.Count;
		}

	}
}