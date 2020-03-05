namespace NameValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Parser
    {
        public InputRecord Parse(string givenNames, string surnames)
        {
            return new InputRecord
            {
                GivenNames = ParseSurnames(givenNames),
                Surnames = ParseSurnames(surnames)
            };
        }

        public IList<string> ParseGivenNames(string value)
        {
            value = RemoveBrackets(value);
            value = RemoveTitles(value);
            value = RemoveNonNameCharacters(value);

            value = Regex.Replace(value, @"[-']", string.Empty);

            return value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.ToUpperInvariant()).ToArray();
        }

        public IList<string> ParseSurnames(string value)
        {
            value = RemoveBrackets(value);
            value = RemoveTitles(value);
            value = RemoveNonNameCharacters(value);

            value = Regex.Replace(value, @"[']", string.Empty);
            value = Regex.Replace(value, @"[-]", " ");

            var names = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.ToUpperInvariant()).ToList();
            var combined = string.Join(string.Empty, names);

            names.Add(combined);

            return names;
        }

        private string RemoveBrackets(string value)
        {
            value = Regex.Replace(value, @"{[^}]*}", " ");
            value = Regex.Replace(value, @"\[[^\]]*\]", " ");
            value = Regex.Replace(value, @"\([^)]*\)", " ");

            return value;
        }

        private string RemoveTitles(string value)
        {
            value = Regex.Replace(value, @"\b(jr|sr|jnr|snr)\.?\b", " ", RegexOptions.IgnoreCase);
            return value;
        }

        private string RemoveNonNameCharacters(string value)
        {
            value = Regex.Replace(value, @"[^a-zA-Z\-']", " ");
            return value;
        }
    }
}