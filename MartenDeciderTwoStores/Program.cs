
using MartenStuff.SchemaMappings.EventStore;
using MartenStuff.Tagging.EventStore;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Add services to the container
ConfigureServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration);

webApplicationBuilder.Services.AddEndpointsApiExplorer();
webApplicationBuilder.Services.AddOpenApiDocument();


var app = webApplicationBuilder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

const string eventStoreConnectionStringKey = "EventStoreConnectionString";

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    webApplicationBuilder.Services.AddControllers();
    services.AddOpenApi();
    services.AddSchemaMappingsEventStore(GetConnectionStringOrThrow(configuration, eventStoreConnectionStringKey));
    services.AddTaggingEventStore(GetConnectionStringOrThrow(configuration, eventStoreConnectionStringKey));
}

string GetConnectionStringOrThrow(IConfiguration configuration, string connectionStringKey)
{
    return configuration.GetConnectionString(connectionStringKey) 
           ?? throw new InvalidOperationException($"Could not find connection string with key {connectionStringKey}");
}