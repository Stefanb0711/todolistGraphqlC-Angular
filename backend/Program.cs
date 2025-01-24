using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using todoList.Services;
using todoList.Models;
using todListBackend.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<Query>();
builder.Services.AddScoped<ISchema, Schema>(sp => new Schema { Query = sp.GetRequiredService<Query>() });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

/*
builder.Services.AddGraphQL(options =>
{
    options.EnableMetrics = false;
})
.AddSystemTextJson();
*/

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbService>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoDbService(settings.ConnectionString, settings.DatabaseName);
});

builder.Services.AddControllers();

builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddScoped<TodoService>();
builder.Services.AddScoped<TokenValidationFilter>();



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


/*
app.UseGraphQL<ISchema>();
app.UseGraphQLPlayground("/playground");
*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");


app.UseAuthorization();

app.MapControllers();

app.Run();
