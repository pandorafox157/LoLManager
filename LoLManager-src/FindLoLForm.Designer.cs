namespace LoLManager
{
    partial class FindLoLForm
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
            if (disposing && (components != null))
            {
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
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.PathListBox = new System.Windows.Forms.ListBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.AddPathButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PathListBox
            // 
            this.PathListBox.FormattingEnabled = true;
            this.PathListBox.ItemHeight = 12;
            this.PathListBox.Location = new System.Drawing.Point(12, 12);
            this.PathListBox.Name = "PathListBox";
            this.PathListBox.Size = new System.Drawing.Size(479, 88);
            this.PathListBox.TabIndex = 0;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(498, 13);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "選擇路徑";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // AddPathButton
            // 
            this.AddPathButton.Location = new System.Drawing.Point(498, 76);
            this.AddPathButton.Name = "AddPathButton";
            this.AddPathButton.Size = new System.Drawing.Size(75, 23);
            this.AddPathButton.TabIndex = 2;
            this.AddPathButton.Text = "加入路徑";
            this.AddPathButton.UseVisualStyleBackColor = true;
            // 
            // FindLoLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 112);
            this.Controls.Add(this.AddPathButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.PathListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindLoLForm";
            this.Text = "設定起始資料夾";
            this.Load += new System.EventHandler(this.FindLoLForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.ListBox PathListBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button AddPathButton;
    }
}