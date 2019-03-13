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
            this.nudUnitsInPlan = new System.Windows.Forms.NumericUpDown();
            this.chkLock = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFoodDetail.SuspendLayout();
            this.ctmReadOnlyTextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUnitsInPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFoodDetail
            // 
            this.pnlFoodDetail.AutoScroll = true;
            this.pnlFoodDetail.Controls.Add(this.label1);
            this.pnlFoodDetail.Controls.Add(this.dataGridView1);
            this.pnlFoodDetail.Controls.Add(this.chkLock);
            this.pnlFoodDetail.Controls.Add(this.nudUnitsInPlan);
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
            // nudUnitsInPlan
            // 
            this.nudUnitsInPlan.Location = new System.Drawing.Point(126, 302);
            this.nudUnitsInPlan.Name = "nudUnitsInPlan";
            this.nudUnitsInPlan.Size = new System.Drawing.Size(60, 20);
            this.nudUnitsInPlan.TabIndex = 11;
            this.nudUnitsInPlan.Validating += new System.ComponentModel.CancelEventHandler(this.nudUnitsInPlan_Validating);
            // 
            // chkLock
            // 
            this.chkLock.AutoSize = true;
            this.chkLock.Location = new System.Drawing.Point(192, 305);
            this.chkLock.Name = "chkLock";
            this.chkLock.Size = new System.Drawing.Size(50, 17);
            this.chkLock.TabIndex = 12;
            this.chkLock.Text = "Lock";
            this.chkLock.UseVisualStyleBackColor = true;
            this.chkLock.Click += new System.EventHandler(this.chkLock_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NName,
            this.Mass,
            this.Cost});
            this.dataGridView1.Location = new System.Drawing.Point(3, 94);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(333, 202);
            this.dataGridView1.TabIndex = 13;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 50;
            // 
            // NName
            // 
            this.NName.HeaderText = "Name";
            this.NName.Name = "NName";
            this.NName.ReadOnly = true;
            this.NName.Width = 175;
            // 
            // Mass
            // 
            this.Mass.HeaderText = "Mass";
            this.Mass.Name = "Mass";
            this.Mass.ReadOnly = true;
            this.Mass.Width = 60;
            // 
            // Cost
            // 
            this.Cost.HeaderText = "Cost";
            this.Cost.Name = "Cost";
            this.Cost.ReadOnly = true;
            this.Cost.Width = 45;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 306);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Amount in plan (x100g):";
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
            ((System.ComponentModel.ISupportInitialize)(this.nudUnitsInPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFoodDetail;
        private System.Windows.Forms.Label lblFoodDetail;
        private System.Windows.Forms.ContextMenuStrip ctmReadOnlyTextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox chkLock;
        private System.Windows.Forms.NumericUpDown nudUnitsInPlan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
    }
}