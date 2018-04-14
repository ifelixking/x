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
			// return m_storage.GetAllUnprocessPage(Sniper_list_xp1024.SNIPER_LIST_CODE).Select(a => new UrlTask() { param = a, url = a.url });
			return null;
		}

		protected override void processDoc(HTMLDocumentClass doc, Storage storage, object param)
		{
			Storage.Page page = param as Storage.Page;
		}
	}
}
