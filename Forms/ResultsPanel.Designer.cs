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
            this.components = new System.ComponentModel.Container();
            this.plainTextResults = new System.Windows.Forms.TextBox();
            this.btnBeginSearch = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.ctmReadOnlyTextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.ctmReadOnlyTextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // plainTextResults
            // 
            this.plainTextResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plainTextResults.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plainTextResults.Location = new System.Drawing.Point(3, 3);
            this.plainTextResults.Multiline = true;
            this.plainTextResults.Name = "plainTextResults";
            this.plainTextResults.Size = new System.Drawing.Size(187, 352);
            this.plainTextResults.TabIndex = 7;
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(201, 384);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.plainTextResults);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(193, 358);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Plain Text";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstResults);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(193, 358);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Food List";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstResults
            // 
            this.lstResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(3, 3);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(187, 352);
            this.lstResults.TabIndex = 0;
            this.lstResults.SelectedIndexChanged += new System.EventHandler(this.lstResults_SelectedIndexChanged);
            this.lstResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstResults_MouseDown);
            // 
            // ctmReadOnlyTextMenu
            // 
            this.ctmReadOnlyTextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.ctmReadOnlyTextMenu.Name = "ctmReadOnlyTextMenu";
            this.ctmReadOnlyTextMenu.Size = new System.Drawing.Size(181, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // ResultsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 415);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnBeginSearch);
            this.Name = "ResultsPanel";
            this.Text = "Results";
            this.Load += new System.EventHandler(this.ResultsPanel_Load);
            this.Resize += new System.EventHandler(this.ResultsPanel_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ctmReadOnlyTextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox plainTextResults;
        private System.Windows.Forms.Button btnBeginSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.ContextMenuStrip ctmReadOnlyTextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}