using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Units
{
    public static class UnitData
    {
        public static List<Unit> Units { get; set; } = new List<Unit>();
        public static List<Rule> Rules { get; set; } = new List<Rule>();

        public static Rule GetRule(Unit firstUnit, Unit secondUnit)
        {
            return Rules.FirstOrDefault(r => r.FirstUnit.Characters == firstUnit.Characters &&
                                             r.SecondUnit.Characters == secondUnit.Characters);
        }

        public static void Load(string unitsFilePath, string rulesFilePath)
        {
            LoadUnits(unitsFilePath);
            LoadRules(rulesFilePath);
        }

        private static void LoadUnits(string unitsFilePath)
        {
            var unitsRaw = File.ReadAllLines(unitsFilePath);

            foreach (var unitRaw in unitsRaw)
            {
                Unit newUnit = new Unit();
                newUnit.Characters = unitRaw.Substring(0, 2).Trim();
                newUnit.MustNotBeginSyllable = GetBoolean(unitRaw.Substring(3, 1));
                newUnit.MustNotEndWordAsOnlyVowel = GetBoolean(unitRaw.Substring(4, 1));
                newUnit.IsVowel = GetBoolean(unitRaw.Substring(5, 1));
                newUnit.IsAlternateVowel = GetBoolean(unitRaw.Substring(6, 1));
                Units.Add(newUnit);
            }
        }

        private static void LoadRules(string rulesFilePath)
        {
            var rulesRaw = File.ReadAllLines(rulesFilePath);

            foreach(var ruleRaw in rulesRaw)
            {
                Rule newRule = new Rule();
                var firstUnitRaw = ruleRaw.Substring(5, 2).Trim();
                var secondUnitRaw = ruleRaw.Substring(8, 2).Trim();
                newRule.FirstUnit = Units.FirstOrDefault(u => u.Characters == firstUnitRaw);
                newRule.SecondUnit = Units.FirstOrDefault(u => u.Characters == secondUnitRaw);
                newRule.MustBegin = GetBoolean(ruleRaw.Substring(0, 1));
                newRule.MustNotBegin = GetBoolean(ruleRaw.Substring(1, 1));
                newRule.NeedsBreak = GetBoolean(ruleRaw.Substring(2, 1));
                newRule.NeedsVowelPrefix = GetIsPrefix(ruleRaw.Substring(3, 1));
                newRule.IllegalOrNeedsVowelSuffix = GetLegality(ruleRaw.Substring(12, 1));
                newRule.MustEnd = GetBoolean(ruleRaw.Substring(13, 1));
                newRule.MustNotEnd = GetBoolean(ruleRaw.Substring(14, 1));
                Rules.Add(newRule);
            }
        }

        private static bool GetBoolean(string input) => input == "1" ? true : false;

        private static bool GetIsPrefix(string input) => input == "-" ? true : false;

        private static Legality GetLegality(string input)
        {
            if (input == "+")
                return Legality.IllegalPair;
            else if (input == "-")
                return Legality.NeedsVowelSuffix;
            else
                return Legality.None;
        }
    }
}