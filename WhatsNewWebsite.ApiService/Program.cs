using System.Text.Json;
using WhatsNewWebsite.ApiService;
using WhatsNewWebsite.DataModel;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var itemSummaries = JsonSerializer.Deserialize<List<WhatsNewItemSummary>>(File.ReadAllText("whatsnewitems.json"));

if (itemSummaries != null)
{
    foreach (var item in itemSummaries)
    {
        // Set the Version property
        item.Version = $"17.{item.MinorRelease}";
    }
}

var summaryApi = new SummaryApi(itemSummaries);

app.MapGet("/whatsnewitem", () =>
{
    return summaryApi.GetRandom(0);
});
app.MapGet("/whatsnewitem/{Id}", (string Id) =>
{
    return summaryApi.Get(Id);
});
app.MapGet("/search/{term}", (string Term) =>
{
    return summaryApi.GetWithTerm(Term);
});
// Endpoint to get items with a specific version
app.MapGet("/whatsnewitem/version/{Version}", (string Version) =>
{
    return summaryApi.GetWithVersion(Version);
});

var itemArticles = JsonSerializer.Deserialize<List<WhatsNewItemArticle>>(File.ReadAllText("whatsnewarticles.json"));
var articleApi = new ArticleApi(itemArticles);

app.MapGet("/article/{Id}", (string Id) =>
{
    return articleApi.Get(Id);
});

app.MapDefaultEndpoints();

app.Run();