using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebKit;

namespace Sniper
{
	[JsonObject(MemberSerialization.OptIn)]
	class Plan
	{
		public Plan()
		{
			Starts = new List<Action>();
			this.Name = "Plan";
		}

		public static string ArchiveOut(IEnumerable<Plan> plan)
		{
			return JsonConvert.SerializeObject(plan);
		}

		public static Plan[] ArchiveIn(string data)
		{
			return JsonConvert.DeserializeObject<Plan[]>(data);
		}

		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public string Param { get; set; }

		[JsonProperty]
		public List<Action> Starts { get; private set; }

		public void Run()
		{
			foreach (var action in Starts) {
				action.Param = this.Param;
				runAction(action);
			}
		}

		private void runAction(Action action)
		{
			action.Run();
			if (action.BreakNext) { return; }
			var results = action.Result;
			foreach (var result in results) {
				foreach (var next in action.Nexts) {
					next.Param = result;
					runAction(next);
				}
			}
		}
	}

	public enum ActionType
	{
		//OpenWeb,
		//FatchScaler,
		FatchTable,
	}

	[JsonObject(MemberSerialization.OptIn)]
	class Action
	{
		public Action(ActionType type)
		{
			Nexts = new List<Action>();
			Type = type;
			this.Name = Type.ToString();
		}

		[JsonProperty]
		public string Name { get; set; }

		[JsonProperty]
		public ActionType Type { get; private set; }

		[JsonProperty]
		public List<Action> Nexts { get; private set; }

		[JsonProperty]
		public string Config { get; set; }

		public string Param { get; set; }

		public void Run()
		{
			BreakNext = false;
			Execute exe = null;
			switch (Type) {
				//case ActionType.OpenWeb:
				//exe = new ExeOpenWeb();
				//break;
				//case ActionType.FatchScaler:
				//exe = new ExeFatchScaler();
				//break;
				case ActionType.FatchTable:
				exe = new ExeFatchTable();
				break;
			}
			exe.Config = this.Config; exe.Param = this.Param;
			exe.Exec();
			this.Result = exe.Result;
			BreakNext = exe.Successed == false;
		}

		public List<string> Result { get; private set; }

		public bool BreakNext { get; private set; }

		public delegate void FinishHandle();
	}

	abstract class Execute
	{
		public string Config { get; set; }
		public string Param { get; set; }
		public List<string> Result { get; private set; }
		public bool? Successed { get; protected set; }
		public string FailedInfo { get; protected set; }
		public Execute()
		{
			Result = new List<string>();
		}
		public void Exec()
		{
			Successed = false;
			try {
				doExec();
			} catch (Exception ex) {
				FailedInfo = ex.Message;
			}
		}
		protected abstract void doExec();
	}

	class ExeFatchTable : Execute
	{
		[JsonObject(MemberSerialization.OptIn)]
		public class Configure
		{
			[JsonProperty]
			public string script { get; set; }
		}

		protected override void doExec()
		{
			//var wkb = this.Param as WebKitBrowser;
			//var config = JsonConvert.DeserializeObject<Configure>(this.Config as string);
			//var strResult = wkb.StringByEvaluatingJavaScriptFromString(config.script);
			//var array = JsonConvert.DeserializeObject(strResult) as JArray;
			//foreach (var item in array) {
			//	this.Result.Add(JsonConvert.SerializeObject(item));
			//}
			//Successed = true;
		}
	}
}



//class ExeOpenWeb : Execute
//{
//	// private bool m_wkbCompleted = false;

//	public class Configure
//	{
//		public string URL { get; set; }
//		// public Int64 WebKitBrowserHandle { get; set; }
//	}

//	protected override void doExec()
//	{
//		//m_wkbCompleted = false;
//		//var config = JsonConvert.DeserializeObject<Configure>(Param);
//		//WebKitBrowser wkb;
//		//if (IntPtr.Zero == new IntPtr(config.WebKitBrowserHandle)) {
//		//	wkb = new WebKitBrowser();
//		//} else {
//		//	wkb = Control.FromHandle(new IntPtr(config.WebKitBrowserHandle)) as WebKitBrowser;
//		//}
//		//wkb.DocumentCompleted += Wkb_DocumentCompleted;
//		//wkb.Navigate(config.URL);
//		//while (!m_wkbCompleted) { Application.DoEvents(); }
//		//wkb.DocumentCompleted -= Wkb_DocumentCompleted;
//		//for (;;) {
//		//	try {
//		//		wkb.StringByEvaluatingJavaScriptFromString("(function(){var _x_script = document.createElement('script'); _x_script.src='https://cdn.bootcss.com/jquery/3.3.1/jquery.min.js'; document.body.appendChild(_x_script);})()");
//		//		break;
//		//	} catch (Exception) { }
//		//	Application.DoEvents();
//		//}
//		//DateTime a = DateTime.Now;
//		//for (;;) {
//		//	Application.DoEvents();
//		//	if ((DateTime.Now - a).TotalMilliseconds > 2000) { break; }
//		//}
//		//this.Result.Add(wkb.Handle);
//		//Successed = true;
//	}

//	private void Wkb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
//	{
//		// this.m_wkbCompleted = true;
//	}
//}

//class ExeFatchScaler : Execute
//{
//	public class Configure
//	{
//		public string script { get; set; }
//	}

//	protected override void doExec()
//	{
//		//WebKitBrowser wkb = this.Param as WebKitBrowser;
//		//Configure config = JsonConvert.DeserializeObject<Configure>(this.Config as string);
//		//this.Result.Add(wkb.StringByEvaluatingJavaScriptFromString(config.script));
//		//Successed = true;
//	}
//}