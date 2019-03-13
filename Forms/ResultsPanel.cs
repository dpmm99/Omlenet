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
        public Action<int> UpdateDetails; //Passes in a food ID

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

        public List<ResultListItem> ResultList {
            get { return lstResults.Items.Cast<ResultListItem>().ToList(); }
            set {
                if (ResultList.SequenceEqual(value)) return; //Don't update the UI needlessly

                var selectedId = lstResults.SelectedItem != null ? ((ResultListItem)lstResults.SelectedItem).Id : -1;
                lstResults.Items.Clear(); lstResults.Items.AddRange(value.ToArray());
                //Restore user selection if possible
                lstResults.SelectedItem = lstResults.Items.Cast<ResultListItem>().FirstOrDefault(p => p.Id == selectedId);
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
            UpdateDetails(((ResultListItem)lstResults.SelectedItem).Id);
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
    }
}
