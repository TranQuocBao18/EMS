var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.EMS_ApiService>("apiservice");

builder.AddProject<Projects.EMS_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
