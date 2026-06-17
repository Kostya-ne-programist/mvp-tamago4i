var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "MVP Back-End працює!!");

app.Run();
