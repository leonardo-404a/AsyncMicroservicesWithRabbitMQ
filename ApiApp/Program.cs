using FluentValidation;
using MongoDB.Driver;
using PizzaPlace.Application;
using PizzaPlace.Domain.FoodModule.Contracts;
using PizzaPlace.Domain.OrderModule.Contracts;
using PizzaPlace.Domain.PubSubModule;
using PizzaPlace.Domain.PubSubModule.Event;
using PizzaPlace.Infrastructure.Modules.BaseModule.Contracts;
using PizzaPlace.Infrastructure.Modules.FoodModule.Contracts;
using PizzaPlace.Infrastructure.Modules.OrderModule.Contracts;
using PizzaPlace.Infrastructure.Modules.PubSubModule;

namespace PizzaPlace.WebApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var eventBusConfig = new EventBusConfig();
        builder.Configuration.GetSection("EventBusConfig").Bind(eventBusConfig);

        var eventBus = new EventBus(eventBusConfig);

        builder.Services.AddSingleton<IBusPublisher>(eventBus);
        builder.Services.AddSingleton<IBusSubscriber>(eventBus);

        var mongoDBConfig = new MongoDbConfig();
        builder.Configuration.GetSection("MongoDBConfig").Bind(mongoDBConfig);

        builder.Services.AddSingleton(mongoDBConfig);
        builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoDBConfig.ConnectionString));

        builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
        builder.Services.AddSingleton<IFoodRepository, FoodRepository>();

        builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(ApiAppApplication).Assembly));
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(ApiAppApplication));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
