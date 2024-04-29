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
        private bool _isValid;

        [SetsRequiredMembers]
        public ScalableCapitalBankDataImport()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Name = "ScalableCapital";
            SupportedFormats = [".CSV"];
            _isValid = false;
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
                foreach (DataRow row in tables[0].Rows)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    var data = new ScalableCapitalCsvRecord
                    {
                        DateTime = $"{row.ItemArray[0]} {row.ItemArray[1]}".ParseOrDefault(DateTime.MinValue),
                        Status = row.ItemArray[2].Parse<State>(),
                        Reference = row.ItemArray[3].Replace("\\\""),
                        Description = row.ItemArray[4].Replace("\\\""),
                        AssetType = row.ItemArray[5].Parse<AssetType>(),
                        Type = row.ItemArray[6].Parse<DataType>(),
                        Isin = row.ItemArray[7].Replace("\\\""),
                        Shares = row.ItemArray[8].ParseOrDefault(0),
                        Price = row.ItemArray[9].ParseOrDefault(0m),
                        Amount = row.ItemArray[10].ParseOrDefault(0m),
                        Fee = row.ItemArray[11].ParseOrDefault(0m),
                        Tax = row.ItemArray[12].ParseOrDefault(0m),
                        Currency = row.ItemArray[13].Parse<Currency>(),
                    };

                    yield return (InternalBankDataRecord)data;
                }
            }
        }

        protected override bool IsValid()
        {
            if (_path is null)
                return false;

            using (var stream = File.Open(_path, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
            {
                var tables = reader.AsDataSet().Tables;
                reader.Close();
                _isValid = CheckHeaderRow(tables[0].Rows[0]);
                return _isValid;
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