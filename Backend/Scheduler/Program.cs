using Scheduler.Database;
using Scheduler.Helpers;
using Scheduler.HttpServer;
using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    class Program
    {
        private readonly static string _port = ConfigurationSettings.AppSettings.Get("AppPort");

        static void Main(string[] args)
        {
            Logger.Log("Scheduler app started.", LogLevel.Info);
            var server = HttpServerFactory.CreateHost(Convert.ToInt32(_port));
            Logger.Log("Http-host initialized.", LogLevel.Info);

            Logger.Log("Starting http-server...", LogLevel.Info);
            server.Start();
            Logger.Log($"Http-server started. Hosted on: http://localhost:{_port}", LogLevel.Info);


            //var ctx = new SchedulerContext();
            //var oper = new Operation() { Name = "oper" };
            //var opers = new List<Operation>() { oper };
            //var eq = new Equipment()
            //{
            //    Name = "eq",
            //    Operations = opers
            //};
            //ctx.Equipments.Add(eq);
            //ctx.SaveChanges();

            ConsoleClosure.WaitForExit();
            server.Stop();
        }
    }
}
