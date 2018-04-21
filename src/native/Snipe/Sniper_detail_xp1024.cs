using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mshtml;
using System.Collections.Concurrent;
using static Snipe.Storage;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Snipe
{
	class Sniper_detail_xp1024 : Sniper
	{

		protected override IEnumerable<UrlTask> GetUrlTaskList()
		{
			// var codes = new int[] { 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, };
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

			//switch (page.sniper) {
			//	case 1001:
			//	case 1002: {
					var divContent = doc.getElementById("read_tpc"); if (divContent == null) { return; }
					var nodes = (IHTMLDOMChildrenCollection)((IHTMLDOMNode)divContent).childNodes;

					// 有效流程: 文本 -> 图片 -> 超链接
					string text = null;
					List<string> imgList = new List<string>();
					List<URL> urlList = new List<URL>();

					foreach (IHTMLDOMNode node in nodes) {

						int nodeType = node.nodeType;

						// 特殊处理 <a>, , 可能有多个 <a> 作为结尾
						if (nodeType == 1 && ((IHTMLElement)node).tagName.ToLower() == "a" && Utils.recFindElementByTag((IHTMLElement)node, "img").Count == 0) {
							IHTMLElement ele = (IHTMLElement)node;
							if (text == null || imgList.Count == 0) { continue; }       // 不合法
							var aText = ele.innerText;
							var aHref = ele.getAttribute("href") as string;
							urlList.Add(new URL() { text = aText, href = aHref });
						} else {
							// 判断是否已经形成了一个 流程, 是则: 形成 一个 art
							if (text != null && imgList.Count > 0 && urlList.Count > 0) {
								Art art = new Art() { text = text, pageID = page.id, images = JsonConvert.SerializeObject(imgList), downloads = JsonConvert.SerializeObject(urlList) };
								arts.Add(art);
								text = null; imgList.Clear(); urlList.Clear();
							}

							if (nodeType == 1) {           // Element node.
								IHTMLElement ele = (IHTMLElement)node;
								List<IHTMLElement> imgElements = Utils.recFindElementByTag(ele, "img");
								if (imgElements.Count > 0){
									if (text == null) { continue; }     // 不合法
									imgList.AddRange(imgElements.Select(e=>e.getAttribute("src") as string));
								} else {
									var tmp = ele.innerText; if (tmp == null) { continue; }
									tmp = tmp.Trim(' ', '='); if (string.IsNullOrEmpty(tmp)) { continue; }
									text += tmp + "\r\n";
								}
							} else if (nodeType == 3) {        // Text node.
								var tmp = node.nodeValue as string; if (tmp == null) { continue; }
								tmp = tmp.Trim(' ', '='); if (string.IsNullOrEmpty(tmp)) { continue; }
								text += tmp + "\r\n";
							} else {
								Debug.Assert(false);
							}
						}
					}

					// 判断是否已经形成了一个 流程, 是则: 形成 一个 art
					if (text != null && imgList.Count > 0 && urlList.Count > 0) {
						Art art = new Art() { text = text, pageID = page.id, images = JsonConvert.SerializeObject(imgList), downloads = JsonConvert.SerializeObject(urlList) };
						arts.Add(art);
						text = null; imgList.Clear(); urlList.Clear();
					}
			//	}
			//	break;
			//	case 1002: {
			//		break;
			//	}
			//	break;
			//}
			storage.AddPageArt(page, arts);
		}
	}
}
