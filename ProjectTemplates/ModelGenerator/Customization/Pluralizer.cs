using Microsoft.EntityFrameworkCore.Design;
using Inflector.Cultures;
using System.Globalization;

namespace Generator.Customization
{
    public class Pluralizer : IPluralizer
    {
        static Inflector.Inflector Plurizer = new Inflector.Inflector(new CultureInfo("en"));
        public Pluralizer()
        {

        }

        public string Pluralize(string identifier)
        {
            return Plurizer.Pluralize(identifier) ?? identifier;
        }

        public string Singularize(string identifier)
        {
            return Plurizer.Singularize(identifier) ?? identifier;
        }
    }
}
