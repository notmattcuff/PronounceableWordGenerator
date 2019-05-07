using System.Collections.Generic;
using System.Linq;
using System;

namespace Units
{
    public class Syllable
    {
        public List<Unit> Units { get; set; } = new List<Unit>();

        public Validation IsValid()
        {
            if (Units.Count == 0) return Validation.Fail;

            if (Units.FirstOrDefault().MustNotBeginSyllable) return Validation.Fail;

            if (GetVowelCount(this) == 0) return Validation.AddUnit;

            if (GetVowelCount(this) > 2) return Validation.SplitSyllable;

            Unit previousUnit = null;
            var foundVowel = false;
            var firstPair = true;
            var needsVowelSuffix = false;

            foreach (var currentUnit in Units)
            {
                if (needsVowelSuffix && !currentUnit.IsVowel) return Validation.AddUnit;

                if (needsVowelSuffix && currentUnit.IsVowel) needsVowelSuffix = false;

                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.IllegalOrNeedsVowelSuffix == Legality.IllegalPair) return Validation.SplitSyllable;

                    if (foundVowel && !previousUnit.IsVowel && currentUnit.IsVowel) return Validation.SplitSyllable;

                    if (currentUnit.IsVowel) foundVowel = true;

                    if (previousUnit.IsVowel && currentUnit.IsAlternateVowel) return Validation.SplitSyllable;

                    if (!firstPair && rule.MustBegin) return Validation.SplitSyllable;

                    if (firstPair && rule.MustNotBegin) return Validation.Fail;

                    if (rule.NeedsBreak) return Validation.SplitSyllable;

                    if (rule.NeedsVowelPrefix && !previousUnit.IsVowel) return Validation.Fail;

                    if (rule.IllegalOrNeedsVowelSuffix == Legality.NeedsVowelSuffix) needsVowelSuffix = true;

                    if (rule.MustEnd && currentUnit != Units.LastOrDefault()) return Validation.SplitSyllable;

                    if (rule.MustNotEnd && currentUnit == Units.LastOrDefault()) return Validation.AddUnit;

                    if (firstPair) firstPair = false;
                }

                previousUnit = currentUnit;
            }

            if (needsVowelSuffix) return Validation.AddUnit;

            return Validation.Valid;
        }

        private bool ValidateNoUnits(Syllable syllable)
        {
            return syllable.Units.Count > 0;
        }

        private bool ValidateUnitMustNotBeginSyllable(Syllable syllable)
        {
            return !syllable.Units.FirstOrDefault().MustNotBeginSyllable;
        }

        private bool ValidateIllegalPairs(Syllable syllable)
        {
            Unit previousUnit = null;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.IllegalOrNeedsVowelSuffix == Legality.IllegalPair) return false;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        private bool ValidateHasVowel(Syllable syllable)
        {
            return GetVowelCount(syllable) > 0;
        }

        private bool ValidateMoreThanTwoVowels(Syllable syllable)
        {
            return GetVowelCount(syllable) <= 2;
        }

        private bool ValidateAllVowelsConsecutive(Syllable syllable)
        {
            Unit previousUnit = null;
            var foundVowel = false;

            foreach (var currentUnit in syllable.Units)
            {
                if (foundVowel && previousUnit != null && !previousUnit.IsVowel && currentUnit.IsVowel)
                {
                    return false;
                }

                if (currentUnit.IsVowel)
                {
                    foundVowel = true;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        private bool ValidateAlternateVowel(Syllable syllable)
        {
            Unit previousUnit = null;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null && previousUnit.IsVowel && currentUnit.IsAlternateVowel)
                {
                    return false;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        private int GetVowelCount(Syllable syllable)
        {
            return syllable.Units.Count(u => u.IsVowel);
        }

        private bool ValidateMustBegin(Syllable syllable)
        {
            Unit previousUnit = null;
            var firstPair = true;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null && !firstPair)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.MustBegin) return false;
                }

                if (previousUnit != null && firstPair) firstPair = false;

                previousUnit = currentUnit;
            }

            return true;
        }

        private bool ValidateMustNotBegin(Syllable syllable)
        {
            Unit previousUnit = null;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.MustNotBegin) return false;

                    return true;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        private bool ValidateNeedsBreak(Syllable syllable)
        {
            Unit previousUnit = null;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.NeedsBreak) return false;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        private bool ValidateNeedsVowelPrefix(Syllable syllable)
        {
            Unit previousUnit = null;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.NeedsVowelPrefix && !previousUnit.IsVowel) return false;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        private bool ValidateNeedsVowelSuffix(Syllable syllable)
        {
            Unit previousUnit = null;
            var needsVowelSuffix = false;

            foreach (var currentUnit in syllable.Units)
            {
                if (needsVowelSuffix && !currentUnit.IsVowel) return false;

                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.IllegalOrNeedsVowelSuffix == Legality.NeedsVowelSuffix) needsVowelSuffix = true;
                }

                previousUnit = currentUnit;
            }

            return !needsVowelSuffix;
        }

        private bool ValidateMustEnd(Syllable syllable)
        {
            Unit previousUnit = null;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.MustEnd && currentUnit != syllable.Units.LastOrDefault()) return false;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        private bool ValidateMustNotEnd(Syllable syllable)
        {
            Unit previousUnit = null;

            foreach (var currentUnit in syllable.Units)
            {
                if (previousUnit != null)
                {
                    var rule = UnitData.GetRule(previousUnit, currentUnit);
                    if (rule.MustNotEnd && currentUnit == syllable.Units.LastOrDefault()) return false;
                }

                previousUnit = currentUnit;
            }

            return true;
        }

        public override string ToString() => string.Concat(Units.Select(u => u.Characters));
    }
}