namespace Omlenet
{
    partial class TargetsPanel
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
            this.cboBodyType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlInputs = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.picCost = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudCostOver = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudCostUnder = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudTarget = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.chkOverride = new System.Windows.Forms.CheckBox();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.lstNutrients = new System.Windows.Forms.ListBox();
            this.nudFoodMass = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCostOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCostUnder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFoodMass)).BeginInit();
            this.SuspendLayout();
            // 
            // cboBodyType
            // 
            this.cboBodyType.FormattingEnabled = true;
            this.cboBodyType.Location = new System.Drawing.Point(3, 18);
            this.cboBodyType.Name = "cboBodyType";
            this.cboBodyType.Size = new System.Drawing.Size(132, 21);
            this.cboBodyType.TabIndex = 15;
            this.cboBodyType.SelectedIndexChanged += new System.EventHandler(this.cboBodyType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Demographic:";
            // 
            // pnlInputs
            // 
            this.pnlInputs.Controls.Add(this.label6);
            this.pnlInputs.Controls.Add(this.picCost);
            this.pnlInputs.Controls.Add(this.label5);
            this.pnlInputs.Controls.Add(this.nudCostOver);
            this.pnlInputs.Controls.Add(this.label4);
            this.pnlInputs.Controls.Add(this.nudCostUnder);
            this.pnlInputs.Controls.Add(this.label1);
            this.pnlInputs.Controls.Add(this.nudMax);
            this.pnlInputs.Controls.Add(this.label2);
            this.pnlInputs.Controls.Add(this.nudTarget);
            this.pnlInputs.Controls.Add(this.label7);
            this.pnlInputs.Controls.Add(this.chkOverride);
            this.pnlInputs.Controls.Add(this.nudMin);
            this.pnlInputs.Location = new System.Drawing.Point(221, 45);
            this.pnlInputs.Name = "pnlInputs";
            this.pnlInputs.Size = new System.Drawing.Size(192, 383);
            this.pnlInputs.TabIndex = 18;
            this.pnlInputs.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Cost graph";
            // 
            // picCost
            // 
            this.picCost.Location = new System.Drawing.Point(6, 271);
            this.picCost.Name = "picCost";
            this.picCost.Size = new System.Drawing.Size(180, 104);
            this.picCost.TabIndex = 12;
            this.picCost.TabStop = false;
            this.picCost.Paint += new System.Windows.Forms.PaintEventHandler(this.picCost_Paint);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(183, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Score reduction per unit above target";
            // 
            // nudCostOver
            // 
            this.nudCostOver.DecimalPlaces = 3;
            this.nudCostOver.Location = new System.Drawing.Point(3, 228);
            this.nudCostOver.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudCostOver.Name = "nudCostOver";
            this.nudCostOver.Size = new System.Drawing.Size(120, 20);
            this.nudCostOver.TabIndex = 10;
            this.nudCostOver.Validating += new System.ComponentModel.CancelEventHandler(this.nud_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Score reduction per unit below target";
            // 
            // nudCostUnder
            // 
            this.nudCostUnder.DecimalPlaces = 3;
            this.nudCostUnder.Location = new System.Drawing.Point(3, 184);
            this.nudCostUnder.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudCostUnder.Name = "nudCostUnder";
            this.nudCostUnder.Size = new System.Drawing.Size(120, 20);
            this.nudCostUnder.TabIndex = 8;
            this.nudCostUnder.Validating += new System.ComponentModel.CancelEventHandler(this.nud_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Score suddenly drops if above";
            // 
            // nudMax
            // 
            this.nudMax.DecimalPlaces = 3;
            this.nudMax.Location = new System.Drawing.Point(3, 140);
            this.nudMax.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(120, 20);
            this.nudMax.TabIndex = 6;
            this.nudMax.Validating += new System.ComponentModel.CancelEventHandler(this.nud_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Target amount";
            // 
            // nudTarget
            // 
            this.nudTarget.DecimalPlaces = 3;
            this.nudTarget.Location = new System.Drawing.Point(3, 96);
            this.nudTarget.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudTarget.Name = "nudTarget";
            this.nudTarget.Size = new System.Drawing.Size(120, 20);
            this.nudTarget.TabIndex = 4;
            this.nudTarget.Validating += new System.ComponentModel.CancelEventHandler(this.nud_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(148, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Score suddenly drops if below";
            // 
            // chkOverride
            // 
            this.chkOverride.AutoSize = true;
            this.chkOverride.Checked = true;
            this.chkOverride.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverride.Location = new System.Drawing.Point(3, 3);
            this.chkOverride.Name = "chkOverride";
            this.chkOverride.Size = new System.Drawing.Size(185, 17);
            this.chkOverride.TabIndex = 2;
            this.chkOverride.Text = "Override daily goal for this nutrient";
            this.chkOverride.UseVisualStyleBackColor = true;
            this.chkOverride.CheckedChanged += new System.EventHandler(this.chkOverride_CheckedChanged);
            // 
            // nudMin
            // 
            this.nudMin.DecimalPlaces = 3;
            this.nudMin.Location = new System.Drawing.Point(3, 52);
            this.nudMin.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudMin.Name = "nudMin";
            this.nudMin.Size = new System.Drawing.Size(120, 20);
            this.nudMin.TabIndex = 1;
            this.nudMin.Validating += new System.ComponentModel.CancelEventHandler(this.nud_Validating);
            // 
            // lstNutrients
            // 
            this.lstNutrients.FormattingEnabled = true;
            this.lstNutrients.Location = new System.Drawing.Point(3, 45);
            this.lstNutrients.Name = "lstNutrients";
            this.lstNutrients.Size = new System.Drawing.Size(212, 381);
            this.lstNutrients.TabIndex = 17;
            this.lstNutrients.SelectedIndexChanged += new System.EventHandler(this.lstNutrients_SelectedIndexChanged);
            // 
            // nudFoodMass
            // 
            this.nudFoodMass.Location = new System.Drawing.Point(221, 19);
            this.nudFoodMass.Name = "nudFoodMass";
            this.nudFoodMass.Size = new System.Drawing.Size(120, 20);
            this.nudFoodMass.TabIndex = 19;
            this.nudFoodMass.Value = new decimal(new int[] {
            63,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(224, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Total weekly food (x100g):";
            // 
            // TargetsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 430);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nudFoodMass);
            this.Controls.Add(this.pnlInputs);
            this.Controls.Add(this.lstNutrients);
            this.Controls.Add(this.cboBodyType);
            this.Controls.Add(this.label3);
            this.HideOnClose = true;
            this.MaximumSize = new System.Drawing.Size(424, 1500);
            this.Name = "TargetsPanel";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide;
            this.Text = "Nutrition Goals";
            this.Load += new System.EventHandler(this.TargetsPanel_Load);
            this.Resize += new System.EventHandler(this.TargetsPanel_Resize);
            this.pnlInputs.ResumeLayout(false);
            this.pnlInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCostOver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCostUnder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFoodMass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cboBodyType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlInputs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picCost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudCostOver;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudCostUnder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudTarget;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkOverride;
        private System.Windows.Forms.NumericUpDown nudMin;
        private System.Windows.Forms.ListBox lstNutrients;
        private System.Windows.Forms.NumericUpDown nudFoodMass;
        private System.Windows.Forms.Label label8;
    }
}