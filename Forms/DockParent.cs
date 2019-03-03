﻿#define PARALLEL
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using static Omlenet.ProgramState;

namespace Omlenet
{
    public partial class DockParent : Form
    {
        private TargetsPanel targetsPanel;
        private FiltersPanel filtersPanel;
        private DetailsPanel detailsPanel;
        private ResultsPanel resultsPanel;

        private List<string> bodyTypes = new List<string> { "Male 19-30" };

        public DockParent()
        {
            programTitle = (Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false).SingleOrDefault() as AssemblyTitleAttribute)?.Title;
            InitializeComponent();
#if !DEBUG
            //TODO: btnRareNutrients.Visible = false;
#endif
        }

        private void DockParent_Load(object sender, EventArgs e)
        {
            //dockPanel1.LoadFromXml() //TODO: If you wanna implement saving/loading the user's docking layout, check out menuItemLayoutByXml at https://github.com/dockpanelsuite/blob/master/DockSample/MainForm.cs
            try
            {
                FormClosingEventHandler onClosingHide = (a, b) =>
                {
                    b.Cancel = true;
                    ((DockContent)a).DockState = DockState.DockTop;
                };

                targetsPanel = new TargetsPanel();
                targetsPanel.CloseButtonVisible = false;
                targetsPanel.IsHidden = false;
                targetsPanel.Show(dockPanel1, DockState.DockLeft);
                targetsPanel.FormClosing += onClosingHide;

                detailsPanel = new DetailsPanel();
                detailsPanel.CloseButtonVisible = false;
                detailsPanel.IsHidden = false;
                detailsPanel.Show(dockPanel1, DockState.DockBottom);
                detailsPanel.FormClosing += onClosingHide;

                filtersPanel = new FiltersPanel();
                filtersPanel.CloseButtonVisible = false;
                filtersPanel.IsHidden = false;
                filtersPanel.Show(dockPanel1, DockState.Document);
                filtersPanel.UpdateDetails = detailsPanel.UpdateDetails; //Allow the details panel to react when you select a food in the filters panel
                filtersPanel.FormClosing += onClosingHide;

                resultsPanel = new ResultsPanel();
                resultsPanel.CloseButtonVisible = false;
                resultsPanel.IsHidden = false;
                resultsPanel.Show(dockPanel1, DockState.DockRight);
                resultsPanel.FormClosing += onClosingHide;
                resultsPanel.OnStop = () =>
                {
                    solver.Stop(); //tmrWaiter will take care of the rest
                };
                resultsPanel.OnStart = () =>
                {
                    progressBar1.Visible = true;
                    tmrWaiter.Enabled = true;

                    targetOverrides = targetsPanel.GetTargetOverrides();
                    targetFoodUnits = targetsPanel.foodMass;
                    InitGASolver();
                    solver.Start();
                    solverState = SolverState.Running;
                };
                resultsPanel.OnContinue = () =>
                {
                    targetOverrides = targetsPanel.GetTargetOverrides();
                    solver.UpdateTargets(GetTrueTargets());
                    solver.UpdateFoodList(foodDescs.Where(p => foodEnabled[p.id]).ToList());
                    solver.UpdateFoodMass(targetFoodUnits = targetsPanel.foodMass);
                    solver.Start();
                    solverState = SolverState.Running;
                    progressBar1.Visible = true;
                    tmrWaiter.Enabled = true;
                };
            }
            catch
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solver != null) solver.Stop();

            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                progressBar1.Value = 50; //Pretend progress
                progressBar1.Visible = true;
                Application.DoEvents();
                bodyType = targetsPanel.bodyType;
                targetFoodUnits = targetsPanel.foodMass;
                using (var bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write))) SerializeInstance(bw);
                progressBar1.Visible = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solver != null) solver.Stop();

            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                progressBar1.Value = 50; //Pretend progress
                progressBar1.Visible = true;
                Application.DoEvents();
                using (var br = new BinaryReader(File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))) DeserializeInstance(br);
                progressBar1.Visible = false;

                //Apply loaded data to UI
                filtersPanel.FilterNow();
                targetsPanel.bodyType = bodyType;
                targetsPanel.foodMass = targetFoodUnits;
                resultsPanel.ResultText = solver.GetWinnerText();

                if (solver.executed) solverState = SolverState.Completed;
                else solverState = SolverState.Ready;
                resultsPanel.UpdateUI();
                targetsPanel.LoadData(nutrients, targets, targetOverrides, bodyTypes);
            }
        }

        private void increaseLoadingProgress(int lineCount, int expectedLineCount)
        {
            var expectedValue = Math.Min(100, (lineCount) * 100 / expectedLineCount);
            if (progressBar1.Value != expectedValue)
            {
                progressBar1.Value = expectedValue;
                Application.DoEvents();
            }
        }

        private void tmrInitializer_Tick(object sender, EventArgs e)
        {
            tmrInitializer.Enabled = false;

            LoadData("Data/", increaseLoadingProgress);

            progressBar1.Visible = false;
            resultsPanel.Enabled = true;
            targetsPanel.Enabled = true;
            filtersPanel.Enabled = true;

            filtersPanel.Ready();

            //TODO: Get body types from file
            targetsPanel.LoadData(nutrients, targets, targetOverrides, bodyTypes);

            //TODO: Error check the mappings between those files
        }

        private void tmrWaiter_Tick(object sender, EventArgs e)
        {
            //Report progress to the user at a regular rate
            progressBar1.Value = solver.GetProgress();
            resultsPanel.ResultText = "Current best:" + Environment.NewLine + solver.GetWinnerText();
            if (progressBar1.Value == 100)
            {
                resultsPanel.ResultText = solver.GetWinnerText();

                progressBar1.Visible = false;
                tmrWaiter.Enabled = false;
                solverState = SolverState.Completed;
                resultsPanel.UpdateUI();
                //TODO: Re-enable UI stuff
                //TODO: Display information about the winner in a better way
            }
        }

        private void viewRarestNutrientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var orderedNutrients = foodNutrients.GroupBy(p => p.nutrientId)
                .Select(p => new { nutrientId = p.Key, foods = p.OrderBy(q => q.nutrientAmount).Reverse().ToList() })
                .OrderBy(p => p.foods.Count).ToList();

            var sb = new StringBuilder();
            foreach (var nutrient in orderedNutrients)
            {
                var name = nutrients.First(p => p.id == nutrient.nutrientId).name;
                var target = targets.First(p => p.nutrientId == nutrient.nutrientId);
                sb.Append(nutrient.nutrientId + " (" + name + ") is found in " + nutrient.foods.Count + " foods");
                if (target.target != 0)
                {
                    var highestFraction = Math.Round(100 * nutrient.foods.First().nutrientAmount / target.target, 1);
                    var desc = foodDescs.First(p => p.id == nutrient.foods[0].foodId);
                    sb.Append(" with at most " + highestFraction + "% DV per 100g, in the food: " + desc.longDesc);
                }
                sb.AppendLine();
            }
            resultsPanel.ResultText = sb.ToString();
        }

        private void DockParent_Resize(object sender, EventArgs e)
        {
            progressBar1.Width = Math.Max(statusStrip1.ClientSize.Width - 25, 10);
        }

        private void DockParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Because it fires the MDI children's closing events first, which set cancel=true so they don't go away, set it back to false when trying to close the main window.
            e.Cancel = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }
}