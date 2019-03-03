namespace Omlenet
{
    partial class ResultsPanel
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
            this.debugResult = new System.Windows.Forms.TextBox();
            this.btnBeginSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // debugResult
            // 
            this.debugResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.debugResult.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.debugResult.Location = new System.Drawing.Point(0, 35);
            this.debugResult.Multiline = true;
            this.debugResult.Name = "debugResult";
            this.debugResult.Size = new System.Drawing.Size(201, 380);
            this.debugResult.TabIndex = 7;
            // 
            // btnBeginSearch
            // 
            this.btnBeginSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBeginSearch.Location = new System.Drawing.Point(101, 2);
            this.btnBeginSearch.Name = "btnBeginSearch";
            this.btnBeginSearch.Size = new System.Drawing.Size(100, 23);
            this.btnBeginSearch.TabIndex = 6;
            this.btnBeginSearch.Text = "Begin Search";
            this.btnBeginSearch.UseVisualStyleBackColor = true;
            this.btnBeginSearch.Click += new System.EventHandler(this.btnBeginSearch_Click);
            // 
            // ResultsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 415);
            this.Controls.Add(this.debugResult);
            this.Controls.Add(this.btnBeginSearch);
            this.Name = "ResultsPanel";
            this.Text = "Results";
            this.Resize += new System.EventHandler(this.ResultsPanel_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox debugResult;
        private System.Windows.Forms.Button btnBeginSearch;
    }
}