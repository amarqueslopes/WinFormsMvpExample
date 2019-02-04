namespace MVP.Source.Forms
{
    partial class SearchView
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
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.statusMessageLabel = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchForLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(39, 62);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(187, 20);
            this.searchTextBox.TabIndex = 0;
            // 
            // statusMessageLabel
            // 
            this.statusMessageLabel.AutoSize = true;
            this.statusMessageLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.statusMessageLabel.Location = new System.Drawing.Point(36, 184);
            this.statusMessageLabel.Name = "statusMessageLabel";
            this.statusMessageLabel.Size = new System.Drawing.Size(0, 13);
            this.statusMessageLabel.TabIndex = 1;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(104, 101);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "searchButton";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // searchForLabel
            // 
            this.searchForLabel.AutoSize = true;
            this.searchForLabel.Location = new System.Drawing.Point(39, 43);
            this.searchForLabel.Name = "searchForLabel";
            this.searchForLabel.Size = new System.Drawing.Size(80, 13);
            this.searchForLabel.TabIndex = 3;
            this.searchForLabel.Text = "searchForLabel";
            // 
            // SearchView
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.searchForLabel);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.statusMessageLabel);
            this.Controls.Add(this.searchTextBox);
            this.Name = "SearchView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchView_FormClosing);
            this.Load += new System.EventHandler(this.SearchView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label statusMessageLabel;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label searchForLabel;
    }
}