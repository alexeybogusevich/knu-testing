using Core.Models;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using Newtonsoft.Json;
using System.Text;
using HttpClientFactory = NBomber.Plugins.Http.CSharp.HttpClientFactory;

const string Host = "https://localhost:7123";
const string ServiceEndpoint = $"{Host}/customers";
var httpFactory = HttpClientFactory.Create();

var postStep = Step.Create("POST Customer",
    clientFactory: httpFactory,
    execute: async context =>
    {
        var customer = new Customer(Guid.NewGuid(), "Customer A Under Load");
        var requestJson = JsonConvert.SerializeObject(customer);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var request =
            Http.CreateRequest("POST", $"{ServiceEndpoint}")
                .WithBody(content)
                .WithCheck(async (response) =>
                            await Task.FromResult(response.IsSuccessStatusCode)
                                ? Response.Ok(customer, statusCode: (int)response.StatusCode)
                                : Response.Fail());

        var response = await Http.Send(request, context);
        return response;
    });

var putStep = Step.Create("PUT Customer",
    clientFactory: httpFactory,
    execute: async context =>
    {
        var customer = context.GetPreviousStepResponse<Customer>();
        if (customer == null)
        {
            return Response.Fail();
        }

        var modifiedCustomer = new Customer(customer.Id, "Customer A Under Load Modified");
        var requestJson = JsonConvert.SerializeObject(modifiedCustomer);
        var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var request =
            Http.CreateRequest("PUT", $"{ServiceEndpoint}/{customer.Id}")
                .WithBody(content)
                .WithCheck(async (response) =>
                            await Task.FromResult(response.IsSuccessStatusCode)
                                ? Response.Ok(modifiedCustomer, statusCode: (int)response.StatusCode)
                                : Response.Fail());

        var response = await Http.Send(request, context);
        return response;
    });

var getStep = Step.Create("GET Customer",
    clientFactory: httpFactory,
    execute: async context =>
    {
        var modifiedCustomer = context.GetPreviousStepResponse<Customer>();
        if (modifiedCustomer == null)
        {
            return Response.Fail();
        }

        var request =
            Http.CreateRequest("GET", $"{ServiceEndpoint}/{modifiedCustomer.Id}")
                .WithCheck(async (response) =>
                            await Task.FromResult(response.IsSuccessStatusCode)
                                ? Response.Ok(modifiedCustomer, statusCode: (int)response.StatusCode)
                                : Response.Fail());

        var response = await Http.Send(request, context);
        return response;
    });

var deleteStep = Step.Create("DELETE Customer",
    clientFactory: httpFactory,
    execute: async context =>
    {
        var customer = context.GetPreviousStepResponse<Customer>();
        if (customer == null)
        {
            return Response.Fail();
        }

        var request =
            Http.CreateRequest("DELETE", $"{ServiceEndpoint}/{customer.Id}")
                .WithCheck(async (response) =>
                            await Task.FromResult(response.IsSuccessStatusCode)
                                ? Response.Ok(statusCode: (int)response.StatusCode)
                                : Response.Fail());

        var response = await Http.Send(request, context);
        if (response == null || response.IsError)
        {
            return Response.Fail();
        }

        return Response.Ok(sizeBytes: response.SizeBytes, statusCode: response.StatusCode);
    });

var crudScenario = ScenarioBuilder
    .CreateScenario("CRUD", postStep, putStep, getStep, deleteStep)
    .WithWarmUpDuration(TimeSpan.FromSeconds(15))
    .WithLoadSimulations(Simulation.KeepConstant(10, TimeSpan.FromSeconds(60)));

NBomberRunner
    .RegisterScenarios(crudScenario)
    .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
    .Run();