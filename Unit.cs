namespace Units
{
    public class Unit
    {
        public string Characters { get; set; }
        public bool MustNotBeginSyllable { get; set; }
        public bool MustNotEndWordAsOnlyVowel { get; set; }
        public bool IsVowel { get; set; }
        public bool IsAlternateVowel { get; set; }
    }
}