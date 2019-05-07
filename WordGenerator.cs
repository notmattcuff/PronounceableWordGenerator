using System.Linq;
using System.Collections.Generic;
using System;

namespace Units
{
    public class WordGenerator
    {
        public List<string> GetWords(int wordLength, string unitFilePath, string rulesFilePath)
        {
            UnitData.Load(unitFilePath, rulesFilePath);
            return GetWordsRecursively(UnitData.Units, wordLength, new Word())
                  .Where(word => word.ToString().Length == wordLength && word.IsValid())
                  .Select(word => word.ToString())
                  .ToList();
        }

        private List<Word> GetWordsRecursively(List<Unit> units, int currentLengthRemaining, Word currentWord)
        {
            var words = new List<Word>();
            if (currentLengthRemaining <= 0)
            {
                words.Add(currentWord);
                return words;
            }
            
            for (int i = 0; i < units.Count; i++)
            {
                var newWord = new Word();
                newWord.Units.AddRange(currentWord.Units);
                newWord.Units.Add(units[i]);
                words.AddRange(GetWordsRecursively(units, currentLengthRemaining - units[i].Characters.Length, newWord));
            }

            return words;
        }
    }
}