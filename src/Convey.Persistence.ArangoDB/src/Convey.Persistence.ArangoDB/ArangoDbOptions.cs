namespace Convey.Persistence.ArangoDB
{
    public class ArangoDbOptions
    {
        public string Url { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}