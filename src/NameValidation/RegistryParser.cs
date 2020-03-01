namespace NameValidation
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class RegistryParser
    {
        public RegistryRecord Parse(string givenNames, string surnames)
        {
            if (givenNames == null)
            {
                throw new ArgumentNullException(nameof(givenNames));
            }

            if (surnames == null)
            {
                throw new ArgumentNullException(nameof(surnames));
            }

            return new RegistryRecord
            {
                GiveNames = Parse(givenNames.ToUpperInvariant()),
                Surnames = Parse(surnames.ToUpperInvariant())
            };
        }

        public IList<RegistryName> Parse(string name)
        {
            var results = new List<RegistryName>();

            var matches = Regex.Matches(name, @"([cao]\[.*?)(?=[cao]\[|$)", RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                var thing = match.Groups[0].Value;
                var nameType = GetNameType(thing);
                var names = Regex.Matches(thing, @"(\[(.*?)\])", RegexOptions.IgnoreCase);

                foreach (Match nameThing in names)
                {
                    var other = nameThing.Groups[2].Value;

                    if (!string.IsNullOrWhiteSpace(other))
                    {
                        results.Add(new RegistryName(other, nameType));
                    }
                }
            }

            return results;
        }

        private NameType GetNameType(string group)
        {
            switch (group[0])
            {
                case 'C':
                    return NameType.Current;
                case 'A':
                    return NameType.Alternate;
                case 'O':
                    return NameType.Original;
            }
            throw new Exception();
        }
    }
}