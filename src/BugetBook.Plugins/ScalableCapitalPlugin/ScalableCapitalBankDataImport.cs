using BankDataImportBase;
using ExcelDataReader;
using ScalableCapitalPlugin.enums;
using ScalableCapitalPlugin.model;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using Currency = ScalableCapitalPlugin.enums.Currency;
using DataType = ScalableCapitalPlugin.enums.DataType;

namespace ScalableCapitalPlugin
{
    public class ScalableCapitalBankDataImport : BankDataImport
    {
        [SetsRequiredMembers]
        public ScalableCapitalBankDataImport()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Name = "ScalableCapital";
            SupportedFormats = [".CSV"];
        }

        public override async IAsyncEnumerable<InternalBankDataRecord> GetBankData(CancellationToken cancellationToken = default)
        {
            if (_path is null)
                throw new ArgumentNullException(nameof(_path));

            using (var stream = File.Open(_path, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
            {
                var tables = reader.AsDataSet().Tables;
                reader.Close();

                bool first = true;
                foreach (DataTable table in tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        if (first)
                        {
                            first = false;
                            if (CheckHeaderRow(row))
                                continue;

                            throw new BankDataException("Ungültiger CSV Header");
                        }

                        var hasDate = DateTime.TryParseExact(row.ItemArray[0].ToString(), "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime date);//2023-12-28
                        if (hasDate)
                        {
                            hasDate = DateTime.TryParseExact(row.ItemArray[1].ToString(), "HH:mm:ss", null, DateTimeStyles.None, out DateTime time);
                            date = date.AddHours(time.Hour);
                            date = date.AddMinutes(time.Minute);
                            date = date.AddSeconds(time.Second);
                        }
                        var hasShares = int.TryParse(row.ItemArray[8].ToString(), out int shares);
                        var hasPrice = decimal.TryParse(row.ItemArray[9].ToString(), out decimal price);
                        var hasAmount = decimal.TryParse(row.ItemArray[10].ToString(), out decimal amount);
                        var hasFee = decimal.TryParse(row.ItemArray[11].ToString(), out decimal fee);
                        var hasTax = decimal.TryParse(row.ItemArray[12].ToString(), out decimal tax);
                        var data = new ScalableCapitalCsvRecord
                        {
                            DateTime = hasDate ? date : DateTime.MinValue,
                            Status = (State)Enum.Parse(typeof(State), row.ItemArray[2].ToString()),
                            Reference = row.ItemArray[3].ToString().Replace("\\\"", ""),
                            Description = row.ItemArray[4].ToString().Replace("\\\"", ""),
                            AssetType = (AssetType)Enum.Parse(typeof(AssetType), row.ItemArray[5].ToString()),
                            Type = (DataType)Enum.Parse(typeof(DataType), row.ItemArray[6].ToString()),
                            Isin = row.ItemArray[7].ToString().Replace("\\\"", ""),
                            Shares = hasShares ? shares : 0,
                            Price = hasPrice ? price : 0,
                            Amount = hasAmount ? amount : 0,
                            Fee = hasFee ? fee : 0,
                            Tax = hasTax ? tax : 0,
                            Currency = (Currency)Enum.Parse(typeof(Currency), row.ItemArray[13].ToString()),
                        };

                        yield return (InternalBankDataRecord)data;
                    }
                }
            }
        }

        public override bool IsValid()
        {
            if (_path is null)
                return false;

            using (var stream = File.Open(_path, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
            {
                var tables = reader.AsDataSet().Tables;
                reader.Close();
                return CheckHeaderRow(tables[0].Rows[0]);
            }
        }

        private static bool CheckHeaderRow(DataRow row)
        {
            if (row.HasErrors ||
                row.ItemArray.Length != 14 ||
                !row.ItemArray[0].ToString().Equals("date") ||
                !row.ItemArray[1].ToString().Equals("time") ||
                !row.ItemArray[2].ToString().Equals("status") ||
                !row.ItemArray[3].ToString().Equals("reference") ||
                !row.ItemArray[4].ToString().Equals("description") ||
                !row.ItemArray[5].ToString().Equals("assetType") ||
                !row.ItemArray[6].ToString().Equals("type") ||
                !row.ItemArray[7].ToString().Equals("isin") ||
                !row.ItemArray[8].ToString().Equals("shares") ||
                !row.ItemArray[9].ToString().Equals("price") ||
                !row.ItemArray[10].ToString().Equals("amount") ||
                !row.ItemArray[11].ToString().Equals("fee") ||
                !row.ItemArray[12].ToString().Equals("tax") ||
                !row.ItemArray[13].ToString().Equals("currency")
                )
                return false;

            return true;
        }
    }
}