namespace BankDataImportBase
{
    public abstract class BankDataImport
    {
        public required string  Name { get; init; }
        public required string[] SupportedFormats { get; init; }

        public required Version RequiredVersion { get; init; }

        protected string? _path;

        public virtual bool SetAndCheckPath(string path)
        {
            if (!File.Exists(path))
                return false;

            if (!SupportedFormats.Contains(Path.GetExtension(path).ToUpper()))
                return false;

            _path = path;

            return true;
        }

        public abstract bool IsValid();
        public abstract IAsyncEnumerable<InternalBankDataRecord> GetBankData(CancellationToken token = default);
    }
}
