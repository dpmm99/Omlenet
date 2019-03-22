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
        public Action OnStop;
        public Action OnStart;
        public Action OnContinue;
        public Action<int> DisplayFood; //Passes in a food ID
        public Action<ushort> DisplayNutrient; //Passes in a nutrient ID

        public void UpdateUI()
        {
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

        public string ResultText { get { return plainTextResults.Text; } set { plainTextResults.Text = value; } }

        public List<ResultListItem> FoodList {
            get { return lstResults.Items.Cast<ResultListItem>().ToList(); }
            set {
                var selectedId = lstResults.SelectedItem != null ? ((ResultListItem)lstResults.SelectedItem).Id : -1;
                lstResults.Items.Clear(); lstResults.Items.AddRange(value.ToArray());
                //Restore user selection if possible
                lstResults.SelectedItem = lstResults.Items.Cast<ResultListItem>().FirstOrDefault(p => p.Id == selectedId);
            }
        }

        private static List<ResultListItem> DgvRowsToResultListItemList(DataGridViewRowCollection items) //TODO: Make it a method for ResultListItem or something
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

        public List<ResultListItem> NutrientList
        {
            get { return DgvRowsToResultListItemList(dgvNutrients.Rows); }
            set
            {
                var oldScroll = dgvNutrients.FirstDisplayedScrollingRowIndex;
                dgvNutrients.Rows.Clear();
                foreach (var item in value)
                {
                    dgvNutrients.Rows.Add(new object[] {
                        item.Id,
                        item.Name,
                        item.Mass,
                        item.Cost,
                    });
                }

                if (dgvNutrients.SortedColumn != null) dgvNutrients.Sort(dgvNutrients.SortedColumn, (dgvNutrients.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending));
                //Restore scroll
                if (oldScroll < dgvNutrients.RowCount && oldScroll > -1) dgvNutrients.FirstDisplayedScrollingRowIndex = oldScroll;
            }
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

        private void ResultsPanel_Resize(object sender, EventArgs e)
        {
            tabControl1.Height = this.ClientSize.Height - btnBeginSearch.Height - 3;
        }

        private void ResultsPanel_Load(object sender, EventArgs e)
        {

        }

        private void lstResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayFood(((ResultListItem)lstResults.SelectedItem).Id);
        }

        string contextMenuTargetText = "";
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(contextMenuTargetText);
        }

        private void lstResults_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuTargetText = String.Join(Environment.NewLine, ((ListBox)sender).Items.Cast<ResultListItem>().Select(p => p.ToString()));
                ctmReadOnlyTextMenu.Show((Control)sender, e.Location);
            }
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
    }
}
