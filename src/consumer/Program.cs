using Dolittle.SDK;
using Dolittle.SDK.Tenancy;
using Consumer.Bootstrapping;
using Consumer.Domain;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions()
);

builder.Configuration.AddJsonFile("./.dolittle/resources.json");
builder.Configuration.AddJsonFile("./data/configuration.json");

var dolittleClient = builder.Configuration.ConfigureClient();

builder.Services.RegisterServices(dolittleClient);

var producer = builder.Build();

producer.SetupRouting(producer.Environment);

var clientTask = producer
    .Services
    .GetRequiredService<Client>()
    .WithContainer(new Container(producer.Services))
    .Start();

Console.WriteLine(
    $"{DateTime.UtcNow} - committing"
);

var tenantId = producer.Services.GetRequiredService<TenantId>();

await dolittleClient
    .AggregateOf<ConsumerAggregateRoot>(
        SingleTenantExecutionContextManager.ForCurrentTenant
    )
    .Perform(
        _ => _.Start(DateTime.UtcNow.ToString("O"))
    );

Console.WriteLine(
    $"{DateTime.UtcNow} - committed"
);

producer.Run();