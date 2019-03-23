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
            this.dgvFoods = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvNutrients = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctmReadOnlyTextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctmDgvCopyMenu = new Omlenet.DataGridViewContextMenu(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoods)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNutrients)).BeginInit();
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
            this.tabControl1.Controls.Add(this.tabPage3);
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
            this.tabPage2.Controls.Add(this.dgvFoods);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(193, 358);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Food List";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvFoods
            // 
            this.dgvFoods.AllowUserToAddRows = false;
            this.dgvFoods.AllowUserToDeleteRows = false;
            this.dgvFoods.AllowUserToResizeColumns = false;
            this.dgvFoods.AllowUserToResizeRows = false;
            this.dgvFoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFoods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgvFoods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFoods.Location = new System.Drawing.Point(3, 3);
            this.dgvFoods.Name = "dgvFoods";
            this.dgvFoods.ReadOnly = true;
            this.dgvFoods.RowHeadersVisible = false;
            this.dgvFoods.Size = new System.Drawing.Size(187, 352);
            this.dgvFoods.TabIndex = 15;
            this.dgvFoods.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFoods_CellMouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 175;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Mass";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Cost";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 55;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvNutrients);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(193, 358);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Nutrient List";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvNutrients
            // 
            this.dgvNutrients.AllowUserToAddRows = false;
            this.dgvNutrients.AllowUserToDeleteRows = false;
            this.dgvNutrients.AllowUserToResizeColumns = false;
            this.dgvNutrients.AllowUserToResizeRows = false;
            this.dgvNutrients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNutrients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NName,
            this.Mass,
            this.Cost});
            this.dgvNutrients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNutrients.Location = new System.Drawing.Point(0, 0);
            this.dgvNutrients.Name = "dgvNutrients";
            this.dgvNutrients.ReadOnly = true;
            this.dgvNutrients.RowHeadersVisible = false;
            this.dgvNutrients.Size = new System.Drawing.Size(193, 358);
            this.dgvNutrients.TabIndex = 14;
            this.dgvNutrients.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvNutrients_CellMouseClick);
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
            this.Cost.Width = 55;
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
            // ctmDgvCopyMenu
            // 
            this.ctmDgvCopyMenu.Name = "ctmReadOnlyTextMenu";
            this.ctmDgvCopyMenu.Size = new System.Drawing.Size(138, 92);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(4, 2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(91, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // ResultsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(201, 415);
            this.Controls.Add(this.btnReset);
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvFoods)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNutrients)).EndInit();
            this.ctmReadOnlyTextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox plainTextResults;
        private System.Windows.Forms.Button btnBeginSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ContextMenuStrip ctmReadOnlyTextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvNutrients;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cost;
        private DataGridViewContextMenu ctmDgvCopyMenu;
        private System.Windows.Forms.DataGridView dgvFoods;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button btnReset;
    }
}