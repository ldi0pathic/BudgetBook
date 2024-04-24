using BankDataImportBase;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using ScalableCapitalPlugin.model;
using ScalableCapitalPlugin.mapper;


namespace ScalableCapitalPlugin
{
    public class ScalableCapitalBankDataImport : BankDataImport
    {
        ScalableCapitalBankDataImport()
        {
            Name = "ScalableCapital";
            SupportedFormats = [".CSV"];
        }

        public override bool IsValid()
        {
            if (_path is null)
                return false;

            try
            {
                using var reader = new StreamReader(_path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.ValidateHeader<ScalableCapitalCsvRecord>();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public override async IAsyncEnumerable<InternalBankDataRecord> GetBankData(CancellationToken cancellationToken = default)
        {
            if (_path is null)
                throw new ArgumentNullException(nameof(_path));

            using (var reader = new StreamReader(_path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", InjectionCharacters = ['=', '@', '+', '-', '\t', '\r', '\"'] }))
            {
                csv.Context.RegisterClassMap<ScalableCapitalCsvRecordsMapper>();
                var records = csv.GetRecordsAsync<ScalableCapitalCsvRecord>(cancellationToken).GetAsyncEnumerator(cancellationToken);

                while (await records.MoveNextAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return (InternalBankDataRecord)records.Current;
                }
            }
        }
    }
}
