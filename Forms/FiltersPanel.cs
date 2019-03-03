﻿using System;
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
    public partial class FiltersPanel : DockContent
    {
        private int pauseToFilter;
        public Action<int> UpdateDetails; //Passes in a food ID

        public FiltersPanel()
        {
            InitializeComponent();
        }

        public void Ready()
        {
            cklFoods.Items.AddRange(foodDescs.ToArray());
            foodEnabled = new Dictionary<int, bool>();
            for (var x = 0; x < foodDescs.Count; x++)
            {
                cklFoods.SetItemChecked(x, true);
                foodEnabled[foodDescs[x].id] = true;
            }

            cboFilterByGroup.Items.AddRange(new List<FoodGroup> { new FoodGroup { id = 0, name = "Show All" } }.Concat(foodGroups.OrderBy(p => p.name)).ToArray());
            cboFilterByGroup.SelectedIndex = 0;
        }

        public void FilterNow()
        {
            lock (tmrFilter)
            {
                pauseToFilter = 1;
                tmrFilter.Enabled = true;
            }
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


        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            lock (tmrFilter) //Mutex prevents the timer event from firing again while it's still processing when I'm debugging or on ultra slow machines
            {
                pauseToFilter = 5;
                tmrFilter.Enabled = true;
            }
        }

        private void cboFilterByGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            lock (tmrFilter)
            {
                pauseToFilter = 1;
                tmrFilter.Enabled = true;
            }
        }

        private void cklFoods_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDetails(((FoodDescription)cklFoods.SelectedItem).id);
        }

        private void cklFoods_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            foodEnabled[(cklFoods.Items[e.Index] as FoodDescription).id] = (e.NewValue == CheckState.Checked);
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

        private void FiltersPanel_Resize(object sender, EventArgs e)
        {
            cklFoods.Height = Math.Max(15, groupBox1.Height - panel1.Height - cklFoods.Top - 1);
            btnCheckShown.Left = (panel1.Width - btnCheckShown.Width) / 2;
            txtFilter.Width = Math.Max(panel1.Width - txtFilter.Left - 3, 30);
            cboFilterByGroup.Width = txtFilter.Width;
        }
    }
}
