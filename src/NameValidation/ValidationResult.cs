namespace NameValidation
{
    using System;

    public class ValidationResult : IComparable<ValidationResult>
    {
        public string NameFromInput { get; set; }
        public string NameVariation { get; set; }
        public string NameFromRegistry { get; set; }
        public int Distance { get; set; }
        public NameType NameType { get; set; }
        public bool IsInitialMatch { get; set; }

        public decimal Proximity => NameFromRegistry == null || NameVariation == null ? 0 : Math.Round((1 - decimal.Divide(Distance, Math.Max(NameFromRegistry.Length, NameVariation.Length))) * 100, 2);


        public int CompareTo(ValidationResult other)
        {
            return Proximity.CompareTo(other.Proximity);
        }

        public override string ToString()
        {
            return $"NameFromInput: {NameFromInput}; NameVariation: {NameVariation}; NameFromRegistry: {NameFromRegistry}; NameType: {NameType}; IsInitialMatch: {IsInitialMatch}; Proximity: {Proximity}; Distance: {Distance};";
        }
    }
}