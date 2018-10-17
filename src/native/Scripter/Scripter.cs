using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Scripter
{
	static class Scripter
	{
		public static void InjectInitializeScript(WebViewer.WebView wkb) {
			// if (wkb.Url == null) { return; }
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Scripter.init.js");
			using (var reader = new StreamReader(stream)) {
				var text = reader.ReadToEnd();
				// wkb.StringByEvaluatingJavaScriptFromString(text);
				wkb.RunJavaScript(text, null);
			}
		}

		//public class CaptureObject{
		//	public string text{get;set;}
		//	public string link{get;set;}
		//}

		// public static 

		public class Attribute
		{
			public string name { get; set; }
			public string value { get; set; }
			public override string ToString() {
				return string.Format("{0}={1}", name, value);
			}
		}

		public class CaptureElementConfig
		{
			public bool tag { get; set; }
			public List<int> classes = new List<int>();
			public bool index { get; set; }
			public bool first { get; set; }
			public bool last { get; set; }
			public bool odd { get; set; }
			public bool even { get; set; }
			public bool content { get; set; }
			public List<int> attrs = new List<int>();

			public bool col_content { get; set; }
			public List<int> col_attrs = new List<int>();
		}

		public class CaptureElement
		{
			public string tagName { get; set; }
			public string innerText { get; set; }
			public string[] classNames { get; set; }
			public int? index { get; set; }
			public bool? isLast { get; set; }
			public Attribute[] attributes { get; set; }
			public bool? output { get; set; }
			public CaptureElement[] children { get; set; }

			// 由 WinForm 端写入
			// public CaptureElementConfig config = new CaptureElementConfig() { tag = true, first = true, col_content = true };
			public CaptureElementConfig config { get; set; }

			public override string ToString() {
				string result = string.Empty;
				if (config.tag) { result += tagName; }
				result += string.Join(string.Empty, config.classes.Select(a => "." + config.classes[a]));
				return result;
			}
		}

		//public class Selector
		//{
		//	public string strJQuery;
		//	public string[] props;
		//	public string[] attrs;
		//}

		//public class SelectElement
		//{
		//	public Attribute[] attrs { get; set; }
		//}

		public class SelectItem
		{
			public string jq { get; set; }
			public int idx { get; set; }
			public Attribute[] attrs { get; set; }
			public SelectItem[] subItems { get; set; }
		}

		public class Query
		{
			public string strJQuery { get; set; }
			public Query[] subs { get; set; }
		}

		public class SelectResult
		{
			public Query query { get; set; }
			public SelectItem[] data { get; set; }
		}

		public delegate void SelectResultHandler(SelectResult result);

		public static void Select(WebViewer.WebView wkb, CaptureElement[] selector, SelectResultHandler handler) {
			var param = JsonConvert.SerializeObject(selector);
			wkb.RunJavaScript(string.Format("{0}({1})", "_x_select", param), new WebViewer.ScriptResultHandler((resultJson) => {
				var result = JsonConvert.DeserializeObject<SelectResult>(resultJson.ToString());
				handler(result);
			}));
		}
	}


}
