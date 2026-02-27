using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDBDemo.Helpers;

namespace MongoDBDemo.Models;

public class OrderItem
{
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}


[BsonCollection("orders")]
public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string CustomerName { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    
    // المصفوفة اللي هنلعب فيها
    public List<OrderItem> Items { get; set; } = new();
}
