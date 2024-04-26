using BondoraPlugin.enums;
using System.Linq;
using System.Xml.XPath;

namespace BondoraPlugin.mapper
{
    public static class DataTypeMapper
    {
        private static readonly Dictionary<DataType, List<string>> AdditionalNames = new()
        {
            { DataType.Interest,  new List<string>{"Go & Grow Zinsen" } },
            { DataType.SavingPlan,  new List<string>{ "Überweisen", "SEPA-Banküberweisung" } },
        };

        public static DataType ToDataType(this string typeString)
        {
            return AdditionalNames.First(pair => pair.Value.Any(name => name.Equals(typeString, StringComparison.OrdinalIgnoreCase))).Key;
        }
    }
}
