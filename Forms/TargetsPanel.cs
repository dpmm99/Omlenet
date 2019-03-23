using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Omlenet
{
    public partial class TargetsPanel : DockContent
    {
        private class DisplayNutrientWithOverride
        {
            public ushort nutrientId;
            public string name;
            public bool overridden;

            public override string ToString()
            {
                return (overridden ? "(Overridden) " : "") + name;
            }
        }

        private List<Nutrient> nutrients; //Direct reference
        private List<NutrientTarget> targets; //Direct reference
        private List<NutrientTarget> overrides; //Not a direct reference--we temporarily allow bad data internally, but use GetTargetOverrides to exclude it
        private List<string> bodyTypes; //Direct reference
        private int lastSelectedIndex = -1;
        private bool programmedUpdate = false;
        private List<NumericUpDown> nuds; //NumericUpDown controls except for the target total food mass one

        public int foodMass { get { return (int)nudFoodMass.Value; } set { nudFoodMass.Value = value; } }

        public byte bodyType
        {
            get
            {
                return (byte)cboBodyType.SelectedIndex;
            }

            set
            {
                cboBodyType.SelectedIndex = value;
            }
        }

        public TargetsPanel()
        {
            InitializeComponent();
            nuds = new List<NumericUpDown>() { nudCostOver, nudCostUnder, nudMax, nudMin, nudTarget };
        }

        public List<NutrientTarget> GetTargetOverrides()
        {
            return overrides.Where(p => p.min <= p.target && p.target <= p.max).ToList();
        }

        public void LoadData(List<Nutrient> nutrients, List<NutrientTarget> targets, List<NutrientTarget> overrides, List<string> bodyTypes)
        {
            this.nutrients = nutrients;
            this.targets = targets;
            this.overrides = overrides.ToList();

            lastSelectedIndex = -1;
            pnlInputs.Hide();
            if (this.bodyTypes == null || !this.bodyTypes.SequenceEqual(bodyTypes))
            {
                this.bodyTypes = bodyTypes;
                cboBodyType.Items.Clear();
                cboBodyType.Items.AddRange(bodyTypes.ToArray());
                cboBodyType.SelectedIndex = 0;
            }

            lstNutrients.Items.Clear();
            lstNutrients.Items.AddRange(nutrients.Select(p => new DisplayNutrientWithOverride
            {
                nutrientId = p.id,
                name = p.name + " (" + p.unitOfMeasure + ")",
                overridden = overrides.Any(q => q.nutrientId == p.id)
            }).ToArray());

            displayInUI();
        }

        public int GetIdOfSelectedNutrient()
        {
            if (lstNutrients.SelectedIndex == -1) return 0;
            return ((DisplayNutrientWithOverride)lstNutrients.SelectedItem).nutrientId;
        }

        private void RefreshNutrientList() //Because it won't update the strings on its own through any easier methods
        {
            var wasSelected = lstNutrients.SelectedIndex;
            typeof(ListBox).InvokeMember("RefreshItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, lstNutrients, new object[] { });
            lstNutrients.SelectedIndex = wasSelected;
        }

        private void commitFromUI()
        {
            if (lastSelectedIndex == -1) return;

            var selectedNutrient = (DisplayNutrientWithOverride)lstNutrients.Items[lastSelectedIndex];
            var target = overrides.FirstOrDefault(p => p.nutrientId == selectedNutrient.nutrientId);

            if (selectedNutrient.overridden != chkOverride.Checked)
            {
                selectedNutrient.overridden = chkOverride.Checked;
                RefreshNutrientList(); //Refresh the bare minimum number of times because it's not pretty
            }
            if (!chkOverride.Checked)
            {
                if (target != null) overrides.Remove(target);

                target = targets.FirstOrDefault(p => p.nutrientId == selectedNutrient.nutrientId);
                displayInUI();
            }
            else
            {
                if (target == null)
                {
                    target = new NutrientTarget { nutrientId = selectedNutrient.nutrientId };
                    overrides.Add(target);
                }

                target.min = (float)nudMin.Value;
                target.target = (float)nudTarget.Value;
                target.max = (float)nudMax.Value;
                target.costUnder = (float)nudCostUnder.Value;
                target.costOver = (float)nudCostOver.Value;
            }
        }

        private void displayInUI()
        {
            if (lstNutrients.SelectedIndex == -1) return;

            var selectedNutrient = ((DisplayNutrientWithOverride)lstNutrients.SelectedItem);
            var target = overrides.FirstOrDefault(p => p.nutrientId == selectedNutrient.nutrientId) ?? targets.FirstOrDefault(p => p.bodyType == bodyType && p.nutrientId == selectedNutrient.nutrientId);
            chkOverride.Checked = selectedNutrient.overridden;

            nudMin.Value = (decimal)target.min;
            nudTarget.Value = (decimal)target.target;
            nudMax.Value = (decimal)target.max;
            nudCostUnder.Value = (decimal)target.costUnder;
            nudCostOver.Value = (decimal)target.costOver;

            //Make the arrow buttons multiplicative by 1% instead of additive so they work better for nutrients with different ranges
            foreach (var nud in nuds)
            {
                nud.Enabled = chkOverride.Checked;
                nud.Increment = nud.Value * 0.01M;
                if (nud.Increment < 0.001M) nud.Increment = 0.001M;
            }


            nudMin.Font = (target.min > target.target) ? new Font(nudTarget.Font, FontStyle.Strikeout) : nudTarget.Font;
            nudMax.Font = (target.target > target.max) ? new Font(nudTarget.Font, FontStyle.Strikeout) : nudTarget.Font;

            picCost.Invalidate();
        }
        
        private void lstNutrients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (programmedUpdate) return;
            if (lstNutrients.SelectedIndex == lastSelectedIndex) return; //Avoid an event-driven infinite loop caused by updating the text
            programmedUpdate = true;
            if (lastSelectedIndex != -1) commitFromUI();
            if (lstNutrients.SelectedIndex == -1)
            {
                pnlInputs.Hide();
            }
            else
            {
                pnlInputs.Show();
            }

            lastSelectedIndex = lstNutrients.SelectedIndex;
            displayInUI();
            programmedUpdate = false;
        }

        private void TargetsPanel_Load(object sender, EventArgs e)
        {

        }

        private void cboBodyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlInputs.Visible = (lstNutrients.SelectedIndex != -1);
            displayInUI();
        }

        private void nud_Validating(object sender, CancelEventArgs e)
        {
            if (programmedUpdate) return;
            commitFromUI();
            displayInUI();
        }

        private void picCost_Paint(object sender, PaintEventArgs e)
        {
            if (lastSelectedIndex != -1)
            {
                var selectedNutrient = (DisplayNutrientWithOverride)lstNutrients.Items[lastSelectedIndex];
                //Use override if set or original target if not
                var target = overrides.FirstOrDefault(p => p.nutrientId == selectedNutrient.nutrientId) ?? targets.FirstOrDefault(p => p.bodyType == bodyType && p.nutrientId == selectedNutrient.nutrientId);

                //Avoid exceptions for invalid data
                if (target.min > target.target || target.target > target.max) return;

                var baseline = picCost.Height / 2;
                var minX = (float)0.95 * target.min;
                var maxX = (float)Math.Min(2 * target.target, 1.05 * target.max);
                var scaleX = (float)picCost.Width / (float)Math.Max(maxX - minX, 0.0001); //Max to avoid division by zero errors that permanently render the picture box broken

                var yAtLeft = (target.target - minX) * target.costUnder;
                if (target.min > minX) yAtLeft += 100;
                var yAtRight = (maxX - target.target) * target.costOver;
                if (target.max < maxX) yAtRight += 100;

                var maxY = (float)Math.Max(yAtLeft, yAtRight);
                var minY = (float)0;
                var scaleY = (float)picCost.Height / (float)Math.Max(maxY - minY, 0.0001);

                var points = new List<PointF>();
                //Leftmost point on the graph (score must be <= the score at the minimum threshold)
                points.Add(new PointF(0, yAtLeft));
                //Point representing the minimum threshold (with the extra 100 subtracted)
                points.Add(new PointF(target.min, (target.target - target.min) * target.costUnder + 100));
                //Point representing the minimum threshold (without the extra 100 subtracted)
                points.Add(new PointF(target.min, (target.target - target.min) * target.costUnder));
                //Point representing the target
                points.Add(new PointF(target.target, 0));
                if (target.max < maxX)
                {
                    //Point representing the maximum threshold (without the extra 100 subtracted)
                    points.Add(new PointF(target.max, (target.max - target.target) * target.costOver));
                    //Point representing the maximum threshold (with the extra 100 subtracted)
                    points.Add(new PointF(target.max, (target.max - target.target) * target.costOver + 100));
                }
                //Rightmost point on the graph
                points.Add(new PointF(maxX, yAtRight));

                e.Graphics.DrawLines(Pens.Black, points.Select(p => new PointF(p.X * scaleX, picCost.Height - 1 - p.Y * scaleY)).ToArray());
            }
        }

        private void chkOverride_CheckedChanged(object sender, EventArgs e)
        {
            if (programmedUpdate) return;
            commitFromUI();
            displayInUI();
        }

        private void TargetsPanel_Resize(object sender, EventArgs e)
        {
            var targetHeight = this.ClientSize.Height - lstNutrients.Top;
            if (targetHeight > 30) lstNutrients.Height = targetHeight;
        }
    }
}
