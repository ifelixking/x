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

		public class Art{
			public long id;
			public long pageID;
			public string text;
			public string downloads;
			public string images;
		}

		private DbConnection m_conn;

		public Storage()
		{
			m_conn = null;
		}

		public void Open()
		{
			if (m_conn != null) { Close(); }
			string connStr = "server=localhost;user=root;database=x;port=3306;password=000000";
			m_conn = new MySqlConnection(connStr);
			m_conn.Open();
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

		public List<Page> GetAllUnprocessPage(int sniperCode)
		{
			List<Page> result = new List<Page>();
			using (var cmd = m_conn.CreateCommand()) {
				cmd.CommandText = string.Format("SELECT id, url, tags FROM page WHERE processed=0 AND sniper={0}", sniperCode);
				using (var reader = cmd.ExecuteReader()) {
					int idxID = reader.GetOrdinal("id");
					int idxURL = reader.GetOrdinal("url");
					int idxTags = reader.GetOrdinal("tags");
					while (reader.Read()) { result.Add(new Page() { id = reader.GetInt64(idxID), url = reader.GetString(idxURL), tags = reader.GetString(idxTags) }); }
					reader.Close();
				}
			}
			return result;
		}

		// 可子线程调用
		public void AddPageArt(Page page, List<Art> arts) {
			using (var cmd = m_conn.CreateCommand()){
				{	
					cmd.CommandText = "INSERT INTO art(pageID, text, downloads, images) VALUES(@pageID, @text, @downloads, @images)";
					var pPageID = cmd.CreateParameter(); pPageID.ParameterName = "@pageID"; pPageID.Value = page.id; cmd.Parameters.Add(pPageID);
					var pText = cmd.CreateParameter(); pText.ParameterName = "@text"; cmd.Parameters.Add(pText);
					var pDownloads = cmd.CreateParameter(); pDownloads.ParameterName = "@downloads"; cmd.Parameters.Add(pDownloads);
					var pImages = cmd.CreateParameter(); pImages.ParameterName = "@images"; cmd.Parameters.Add(pImages);
					foreach (var art in arts){
						pText.Value = art.text; pDownloads.Value = art.downloads; pImages.Value = art.images;
						cmd.ExecuteNonQuery();
					}
				}
				cmd.Parameters.Clear();
				{
					cmd.CommandText = "UPDAE page SET processed=1, artCount=@artCount WHERE id=@id";
					var pArtCount = cmd.CreateParameter(); pArtCount.ParameterName = "@artCount"; cmd.Parameters.Add(pArtCount); pArtCount.Value = arts.Count;
					var pID = cmd.CreateParameter(); pID.ParameterName = "@id"; cmd.Parameters.Add(pID); pID.Value = page.id;
					cmd.ExecuteNonQuery();
				}
			}
		}

	}
}