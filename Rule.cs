    
namespace Units
{
    public class Rule
    {
        public Unit FirstUnit { get; set; }
        public Unit SecondUnit { get; set; }
        public bool MustBegin { get; set; }
        public bool MustNotBegin { get; set; }
        public bool NeedsBreak { get; set; }
        public bool NeedsVowelPrefix { get; set; }
        public Legality IllegalOrNeedsVowelSuffix { get; set; }
        public bool MustEnd { get; set; }
        public bool MustNotEnd { get; set; }
    }

    public enum Legality
    {
        IllegalPair,
        NeedsVowelSuffix,
        None
    }
}