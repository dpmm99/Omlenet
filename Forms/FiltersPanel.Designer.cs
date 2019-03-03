namespace Omlenet
{
    partial class FiltersPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiltersPanel));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboFilterByGroup = new System.Windows.Forms.ComboBox();
            this.btnCheckShown = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cklFoods = new System.Windows.Forms.CheckedListBox();
            this.tmrFilter = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.cklFoods);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 406);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Acceptable food items";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboFilterByGroup);
            this.panel1.Controls.Add(this.btnCheckShown);
            this.panel1.Controls.Add(this.txtFilter);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 77);
            this.panel1.TabIndex = 14;
            // 
            // cboFilterByGroup
            // 
            this.cboFilterByGroup.FormattingEnabled = true;
            this.cboFilterByGroup.Location = new System.Drawing.Point(64, 53);
            this.cboFilterByGroup.Name = "cboFilterByGroup";
            this.cboFilterByGroup.Size = new System.Drawing.Size(200, 21);
            this.cboFilterByGroup.TabIndex = 10;
            this.cboFilterByGroup.SelectedIndexChanged += new System.EventHandler(this.cboFilterByGroup_SelectedIndexChanged);
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
            this.toolTip1.SetToolTip(this.txtFilter, resources.GetString("txtFilter.ToolTip"));
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
            // tmrFilter
            // 
            this.tmrFilter.Interval = 20;
            this.tmrFilter.Tick += new System.EventHandler(this.tmrFilter_Tick);
            // 
            // FiltersPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 406);
            this.Controls.Add(this.groupBox1);
            this.Name = "FiltersPanel";
            this.Text = "Foods";
            this.Resize += new System.EventHandler(this.FiltersPanel_Resize);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboFilterByGroup;
        private System.Windows.Forms.Button btnCheckShown;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox cklFoods;
        private System.Windows.Forms.Timer tmrFilter;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}