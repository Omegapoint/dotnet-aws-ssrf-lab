var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "I am a server set up by the attacker. I will just redirect to anything after 1.bp.blogspot.com.");

app.MapGet("/bp.blogspot.com/{yourHostHere}", (string yourHostHere) => Results.Redirect("https://"+yourHostHere));
app.MapGet("/{*catchall}", (string catchall) => $"Not quite: {catchall}");

app.Run();
