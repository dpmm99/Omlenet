#define PARALLEL
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
using System.Threading;
using static Omlenet.USDAFormat;
using System.Reflection;

namespace Omlenet
{
    public partial class Form1 : Form
    {
        private string programTitle = (Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false).SingleOrDefault() as AssemblyTitleAttribute)?.Title;

        private GASolver solver;
        private List<FoodNutrient> foodNutrients;
        private List<FoodGroup> foodGroups;
        private List<FoodDescription> foodDescs;
        private Dictionary<int, bool> foodEnabled;
        private List<Nutrient> nutrients;
        private List<NutrientTarget> targets;
        private List<NutrientTarget> targetOverrides = new List<NutrientTarget>();
        private int pauseToFilter;
        private byte bodyType; //For the sake of serialization/deserialization

        public Form1()
        {
            InitializeComponent();
#if !DEBUG
            btnRareNutrients.Visible = false;
#endif
        }

        private const uint MAGIC_NUMBER = 0x0b1ebe7;
        private const ushort FILE_VERSION = 0;
        private void SerializeInstance(BinaryWriter bw)
        {
            bw.Write(MAGIC_NUMBER);
            bw.Write(FILE_VERSION);

            bw.Write(bodyType);

            //Save nutrient target overrides
            bw.Write(targetOverrides.Count);
            foreach (var o in targetOverrides)
            {
                bw.Write(o.nutrientId);
                bw.Write(o.min);
                bw.Write(o.target);
                bw.Write(o.max);
                bw.Write(o.costUnder);
                bw.Write(o.costOver);
            }

            //Save list of disabled foods
            bw.Write(foodEnabled.Count(p => !p.Value));
            foreach (var f in foodEnabled.Where(p => !p.Value).Select(p => foodDescs.First(q => q.id == p.Key)))
            {
                bw.Write(f.id);
            }

            //Save whether the GA has been executed
            bw.Write(solver == null || !solver.executed);

            //Save winner's foods, if any (in the future, the 'winner' can exist even if the solver has not executed)
            if (solver != null && solver.HasWinner)
            {
                var winningFoods = solver.GetWinningFoods();
                bw.Write(winningFoods.Count);
                foreach (var w in winningFoods)
                {
                    bw.Write(w.Key);
                    bw.Write(w.Value);
                }
            }
            else bw.Write(0);
        }

