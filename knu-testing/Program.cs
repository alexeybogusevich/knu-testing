using Core;
using Core.Data;
using Core.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));

var app = builder.Build();
app.UseEndpointDefinitions();

app.Run();