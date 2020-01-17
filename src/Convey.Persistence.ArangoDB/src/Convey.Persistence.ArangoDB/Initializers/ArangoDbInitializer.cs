using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ArangoDB.Client;

namespace Convey.Persistence.ArangoDB.Initializers
{
    internal sealed class ArangoDbInitializer : IArangoDbInitializer
    {
        private static int _initialized;
        private readonly ArangoDbOptions options;

        public ArangoDbInitializer(ArangoDbOptions options)
        {
            this.options = options;
        }

        public Task InitializeAsync()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 1)
            {
                return Task.CompletedTask;
            }

            SetDatabaseSetting();

            return Task.CompletedTask;
        }

        private void SetDatabaseSetting()
        {
            ArangoDatabase.ChangeSetting(s =>
            {
                s.Database = options.Database;
                s.Url = options.Url;
                s.Credential = new NetworkCredential(
                    options.User,
                    options.Password);
            });
        }
    }
}