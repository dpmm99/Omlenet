using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Omlenet.USDAFormat;

namespace Omlenet
{
    public static class ProgramState
    {
        public static string programTitle;
        public static GASolver solver;
        public static List<FoodNutrient> foodNutrients;
        public static List<FoodGroup> foodGroups;
        public static List<FoodDescription> foodDescs;
        public static Dictionary<int, bool> foodEnabled;
        public static List<Nutrient> nutrients;
        public static List<NutrientTarget> targets;
        public static List<NutrientTarget> targetOverrides = new List<NutrientTarget>();
        public static byte bodyType; //For the sake of serialization/deserialization
        public static SolverState solverState = SolverState.Loading;
        public static int targetFoodUnits;

        public enum SolverState
        {
            Loading,
            Ready,
            Running,
            Completed,
        }

        private const uint MAGIC_NUMBER = 0x0b1ebe7;
        private const ushort FILE_VERSION = 0;
        public static void SerializeInstance(BinaryWriter bw)
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
            //Save target food units
            bw.Write(targetFoodUnits);

            //Save list of disabled foods
            bw.Write(foodEnabled.Count(p => !p.Value));
            foreach (var f in foodEnabled.Where(p => !p.Value).Select(p => foodDescs.First(q => q.id == p.Key)))
            {
                bw.Write(f.id);
            }

            //Save whether the GA has been executed
            bw.Write(solver != null && solver.executed);

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

        public static void DeserializeInstance(BinaryReader br)
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
            //Load target food units
            targetFoodUnits = br.ReadInt32();

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
        
        public static List<NutrientTarget> GetTrueTargets()
        {
            return targetOverrides
                .Concat(targets.Where(p => p.bodyType == bodyType && !targetOverrides.Any(q => q.nutrientId == p.nutrientId)))
                .OrderBy(p => p.nutrientId).ToList();
        }

        public static void InitGASolver()
        {
            var goodFoods = foodDescs.Where(p => foodEnabled[p.id]).ToList();

            //Use the overrides and fill in the gaps with targets for the selected body type
            var trueTargets = GetTrueTargets();

            solver = new GASolver(goodFoods, trueTargets, nutrients, foodNutrients, targetFoodUnits);
        }

        public static void LoadData(string path, Action<int, int> increaseLoadingProgress)
        {
            var lineCount = 0;
            var expectedLineCount = 7794 + 26 + 644126 + 150 + 150; //It's okay if they change a bit, but this is how many lines were in those files at the start

            foodDescs = loadAsList(FoodDescription.FromStream, path + "FOOD_DES.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            foodGroups = loadAsList(FoodGroup.FromStream, path + "FD_GROUP.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            foodNutrients = loadAsList(FoodNutrient.FromStream, path + "NUT_DATA.txt", ref lineCount, expectedLineCount, increaseLoadingProgress)
                .Where(p => p.nutrientAmount != 0) //It's kinda silly that these are even included. Save a bunch of time by ignoring them (roughly 1/3 of the whole file).
                .OrderBy(p => p.foodId).ToList();
            nutrients = loadAsList(Nutrient.FromStream, path + "NUTR_DEF.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            targets = loadAsList(NutrientTarget.FromStream, path + "Targets.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
        }

    }
}
