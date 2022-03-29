using Dolittle.SDK;
using producer;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions()
);

builder.Configuration.AddJsonFile("./.dolittle/resources.json");

var dolittleClient = builder.Configuration.ConfigureClient();

builder.Services.RegisterServices(dolittleClient);

var producer = builder.Build();

producer.SetupRouting(producer.Environment);

producer.Services.GetRequiredService<Client>().WithContainer(new Container(producer.Services)).Start();

producer.Run();