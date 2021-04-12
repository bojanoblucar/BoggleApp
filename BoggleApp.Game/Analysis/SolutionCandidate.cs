using System;
using System.Collections.Generic;
using System.Text;

namespace BoggleApp.Game.Analysis
{
    public class SolutionCandidate
    {
        private readonly string value;

        private int index = 0;

        public SolutionCandidate(string value)
        {
            this.value = value;
        }


        public string GetNextLetter()
        {
            if (!HasNextLetter())
                return null;

            var letter = value[index].ToString();

            bool isTwoLetterSpecialCase = false;
            if (
                ((letter.ToLower() == "n" || letter.ToLower() == "l") && value.Length > index + 1 && value[index + 1].ToString().ToLower() == "j")
                || (letter.ToLower() == "d" && value.Length > index + 1 && value[index + 1].ToString().ToLower() == "ž"))

            {
                isTwoLetterSpecialCase = true;
                letter = value.Substring(index, 2);
            }

            index += isTwoLetterSpecialCase ? 2 : 1;
            return letter;
        }

        public bool HasNextLetter()
        {
            return value.Length > index;
        }
    }
}
