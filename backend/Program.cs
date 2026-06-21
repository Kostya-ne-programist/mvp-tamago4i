using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using backend.Data;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введіть JWT-токен, отриманий з ендпоінта /auth/login."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Тамагочі MVP API v1");
    options.DocumentTitle = "Тамагочі MVP API";
});

app.UseAuthentication();
app.UseAuthorization();

app.Logger.LogInformation("Застосунок запущено. Середовище: {Env}",
    app.Environment.EnvironmentName);

app.MapGet("/", () => "MVP Back-End: CRUD-ендпоінти працюють!")
    .WithTags("Service");

// PETS CRUD
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

// PETS ACTIONS
app.MapPost("/pets/{id}/feed", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    if (!pet.IsAlive) return Results.BadRequest(new { message = "Питомець помер." });
    pet.Hunger = Math.Max(0, pet.Hunger - 20);
    pet.Happiness = Math.Min(100, pet.Happiness + 10);
    await db.SaveChangesAsync();
    return Results.Ok(pet);
}).WithTags("Pets");

app.MapPost("/pets/{id}/play", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    if (!pet.IsAlive) return Results.BadRequest(new { message = "Питомець помер." });
    pet.Happiness = Math.Min(100, pet.Happiness + 20);
    pet.Health = Math.Max(0, pet.Health - 10);
    await db.SaveChangesAsync();
    return Results.Ok(pet);
}).WithTags("Pets");

app.MapPost("/pets/{id}/heal", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    if (!pet.IsAlive) return Results.BadRequest(new { message = "Питомець помер." });
    pet.Health = Math.Min(100, pet.Health + 30);
    await db.SaveChangesAsync();
    return Results.Ok(pet);
}).WithTags("Pets");

app.MapPost("/pets/{id}/sleep", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    if (!pet.IsAlive) return Results.BadRequest(new { message = "Питомець помер, не може спати." });
    pet.Health = Math.Min(100, pet.Health + 25);
    pet.Happiness = Math.Max(0, pet.Happiness - 10);
    pet.Hunger = Math.Min(100, pet.Hunger + 15);
    await db.SaveChangesAsync();
    return Results.Ok(new { message = "😴 Питомець поспав і відновив здоров'я!", pet });
}).WithTags("Pets");

app.MapPatch("/pets/{id}/rename", async (int id, RenameDto dto, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    pet.Name = dto.Name;
    await db.SaveChangesAsync();
    return Results.Ok(pet);
}).WithTags("Pets");

app.MapGet("/pets/{id}/status", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    if (!pet.IsAlive)
        return Results.Ok(new { petId = id, status = "💀 Питомець помер..." });
    var messages = new List<string>();
    if (pet.Hunger > 70) messages.Add("😢 Дуже голодний!");
    else if (pet.Hunger > 40) messages.Add("🍽️ Трохи голодний");
    if (pet.Health < 30) messages.Add("🏥 Критичний стан здоров'я!");
    else if (pet.Health < 60) messages.Add("😷 Не дуже здоровий");
    if (pet.Happiness < 30) messages.Add("😭 Дуже сумний!");
    else if (pet.Happiness < 60) messages.Add("😕 Не дуже щасливий");
    if (messages.Count == 0) messages.Add("😊 Все чудово! Питомець щасливий і здоровий!");
    return Results.Ok(new { petId = id, name = pet.Name, status = string.Join(" ", messages) });
}).WithTags("Pets");

app.MapPost("/pets/{id}/feeditem", async (int id, int itemId, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    if (!pet.IsAlive) return Results.BadRequest(new { message = "Питомець помер." });
    var item = await db.ShopItems.FindAsync(itemId);
    if (item is null) return Results.NotFound();
    if (pet.Coins < item.Price)
        return Results.BadRequest(new { message = "Недостатньо монет!" });
    pet.Coins -= item.Price;
    pet.Hunger = Math.Max(0, pet.Hunger + item.HungerEffect);
    pet.Happiness = Math.Min(100, pet.Happiness + item.HappinessEffect);
    pet.Health = Math.Min(100, pet.Health + item.HealthEffect);
    await db.SaveChangesAsync();
    return Results.Ok(new { message = $"🍖 Питомець з'їв {item.Name}!", pet });
}).WithTags("Pets");

app.MapPost("/pets/{id}/work", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    if (!pet.IsAlive) return Results.BadRequest(new { message = "Питомець помер." });
    pet.Coins += 30;
    pet.Happiness = Math.Max(0, pet.Happiness - 15);
    pet.Hunger = Math.Min(100, pet.Hunger + 10);
    await db.SaveChangesAsync();
    return Results.Ok(new { message = "💼 Питомець попрацював і заробив 30 монет!", pet });
}).WithTags("Pets");

app.MapGet("/pets/{id}/wallet", async (int id, AppDbContext db) =>
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();
    return Results.Ok(new
    {
        petId = id,
        name = pet.Name,
        coins = pet.Coins,
        level = pet.Level,
        experience = pet.Experience
    });
}).WithTags("Pets");

// SHOP
app.MapGet("/shop", async (AppDbContext db) =>
    await db.ShopItems.ToListAsync())
    .WithTags("Shop");

// AUTH
app.MapPost("/auth/register", async (RegisterDto dto, AppDbContext db) =>
{
    if (await db.Users.AnyAsync(u => u.Email == dto.Email))
        return Results.Conflict("Користувач з таким email вже існує.");
    var user = new User
    {
        Name = dto.Name,
        Email = dto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Role = "user"
    };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}",
        new { user.Id, user.Name, user.Email, user.Role });
}).WithTags("Auth");

app.MapPost("/auth/login", async (LoginDto dto, AppDbContext db, IConfiguration config) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        return Results.Unauthorized();
    var token = CreateToken(user, config);
    return Results.Ok(new { access_token = token, token_type = "Bearer" });
}).WithTags("Auth");

app.MapGet("/auth/me", (ClaimsPrincipal principal) =>
    Results.Ok(new
    {
        Id = principal.FindFirstValue(ClaimTypes.NameIdentifier),
        Email = principal.FindFirstValue(ClaimTypes.Email),
        Role = principal.FindFirstValue(ClaimTypes.Role)
    }))
    .RequireAuthorization()
    .WithTags("Auth");

string CreateToken(User user, IConfiguration config)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);
    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.Run();

record RegisterDto(string Name, string Email, string Password);
record LoginDto(string Email, string Password);
record RenameDto(string Name);