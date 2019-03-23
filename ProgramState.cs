using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Omlenet.USDAFormat;

namespace Omlenet
{
    public static class ProgramState
    {
        public static string programTitle;
        public static GASolver solver;
        public static List<FoodNutrient> foodNutrients;
        public static Dictionary<int, List<FoodNutrient>> foodNutrientDict;
        public static List<FoodGroup> foodGroups;
        public static List<FoodDescription> foodDescs;
        public static HashSet<int> foodEnabled;
        public static List<Nutrient> nutrients;
        public static List<NutrientTarget> targets;
        public static List<NutrientTarget> targetOverrides = new List<NutrientTarget>();
        public static HashSet<int> foodLocked = new HashSet<int>();
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
        private const ushort FILE_VERSION = 1;
        public static void SerializeInstance(BinaryWriter bw)
        {
            bw.Write(MAGIC_NUMBER);
            bw.Write(FILE_VERSION);

            bw.Write(bodyType);

            //Save nutrient target overrides
            bw.Write(targetOverrides.Count);
            foreach (var o in targetOverrides)
            {
                bw.Write(nutrientInternalIdToUsdaIdMapping[o.nutrientId]); //Remap to USDA nutrient IDs for saving
                bw.Write(o.min);
                bw.Write(o.target);
                bw.Write(o.max);
                bw.Write(o.costUnder);
                bw.Write(o.costOver);
            }
            //Save target food units
            bw.Write(targetFoodUnits);

            //Save list of disabled foods
            bw.Write(foodDescs.Count - foodEnabled.Count);
            foreach (var f in foodDescs.Where(p => !foodEnabled.Contains(p.id)))
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

            //Save locked foods
            bw.Write(foodLocked.Count);
            foreach (var foodId in foodLocked)
            {
                bw.Write(foodId);
            }
        }

