using BankDataImportBase;
using CsvHelper;
using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration;
using ScalableCapitalPlugin.model;
using ScalableCapitalPlugin.mapper;

namespace ScalableCapitalPlugin
{
    public class BankDataImport : IBankDataImport
    {
        private string? _path;
       
        public string Name => "ScalableCapital";
        public string[] SupportedFormats => [".CSV"];

        public bool SetAndCheckPath(string path)
        {
            if(!File.Exists(path)) 
                return false;

            if(!SupportedFormats.Contains(Path.GetExtension(path).ToUpper()))
                return false;

            _path = path;

            return true;
        }

        public bool IsValid()
        {
            if(_path is null)
                return false;

            try
            {
                using var reader = new StreamReader(_path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.ValidateHeader<ScalableCapitalCsvRecords>();     
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async IAsyncEnumerable<InternalBankDataFormat> GetBankData([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (_path is null)
               throw new ArgumentNullException(nameof(_path));

            using (var reader = new StreamReader(_path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";", InjectionCharacters = ['=', '@', '+', '-', '\t', '\r', '\"'] }))
            {
                csv.Context.RegisterClassMap<ScalableCapitalCsvRecordsMapper>();
                var records = csv.GetRecordsAsync<ScalableCapitalCsvRecords>(cancellationToken).GetAsyncEnumerator(cancellationToken);

                while (await records.MoveNextAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return (InternalBankDataFormat)records.Current;
                }
            }
        }
    }
}
