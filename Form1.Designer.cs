namespace Omlenet
{
    partial class Form1
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tmrInitializer = new System.Windows.Forms.Timer(this.components);
            this.btnBeginSearch = new System.Windows.Forms.Button();
            this.tmrWaiter = new System.Windows.Forms.Timer(this.components);
            this.debugResult = new System.Windows.Forms.TextBox();
            this.cklFoods = new System.Windows.Forms.CheckedListBox();
            this.pnlFoodDetail = new System.Windows.Forms.Panel();
            this.lblFoodDetail = new System.Windows.Forms.Label();
            this.cboBodyType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCheckShown = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tmrFilter = new System.Windows.Forms.Timer(this.components);
            this.btnRareNutrients = new System.Windows.Forms.Button();
            this.btnTargets = new System.Windows.Forms.Button();
            this.pnlUserControls = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ctmReadOnlyTextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cboFilterByGroup = new System.Windows.Forms.ComboBox();
            this.pnlFoodDetail.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlUserControls.SuspendLayout();
            this.ctmReadOnlyTextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 427);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(800, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // tmrInitializer
            // 
            this.tmrInitializer.Enabled = true;
            this.tmrInitializer.Interval = 10;
            this.tmrInitializer.Tick += new System.EventHandler(this.tmrInitializer_Tick);
            // 
            // btnBeginSearch
            // 
            this.btnBeginSearch.Location = new System.Drawing.Point(682, 12);
            this.btnBeginSearch.Name = "btnBeginSearch";
            this.btnBeginSearch.Size = new System.Drawing.Size(106, 23);
            this.btnBeginSearch.TabIndex = 1;
            this.btnBeginSearch.Text = "Begin Search";
            this.btnBeginSearch.UseVisualStyleBackColor = true;
            this.btnBeginSearch.Click += new System.EventHandler(this.btnBeginSearch_Click);
            // 
            // tmrWaiter
            // 
            this.tmrWaiter.Interval = 50;
            this.tmrWaiter.Tick += new System.EventHandler(this.tmrWaiter_Tick);
            // 
            // debugResult
            // 
            this.debugResult.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.debugResult.Location = new System.Drawing.Point(591, 41);
            this.debugResult.Multiline = true;
            this.debugResult.Name = "debugResult";
            this.debugResult.Size = new System.Drawing.Size(197, 380);
            this.debugResult.TabIndex = 5;
            // 
            // cklFoods
            // 
            this.cklFoods.Dock = System.Windows.Forms.DockStyle.Top;
            this.cklFoods.FormattingEnabled = true;
            this.cklFoods.Location = new System.Drawing.Point(3, 16);
            this.cklFoods.Name = "cklFoods";
            this.cklFoods.Size = new System.Drawing.Size(268, 304);
            this.cklFoods.TabIndex = 6;
            this.cklFoods.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cklFoods_ItemCheck);
            this.cklFoods.SelectedIndexChanged += new System.EventHandler(this.cklFoods_SelectedIndexChanged);
            // 
            // pnlFoodDetail
            // 
            this.pnlFoodDetail.AutoScroll = true;
            this.pnlFoodDetail.Controls.Add(this.lblFoodDetail);
            this.pnlFoodDetail.Location = new System.Drawing.Point(430, 12);
            this.pnlFoodDetail.Name = "pnlFoodDetail";
            this.pnlFoodDetail.Size = new System.Drawing.Size(155, 406);
            this.pnlFoodDetail.TabIndex = 9;
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
            // cboBodyType
            // 
            this.cboBodyType.FormattingEnabled = true;
            this.cboBodyType.Location = new System.Drawing.Point(12, 25);
            this.cboBodyType.Name = "cboBodyType";
            this.cboBodyType.Size = new System.Drawing.Size(132, 21);
            this.cboBodyType.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Demographic:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.cklFoods);
            this.groupBox1.Location = new System.Drawing.Point(150, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 409);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Acceptable food items";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboFilterByGroup);
            this.panel1.Controls.Add(this.btnCheckShown);
            this.panel1.Controls.Add(this.txtFilter);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(4, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 77);
            this.panel1.TabIndex = 14;
            // 
            // btnCheckShown
            // 
            this.btnCheckShown.Location = new System.Drawing.Point(3, 3);
            this.btnCheckShown.Name = "btnCheckShown";
            this.btnCheckShown.Size = new System.Drawing.Size(261, 23);
            this.btnCheckShown.TabIndex = 9;
            this.btnCheckShown.Text = "Check/uncheck all shown";
            this.btnCheckShown.UseVisualStyleBackColor = true;
            this.btnCheckShown.Click += new System.EventHandler(this.btnCheckShown_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(64, 31);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(200, 20);
            this.txtFilter.TabIndex = 8;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "View filter:";
            // 
            // tmrFilter
            // 
            this.tmrFilter.Interval = 20;
            this.tmrFilter.Tick += new System.EventHandler(this.tmrFilter_Tick);
            // 
            // btnRareNutrients
            // 
            this.btnRareNutrients.Location = new System.Drawing.Point(591, 12);
            this.btnRareNutrients.Name = "btnRareNutrients";
            this.btnRareNutrients.Size = new System.Drawing.Size(85, 23);
            this.btnRareNutrients.TabIndex = 1;
            this.btnRareNutrients.Text = "Rare nutrients";
            this.btnRareNutrients.UseVisualStyleBackColor = true;
            this.btnRareNutrients.Click += new System.EventHandler(this.btnRareNutrients_Click);
            // 
            // btnTargets
            // 
            this.btnTargets.Location = new System.Drawing.Point(12, 52);
            this.btnTargets.Name = "btnTargets";
            this.btnTargets.Size = new System.Drawing.Size(132, 23);
            this.btnTargets.TabIndex = 14;
            this.btnTargets.Text = "Set target overrides";
            this.btnTargets.UseVisualStyleBackColor = true;
            this.btnTargets.Click += new System.EventHandler(this.btnTargets_Click);
            // 
            // pnlUserControls
            // 
            this.pnlUserControls.Controls.Add(this.btnLoad);
            this.pnlUserControls.Controls.Add(this.btnSave);
            this.pnlUserControls.Controls.Add(this.btnRareNutrients);
            this.pnlUserControls.Controls.Add(this.btnTargets);
            this.pnlUserControls.Controls.Add(this.groupBox1);
            this.pnlUserControls.Controls.Add(this.pnlFoodDetail);
            this.pnlUserControls.Controls.Add(this.cboBodyType);
            this.pnlUserControls.Controls.Add(this.debugResult);
            this.pnlUserControls.Controls.Add(this.label3);
            this.pnlUserControls.Controls.Add(this.btnBeginSearch);
            this.pnlUserControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserControls.Enabled = false;
            this.pnlUserControls.Location = new System.Drawing.Point(0, 0);
            this.pnlUserControls.Name = "pnlUserControls";
            this.pnlUserControls.Size = new System.Drawing.Size(800, 450);
            this.pnlUserControls.TabIndex = 15;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 145);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(132, 23);
            this.btnLoad.TabIndex = 17;
            this.btnLoad.Text = "Load state";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 116);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save state";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Omlenet files (*.omn)|*.omn|All files(*.*)|*.*";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "omn";
            this.openFileDialog1.Filter = "Omlenet files (*.omn)|*.omn|All files(*.*)|*.*";
            // 
            // cboFilterByGroup
            // 
            this.cboFilterByGroup.FormattingEnabled = true;
            this.cboFilterByGroup.Location = new System.Drawing.Point(64, 53);
            this.cboFilterByGroup.Name = "cboFilterByGroup";
            this.cboFilterByGroup.Size = new System.Drawing.Size(198, 21);
            this.cboFilterByGroup.TabIndex = 10;
            this.cboFilterByGroup.SelectedIndexChanged += new System.EventHandler(this.cboFilterByGroup_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pnlUserControls);
            this.Name = "Form1";
            this.Text = "Omlenet";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.pnlFoodDetail.ResumeLayout(false);
            this.pnlFoodDetail.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlUserControls.ResumeLayout(false);
            this.pnlUserControls.PerformLayout();
            this.ctmReadOnlyTextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer tmrInitializer;
        private System.Windows.Forms.Button btnBeginSearch;
        private System.Windows.Forms.Timer tmrWaiter;
        private System.Windows.Forms.TextBox debugResult;
        private System.Windows.Forms.CheckedListBox cklFoods;
        private System.Windows.Forms.Panel pnlFoodDetail;
        private System.Windows.Forms.Label lblFoodDetail;
        private System.Windows.Forms.ComboBox cboBodyType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCheckShown;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer tmrFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRareNutrients;
        private System.Windows.Forms.Button btnTargets;
        private System.Windows.Forms.Panel pnlUserControls;
        private System.Windows.Forms.ContextMenuStrip ctmReadOnlyTextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboFilterByGroup;
    }
}

