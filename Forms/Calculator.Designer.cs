namespace Omlenet
{
    partial class Calculator
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudDays = new System.Windows.Forms.NumericUpDown();
            this.nudFoodMass = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFoodMass)).BeginInit();
            this.SuspendLayout();
            // 
            // plainTextResults
            // 
            this.plainTextResults.Size = new System.Drawing.Size(199, 265);
            // 
            // btnBeginSearch
            // 
            this.btnBeginSearch.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(0, 26);
            this.tabControl1.Size = new System.Drawing.Size(213, 297);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(205, 271);
            // 
            // tabPage2
            // 
            this.tabPage2.Size = new System.Drawing.Size(193, 271);
            // 
            // tabPage3
            // 
            this.tabPage3.Size = new System.Drawing.Size(193, 271);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Calculate for day count:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Mass (g) of selected food:";
            // 
            // nudDays
            // 
            this.nudDays.Location = new System.Drawing.Point(142, 57);
            this.nudDays.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudDays.Name = "nudDays";
            this.nudDays.Size = new System.Drawing.Size(59, 20);
            this.nudDays.TabIndex = 22;
            this.toolTip1.SetToolTip(this.nudDays, "The costs shown below are based on nutrition goals adjusted for the number of day" +
        "s entered here.");
            this.nudDays.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDays.ValueChanged += new System.EventHandler(this.nudDays_ValueChanged);
            // 
            // nudFoodMass
            // 
            this.nudFoodMass.Location = new System.Drawing.Point(139, 31);
            this.nudFoodMass.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudFoodMass.Name = "nudFoodMass";
            this.nudFoodMass.Size = new System.Drawing.Size(62, 20);
            this.nudFoodMass.TabIndex = 21;
            this.toolTip1.SetToolTip(this.nudFoodMass, "Enter the number of grams of the selected food to include in the calculation.");
            this.nudFoodMass.ValueChanged += new System.EventHandler(this.nudFoodMass_ValueChanged);
            // 
            // Calculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 323);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudDays);
            this.Controls.Add(this.nudFoodMass);
            this.Enabled = false;
            this.Name = "Calculator";
            this.Text = "Calculator";
            this.Controls.SetChildIndex(this.btnBeginSearch, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.btnReset, 0);
            this.Controls.SetChildIndex(this.nudFoodMass, 0);
            this.Controls.SetChildIndex(this.nudDays, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFoodMass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudDays;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown nudFoodMass;
    }
}