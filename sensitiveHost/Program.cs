var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "I contain sensitive information including credentials. If you can read this then the attack has been successful.");

app.Run();
