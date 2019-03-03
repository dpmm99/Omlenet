namespace Omlenet
{
    partial class DetailsPanel
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
            this.pnlFoodDetail = new System.Windows.Forms.Panel();
            this.lblFoodDetail = new System.Windows.Forms.Label();
            this.ctmReadOnlyTextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFoodDetail.SuspendLayout();
            this.ctmReadOnlyTextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFoodDetail
            // 
            this.pnlFoodDetail.AutoScroll = true;
            this.pnlFoodDetail.Controls.Add(this.lblFoodDetail);
            this.pnlFoodDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFoodDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlFoodDetail.Name = "pnlFoodDetail";
            this.pnlFoodDetail.Size = new System.Drawing.Size(190, 341);
            this.pnlFoodDetail.TabIndex = 10;
            // 
            // lblFoodDetail
            // 
            this.lblFoodDetail.AutoSize = true;
            this.lblFoodDetail.Location = new System.Drawing.Point(12, 10);
            this.lblFoodDetail.Name = "lblFoodDetail";
            this.lblFoodDetail.Size = new System.Drawing.Size(67, 13);
            this.lblFoodDetail.TabIndex = 0;
            this.lblFoodDetail.Text = "Food details:";
            this.lblFoodDetail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblFoodDetail_MouseDown);
            // 
            // ctmReadOnlyTextMenu
            // 
            this.ctmReadOnlyTextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.ctmReadOnlyTextMenu.Name = "ctmReadOnlyTextMenu";
            this.ctmReadOnlyTextMenu.Size = new System.Drawing.Size(100, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.copyToolStripMenuItem.Text = "&Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // DetailsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 341);
            this.Controls.Add(this.pnlFoodDetail);
            this.Name = "DetailsPanel";
            this.Text = "Information";
            this.Load += new System.EventHandler(this.DetailsPanel_Load);
            this.pnlFoodDetail.ResumeLayout(false);
            this.pnlFoodDetail.PerformLayout();
            this.ctmReadOnlyTextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFoodDetail;
        private System.Windows.Forms.Label lblFoodDetail;
        private System.Windows.Forms.ContextMenuStrip ctmReadOnlyTextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}