        private void SaveInstance()
        {
            var res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                progressBar1.Value = 50; //Pretend progress
                progressBar1.Visible = true;
                Application.DoEvents();
                bodyType = (byte)cboBodyType.SelectedIndex;
                using (var bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write))) SerializeInstance(bw);
                progressBar1.Visible = false;
            }
        }

        private void DeserializeInstance(BinaryReader br)
        {
            if (br.ReadUInt32() != MAGIC_NUMBER) throw new Exception("Magic number mismatch--is this an " + programTitle + " file?");
            if (br.ReadUInt16() > FILE_VERSION) throw new Exception("This file appears to have been created by a newer version of " + programTitle + " and cannot be loaded.");

            bodyType = br.ReadByte();

            //Load nutrient target overrides
            var overrideCount = br.ReadInt32();
            targetOverrides = new List<NutrientTarget>();
            for (var x = 0; x < overrideCount; x++)
            {
                targetOverrides.Add(new NutrientTarget
                {
                    nutrientId = br.ReadUInt16(),
                    min = br.ReadSingle(),
                    target = br.ReadSingle(),
                    max = br.ReadSingle(),
                    costUnder = br.ReadSingle(),
                    costOver = br.ReadSingle(),
                });
            }

            //Load list of disabled foods
            foodEnabled = foodDescs.ToDictionary(p => p.id, p => true);
            var disabledFoodCount = br.ReadInt32();
            for (var x = 0; x < disabledFoodCount; x++)
            {
                foodEnabled[br.ReadInt32()] = false;
            }

            InitGASolver();
            //Load whether the GA has been executed
            solver.executed = br.ReadBoolean();

            //Load winner's foods
            var solverFoodCount = br.ReadInt32();
            for (var x = 0; x < solverFoodCount; x++)
            {
                var id = br.ReadInt32();
                var count = br.ReadInt32();
                solver.SetFood(id, count);
            }
        }

        private void LoadInstance()
        {
            var res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                progressBar1.Value = 50; //Pretend progress
                progressBar1.Visible = true;
                Application.DoEvents();
                using (var br = new BinaryReader(File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read))) DeserializeInstance(br);
                progressBar1.Visible = false;

                //Apply loaded data to UI
                lock (tmrFilter)
                {
                    pauseToFilter = 1;
                    tmrFilter.Enabled = true;
                }
                cboBodyType.SelectedIndex = bodyType;
                debugResult.Text = solver.GetWinnerText();

                if (solver.executed) btnBeginSearch.Text = "Continue Search";
                else btnBeginSearch.Text = "Begin Search";
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

            var lineCount = 0;
            var expectedLineCount = 7794 + 26 + 644126 + 150 + 150; //It's okay if they change a bit, but this is how many lines were in those files at the start

            foodDescs = loadAsList(FoodDescription.FromStream, "Data/FOOD_DES.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            foodGroups = loadAsList(FoodGroup.FromStream, "Data/FD_GROUP.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            foodNutrients = loadAsList(FoodNutrient.FromStream, "Data/NUT_DATA.txt", ref lineCount, expectedLineCount, increaseLoadingProgress)
                .Where(p => p.nutrientAmount != 0) //It's kinda silly that these are even included. Save a bunch of time by ignoring them (roughly 1/3 of the whole file).
                .OrderBy(p => p.foodId).ToList();
            nutrients = loadAsList(Nutrient.FromStream, "Data/NUTR_DEF.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            targets = loadAsList(NutrientTarget.FromStream, "Data/Targets.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);

            progressBar1.Visible = false;
            pnlUserControls.Enabled = true;

            cklFoods.Items.AddRange(foodDescs.ToArray());
            foodEnabled = new Dictionary<int, bool>();
            for (var x = 0; x < foodDescs.Count; x++)
            {
                cklFoods.SetItemChecked(x, true);
                foodEnabled[foodDescs[x].id] = true;
            }

            //TODO: Get body types from file
            cboBodyType.Items.Add("Male 19-30");
            cboBodyType.SelectedIndex = 0;

            cboFilterByGroup.Items.AddRange(new List<FoodGroup> { new FoodGroup { id = 0, name = "Show All" } }.Concat(foodGroups.OrderBy(p => p.name)).ToArray());
            cboFilterByGroup.SelectedIndex = 0;

            //TODO: Error check the mappings between those files
        }

        private void InitGASolver()
        {
            var targetFoodUnits = 9 * 7; //TODO: Get from somewhere and get research data for it. Note that this is in 100g increments to match well with the data.
            var goodFoods = foodDescs.Where(p => foodEnabled[p.id]).ToList();

            //Use the overrides and fill in the gaps with targets for the selected body type
            var trueTargets = targetOverrides
                .Concat(targets.Where(p => p.bodyType == cboBodyType.SelectedIndex && !targetOverrides.Any(q => q.nutrientId == p.nutrientId)))
                .OrderBy(p => p.nutrientId).ToList();

            solver = new GASolver(goodFoods, trueTargets, nutrients, foodNutrients, targetFoodUnits);
        }

        private void btnBeginSearch_Click(object sender, EventArgs e)
        {
            if (btnBeginSearch.Text == "Stop Search")
            {
                solver.Stop(); //tmrWaiter will take care of the rest
            }
            else if (btnBeginSearch.Text == "Continue Search")
            {
                solver.UpdateFoodList(foodDescs.Where(p => foodEnabled[p.id]).ToList());
                solver.Start();
                progressBar1.Visible = true;
                tmrWaiter.Enabled = true;
                btnBeginSearch.Text = "Stop Search";
            }
            else
            {
                progressBar1.Visible = true;
                tmrWaiter.Enabled = true;
                btnBeginSearch.Text = "Stop Search";

                InitGASolver();
                solver.Start();
            }
        }

        private void tmrWaiter_Tick(object sender, EventArgs e)
        {
            //Report progress to the user at a regular rate
            progressBar1.Value = solver.GetProgress();
            debugResult.Text = "Current best:" + Environment.NewLine + solver.GetWinnerText();
            if (progressBar1.Value == 100)
            {
                debugResult.Text = solver.GetWinnerText();

                progressBar1.Visible = false;
                tmrWaiter.Enabled = false;
                btnBeginSearch.Text = "Continue Search";
                //TODO: Re-enable UI stuff
                //TODO: Display information about the winner in a better way
            }
        }

        private void cklFoods_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: Show detail
            var item = cklFoods.SelectedItem as FoodDescription;
            lblFoodDetail.Text = "Food details:" + Environment.NewLine +
                "Full name: " + item.longDesc + Environment.NewLine +
                "Group: " + foodGroups.First(p => p.id == item.foodGroupId); //TODO: Add tags and stuff
            //TODO: Output details like GASolver.GenerateChromosomeText but without the totals
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //Move form items around
            var right = this.ClientSize.Width - 10;
            progressBar1.Width = right;
            progressBar1.Top = this.ClientSize.Height - progressBar1.Height;
            var bottom = progressBar1.Top - 10;
            pnlFoodDetail.Height = bottom - pnlFoodDetail.Top;
            debugResult.Height = bottom - debugResult.Top;
            debugResult.Left = right - debugResult.Width;
            btnBeginSearch.Left = right - btnBeginSearch.Width;
            groupBox1.Height = bottom - groupBox1.Top;
            panel1.Top = groupBox1.ClientSize.Height - panel1.Height - 6;
            cklFoods.Height = panel1.Top - 15;
            pnlFoodDetail.Width = debugResult.Left - pnlFoodDetail.Left - 6;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            lock (tmrFilter) //Mutex prevents the timer event from firing again while it's still processing when I'm debugging or on ultra slow machines
            {
                pauseToFilter = 5;
                tmrFilter.Enabled = true;
            }
        }

        private void btnCheckShown_Click(object sender, EventArgs e)
        {
            if (cklFoods.CheckedIndices.Count != 0)
            {
                //Uncheck all
                while (cklFoods.CheckedIndices.Count != 0)
                {
                    var idx = cklFoods.CheckedIndices[0];
                    foodEnabled[(cklFoods.Items[idx] as FoodDescription).id] = false;
                    cklFoods.SetItemChecked(idx, false);
                }
            }
            else
            {
                //Check all
                for (var x = 0; x < cklFoods.Items.Count; x++)
                {
                    cklFoods.SetItemChecked(x, true);
                    foodEnabled[(cklFoods.Items[x] as FoodDescription).id] = true;
                }
            }
        }

        private void cklFoods_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            foodEnabled[(cklFoods.Items[e.Index] as FoodDescription).id] = (e.NewValue == CheckState.Checked);
        }

        private void tmrFilter_Tick(object sender, EventArgs e)
        {
            if (pauseToFilter-- != 0) return;

            lock (tmrFilter)
            {
                tmrFilter.Enabled = false;
                var delimiters = new char[] { ' ', ',' };
                var badWords = new List<string>();
                var wholeWords = new List<string>();
                var query = txtFilter.Text.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                for (var x = 0; x < query.Count; x++)
                {
                    if (!query[x].Contains('-')) query[x] = query[x].Replace(",", ""); //Drop commas (treat like delimiters except in hyphenated phrases)
                    query[x] = query[x].Replace('-', ' '); //Hyphens allow multi-word string matches
                    if (query[x].StartsWith("!"))
                    {
                        badWords.Add(query[x].Replace("!", ""));
                        query.RemoveAt(x);
                        x--;
                    }
                    else if (query[x].StartsWith("$"))
                    {
                        wholeWords.Add(query[x].Replace("$", ""));
                        query.RemoveAt(x);
                        x--;
                    }
                }

                //Search!
                var filteredFoods = foodDescs.Where(p =>
                {
                    bool okaySoFar = true;
                    if (cboFilterByGroup.SelectedIndex != 0 && p.foodGroupId != (cboFilterByGroup.SelectedItem as FoodGroup).id) return false;

                    if (wholeWords.Count != 0 || badWords.Count != 0) //Only put forth the effort to split everything if you're going to use it //TODO: Could optimize
                    {
                        var commonNameTokens = p.commonName.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        var longDescTokens = p.longDesc.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        //Bad words -> instant removal
                        if (badWords.Any(q => commonNameTokens.Contains(q)) || badWords.Any(q => longDescTokens.Contains(q))) return false;

                        //ALL whole words have to match, NO bad words can match, and ANY ONE other word must match.
                        okaySoFar = wholeWords.Except(commonNameTokens).Except(longDescTokens).Count() == 0;
                    }
                    return okaySoFar && query.Count == 0 || query.Any(q => p.commonName.ToLower().Contains(q)) || query.Any(q => p.longDesc.ToLower().Contains(q));
                }).ToArray(); //TODO: Maybe allow filtering with more things like nutrients

                var wasSelected = cklFoods.SelectedIndex != -1 ? (cklFoods.SelectedItem as FoodDescription) : null;
                cklFoods.Items.Clear();
                cklFoods.Items.AddRange(filteredFoods);

                //Set checked state of each item
                for (var x = 0; x < filteredFoods.Length; x++)
                {
                    if (foodEnabled[filteredFoods[x].id]) cklFoods.SetItemChecked(x, true);
                }

                //Recover selection
                if (wasSelected != null)
                {
                    var toSelect = Array.FindIndex(filteredFoods, p => p.id == wasSelected.id);
                    if (toSelect >= 0) cklFoods.SetSelected(toSelect, true);
                }
            }
        }

        private void btnRareNutrients_Click(object sender, EventArgs e)
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
            lblFoodDetail.Text = sb.ToString();
        }

        private void btnTargets_Click(object sender, EventArgs e)
        {
            var dlg = new NutrientTargetOverrideDialog(nutrients, targets, targetOverrides);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                targetOverrides = dlg.GetTargetOverrides();
            }
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

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (solver != null) solver.Stop();
            LoadInstance();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (solver != null) solver.Stop();
            SaveInstance();
        }

        private void cboFilterByGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (tmrFilter)
            {
                pauseToFilter = 1;
                tmrFilter.Enabled = true;
            }
        }
    }
}
