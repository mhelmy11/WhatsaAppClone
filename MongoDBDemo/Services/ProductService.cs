using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDemo.Database;
using MongoDBDemo.Models;

namespace MongoDBDemo.Services;

public class ProductService
{

    private readonly IMongoCollection<Product> _products;
    private readonly IMongoCollection<Order> _orders;

    public ProductService(MongoDbContext mongoDbContext)
    {
        _products = mongoDbContext.GetCollection<Product>();
        _orders = mongoDbContext.GetCollection<Order>();
    }

    public async Task CreateAsync(Product newProduct) =>
        await _products.InsertOneAsync(newProduct);

    // --- 2. Find (Get All or By Filter) ---
    public async Task<List<Product>> GetAsync() =>
        await _products.Find(_ => true).ToListAsync();

    public async Task<Product?> GetByIdAsync(string id) =>
        await _products.Find(x => x.Id == id).FirstOrDefaultAsync();

    // --- 3. Update ---
    public async Task UpdateAsync(string id, Product updatedProduct) =>
        await _products.ReplaceOneAsync(x => x.Id == id, updatedProduct);

    // --- 4. Delete ---
    public async Task RemoveAsync(string id) =>
        await _products.DeleteOneAsync(x => x.Id == id);

    public async Task<List<AveragePriceResponseDTO>> GetAveragePriceByCategory()
    {
        var result = await _products.Aggregate()
                    .Group(p => p.Category, g => new AveragePriceResponseDTO
                    {
                        Category = g.Key,
                        AveragePrice = g.Average(p => p.Price)
                    })
                    .ToListAsync();
        return result;

    }

    public async Task<string> CreateOrderAsync(Order newOrder)
    {
        await _orders.InsertOneAsync(newOrder);
        return newOrder.Id!;
    }


    public async Task<List<Order>> GetOrdersAsync() =>
        await _orders.Find(_ => true).ToListAsync();
    public async Task AddItemToOrder(string orderId, OrderItem newItem)
    {

        var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId);
        var update = Builders<Order>.Update.Push(o => o.Items, newItem);

        await _orders.UpdateOneAsync(filter, update);

    }

    public async Task RemoveItemFromOrder(string orderId, string productId)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId);
        var update = Builders<Order>.Update.PullFilter(o => o.Items, i => i.ProductId == productId);

        await _orders.UpdateOneAsync(filter, update);
    }

    public async Task UpdateItemQuantityInOrder(string orderId, string productId, int newQuantity)
    {
        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Id, orderId),
            Builders<Order>.Filter.ElemMatch(o => o.Items, i => i.ProductId == productId)
        );

        var update = Builders<Order>.Update.Set("Items.$.Quantity", newQuantity);

        await _orders.UpdateOneAsync(filter, update);
    }



}
