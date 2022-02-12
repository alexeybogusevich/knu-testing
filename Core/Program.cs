using Core;
using Core.Data;
using Core.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(nameof(ApplicationDbContext)));
builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));

var app = builder.Build();
app.UseEndpointDefinitions();

app.Run();

public partial class Program { }