namespace Convey.Persistence.ArangoDB.Builders
{
    internal sealed class ArangoDbOptionsBuilder : IArangoDbOptionsBuilder
    {
        private readonly ArangoDbOptions _options = new ArangoDbOptions();
        
        public IArangoDbOptionsBuilder WithUrl(string url)
        {
            _options.Url = url;
            return this;
        }

        public IArangoDbOptionsBuilder WithDatabase(string database)
        {
            _options.Database = database;
            return this;
        }

        public IArangoDbOptionsBuilder WithUser(string user)
        {
            _options.User = user;
            return this;
        }

        public IArangoDbOptionsBuilder WithPassword(string password)
        {
            _options.Password = password;
            return this;
        }

        public ArangoDbOptions Build()
            => _options;
    }
}