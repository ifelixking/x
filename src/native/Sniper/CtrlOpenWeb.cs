using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sniper
{
	public partial class CtrlOpenWeb : UserControl
	{
		public CtrlOpenWeb()
		{
			InitializeComponent();
		}

		private Action m_action;
		internal Action Action { get{
			return m_action;
		} set{
			m_action = value;
			// m_action .Config as string
		} }

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			
		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
