global using Salamandra.Utils;

global using SQLite;

namespace Salamandra.Api
{
    public sealed class DofusApi
    {
        public Logger Logger { get; private set; }
        public SQLiteAsyncConnection Database { get; private set; }

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
            _instance = new();
            return _instance;
        }
    }
}
