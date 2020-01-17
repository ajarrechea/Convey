namespace Convey.Persistence.ArangoDB
{
    public interface IArangoDbOptionsBuilder
    {
        IArangoDbOptionsBuilder WithUrl(string url);
        IArangoDbOptionsBuilder WithDatabase(string database);
        IArangoDbOptionsBuilder WithUser(string user);
        IArangoDbOptionsBuilder WithPassword(string password);
        ArangoDbOptions Build();
    }
}