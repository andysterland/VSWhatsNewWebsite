var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.WhatsNewWebsite_ApiService>("apiservice");

builder.AddProject<Projects.WhatsNewWebsite_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
