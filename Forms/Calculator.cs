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
using static Omlenet.ProgramState;

namespace Omlenet
{
    public partial class Calculator : ResultsPanel
    {
        public List<FoodAmount> Foods = new List<FoodAmount>();
        private int selectedFoodId = -1;

        public Calculator()
        {
            InitializeComponent();
            Resize -= ResultsPanel_Resize;
            Resize += Calculator_Resize;
        }

        private const uint MAGIC_NUMBER = 0x0b1ca1c;
        private const ushort FILE_VERSION = 0;
        public void Serialize(BinaryWriter bw)
        {
            bw.Write(MAGIC_NUMBER);
            bw.Write(FILE_VERSION);

            bw.Write(Foods.Count);
            foreach (var food in Foods)
            {
                bw.Write(food.amount);
                bw.Write(food.foodId);
            }
        }

        public void Deserialize(BinaryReader br)
        {
            if (br.ReadUInt32() != MAGIC_NUMBER) throw new Exception("Magic number mismatch--is this an " + programTitle + " file?");
            var fileVersion = br.ReadUInt16();
            if (fileVersion > FILE_VERSION) throw new Exception("This file appears to have been created by a newer version of " + programTitle + " and cannot be loaded.");

            var foodCount = br.ReadInt32();
            Foods = new List<FoodAmount>(foodCount);
            for (var x = 0; x < foodCount; x++)
            {
                var amount = br.ReadSingle(); //I should have saved in the other order, but oh well.
                Foods.Add(new FoodAmount(br.ReadInt32(), amount));
            }

            Calculate();
        }

        public void OnFoodSelectionChanged(int foodId)
        {
            if (programmaticUpdate) return;
            selectedFoodId = foodId;
            programmaticUpdate = true;
            nudFoodMass.Value = (decimal)(Foods.FirstOrDefault(p => p.foodId == selectedFoodId)?.amount ?? 0);
            programmaticUpdate = false;
        }

        private float[] FoodsToNutrients()
        {
            var nutrientAmounts = new float[nutrients.Max(p => p.id) + 1];
            foreach (var f in Foods)
            {
                foreach (var n in f.GetNutrients())
                {
                    nutrientAmounts[n.nutrientId] += n.nutrientAmount * f.amount * 0.01f;
                }
            }
            return nutrientAmounts;
        }

        protected int GetSelectedResultListItemId(DataGridView dgv)
        {
            return dgv.SelectedRows.Count == 0 ? -1 : (int)dgv.SelectedRows[0].Cells[0].Value; //Assumes the first cell is the ID
        }

        protected void SetSelectedResultListItemId(DataGridView dgv, int id)
        {
            if (id != -1)
            {
                for (var x = 0; x < dgv.Rows.Count; x++)
                {
                    if ((int)dgv.Rows[x].Cells[0].Value == id)
                    {
                        dgv.Rows[x].Cells[0].Selected = true;
                        break;
                    }
                }
            }
        }

        public void Calculate()
        {
            programmaticUpdate = true;
            var totalNutrients = FoodsToNutrients();
            var scorer = new Scorer(GetTrueTargets(), (float)nudDays.Value, totalNutrients, nutrients);

            var totalScore = scorer.Score(totalNutrients);
            var scoreSpace = (float[])null;
            var foodResults = new List<ResultListItem>();
            var nutrientResults = new List<ResultListItem>();

            //Score each food individually
            foreach (var food in Foods)
            {
                var result = new ResultListItem();
                result.Id = food.foodId;
                result.Mass = food.amount;
                result.Name = food.GetFood().longDesc;
                result.Cost = scorer.ScoreDifference(totalScore, totalNutrients, food.GetNutrients(), food.amount * -0.01f, ref scoreSpace);
                foodResults.Add(result);
            }
            var oldSelectedId = GetSelectedResultListItemId(dgvFoods);
            FoodList = foodResults;
            SetSelectedResultListItemId(dgvFoods, oldSelectedId); //Restore old selection if possible

            //Score each nutrient individually
            for (ushort x = 0; x < totalNutrients.Length; x++)
            {
                var result = new ResultListItem();
                result.Id = x;
                result.Mass = totalNutrients[x];
                result.Name = nutrients.First(p => p.id == x).name;
                var nutrientChange = new FoodNutrient[] { new FoodNutrient { nutrientId = x, nutrientAmount = totalNutrients[x] } };
                result.Cost = scorer.ScoreDifference(totalScore, totalNutrients, nutrientChange, -1f, ref scoreSpace);
                nutrientResults.Add(result);
            }
            oldSelectedId = GetSelectedResultListItemId(dgvNutrients);
            NutrientList = nutrientResults;
            SetSelectedResultListItemId(dgvNutrients, oldSelectedId); //Restore old selection if possible

            ResultText = "Cost: " + Math.Round(totalScore, 1) + Environment.NewLine +
                string.Join(Environment.NewLine, nutrients.Select(p => p.name + ": " + Math.Round(totalNutrients[p.id], 2) + p.unitOfMeasure +
                (targets.Any(q => q.nutrientId == p.id && q.target > 0) ? " (" + Math.Round(totalNutrients[p.id] / scorer.Targets.First(q => q.nutrientId == p.id).target * 100, 1) + "% of target)" : ""))) +
                Environment.NewLine +
                string.Join(Environment.NewLine, Foods.Select(p => Math.Round(p.amount, 2) + "g of " + p.GetFood().longDesc)); //TODO: List nutrients per food, too
            programmaticUpdate = false;
        }

        private bool programmaticUpdate = false;
        private void nudFoodMass_ValueChanged(object sender, EventArgs e)
        {
            if (programmaticUpdate) return;
            if (selectedFoodId == -1) return;
            var food = Foods.FirstOrDefault(p => p.foodId == selectedFoodId);
            if (food == null) Foods.Add(new FoodAmount(selectedFoodId, (float)nudFoodMass.Value));
            else food.amount = (float)nudFoodMass.Value;
            if (nudFoodMass.Value == 0) Foods.Remove(food);

            Calculate();
        }

        private void nudDays_ValueChanged(object sender, EventArgs e)
        {
            if (programmaticUpdate) return;
            Calculate();
        }

        protected void Calculator_Resize(object sender, EventArgs e)
        {
            tabControl1.Height = this.ClientSize.Height - nudDays.Top - nudDays.Height - 3;
        }

        //TODO: The reset button needs to be enabled always
    }
}
