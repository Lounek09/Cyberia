global using SQLite;

using Cyberia.Chronicle;

namespace Cyberia.Api
{
    public sealed class DofusApi
    {
        public Logger Logger { get; init; }
        public SQLiteAsyncConnection Database { get; init; }

        internal static DofusApi Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Api before !") : _instance;
            private set => _instance = value;
        }
        private static DofusApi? _instance;

        internal DofusApi()
        {
            Logger = new("api");
            Database = new(Constant.DATABASE_PATH);
        }

        public static DofusApi Build()
        {
            _instance ??= new();
            return _instance;
        }
    }
}
