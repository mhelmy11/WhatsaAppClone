


using MongoDB.Driver;
using MongoDBDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// 1. نقرأ الإعدادات
var mongoSettings = builder.Configuration.GetSection("MongoDbSettings");

// 2. نسجل الـ MongoClient كـ Singleton
builder.Services.AddSingleton<IMongoClient>
    (
    sp => new MongoClient(mongoSettings["ConnectionString"])
    );

// 3. نسجل الـ Database (اختياري بس بيريحك في الـ Injection)
builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoSettings["DatabaseName"]);
});


builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();




app.Run();

