namespace Sniper
{
	partial class Form2
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btn_Open = new System.Windows.Forms.ToolStripButton();
			this.btn_Save = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btn_CreatePlan = new System.Windows.Forms.ToolStripButton();
			this.btn_Action = new System.Windows.Forms.ToolStripSplitButton();
			this.btn_Action_OpenWeb = new System.Windows.Forms.ToolStripMenuItem();
			this.btn_Action_FatchTable = new System.Windows.Forms.ToolStripMenuItem();
			this.btn_Delete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_Open,
            this.btn_Save,
            this.toolStripSeparator1,
            this.btn_CreatePlan,
            this.btn_Action,
            this.btn_Delete,
            this.toolStripSeparator2,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripButton8});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(727, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btn_Open
			// 
			this.btn_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btn_Open.Image = ((System.Drawing.Image)(resources.GetObject("btn_Open.Image")));
			this.btn_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btn_Open.Name = "btn_Open";
			this.btn_Open.Size = new System.Drawing.Size(44, 22);
			this.btn_Open.Text = "Open";
			this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
			// 
			// btn_Save
			// 
			this.btn_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btn_Save.Image = ((System.Drawing.Image)(resources.GetObject("btn_Save.Image")));
			this.btn_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btn_Save.Name = "btn_Save";
			this.btn_Save.Size = new System.Drawing.Size(39, 22);
			this.btn_Save.Text = "Save";
			this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btn_CreatePlan
			// 
			this.btn_CreatePlan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btn_CreatePlan.Image = ((System.Drawing.Image)(resources.GetObject("btn_CreatePlan.Image")));
			this.btn_CreatePlan.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btn_CreatePlan.Name = "btn_CreatePlan";
			this.btn_CreatePlan.Size = new System.Drawing.Size(36, 22);
			this.btn_CreatePlan.Text = "Plan";
			this.btn_CreatePlan.Click += new System.EventHandler(this.btn_CreatePlan_Click);
			// 
			// btn_Action
			// 
			this.btn_Action.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btn_Action.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_Action_OpenWeb,
            this.btn_Action_FatchTable});
			this.btn_Action.Image = ((System.Drawing.Image)(resources.GetObject("btn_Action.Image")));
			this.btn_Action.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btn_Action.Name = "btn_Action";
			this.btn_Action.Size = new System.Drawing.Size(60, 22);
			this.btn_Action.Text = "Action";
			this.btn_Action.ButtonClick += new System.EventHandler(this.btn_Action_OpenWeb_Click);
			// 
			// btn_Action_OpenWeb
			// 
			this.btn_Action_OpenWeb.Name = "btn_Action_OpenWeb";
			this.btn_Action_OpenWeb.Size = new System.Drawing.Size(138, 22);
			this.btn_Action_OpenWeb.Text = "OpenWeb";
			this.btn_Action_OpenWeb.Click += new System.EventHandler(this.btn_Action_OpenWeb_Click);
			// 
			// btn_Action_FatchTable
			// 
			this.btn_Action_FatchTable.Name = "btn_Action_FatchTable";
			this.btn_Action_FatchTable.Size = new System.Drawing.Size(138, 22);
			this.btn_Action_FatchTable.Text = "FatchTable";
			this.btn_Action_FatchTable.Click += new System.EventHandler(this.btn_Action_FatchTable_Click);
			// 
			// btn_Delete
			// 
			this.btn_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btn_Delete.Image = ((System.Drawing.Image)(resources.GetObject("btn_Delete.Image")));
			this.btn_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btn_Delete.Name = "btn_Delete";
			this.btn_Delete.Size = new System.Drawing.Size(49, 22);
			this.btn_Delete.Text = "Delete";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton6
			// 
			this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
			this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.Size = new System.Drawing.Size(107, 22);
			this.toolStripButton6.Text = "toolStripButton6";
			// 
			// toolStripButton7
			// 
			this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
			this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton7.Name = "toolStripButton7";
			this.toolStripButton7.Size = new System.Drawing.Size(107, 22);
			this.toolStripButton7.Text = "toolStripButton7";
			// 
			// toolStripButton8
			// 
			this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
			this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton8.Name = "toolStripButton8";
			this.toolStripButton8.Size = new System.Drawing.Size(107, 22);
			this.toolStripButton8.Text = "toolStripButton8";
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			this.splitContainer1.Size = new System.Drawing.Size(727, 587);
			this.splitContainer1.SplitterDistance = 242;
			this.splitContainer1.TabIndex = 1;
			// 
			// treeView1
			// 
			this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(240, 585);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(727, 612);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "Form2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form2";
			this.Shown += new System.EventHandler(this.Form2_Shown);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btn_Open;
		private System.Windows.Forms.ToolStripButton btn_Save;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btn_CreatePlan;
		private System.Windows.Forms.ToolStripButton btn_Delete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButton6;
		private System.Windows.Forms.ToolStripButton toolStripButton7;
		private System.Windows.Forms.ToolStripButton toolStripButton8;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ToolStripSplitButton btn_Action;
		private System.Windows.Forms.ToolStripMenuItem btn_Action_OpenWeb;
		private System.Windows.Forms.ToolStripMenuItem btn_Action_FatchTable;
	}
}