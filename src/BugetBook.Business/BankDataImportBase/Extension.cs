namespace BankDataImportBase
{
    public static class Extension
    {
        public static decimal ParseOrDefault(this object value, decimal def) => value != null ? ParseOrDefault(value.ToString(), def) : def;

        public static int ParseOrDefault(this object value, int def) => value != null ? ParseOrDefault(value.ToString(), def) : def;

        public static DateTime ParseOrDefault(this object value, DateTime def) => value != null ? ParseOrDefault(value.ToString(), def) : def;

        public static T Parse<T>(this object value) where T : Enum => Parse<T>(value.ToString());

        public static string Replace(this object value, string oldValue, string? newValue = "") => value.ToString().Replace(oldValue, newValue);

        public static decimal ParseOrDefault(this string value, decimal def)
        {
            var ok = decimal.TryParse(value, out decimal result);
            return ok ? result : def;
        }

        public static int ParseOrDefault(this string value, int def)
        {
            var ok = int.TryParse(value, out int result);
            return ok ? result : def;
        }

        public static DateTime ParseOrDefault(this string value, DateTime def)
        {
            var ok = DateTime.TryParse(value, out DateTime result);
            return ok ? result : def;
        }

        public static T Parse<T>(this string value) where T : Enum
        {
            if (!Enum.TryParse(typeof(T), value, true, out object? result))
                throw new ArgumentException($"Value ist kein Enum von Type {typeof(T)}");

            return (T)result;
        }
    }
}