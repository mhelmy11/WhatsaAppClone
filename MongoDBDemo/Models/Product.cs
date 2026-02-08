using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBDemo.Models;

public class Product
{

    [BsonId] // ده بيعرف مونجو إن الحقل ده هو الـ Primary Key
    [BsonRepresentation(BsonType.ObjectId)] // بيحوله من ObjectId لـ string عشان الـ API
    public string? Id { get; set; }

    [BsonElement("Name")] // لو عاوز اسم الحقل في الداتابيز مختلف عن الـ C#
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }
    public string Category { get; set; } = null!;

}
