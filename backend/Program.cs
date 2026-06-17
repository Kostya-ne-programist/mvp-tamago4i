using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var welcome = app.Configuration["AppSettings:WelcomeMessage"];
var version = app.Configuration["AppSettings:Version"];

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Тамагочі MVP API v1");
    options.DocumentTitle = "Тамагочі MVP API";
});

app.Logger.LogInformation("Застосунок запущено. Середовище: {Env}",
    app.Environment.EnvironmentName);

app.MapGet("/", () => "MVP Back-End: CRUD-ендпоінти працюють!")
    .WithTags("Service");

app.MapGet("/pets", async (AppDbContext db) =>
    await db.Pets.ToListAsync())
    .WithTags("Pets");

app.MapGet("/pets/{id}", async (int id, AppDbContext db) =>
    await db.Pets.FindAsync(id) is Pet pet
        ? Results.Ok(pet)
        : Results.NotFound())
    .WithTags("Pets");

app.MapPost("/pets", async (Pet pet, AppDbContext db) =>
{
    db.Pets.Add(pet);
    await db.SaveChangesAsync();
    return Results.Created($"/pets/{pet.Id}", pet);
}).WithTags("Pets");

app.MapPut("/pets/{id}", async (int id, Pet input, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();

    pet.Name = input.Name;
    pet.Species = input.Species;
    pet.Hunger = input.Hunger;
    pet.Happiness = input.Happiness;
    pet.Health = input.Health;

    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Pets");

app.MapDelete("/pets/{id}", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();

    db.Pets.Remove(pet);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).WithTags("Pets");

app.Run();