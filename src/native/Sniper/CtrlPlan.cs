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
	public partial class CtrlPlan : UserControl
	{
		public CtrlPlan()
		{
			InitializeComponent();
		}

		private Plan m_plan;
		internal Plan Plan {
			get {
				return m_plan;
			}
			set {
				m_plan = value;
				textBox1.Text = m_plan.Name;
				richTextBox1.Text = m_plan.Param as string;
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (m_plan != null) {
				m_plan.Name = textBox1.Text;
			}
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
			if (m_plan != null) {
				m_plan.Param = richTextBox1.Text;
			}
		}
	}
}
