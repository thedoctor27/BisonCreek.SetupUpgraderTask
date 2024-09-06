using log4net.Appender;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using System.Globalization;
using System.Diagnostics;

namespace VisitForm.Edge.Gate.Service
{

    public class GenericEdgeGateManager : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer? m_timer;
        private readonly IServiceScopeFactory m_scopeFactory;
        private bool UpdateIsDone = false;
        public GenericEdgeGateManager(IServiceScopeFactory scopeFactory)
        {
            m_scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            m_timer = new Timer(DoWork, null, System.TimeSpan.Zero, System.TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);
            
            Console.WriteLine(DateTime.Now + " Service Running");
            File.AppendAllText(@"C:\Temp\EdgeTest.txt", "Working :" + DateTime.Now.ToString() + Environment.NewLine);
 
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            m_timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            m_timer?.Dispose();
        }

    }
}
