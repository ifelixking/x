using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scripter
{
	class MyTreeView : TreeView
	{
		// .net bug: double click the checkbox of node will skip the treeViewCE_BeforeCheck event
		protected override void WndProc(ref Message m) {
			// Suppress WM_LBUTTONDBLCLK
			if (m.Msg == 0x203) { m.Result = IntPtr.Zero; } else base.WndProc(ref m);
		}
	}
}
