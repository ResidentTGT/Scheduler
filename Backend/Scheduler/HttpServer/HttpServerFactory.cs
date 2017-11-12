using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.HttpServer
{
    internal static class HttpServerFactory
    {
        public static NancyHost CreateHost(int port, Dictionary<Type, object> dependencies = null)
        {
            var configuration = new HostConfiguration();
            configuration.UrlReservations.CreateAutomatically = true;

            var bootstraper = new AppNancyBootstrapper(dependencies);

            var uri = new Uri($"http://localhost:{port}");

            var host = new NancyHost(uri, bootstraper, configuration);

            return host;
        }

    }
}
