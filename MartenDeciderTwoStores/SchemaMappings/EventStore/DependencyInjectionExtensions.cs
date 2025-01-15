using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using MartenDeciderTwoStores.SchemaMappings.Model;
using Weasel.Core;
using Wolverine;
using Wolverine.Marten;

namespace MartenDeciderTwoStores.SchemaMappings.EventStore;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSchemaMappingsEventStore(
        this IServiceCollection services,
        string eventStoreConnectionString
    )
    {
        services.AddWolverine(ExtensionDiscovery.Automatic, opts =>
        {
            // This middleware will apply to the HTTP
            // endpoints as well
            opts.Policies.AutoApplyTransactions();

            // Setting up the outbox on all locally handled
            // background tasks
            opts.Policies.UseDurableLocalQueues();
            
            opts.Policies.MessageExecutionLogLevel(LogLevel.None);
            // Turn down Wolverine's built in logging of all successful
            // message processing
            opts.Policies.MessageSuccessLogLevel(LogLevel.Debug);
        });
        services.AddMarten(opts =>
            {
                opts.Connection(eventStoreConnectionString);
                opts.Events.DatabaseSchemaName = "schema_mappings_eventstore";
                opts.DatabaseSchemaName = "schema_mappings_eventstore";
                opts.AutoCreateSchemaObjects = AutoCreate.All;
                opts.Projections.UseIdentityMapForAggregates = true;
                opts.Events.MetadataConfig.HeadersEnabled = true;
                opts.Events.MetadataConfig.CausationIdEnabled = true;
                opts.Events.MetadataConfig.CorrelationIdEnabled = true;
                opts.UseSystemTextJsonForSerialization(casing: Casing.SnakeCase);
                // opts.Events.Subscribe(new AutomationActivationSubscription(), s =>
                // {
                // s.IncludeType<AutomationActivated>();
                // s.IncludeType<AutomationDeactivated>();
                // s.Options.SubscribeFromPresent();
                // });
                opts.Events.AddEventType(typeof(SchemaMappingCreated));
                opts.Events.AddEventType(typeof(SchemaMappingUpdated));
                opts.Events.AddEventType(typeof(SchemaMappingDeleted));
                
                opts.Projections.Add<SchemaMappingProjection>(ProjectionLifecycle.Inline);
            })
            .IntegrateWithWolverine(x => x.MessageStorageSchemaName = "wolverine")
            .UseLightweightSessions()
            .AddAsyncDaemon(DaemonMode.HotCold);

        // .PublishEventsToWolverine("Everything");

        // services.AddControllers()
            // .AddApplicationPart(typeof(AutomationController).Assembly)
            // .AddControllersAsServices();
        // services.AddSingleton<Triggers>();
        // services.AddSingleton<IRuleKeyDatasourceProvider, RuleKeyDatasourceProvider>();
        return services;
    }

}