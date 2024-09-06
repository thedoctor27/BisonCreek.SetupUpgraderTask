using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using VisitForm.Edge.Gate.Service;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options => {
        options.ServiceName = "Edge Service Bison Creek";
    })
    .ConfigureServices(services => {
        LoggerProviderOptions.RegisterProviderOptions<
            EventLogSettings, EventLogLoggerProvider>(services);

        services.AddHostedService<GenericEdgeGateManager>();
    })
    .ConfigureLogging((context, logging) => {
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();

await host.RunAsync();