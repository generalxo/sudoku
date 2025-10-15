var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Sudoku_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Sudoku_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

var client = builder.AddNpmApp("client", "../client")
    .WithReference(apiService)
    .WithHttpEndpoint(env: "VITE_PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
