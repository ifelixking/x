using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mshtml;
using System.Collections.Concurrent;

namespace Snipe
{
	class Sniper_detail_xp1024 : Sniper
	{

		protected override IEnumerable<UrlTask> GetUrlTaskList()
		{
			var codes = new int[] { 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, };
			Storage storage = new Storage(); storage.Open();
			var result = storage.GetAllUnprocessPage(codes).Select(a => new UrlTask() { param = a, url = a.url });
			storage.Close();
			return result;
		}

		protected override void processDoc(HTMLDocumentClass doc, Storage storage, object param)
		{
			Storage.Page page = param as Storage.Page;
			List<Storage.Art> arts = new List<Storage.Art>();

			switch (page.sniper) {
				case 1001:{
					
				}break;
			}
			storage.AddPageArt(page, arts);
		}
	}
}
