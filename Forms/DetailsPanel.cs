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
using static Omlenet.ProgramState;

namespace Omlenet
{
    public partial class DetailsPanel : DockContent
    {
        private FoodDescription displayedFoodItem;

        public DetailsPanel()
        {
            InitializeComponent();
        }

        private void DetailsPanel_Load(object sender, EventArgs e)
        {

        }

        public void UpdateDetails(int foodId = -1)
        {
            if (foodId != -1) displayedFoodItem = foodDescs.FirstOrDefault(p => p.id == foodId);
            if (displayedFoodItem == null)
            {
                chkLock.Visible = nudUnitsInPlan.Visible = label1.Visible = false;
                return;
            }
            lblFoodDetail.Text = "Food details:" + Environment.NewLine +
                "Full name: " + displayedFoodItem.longDesc + Environment.NewLine +
                "Group: " + foodGroups.First(p => p.id == displayedFoodItem.foodGroupId); //TODO: Add tags and stuff to foods

            //Only show the fields that link to the winning chromosome if this food is enabled
            //chkLock.Visible = nudUnitsInPlan.Visible = label1.Visible = foodEnabled[displayedFoodItem.id] && solver != null;
            chkLock.Visible = nudUnitsInPlan.Visible = label1.Visible = foodEnabledB.Contains(displayedFoodItem.id) && solver != null;

            int count = 0;
            if (solver != null && solver.HasWinner) solver.GetWinningFoods().TryGetValue(displayedFoodItem.id, out count);
            nudUnitsInPlan.Value = count;
            chkLock.Checked = foodLocked.Contains(displayedFoodItem.id);
            UpdateTable();
        }

        private void UpdateTable() //TODO: Make the table grow with DetailsPanel and move the NumericUpDown and checkbox
        {
            var count = (int)nudUnitsInPlan.Value;
            //Hide the table if the food isn't in the chromosome
            if (count == 0)
            {
                dataGridView1.Visible = false;
                return;
            }

            var foodDetails = foodNutrientDict[displayedFoodItem.id];
            var oldScroll = dataGridView1.FirstDisplayedScrollingRowIndex;
            dataGridView1.Rows.Clear();
            foreach (var nutrient in foodDetails.Where(p => p.nutrientAmount * count != 0))
            {
                var nutrientMeta = nutrients.First(p => p.id == nutrient.nutrientId);
                var nutrientAmount = (nutrient.nutrientAmount * count);
                var nutrientCost = (solver != null && solver.HasWinner ? solver.CalculateNutrientCostDifference(nutrient.nutrientId, nutrientAmount) : 0);
                dataGridView1.Rows.Add(new object[] {
                    nutrient.nutrientId,
                    nutrientMeta.name,
                    nutrientAmount + nutrientMeta.unitOfMeasure,
                    Math.Round(nutrientCost, 1)
                });
            }
            //Restore scroll
            if (oldScroll < dataGridView1.RowCount && oldScroll > -1) dataGridView1.FirstDisplayedScrollingRowIndex = oldScroll;
        }

        string contextMenuTargetText = "";
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(contextMenuTargetText);
        }

        private void lblFoodDetail_MouseDown(object sender, MouseEventArgs e)
        {
            contextMenuTargetText = ((Control)sender).Text;
            ctmReadOnlyTextMenu.Show((Control)sender, e.Location);
        }

        private void nudUnitsInPlan_Validating(object sender, CancelEventArgs e)
        {
            if (displayedFoodItem == null || solver == null) return;
            solver.SetFood(displayedFoodItem.id, (int)nudUnitsInPlan.Value);
            UpdateTable();
        }

        private void chkLock_Click(object sender, EventArgs e)
        {
            if (displayedFoodItem == null) return;
            if (chkLock.Checked) foodLocked.Add(displayedFoodItem.id);
            else foodLocked.Remove(displayedFoodItem.id);
        }

        public void LockInputs(bool locked)
        {
            chkLock.Enabled = !locked;
            nudUnitsInPlan.Enabled = !locked;
        }
    }
}
