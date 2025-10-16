using System.Buffers.Text;
using System.Diagnostics;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Sudoku_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

apiService.WithCommand("scalar", "Sclar UI", executeCommand: async _ => {
    try 
    { 
        var enpoint = apiService.GetEndpoint("https").Url; 
        var url = $"{enpoint}/scalar/v1";
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); 
        return new ExecuteCommandResult { Success = true };
    }
    catch (Exception ex) 
    { 
        return new ExecuteCommandResult { Success = false, ErrorMessage = ex.Message };
    } 
});

var client = builder.AddNpmApp("client", "../client")
    .WithReference(apiService)
    .WithHttpEndpoint(env: "VITE_PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
