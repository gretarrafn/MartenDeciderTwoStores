using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using MartenStuff.Tagging.Model;
using Weasel.Core;
using Wolverine.Marten;

namespace MartenStuff.Tagging.EventStore;

public interface ITaggingStore : IDocumentStore;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTaggingEventStore(
        this IServiceCollection services,
        string eventStoreConnectionString
    )
    {
        services.AddMartenStore<ITaggingStore>(opts =>
            {
                opts.Connection(eventStoreConnectionString);
                opts.Events.DatabaseSchemaName = "tagging_eventstore";
                opts.DatabaseSchemaName = "tagging_eventstore";
                opts.AutoCreateSchemaObjects = AutoCreate.All;
                opts.Projections.UseIdentityMapForAggregates = true;
                opts.Events.MetadataConfig.HeadersEnabled = true;
                opts.Events.MetadataConfig.CausationIdEnabled = true;
                opts.Events.MetadataConfig.CorrelationIdEnabled = true;
                opts.UseSystemTextJsonForSerialization(casing: Casing.SnakeCase);
                opts.Events.AddEventType(typeof(TaggingCreated));
                opts.Events.AddEventType(typeof(TaggingUpdated));
                opts.Events.AddEventType(typeof(TaggingDeleted));
                
                opts.Projections.Add<TaggingProjection>(ProjectionLifecycle.Inline);
            })
            .IntegrateWithWolverine("wolverine")
            .AddAsyncDaemon(DaemonMode.HotCold);
        
        return services;
    }

}

