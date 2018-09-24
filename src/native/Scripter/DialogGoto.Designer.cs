namespace Scripter
{
	partial class DialogGoto
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
			this.btn_ok = new System.Windows.Forms.Button();
			this.btn_cancel = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btn_ok
			// 
			this.btn_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_ok.Location = new System.Drawing.Point(455, 41);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new System.Drawing.Size(75, 23);
			this.btn_ok.TabIndex = 1;
			this.btn_ok.Text = "OK";
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
			// 
			// btn_cancel
			// 
			this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_cancel.Location = new System.Drawing.Point(536, 41);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_cancel.TabIndex = 2;
			this.btn_cancel.Text = "Cancel";
			this.btn_cancel.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.FormattingEnabled = true;
			this.textBox1.Location = new System.Drawing.Point(12, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(599, 20);
			this.textBox1.TabIndex = 0;
			// 
			// DialogGoto
			// 
			this.AcceptButton = this.btn_ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btn_cancel;
			this.ClientSize = new System.Drawing.Size(623, 76);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.btn_cancel);
			this.Controls.Add(this.btn_ok);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogGoto";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DialogGoto";
			this.Load += new System.EventHandler(this.DialogGoto_Load);
			this.Shown += new System.EventHandler(this.DialogGoto_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btn_ok;
		private System.Windows.Forms.Button btn_cancel;
		private System.Windows.Forms.ComboBox textBox1;
	}
}