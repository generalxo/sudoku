var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Sudoku_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var client = builder.AddNpmApp("client", "../client")
    .WithReference(apiService)
    .WithHttpEndpoint(env: "VITE_PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
