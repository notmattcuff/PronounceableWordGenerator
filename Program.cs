using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Units;

namespace RandomWordGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = new WordGenerator().GetWords(4, "Units.txt", "Rules.txt");
            File.WriteAllLines("4LetterTest.txt", words);
        }
    }
}