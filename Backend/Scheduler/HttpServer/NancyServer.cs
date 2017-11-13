using Nancy;
using Scheduler.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.HttpServer
{
    public class NancyServer : NancyModule
    {
        private DbManager _dbManager;

        public NancyServer() : base("/")
        {
            _dbManager = new DbManager();

            ConfigureRoutes();
        }

        private void ConfigureRoutes()
        {
            Get["/"] = Index;
            Get["/details"] = GetDetails;
        }

        private object Index(dynamic parameters)
        {
            return View["wwwroot/index.html"];
        }

        private object GetDetails(dynamic parameters)
        {
            var details = _dbManager.GetDetails();
            return details;
        }
    }
}
