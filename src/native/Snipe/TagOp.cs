using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snipe
{
	class TagOp : Sniper
	{

		public override void Run()
		{
			Storage storage = null;
			try {
				storage = new Storage(); storage.Open();
				buildTagFromPage(storage);
			} catch (Exception ex) {
				EMIT_LOG(ex.ToString());
			} finally {
				storage.Close();
			}
			EMIT_FINISH();
		}

		private void buildTagFromPage(Storage storage)
		{
			var list = storage.GetArtIDWithTagsStringList();
			Dictionary<String, long> dicTag = storage.GetAllTag();
			using (var trans = storage.CreateTransaction()) {
				var artTagList = new List<KeyValuePair<long, long>>();
				var split = new string[] { ";" };
				var processedPageIDs = new List<long>();
				int i = 0;
				foreach (var item in list) {
					EMIT_PROGRESS(++i, list.Count);
					var tagNames = item.tags.Split(split, StringSplitOptions.RemoveEmptyEntries);
					foreach (var tagName in tagNames) {
						long tagID;
						if (!dicTag.TryGetValue(tagName, out tagID)) {
							var newTag = storage.NewTag(tagName, trans);
							dicTag.Add(tagName, newTag.id);
							tagID = newTag.id;
						}
						artTagList.Add(new KeyValuePair<long, long>(item.artID, tagID));
					}
					processedPageIDs.Add(item.pageID);
				}
				storage.AddRelArtTag(artTagList, trans);
				if (processedPageIDs.Count > 0) { storage.setPageProcessStage(processedPageIDs.Distinct(), 2, trans); }
				trans.Commit();
			}
		}
	}
}
