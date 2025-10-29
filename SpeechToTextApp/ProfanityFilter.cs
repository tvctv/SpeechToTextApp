using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SpeechToTextApp
{
    public class ProfanityFilter
    {
        private readonly List<string> _badWords = new()
        {
            "damn","hell","shit","fuck","bitch","asshole","bastard","dick","piss","crap"
        };

        private readonly List<Regex> _patterns;

        public ProfanityFilter()
        {
            _patterns = new List<Regex>();
            foreach (var w in _badWords)
            {
                _patterns.Add(new Regex(@"\b" + Regex.Escape(w) + @"\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
            }
        }

        public string Clean(string input)
        {
            var result = input;
            foreach (var rx in _patterns)
            {
                result = rx.Replace(result, m => new string('•', m.Value.Length));
            }
            return result;
        }
    }
}