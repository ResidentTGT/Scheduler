using Nancy;
using Nancy.ModelBinding;
using Scheduler.Database;
using Scheduler.Dto;
using Scheduler.Model;
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
            Post["/create-detail"] = CreateDetail;
        }

        private object Index(dynamic parameters)
        {
            return View["wwwroot/index.html"];
        }

        private object GetDetails(dynamic parameters)
        {
            var details = _dbManager.GetDetails();
            var dtoDetails = details.Select(d => DtoConverter.ConvertDetail(d)).ToList();

            return dtoDetails;
        }

        private object CreateDetail(dynamic parameters)
        {
            var requestBody = this.Bind<DetailDto>();
            var detailId = _dbManager.CreateDetail(DtoConverter.ConvertDetail(requestBody));

            return detailId;
        }
    }
}
