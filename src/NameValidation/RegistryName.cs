namespace NameValidation
{
    using System;

    public class RegistryName
    {
        public RegistryName(string name, NameType nameType = NameType.Current)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name.ToUpperInvariant();
            NameType = nameType;
        }
        public NameType NameType { get; set; }
        public string Name { get; set; }
    }
}