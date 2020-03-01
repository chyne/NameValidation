namespace NameValidation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RecordValidationResult
    {
        public ICollection<ValidationResult> GivenNameResults { get; set; } = new List<ValidationResult>();
        public ICollection<ValidationResult> SurnameResults { get; set; } = new List<ValidationResult>();

        public ValidationResult BestSurnameResult => SurnameResults.OrderBy(r => r.NameType).Max();
        public ICollection<ValidationResult> BestGivenNameResults => GivenNameResults.GroupBy(r => r.NameFromInput).Select(g => g.OrderBy(r => r.NameType).Max()).ToList();

        public decimal Score()
        {
            var total = BestGivenNameResults.Count + 1;

            var score = 0;

            if (BestSurnameResult.Proximity == 100)
            {
                score += 2;
            }
            else if (BestSurnameResult.Proximity > 85)
            {
                score += 1;
            }

            foreach (var result in BestGivenNameResults)
            {
                if (result.Proximity == 100)
                {
                    score += 2;
                }
                else if (result.IsInitialMatch || result.Proximity > 85)
                {
                    score += 1;
                }
            }

            return Math.Round((decimal.Divide(score, total * 2)) * 100, 2);
        }
    }
}