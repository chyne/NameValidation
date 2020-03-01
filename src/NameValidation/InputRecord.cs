namespace NameValidation
{
    using System.Collections.Generic;

    public class InputRecord
    {
        public IList<string> GivenNames { get; set; } = new List<string>();
        public IList<string> Surnames { get; set; } = new List<string>();
    }
}