        public static void DeserializeInstance(BinaryReader br)
        {
            if (br.ReadUInt32() != MAGIC_NUMBER) throw new Exception("Magic number mismatch--is this an " + programTitle + " file?");
            var fileVersion = br.ReadUInt16();
            if (fileVersion > FILE_VERSION) throw new Exception("This file appears to have been created by a newer version of " + programTitle + " and cannot be loaded.");

            bodyType = br.ReadByte();

            //Load nutrient target overrides
            var overrideCount = br.ReadInt32();
            targetOverrides = new List<NutrientTarget>();
            for (var x = 0; x < overrideCount; x++)
            {
                targetOverrides.Add(new NutrientTarget
                {
                    nutrientId = nutrientUsdaIdToInternalIdMapping[br.ReadUInt16()], //Remap from USDA to internal ID
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
            foodEnabled = new HashSet<int>(foodDescs.Select(p => p.id));
            var disabledFoodCount = br.ReadInt32();
            for (var x = 0; x < disabledFoodCount; x++)
            {
                foodEnabled.Remove(br.ReadInt32());
            }

            //Load whether the GA has been executed
            var executed = br.ReadBoolean();

            //Load winner's foods
            var solverFoodCount = br.ReadInt32();
            var foodCountsDict = new Dictionary<int, int>();
            for (var x = 0; x < solverFoodCount; x++)
            {
                var id = br.ReadInt32();
                var count = br.ReadInt32();
                foodCountsDict.Add(id, count);
            }

            if (fileVersion > 0)
            {
                //Load locked foods
                var lockedFoodCount = br.ReadInt32();
                foodLocked = new HashSet<int>();
                for (var x = 0; x < lockedFoodCount; x++)
                {
                    foodLocked.Add(br.ReadInt32());
                }
            }
            else
            {
                foodLocked = new HashSet<int>();
            }

            InitGASolver();
            solver.executed = executed;
            solver.SetFoods(foodCountsDict);
        }
        
        public static List<NutrientTarget> GetTrueTargets()
        {
            return targetOverrides
                .Concat(targets.Where(p => p.bodyType == bodyType && !targetOverrides.Any(q => q.nutrientId == p.nutrientId)))
                .OrderBy(p => p.nutrientId).ToList();
        }

        public static void InitGASolver()
        {
            var goodFoods = foodDescs.Where(p => foodEnabled.Contains(p.id)).ToList();

            //Use the overrides and fill in the gaps with targets for the selected body type
            var trueTargets = GetTrueTargets();

            solver = new GASolver(goodFoods, trueTargets, nutrients, foodNutrientDict, foodLocked, targetFoodUnits);
        }

        private static ushort[] nutrientUsdaIdToInternalIdMapping;
        private static ushort[] nutrientInternalIdToUsdaIdMapping;
        private static void RemapNutrientIDs() //Affects the speed of scoring by something like 2.5%
        {
            //Need to modify foodNutrients, nutrients, and targets.
            //First, generate a mapping (both ways because we may as well save with the USDA IDs)
            nutrientUsdaIdToInternalIdMapping = new ushort[nutrients.Max(p => p.id) + 1];
            nutrientInternalIdToUsdaIdMapping = new ushort[nutrients.Count];
            for (ushort x = 0; x < nutrients.Count; x++)
            {
                nutrientUsdaIdToInternalIdMapping[nutrients[x].id] = x;
                nutrientInternalIdToUsdaIdMapping[x] = nutrients[x].id;
            }

            //Then switch out all the new IDs
            foreach (var fn in foodNutrients) fn.nutrientId = nutrientUsdaIdToInternalIdMapping[fn.nutrientId];
            foreach (var t in targets) t.nutrientId = nutrientUsdaIdToInternalIdMapping[t.nutrientId];
            foreach (var n in nutrients) n.id = nutrientUsdaIdToInternalIdMapping[n.id];
            //The foodNutrientDict is already remapped because it's the same objects as foodNutrients

            //TODO: All the displays that show nutrient IDs are now showing the internal IDs. Is that acceptable? Do I even need them to show in the first place?
        }

        public static void LoadData(string path, Action<int, int> increaseLoadingProgress)
        {
            var lineCount = 0;
            var expectedLineCount = 7794 + 26 + 644126 + 150 + 150; //It's okay if they change a bit, but this is how many lines were in those files at the start

            foodNutrients = loadAsList(FoodNutrient.FromStream, path + "NUT_DATA.txt", ref lineCount, expectedLineCount, increaseLoadingProgress)
                .Where(p => p.nutrientAmount != 0) //It's kinda silly that these are even included. Save a bunch of time by ignoring them (roughly 1/3 of the whole file).
                .OrderBy(p => p.foodId).ToList();

            //Turn that data into a dictionary on a separate thread for the sake of speed
            var cpuThread = new Thread(GenerateNutrientDictionary);
            cpuThread.Start();

            foodDescs = loadAsList(FoodDescription.FromStream, path + "FOOD_DES.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            foodGroups = loadAsList(FoodGroup.FromStream, path + "FD_GROUP.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            nutrients = loadAsList(Nutrient.FromStream, path + "NUTR_DEF.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);
            targets = loadAsList(NutrientTarget.FromStream, path + "Targets.txt", ref lineCount, expectedLineCount, increaseLoadingProgress);

            //Overwrite the USDA IDs so I can make everything into dense arrays with the index being the ID
            RemapNutrientIDs();

            //Wait for the dictionary to finish being generated
            cpuThread.Join();
        }

        private static void GenerateNutrientDictionary()
        {
            foodNutrientDict = new Dictionary<int, List<FoodNutrient>>(10000);
            var lastList = (List<FoodNutrient>)null;
            var lastFoodId = -1;
            //Basically if GroupBy assumed the IEnumerable is already grouped, this would be equivalent to foodNutrients.GroupBy(p => p.foodId).ToDictionary()
            for (var x = 0; x < foodNutrients.Count; x++)
            {
                if (foodNutrients[x].foodId != lastFoodId)
                {
                    foodNutrientDict.Add(lastFoodId = foodNutrients[x].foodId, lastList = new List<FoodNutrient>());
                }
                lastList.Add(foodNutrients[x]);
            }
        }

    }
}
