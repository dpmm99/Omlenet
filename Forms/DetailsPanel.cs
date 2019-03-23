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
        private Nutrient displayedNutrient;
        public Action OnEditWinner;

        public DetailsPanel()
        {
            InitializeComponent();
        }

        private void DetailsPanel_Load(object sender, EventArgs e)
        {

        }

        public void DisplayNutrient(ushort nutrientId)
        {
            displayedNutrient = nutrients.FirstOrDefault(p => p.id == nutrientId);
            displayedFoodItem = null;
            chkLock.Visible = nudUnitsInPlan.Visible = label1.Visible = false;
            lblFoodDetail.Text = "Nutrient details:" + Environment.NewLine +
                "Name: " + displayedNutrient.name + Environment.NewLine +
                "Unit of measure: " + displayedNutrient.unitOfMeasure;

            UpdateTable();
        }

        public void DisplayFood(int foodId = -1)
        {
            displayedNutrient = null;
            if (foodId != -1) displayedFoodItem = foodDescs.FirstOrDefault(p => p.id == foodId);
            if (displayedFoodItem == null)
            {
                lblFoodDetail.Text = "Nothing selected";
                chkLock.Visible = nudUnitsInPlan.Visible = label1.Visible = false;
                return;
            }
            lblFoodDetail.Text = "Food details:" + Environment.NewLine +
                "Full name: " + displayedFoodItem.longDesc + Environment.NewLine +
                "Group: " + foodGroups.First(p => p.id == displayedFoodItem.foodGroupId); //TODO: Add tags and stuff to foods

            //Only show the fields that link to the winning chromosome if this food is enabled
            chkLock.Visible = nudUnitsInPlan.Visible = label1.Visible = foodEnabled.Contains(displayedFoodItem.id) && solver != null;

            int count = 0;
            if (solver != null && solver.HasWinner) solver.GetWinningFoods().TryGetValue(displayedFoodItem.id, out count);
            nudUnitsInPlan.Value = count;
            chkLock.Checked = foodLocked.Contains(displayedFoodItem.id);
            UpdateTable();
        }

        private void UpdateTable()
        {
            var oldScroll = dataGridView1.FirstDisplayedScrollingRowIndex;
            dataGridView1.Rows.Clear();

            if (displayedNutrient != null) //Details of a nutrient include a table of foods in the current winner that contain this nutrient
            {
                if (solver == null || !solver.HasWinner)
                {
                    dataGridView1.Visible = false;
                    return;
                }
                dataGridView1.Visible = true;

                var foods = solver.GetWinningFoods();
                foreach (var food in foods)
                {
                    var details = foodNutrientDict[food.Key];
                    var nutrientAmount = details.FirstOrDefault(p => p.nutrientId == displayedNutrient.id)?.nutrientAmount;
                    if (nutrientAmount == null) continue;
                    //Multiply by the count of this food item
                    nutrientAmount *= food.Value;

                    var foodNutrientCost = solver.CalculateNutrientCostDifference(displayedNutrient.id, nutrientAmount.Value);
                    dataGridView1.Rows.Add(new object[] {
                        food.Key,
                        foodDescs.First(p => p.id == food.Key).longDesc,
                        nutrientAmount,
                        Math.Round(foodNutrientCost, 1)
                    });
                }
                if (dataGridView1.SortedColumn != null) dataGridView1.Sort(dataGridView1.SortedColumn, (dataGridView1.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }
            else if (displayedFoodItem != null) //Details of a food include a table of nutrients that the current winner contains because of this food
            {
                var count = (int)nudUnitsInPlan.Value;
                //Hide the table if the food isn't in the chromosome
                if (count == 0)
                {
                    dataGridView1.Visible = false;
                    return;
                }
                dataGridView1.Visible = true;

                var foodDetails = foodNutrientDict[displayedFoodItem.id];
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
                if (dataGridView1.SortedColumn != null) dataGridView1.Sort(dataGridView1.SortedColumn, (dataGridView1.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending));
            }
            else
            {
                dataGridView1.Visible = false;
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
            OnEditWinner();
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

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ctmDgvCopyMenu.Show(sender as DataGridView, e.ColumnIndex, e.RowIndex, e.Location);
        }

        private void DetailsPanel_Resize(object sender, EventArgs e)
        {
            if (this.Width > 333)
            {
                dataGridView1.Width = pnlFoodDetail.Width - SystemInformation.VerticalScrollBarWidth - 6;
                dataGridView1.Columns[1].Width = dataGridView1.Width - 175;
            }
            if (this.Height > 354)
            {
                dataGridView1.Height = pnlFoodDetail.Height - dataGridView1.Top - 58;
                nudUnitsInPlan.Top = dataGridView1.Bottom + 6;
                chkLock.Top = nudUnitsInPlan.Top + 3;
                label1.Top = nudUnitsInPlan.Top + 4;
            }
        }
    }
}
