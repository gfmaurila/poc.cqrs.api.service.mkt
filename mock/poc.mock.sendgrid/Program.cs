using MongoDB.Driver;
using poc.mock.sendgrid.Model;
using poc.mock.sendgrid.Service;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB
var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDbSettings>();
builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(settings.ConnectionString));
builder.Services.AddScoped(sp => new MongoDatabaseFactory(sp.GetRequiredService<IMongoClient>(), settings.Database));
builder.Services.AddScoped(sp => sp.GetRequiredService<MongoDatabaseFactory>().GetDatabase());
builder.Services.AddScoped<IEmailService, EmailService>();



var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
