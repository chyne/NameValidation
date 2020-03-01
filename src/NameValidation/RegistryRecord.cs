namespace NameValidation
{
    using System.Collections.Generic;

    public class RegistryRecord
    {
        public IList<RegistryName> GiveNames { get; set; } = new List<RegistryName>();
        public IList<RegistryName> Surnames { get; set; } = new List<RegistryName>();
    }
}