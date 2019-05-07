using System.Collections.Generic;
using System.Linq;
using System;

namespace Units
{
    public class Word
    {
        public List<Unit> Units { get; set; } = new List<Unit>();

        public bool IsValid() => GetSyllables() != null;

        private List<Syllable> GetSyllables()
        {
            var currentSyllable = new Syllable();
            var syllables = new List<Syllable>() { currentSyllable };
            
            foreach (var unit in Units)
            {
                currentSyllable.Units.Add(unit);

                var syllableValidationResult = currentSyllable.IsValid();
                if (syllableValidationResult != Validation.Valid)
                {
                    if (syllableValidationResult == Validation.Fail) return null;

                    if (syllableValidationResult == Validation.AddUnit) continue;

                    if (syllableValidationResult == Validation.SplitSyllable)
                    {
                        currentSyllable.Units.Remove(unit);
                        
                        currentSyllable = AddNewSyllable(syllables, unit);
                        var newSyllableValidationResult = currentSyllable.IsValid();

                        if (newSyllableValidationResult != Validation.Valid || 
                            newSyllableValidationResult != Validation.AddUnit) return null;
                    }
                }
            }

            if (!syllables.All(s => s.IsValid() == Validation.Valid)) return null;

            return syllables;
        }

        private Syllable AddNewSyllable(List<Syllable> syllables, Unit currentUnit)
        {
            var newSyllable = new Syllable();
            newSyllable.Units.Add(currentUnit);
            syllables.Add(newSyllable);
            return newSyllable;
        }

        public override string ToString() => string.Concat(Units.Select(u => u.Characters));
    }
}