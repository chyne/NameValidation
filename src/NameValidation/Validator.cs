namespace NameValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Validator
    {
        private readonly IDictionary<string, ICollection<string>> _alternates;

        public Validator(IEnumerable<ICollection<string>> alternates)
        {
            if (alternates == null)
            {
                throw new ArgumentNullException(nameof(alternates));
            }
            _alternates = new Dictionary<string, ICollection<string>>();

            foreach (var grouping in alternates)
            {
                foreach (var name in grouping)
                {
                    _alternates.Add(name, grouping);
                }
            }
        }

        public RecordValidationResult Validate(InputRecord input, RegistryRecord registry)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var result = new RecordValidationResult()
            {
                GivenNameResults = input.GivenNames.SelectMany(name => Validate(GetAlternates(name), registry.GiveNames, name)).ToList(),
                SurnameResults = input.Surnames.SelectMany(name => Validate(new[] { name }, registry.Surnames, name)).ToList()
            };

            return result;
        }

        public List<ValidationResult> Validate(IEnumerable<string> inputNames, ICollection<RegistryName> registryNames, string original)
        {
            if (inputNames == null)
            {
                throw new ArgumentNullException(nameof(inputNames));
            }

            if (registryNames == null)
            {
                throw new ArgumentNullException(nameof(registryNames));
            }

            var result = new List<ValidationResult>();

            foreach (var inputName in inputNames)
            {
                foreach (var registryName in registryNames)
                {
                    result.Add(new ValidationResult
                    {
                        NameFromInput = original,
                        NameVariation = inputName,
                        NameFromRegistry = registryName.Name,
                        Distance = Levenshtein(inputName, registryName.Name),
                        NameType = registryName.NameType,
                        IsInitialMatch = inputName.Length == 1 && registryName.Name.StartsWith(inputName, StringComparison.InvariantCultureIgnoreCase)
                    });
                }
            }

            return result;
        }

        private int Levenshtein(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        private IEnumerable<string> GetAlternates(string name)
        {
            return _alternates.ContainsKey(name) ? _alternates[name] : new[] { name };
        }
    }
}
