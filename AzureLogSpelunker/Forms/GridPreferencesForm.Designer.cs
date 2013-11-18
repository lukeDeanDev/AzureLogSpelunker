namespace AzureLogSpelunker.Forms
{
    partial class GridPreferencesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridPreferencesForm));
            this.label1 = new System.Windows.Forms.Label();
            this.columnLayout = new System.Windows.Forms.CheckedListBox();
            this.btDown = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.cbWordwrap = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Column layout";
            // 
            // columnLayout
            // 
            this.columnLayout.FormattingEnabled = true;
            this.columnLayout.Location = new System.Drawing.Point(12, 25);
            this.columnLayout.Name = "columnLayout";
            this.columnLayout.Size = new System.Drawing.Size(160, 259);
            this.columnLayout.TabIndex = 1;
            // 
            // btDown
            // 
            this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
            this.btDown.Location = new System.Drawing.Point(178, 57);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(26, 26);
            this.btDown.TabIndex = 22;
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // btUp
            // 
            this.btUp.Image = ((System.Drawing.Image)(resources.GetObject("btUp.Image")));
            this.btUp.Location = new System.Drawing.Point(178, 25);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(26, 26);
            this.btUp.TabIndex = 21;
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // cbWordwrap
            // 
            this.cbWordwrap.AutoSize = true;
            this.cbWordwrap.Location = new System.Drawing.Point(257, 25);
            this.cbWordwrap.Name = "cbWordwrap";
            this.cbWordwrap.Size = new System.Drawing.Size(75, 17);
            this.cbWordwrap.TabIndex = 23;
            this.cbWordwrap.Text = "Wordwrap";
            this.cbWordwrap.UseVisualStyleBackColor = true;
            // 
            // GridPreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 311);
            this.Controls.Add(this.cbWordwrap);
            this.Controls.Add(this.btDown);
            this.Controls.Add(this.btUp);
            this.Controls.Add(this.columnLayout);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridPreferencesForm";
            this.Text = "Grid Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GridPreferencesForm_FormClosing);
            this.Load += new System.EventHandler(this.GridPreferencesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox columnLayout;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.CheckBox cbWordwrap;
    }
}