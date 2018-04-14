namespace Snipe
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.button2 = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txt_url = new System.Windows.Forms.TextBox();
			this.txt_pageStart = new System.Windows.Forms.TextBox();
			this.txt_eleContainer = new System.Windows.Forms.TextBox();
			this.txt_eleText = new System.Windows.Forms.TextBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.txt_msInterval = new System.Windows.Forms.TextBox();
			this.txt_eleItor = new System.Windows.Forms.TextBox();
			this.txt_eleURL = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.txt_pageEnd = new System.Windows.Forms.TextBox();
			this.txt_attrURL = new System.Windows.Forms.TextBox();
			this.txt_attrText = new System.Windows.Forms.TextBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.progressBar2 = new System.Windows.Forms.ProgressBar();
			this.txt_log = new System.Windows.Forms.TextBox();
			this.lab1 = new System.Windows.Forms.Label();
			this.lab2 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(871, 820);
			this.tabControl1.TabIndex = 12;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.lab2);
			this.tabPage1.Controls.Add(this.lab1);
			this.tabPage1.Controls.Add(this.txt_log);
			this.tabPage1.Controls.Add(this.progressBar2);
			this.tabPage1.Controls.Add(this.progressBar1);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(863, 794);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(68, 74);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 0;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.splitContainer1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(863, 794);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Panel2.Controls.Add(this.textBox1);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.txt_url);
			this.splitContainer1.Panel2.Controls.Add(this.txt_pageStart);
			this.splitContainer1.Panel2.Controls.Add(this.txt_eleContainer);
			this.splitContainer1.Panel2.Controls.Add(this.txt_eleText);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Panel2.Controls.Add(this.txt_msInterval);
			this.splitContainer1.Panel2.Controls.Add(this.txt_eleItor);
			this.splitContainer1.Panel2.Controls.Add(this.txt_eleURL);
			this.splitContainer1.Panel2.Controls.Add(this.button1);
			this.splitContainer1.Panel2.Controls.Add(this.txt_pageEnd);
			this.splitContainer1.Panel2.Controls.Add(this.txt_attrURL);
			this.splitContainer1.Panel2.Controls.Add(this.txt_attrText);
			this.splitContainer1.Size = new System.Drawing.Size(857, 788);
			this.splitContainer1.SplitterDistance = 224;
			this.splitContainer1.TabIndex = 15;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(224, 788);
			this.treeView1.TabIndex = 13;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(21, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(23, 12);
			this.label2.TabIndex = 16;
			this.label2.Text = "URL";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(50, 133);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(340, 21);
			this.textBox1.TabIndex = 15;
			this.textBox1.Text = "http://1024.917rbb.pw/pw/thread.php?fid=22";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(21, 163);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 12);
			this.label1.TabIndex = 14;
			this.label1.Text = "URL";
			// 
			// txt_url
			// 
			this.txt_url.Location = new System.Drawing.Point(50, 160);
			this.txt_url.Name = "txt_url";
			this.txt_url.Size = new System.Drawing.Size(340, 21);
			this.txt_url.TabIndex = 13;
			this.txt_url.Text = "http://1024.917rbb.pw/pw/thread.php?fid=22";
			// 
			// txt_pageStart
			// 
			this.txt_pageStart.Location = new System.Drawing.Point(22, 228);
			this.txt_pageStart.Name = "txt_pageStart";
			this.txt_pageStart.Size = new System.Drawing.Size(100, 21);
			this.txt_pageStart.TabIndex = 3;
			this.txt_pageStart.Text = "1";
			// 
			// txt_eleContainer
			// 
			this.txt_eleContainer.Location = new System.Drawing.Point(22, 317);
			this.txt_eleContainer.Name = "txt_eleContainer";
			this.txt_eleContainer.Size = new System.Drawing.Size(259, 21);
			this.txt_eleContainer.TabIndex = 6;
			this.txt_eleContainer.Text = "table:#ajaxtable/tbody:@1";
			// 
			// txt_eleText
			// 
			this.txt_eleText.Location = new System.Drawing.Point(22, 344);
			this.txt_eleText.Name = "txt_eleText";
			this.txt_eleText.Size = new System.Drawing.Size(259, 21);
			this.txt_eleText.TabIndex = 7;
			this.txt_eleText.Text = "td:@1/h3/a";
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.listView1.FullRowSelect = true;
			this.listView1.Location = new System.Drawing.Point(22, 398);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(429, 171);
			this.listView1.TabIndex = 2;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "No";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "text";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "url";
			// 
			// txt_msInterval
			// 
			this.txt_msInterval.Location = new System.Drawing.Point(23, 255);
			this.txt_msInterval.Name = "txt_msInterval";
			this.txt_msInterval.Size = new System.Drawing.Size(100, 21);
			this.txt_msInterval.TabIndex = 5;
			this.txt_msInterval.Text = "1000";
			// 
			// txt_eleItor
			// 
			this.txt_eleItor.Location = new System.Drawing.Point(287, 317);
			this.txt_eleItor.Name = "txt_eleItor";
			this.txt_eleItor.Size = new System.Drawing.Size(100, 21);
			this.txt_eleItor.TabIndex = 11;
			this.txt_eleItor.Text = "tr";
			// 
			// txt_eleURL
			// 
			this.txt_eleURL.Location = new System.Drawing.Point(22, 371);
			this.txt_eleURL.Name = "txt_eleURL";
			this.txt_eleURL.Size = new System.Drawing.Size(259, 21);
			this.txt_eleURL.TabIndex = 8;
			this.txt_eleURL.Text = "td:@1/h3/a";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(234, 226);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// txt_pageEnd
			// 
			this.txt_pageEnd.Location = new System.Drawing.Point(128, 228);
			this.txt_pageEnd.Name = "txt_pageEnd";
			this.txt_pageEnd.Size = new System.Drawing.Size(100, 21);
			this.txt_pageEnd.TabIndex = 4;
			this.txt_pageEnd.Text = "1";
			// 
			// txt_attrURL
			// 
			this.txt_attrURL.Location = new System.Drawing.Point(287, 371);
			this.txt_attrURL.Name = "txt_attrURL";
			this.txt_attrURL.Size = new System.Drawing.Size(100, 21);
			this.txt_attrURL.TabIndex = 10;
			this.txt_attrURL.Text = "href";
			// 
			// txt_attrText
			// 
			this.txt_attrText.Location = new System.Drawing.Point(287, 344);
			this.txt_attrText.Name = "txt_attrText";
			this.txt_attrText.Size = new System.Drawing.Size(100, 21);
			this.txt_attrText.TabIndex = 9;
			this.txt_attrText.Text = "innerText";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(68, 115);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(697, 12);
			this.progressBar1.TabIndex = 1;
			// 
			// progressBar2
			// 
			this.progressBar2.Location = new System.Drawing.Point(68, 145);
			this.progressBar2.Name = "progressBar2";
			this.progressBar2.Size = new System.Drawing.Size(697, 12);
			this.progressBar2.TabIndex = 2;
			// 
			// txt_log
			// 
			this.txt_log.Location = new System.Drawing.Point(68, 163);
			this.txt_log.Multiline = true;
			this.txt_log.Name = "txt_log";
			this.txt_log.Size = new System.Drawing.Size(697, 451);
			this.txt_log.TabIndex = 3;
			// 
			// lab1
			// 
			this.lab1.AutoSize = true;
			this.lab1.Location = new System.Drawing.Point(66, 100);
			this.lab1.Name = "lab1";
			this.lab1.Size = new System.Drawing.Size(41, 12);
			this.lab1.TabIndex = 4;
			this.lab1.Text = "label3";
			// 
			// lab2
			// 
			this.lab2.AutoSize = true;
			this.lab2.Location = new System.Drawing.Point(66, 130);
			this.lab2.Name = "lab2";
			this.lab2.Size = new System.Drawing.Size(41, 12);
			this.lab2.TabIndex = 5;
			this.lab2.Text = "label4";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(871, 820);
			this.Controls.Add(this.tabControl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txt_url;
		private System.Windows.Forms.TextBox txt_pageStart;
		private System.Windows.Forms.TextBox txt_eleContainer;
		private System.Windows.Forms.TextBox txt_eleText;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.TextBox txt_msInterval;
		private System.Windows.Forms.TextBox txt_eleItor;
		private System.Windows.Forms.TextBox txt_eleURL;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txt_pageEnd;
		private System.Windows.Forms.TextBox txt_attrURL;
		private System.Windows.Forms.TextBox txt_attrText;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox txt_log;
		private System.Windows.Forms.ProgressBar progressBar2;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lab2;
		private System.Windows.Forms.Label lab1;
	}
}

