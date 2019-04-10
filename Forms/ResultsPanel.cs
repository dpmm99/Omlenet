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
    public partial class ResultsPanel : DockContent
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action OnStop;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action OnStart;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action OnContinue;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action OnReset;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<int> DisplayFood; //Passes in a food ID
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<ushort> DisplayNutrient; //Passes in a nutrient ID

        public void UpdateUI()
        {
            btnReset.Enabled = solverState == SolverState.Completed;
            if (solverState == SolverState.Loading)
            {
                btnBeginSearch.Text = "(Loading...)";
                return;
            }
            else if (solverState == SolverState.Ready) btnBeginSearch.Text = "Begin Search";
            else if (solverState == SolverState.Running) btnBeginSearch.Text = "Stop Search";
            else if (solverState == SolverState.Completed) btnBeginSearch.Text = "Continue Search";
            btnBeginSearch.Enabled = true;
        }

        public ResultsPanel()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ResultText { get { return plainTextResults.Text; } set { plainTextResults.Text = value; } }

        protected static List<ResultListItem> DgvRowsToResultListItemList(DataGridViewRowCollection items)
        {
            var ret = new List<ResultListItem>(items.Count);
            foreach (var item in items)
            {
                ret.Add(new ResultListItem {
                    Id = (int)((DataGridViewRow)item).Cells[0].Value,
                    Name = (string)((DataGridViewRow)item).Cells[1].Value,
                    Mass = (float)((DataGridViewRow)item).Cells[2].Value,
                    Cost = (float)((DataGridViewRow)item).Cells[3].Value,
                });
            }
            return ret;
        }

        protected void UpdateResultListDGV(DataGridView dgv, List<ResultListItem> value)
        {
            var oldScroll = dgv.FirstDisplayedScrollingRowIndex;
            dgv.Rows.Clear();
            foreach (var item in value)
            {
                dgv.Rows.Add(new object[] {
                        item.Id,
                        item.Name,
                        item.Mass,
                        item.Cost,
                    });
            }

            if (dgv.SortedColumn != null) dgv.Sort(dgv.SortedColumn, (dgv.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending));
            //Restore scroll
            if (oldScroll < dgv.RowCount && oldScroll > -1) dgv.FirstDisplayedScrollingRowIndex = oldScroll;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ResultListItem> FoodList
        {
            get { return DgvRowsToResultListItemList(dgvFoods.Rows); }
            set { UpdateResultListDGV(dgvFoods, value); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ResultListItem> NutrientList
        {
            get { return DgvRowsToResultListItemList(dgvNutrients.Rows); }
            set { UpdateResultListDGV(dgvNutrients, value); }
        }

        private void btnBeginSearch_Click(object sender, EventArgs e)
        {
            btnBeginSearch.Enabled = false;
            if (solverState == SolverState.Running)
            {
                OnStop();
            }
            else if (solverState == SolverState.Completed)
            {
                OnContinue();
                UpdateUI();
            }
            else if (solverState == SolverState.Ready)
            {
                OnStart();
                UpdateUI();
            }
        }

        protected void ResultsPanel_Resize(object sender, EventArgs e)
        {
            tabControl1.Height = this.ClientSize.Height - btnBeginSearch.Height - 3;
        }

        private void ResultsPanel_Load(object sender, EventArgs e)
        {

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        string contextMenuTargetText = "";
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(contextMenuTargetText);
        }

        private void dgvNutrients_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex != -1)
            {
                DisplayNutrient((ushort)(int)dgvNutrients.Rows[e.RowIndex].Cells[0].Value); //Apparently you can't cast straight to ushort, ha
            }
            else if (e.Button == MouseButtons.Right)
            {
                ctmDgvCopyMenu.Show(sender as DataGridView, e.ColumnIndex, e.RowIndex, e.Location);
            }
        }

        private void dgvFoods_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex != -1)
            {
                DisplayFood((int)dgvFoods.Rows[e.RowIndex].Cells[0].Value);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ctmDgvCopyMenu.Show(sender as DataGridView, e.ColumnIndex, e.RowIndex, e.Location);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            OnReset();
            UpdateUI();
        }
    }
}
