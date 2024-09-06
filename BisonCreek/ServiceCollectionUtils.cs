using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitForm.Edge.GenericEdge
{
    public static class ServiceCollectionUtils
    {
        public static void AddHostedService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, IHostedService, TService
        {
            services.AddSingleton<TService, TImplementation>();
            services.AddHostedService<HostedServiceWrapper<TService>>();
        }

        private class HostedServiceWrapper<TService> : IHostedService
        {
            private readonly IHostedService _hostedService;

            public HostedServiceWrapper(TService hostedService)
            {
                _hostedService = (IHostedService)hostedService;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
                return _hostedService.StartAsync(cancellationToken);
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return _hostedService.StopAsync(cancellationToken);
            }
        }
    }
}
