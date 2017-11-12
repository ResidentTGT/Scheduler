using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.HttpServer
{
    public class NancyServer : NancyModule
    {
        public NancyServer() : base("/")
        {     
            ConfigureRoutes();
        }

        private void ConfigureRoutes()
        {
            Get["/"] = Index;           
        }

        private object Index(dynamic parameters)
        {
            return View["wwwroot/index.html"];
        }
    }
}
