using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orders.Api;
using Orders.Api.Commands;
using Orders.Api.Commands.CommandHandlers;
using Orders.Api.Data;
using Orders.Api.Dtos;
using Orders.Api.Queries;
using Orders.Api.Queries.QueryHandlers;
using Orders.Api.Repositories;
using StackExchange.Redis;
using Order = Orders.Api.Entities.Order;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IReadOrdersRepository, ReadOrdersRepository>();
builder.Services.AddScoped<IWriteOrdersRepository, WriteOrdersRepository>();
builder.Services.AddScoped<IEventStoreRepository, RedisEventStoreRepository>();
builder.Services.AddCommandHandlers(typeof(Program));
builder.Services.AddQueryHandlers(typeof(Program));

builder.Services.AddEventHandlers(typeof(Program));
builder.Services.AddSingleton<IEventListener>((provider => 
    new EventListener(provider.GetRequiredService<IDatabase>(),
    provider.GetRequiredService<IOptions<RedisConfig>>(),
    provider.GetRequiredService<ILogger<EventListener>>(), 
    provider)));

builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection("RedisConfig"));
var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetSection("RedisConfig:Url").Value!);
var redisDatabase = redis.GetDatabase();
builder.Services.AddSingleton(redisDatabase);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

using var serviceScope = app.Services.CreateScope();
var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();

app.ListenForRedisEvents();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/orders/{orderId:guid}",
    async (Guid orderId, [FromServices] IQueryHandler<GetOrderByIdQuery, Order> queryHandler) =>
    {
        var result = await queryHandler.HandleAsync(new GetOrderByIdQuery(orderId));
        return result is not { } ? Results.NotFound() : Results.Ok(result);
    });

app.MapGet("/orders", async ([FromServices] IQueryHandler<GetOrderQuery, Order> queryHandler) =>
{
    var result = await queryHandler.HandleAsync(new GetOrderQuery());
    return result is not { } ? Results.NotFound() : Results.Ok(result);
});

app.MapPut("/orders/{orderId:guid}", async (Guid orderId, [FromBody] OrderForUpdateDto orderForUpdateDto,
    [FromServices] IMapper mapper, [FromServices] ICommandHandler<UpdateOrderCommand> commandHandler) =>
{
    var order = mapper.Map<Order>(orderForUpdateDto);
    order.Id = orderId;

    await commandHandler.HandleAsync(new UpdateOrderCommand(order));
    return Results.Ok();
}).WithName("GetOrderById");

app.MapPost("/orders",
    async ([FromBody] OrderForCreateDto orderForCreateDto, [FromServices] IMapper mapper,
        [FromServices] ICommandHandler<CreateOrderCommand> commandHandler) =>
    {
        var order = mapper.Map<Order>(orderForCreateDto);
        order.Id = Guid.NewGuid();

        await commandHandler.HandleAsync(new CreateOrderCommand(order));
        return Results.CreatedAtRoute("GetOrderById", new { orderId = order.Id });
    });

app.Run();