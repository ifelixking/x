namespace Collecter
{
	partial class FormMain
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.progress2 = new System.Windows.Forms.ToolStripProgressBar();
			this.labTip = new System.Windows.Forms.ToolStripStatusLabel();
			this.progress1 = new System.Windows.Forms.ToolStripProgressBar();
			this.webKitBrowser1 = new WebKit.WebKitBrowser();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(573, 25);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progress2,
            this.labTip,
            this.progress1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 414);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(573, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// progress2
			// 
			this.progress2.Name = "progress2";
			this.progress2.Size = new System.Drawing.Size(100, 16);
			// 
			// labTip
			// 
			this.labTip.AutoSize = false;
			this.labTip.AutoToolTip = true;
			this.labTip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.labTip.Name = "labTip";
			this.labTip.Size = new System.Drawing.Size(300, 17);
			this.labTip.Text = "about:blank";
			this.labTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labTip.DoubleClick += new System.EventHandler(this.labTip_DoubleClick);
			// 
			// progress1
			// 
			this.progress1.Name = "progress1";
			this.progress1.Size = new System.Drawing.Size(100, 16);
			// 
			// webKitBrowser1
			// 
			this.webKitBrowser1.AllowDrop = true;
			this.webKitBrowser1.BackColor = System.Drawing.Color.White;
			this.webKitBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webKitBrowser1.Location = new System.Drawing.Point(0, 25);
			this.webKitBrowser1.Name = "webKitBrowser1";
			this.webKitBrowser1.Password = null;
			this.webKitBrowser1.PrivateBrowsing = false;
			this.webKitBrowser1.Size = new System.Drawing.Size(573, 389);
			this.webKitBrowser1.TabIndex = 2;
			this.webKitBrowser1.Url = null;
			this.webKitBrowser1.Username = null;
			this.webKitBrowser1.ProgressChanged += new WebKit.ProgressChangedEventHandler(this.webKitBrowser1_ProgressChanged);
			this.webKitBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webKitBrowser1_DocumentCompleted);
			this.webKitBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webKitBrowser1_Navigated);
			this.webKitBrowser1.Navigating += new WebKit.WebKitBrowserNavigatingEventHandler(this.webKitBrowser1_Navigating);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(573, 436);
			this.Controls.Add(this.webKitBrowser1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormMain";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.Shown += new System.EventHandler(this.FormMain_Shown);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel labTip;
		private System.Windows.Forms.ToolStripProgressBar progress1;
		private System.Windows.Forms.ToolStripProgressBar progress2;
		internal WebKit.WebKitBrowser webKitBrowser1;
	}
}