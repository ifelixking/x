using mshtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Snipe
{
	static class Utils
	{
		#region HTML Document
		public static HTMLDocumentClass getDocument(string url)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
			webRequest.Method = "GET";
			HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
			StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
			string content = sr.ReadToEnd();
			HTMLDocumentClass doc = new HTMLDocumentClass();
			((IHTMLDocument2)doc).write(new object[] { content });
			((IHTMLDocument2)doc).close();
			return doc;
		}

		public static HTMLDocumentClass retryGetDocument(string url)
		{
			for (var i = 0; i < 10; ++i) {
				try {
					HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
					webRequest.Method = "GET";
					HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
					StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
					string content = sr.ReadToEnd();
					HTMLDocumentClass doc = new HTMLDocumentClass();
					((IHTMLDocument2)doc).write(new object[] { content });
					((IHTMLDocument2)doc).close();
					return doc;
				} catch (Exception) {
					// MessageBox.Show(e.ToString());
					Thread.Sleep(5000);
					continue;
				}
			}
			throw new Exception();
		}

		public static IEnumerable<IHTMLElement> stepElement(HTMLDocumentClass doc, IHTMLElement eleParent, string strStep)
		{
			var arr = strStep.Split(':');
			if (arr.Length == 1) {
				var tagName = arr[0];
				if (eleParent == null) { eleParent = doc.body; }
				var eleList = (IHTMLElementCollection)eleParent.children;
				return eleList.Cast<IHTMLElement>().SelectMany((e) => e.tagName.ToLower() == tagName ? new IHTMLElement[] { e } : new IHTMLElement[0]);
			} else if (arr.Length == 2) {
				var tagName = arr[0];
				var filter = arr[1];
				if (filter[0] == '#') {
					var eleID = filter.Substring(1);
					if (eleParent == null) {
						var ele = doc.getElementById(eleID);
						return ele == null ? new IHTMLElement[0] : new IHTMLElement[] { ele };
					} else {
						return ((IHTMLElementCollection)eleParent.children).Cast<IHTMLElement>().SelectMany((e) => e.tagName.ToLower() == tagName && e.id.ToLower() == eleID ? new IHTMLElement[] { e } : new IHTMLElement[0]);
					}
				} else if (filter[0] == '@') {
					var eleIndex = int.Parse(filter.Substring(1));
					var tmp = ((IHTMLElementCollection)eleParent.children).Cast<IHTMLElement>().SelectMany((e) => (e.tagName.ToLower() == tagName ? new IHTMLElement[] { e } : new IHTMLElement[0]));
					IHTMLElement result;
					if (eleIndex >= 0) {
						result = tmp.ElementAtOrDefault(eleIndex);
					} else {
						var tmp1 = tmp.ToArray();
						result = tmp1.ElementAtOrDefault(tmp1.Length + eleIndex);
					}
					return result == null ? new IHTMLElement[0] : new IHTMLElement[] { result };
				} else if (filter[0] == '.') {
					var classNames = filter.Substring(1).Replace('.',' ');	// 多个 class 时, className 以空格分隔
					return ((IHTMLElementCollection)eleParent.children).Cast<IHTMLElement>().SelectMany((e) => {
						return (e.tagName.ToLower() == tagName && e.className == classNames ? new IHTMLElement[] { e } : new IHTMLElement[0]);
					});
				} else {
					return new IHTMLElement[0];
				}
			} else {
				return new IHTMLElement[0];
			}
		}

		public static IHTMLElement stepToElement(HTMLDocumentClass doc, IHTMLElement eleParent, IEnumerable<String> steps)
		{
			var ele = eleParent;
			foreach (var step in steps) {
				ele = stepElement(doc, ele, step).FirstOrDefault();
			}
			return ele;
		}

		public static IEnumerable<IHTMLElement> getElementItor(HTMLDocumentClass doc, string pathContainer, string stepItor)
		{
			IHTMLElement eleParent = null;
			if (pathContainer.StartsWith("/")) { eleParent = doc.body; }
			eleParent = stepToElement(doc, eleParent, pathContainer.Split('/'));
			return stepElement(doc, eleParent, stepItor);
		}

		public static List<string[]> getElementAttr(HTMLDocumentClass doc, IEnumerable<IHTMLElement> eleContainer, PathAttribute[] paList)
		{
			List<string[]> result = new List<string[]>();
			List<string> temp = new List<string>(paList.Length);
			foreach (var eleParent in eleContainer) {
				temp.Clear();
				foreach (var pa in paList) {
					var ele = stepToElement(doc, eleParent, pa.pathElement);
					if (ele == null) { break; }
					string attr = ele.getAttribute(pa.attributeName) as string;
					temp.Add(attr);
				}
				if (temp.Count != paList.Length) { continue; }
				result.Add(temp.ToArray());
			}
			return result;
		}

		public struct PathAttribute
		{
			public IEnumerable<String> pathElement;
			public string attributeName;
		}

		public static string mergeUrlParam(string url, params string[] paramWithValues)
		{
			var result = string.Format("{0}{3}{1}={2}", url, paramWithValues[0], paramWithValues[1], url.Contains('?') ? '&' : '?');
			for (var i = 2; i < paramWithValues.Length; i += 2) {
				result = string.Format("{0}&{1}={2}", result, paramWithValues[0], paramWithValues[1]);
			}
			return result;
		}
		#endregion
	}
}
