


using Base62;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDBDemo.Database;
using MongoDBDemo.Models;
using MongoDBDemo.Repositories;
using MongoDBDemo.Services;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddIdGen(1); 


#region MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));//then we can inject IOptions<MongoDbSettings> to get the settings values
var MongoDBSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient>(new MongoClient(MongoDBSettings.ConnectionString));

// Register MongoDB factory
builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return new MongoDbContext(client, MongoDBSettings.DatabaseName);
});
#endregion

#region Identity And DBContext
builder.Services.AddDbContext<SqlServerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("SqlServerSettings")["ConnectionString"]);
});

builder.Services.AddIdentity<User, Role>(

    options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 4;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
 
    }

    ).AddEntityFrameworkStores<SqlServerDbContext>().AddDefaultTokenProviders(); ;

#endregion


builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ContactRepository>();
builder.Services.AddScoped<GroupRepository>();
builder.Services.AddScoped<GroupMemberRepository>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PresenceRepository>();
builder.Services.AddScoped<ConversationRepository>();
builder.Services.AddScoped<IUserConversationRepository, UserConversationRepository>();
builder.Services.AddScoped<IUserConversationService, UserConversationService>();
builder.Services.AddSingleton<Base62Converter>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();




app.Run();

