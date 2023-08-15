var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
var app = builder.Build();

var i0wpcomHost = app.Configuration["i0wpcomHost"];

app.MapGet("/", () => "This is gravatar, we provide avatars given a hash.");

// If provided a d=imageIdentifier, gravatar will still redirect to http://i0.wp.com.
// However i0.wp.com has been patched so we need to mock them both.

app.MapGet("/avatar/{discarded}", (string d) => Results.Redirect($"{i0wpcomHost}{d}"));

app.Run();

