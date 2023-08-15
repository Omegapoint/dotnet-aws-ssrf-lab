using System.Text.Json;
using System.Web;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

HttpMessageHandler handler = new HttpClientHandler()
{
    MaxAutomaticRedirections = 10,
    AllowAutoRedirect = true
};
var defaultReqParams = HttpUtility.UrlEncode(JsonSerializer.Serialize(new { someValue = "value" }));
var gravatarSource = builder.Configuration.GetValue<string>("GravatarSource");

app.MapGet("/", () => "This is the grafana host. Grafana is an open source software for monitoring and creating graphs. ");

//This mimics
//https://github.com/grafana/grafana/blob/78febbbeef1f23ccbb88c2bd3acd2e9c2011e02a/pkg/api/api.go#L423
//hash will contain the rest of the url after the final /, url-decoded. We will use this hash to redirect to gravatar.
app.MapGet("/avatar/{hash}", async (string hash) => {
    HttpClient httpClient = new HttpClient(handler);
    var avatar = await httpClient.GetAsync($"{gravatarSource}{hash}");//?{defaultReqParams}");
    return await avatar.Content.ReadAsStringAsync();
});

app.Run();
