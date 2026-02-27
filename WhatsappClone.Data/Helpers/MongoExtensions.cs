namespace WhatsappClone.Data.Helpers
{
    public static class MongoExtensions
    {
        public static string GetCollectionName<T>()
            => (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                         .FirstOrDefault() as BsonCollectionAttribute)?
                         .CollectionName!;
    }











    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }
        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}

