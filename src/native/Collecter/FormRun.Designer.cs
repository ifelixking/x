namespace Collecter
{
	partial class FormRun
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRun));
			this.webKitBrowser1 = new WebKit.WebKitBrowser();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnReload = new System.Windows.Forms.ToolStripButton();
			this.btnNavTo = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.prograss = new System.Windows.Forms.ToolStripProgressBar();
			this.labTip = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
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
			this.webKitBrowser1.Size = new System.Drawing.Size(624, 395);
			this.webKitBrowser1.TabIndex = 0;
			this.webKitBrowser1.Url = null;
			this.webKitBrowser1.Username = null;
			this.webKitBrowser1.ProgressChanged += new WebKit.ProgressChangedEventHandler(this.webKitBrowser1_ProgressChanged);
			this.webKitBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webKitBrowser1_DocumentCompleted);
			this.webKitBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webKitBrowser1_Navigated);
			this.webKitBrowser1.Navigating += new WebKit.WebKitBrowserNavigatingEventHandler(this.webKitBrowser1_Navigating);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnReload,
            this.btnNavTo});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(624, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnReload
			// 
			this.btnReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnReload.Image = ((System.Drawing.Image)(resources.GetObject("btnReload.Image")));
			this.btnReload.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnReload.Name = "btnReload";
			this.btnReload.Size = new System.Drawing.Size(23, 22);
			this.btnReload.Text = "toolStripButton1";
			this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
			// 
			// btnNavTo
			// 
			this.btnNavTo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNavTo.Image = ((System.Drawing.Image)(resources.GetObject("btnNavTo.Image")));
			this.btnNavTo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNavTo.Name = "btnNavTo";
			this.btnNavTo.Size = new System.Drawing.Size(23, 22);
			this.btnNavTo.Text = "toolStripButton1";
			this.btnNavTo.Click += new System.EventHandler(this.btnNavTo_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prograss,
            this.labTip});
			this.statusStrip1.Location = new System.Drawing.Point(0, 420);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(624, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// prograss
			// 
			this.prograss.Name = "prograss";
			this.prograss.Size = new System.Drawing.Size(100, 16);
			this.prograss.Visible = false;
			// 
			// labTip
			// 
			this.labTip.Name = "labTip";
			this.labTip.Size = new System.Drawing.Size(77, 17);
			this.labTip.Text = "about:blank";
			// 
			// FormRun
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 442);
			this.Controls.Add(this.webKitBrowser1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "FormRun";
			this.Text = "FormRun";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private WebKit.WebKitBrowser webKitBrowser1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnReload;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripProgressBar prograss;
		private System.Windows.Forms.ToolStripStatusLabel labTip;
		private System.Windows.Forms.ToolStripButton btnNavTo;
	}
}