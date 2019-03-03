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

        public Action OnStop;
        public Action OnStart;
        public Action OnContinue;

        public ResultsPanel()
        {
            InitializeComponent();
        }

        public string ResultText { get { return debugResult.Text; } set { debugResult.Text = value; } }

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
            debugResult.Height = this.ClientSize.Height - btnBeginSearch.Height - 3;
        }
    }
}
