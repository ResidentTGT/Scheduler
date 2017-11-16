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
            Get["/delete-detail"] = DeleteDetail;

            Get["/equipments"] = GetEquipments;
            Post["/create-equipment"] = CreateEquipment;
            Get["/delete-equipment"] = DeleteEquipment;

            Get["/orders"] = GetOrders;
            //Post["/create-order"] = CreateOrder;
            //Get["/delete-order"] = DeleteOrder;
        }

        private object Index(dynamic parameters)
        {
            return View["wwwroot/index.html"];
        }

        #region DetailsApi
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

            return Response.AsJson(detailId);
        }

        private object DeleteDetail(dynamic parameters)
        {
            _dbManager.DeleteDetail(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        #endregion

        #region EquipmentApi
        private object GetEquipments(dynamic parameters)
        {
            var equipments = _dbManager.GetEquipments();
            var dtoEquipments = equipments.Select(e => DtoConverter.ConvertEquipment(e)).ToList();

            return dtoEquipments;
        }

        private object CreateEquipment(dynamic parameters)
        {
            var requestBody = this.Bind<Equipment>();
            var equipmentId = _dbManager.CreateEquipment(requestBody);

            return Response.AsJson(equipmentId);
        }

        private object DeleteEquipment(dynamic parameters)
        {
            _dbManager.DeleteEquipment(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        #endregion


        private object GetOrders(dynamic parameters)
        {
            var orders = _dbManager.GetOrders();
            var dtoOrders = orders.Select(d => DtoConverter.ConvertOrder(d)).ToList();

            return dtoOrders;
        }
    }
}
