using WorkflowEngineApi.Models;
using WorkflowEngineApi.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var workflowService = new WorkflowService();

app.MapPost("/workflow-definitions", (WorkflowDefinition def) =>
{
    var success = workflowService.AddWorkflowDefinition(def);
    return success ? Results.Ok("Workflow definition added.") : Results.BadRequest("Invalid or duplicate definition.");
});

app.MapGet("/workflow-definitions/{id}", (string id) =>
{
    var def = workflowService.GetWorkflowDefinition(id);
    return def != null ? Results.Ok(def) : Results.NotFound();
});

app.MapPost("/workflow-definitions/{id}/start", (string id) =>
{
    var instance = workflowService.StartInstance(id);
    return instance != null ? Results.Ok(instance) : Results.BadRequest("Invalid definition ID.");
});

app.MapPost("/instances/{instanceId}/actions/{actionId}", (Guid instanceId, string actionId) =>
{
    var success = workflowService.ExecuteAction(instanceId, actionId);
    return success ? Results.Ok("Action executed.") : Results.BadRequest("Invalid transition.");
});

app.MapGet("/instances/{id}", (Guid id) =>
{
    var instance = workflowService.GetInstance(id);
    return instance != null ? Results.Ok(instance) : Results.NotFound();
});
foreach (var route in app.Services.GetRequiredService<EndpointDataSource>().Endpoints)
{
    Console.WriteLine(route.DisplayName);
}

app.Run();
