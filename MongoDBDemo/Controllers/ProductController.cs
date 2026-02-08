using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDemo.Models;
using MongoDBDemo.Services;

namespace MongoDBDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }
        [HttpPost]

        // public async Task<IActionResult> CreateProduct()
        // {

        //     var newProduct = new Product
        //     {
        //         Name = "iPhone 14 Pro",
        //         Price = 999.99m,
        //         Category = "Electronics"
        //     };

        //     await productService.CreateAsync(newProduct);
        //     return CreatedAtAction(nameof(GetProducts), new { id = newProduct.Id }, newProduct);

        // }

        public async Task<IActionResult> CreateProduct(Product newProduct)
        {

            await productService.CreateAsync(newProduct);

            return Ok("Product created successfully!");
        }



        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productService.GetAsync();
            return Ok(products);
        }

        [HttpGet("GetAveragePriceByCategory")]

        public async Task<IActionResult> GetAveragePriceByCategory()
        {
            var result = await productService.GetAveragePriceByCategory();
            return Ok(result);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await productService.RemoveAsync(id);
            return Ok("Product deleted successfully!");
        }


        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(Order newOrder)
        {
            await productService.CreateOrderAsync(newOrder);
            return Ok($"Order {newOrder.Id} created successfully!");
        }


        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await productService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpPost("AddItemToOrder/{orderId}")]
        public async Task<IActionResult> AddItemToOrder(string orderId, OrderItem newItem)
        {
            await productService.AddItemToOrder(orderId, newItem);
            return Ok("Item added to order successfully!");
        }

        [HttpDelete("RemoveItemFromOrder/{orderId}/{productId}")]
        public async Task<IActionResult> RemoveItemFromOrder(string orderId, string productId)
        {
            await productService.RemoveItemFromOrder(orderId, productId);
            return Ok("Item removed from order successfully!");
        }

        [HttpPut("UpdateItemQuantityInOrder/{orderId}/{productId}")]
        public async Task<IActionResult> UpdateItemQuantityInOrder(string orderId, string productId, int newQuantity)
        {
            await productService.UpdateItemQuantityInOrder(orderId, productId, newQuantity);
            return Ok("Item quantity updated in order successfully!");
        }







    }
}
