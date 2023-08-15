using System.Text.RegularExpressions;
using System.Web;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "This an image server provided by WordPress.");

//i0.wp apparently used to have an exploitable url parser. The expected request format
//is i0.wp.com/{domainOfImage}/{pathOfImage}, but wp also wanted to off load itself
//when it detected *.bp.blogspot.com in the path. Before patching it was possible to
//trick it into an open redirect.
//We don't know why the trick worked, the below is guesswork of sloppy url-parsing that
//seems to fit the bill. But we do know the input that worked and the end result of that.
//That's what we're trying to mimic. 
//
//Also, .Net HttpClient won't allow us to redirect from https to http. This probably does 
//not match the original.

app.MapGet("/{*catchall}", (HttpRequest request, string catchall) => {
    var path = HttpUtility.UrlDecode(request.Path);
    //The query string is anything that has ?s and ;s but no /s.
    var queryString = Regex.Match(path, @"\?([^/]*;)*([^/]*)*").ToString();
    //The image specifier matches /{domainOfImage}/{pathOfImage} but has no ?s and ;s
    var imageSpecifier = Regex.Match(path, @"/([^?;]*)/([^?;]*)");
    if (imageSpecifier.Groups[1].ToString().EndsWith("bp.blogspot.com"))
    {
        var url = "https:/"+path.Replace(queryString,""); //Just remove the query string.
        return Results.Redirect(url);
    }

    return Results.Text($"I will find the image specified by {catchall}.");
});

app.Run();
