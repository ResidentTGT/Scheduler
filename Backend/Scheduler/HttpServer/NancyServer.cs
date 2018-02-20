using Nancy;
using Nancy.ModelBinding;
using Scheduler.Core;
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
        private DtoConverter _dtoConverter;

        public NancyServer() : base("/")
        {
            _dbManager = new DbManager();
            _dtoConverter = new DtoConverter();

            ConfigureRoutes();
        }

        private void ConfigureRoutes()
        {
            Get["/"] = Index;

            Get["/details"] = GetDetails;
            Get["/details-without-routes"] = GetDetailsWithoutRoutes;
            Post["/create-detail"] = CreateDetail;
            Get["/delete-detail"] = DeleteDetail;

            Get["/equipments"] = GetEquipments;
            Post["/create-equipment"] = CreateEquipment;
            Get["/delete-equipment"] = DeleteEquipment;

            Get["/orders"] = GetOrders;
            Post["/create-order"] = CreateOrder;
            Get["/delete-order"] = DeleteOrder;
            Get["/calculate-order"] = CalculateOrder;

            Get["/production-items"] = GetProductionItems;
            Post["/create-production-item"] = CreateProductionItem;
            Get["/delete-production-item"] = DeleteProductionItem;

            Get["/operations"] = GetOperations;
            Get["/operations?detailId={detailId}"] = GetOperationsByDetailId;
            Post["/create-operation"] = CreateOperation;
            Get["/delete-operation"] = DeleteOperation;

            Get["/routes"] = GetRoutes;
            Post["/create-route"] = CreateRoute;
            Get["/delete-route"] = DeleteRoute;

            Get["/conveyors"] = GetConveyors;
            Get["/workshops"] = GetWorkshops;
        }

        private object Index(dynamic parameters)
        {
            return View["wwwroot/index.html"];
        }

        private object CalculateOrder(dynamic parameters)
        {
            var calculation = new Calculation();
            var order = calculation.CalculateOrderById(Request.Query["id"]);

            return _dtoConverter.ConvertOrderForViewing(order);
        }

        #region DetailsApi
        private object GetDetails(dynamic parameters)
        {
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var details = _dbManager.GetDetails(pageNumber, pageSize);

            var dtoDetails = details.Select(d => _dtoConverter.ConvertDetail(d)).ToList();

            return dtoDetails;
        }

        private object GetDetailsWithoutRoutes(dynamic parameters)
        {
            var details = _dbManager.GetDetailsWithoutRoutes();
            var dtoDetails = details.Select(d => _dtoConverter.ConvertDetail(d)).ToList();

            return dtoDetails;
        }

        private object CreateDetail(dynamic parameters)
        {
            var requestBody = this.Bind<DetailDto>();
            var detailId = _dbManager.CreateDetail(_dtoConverter.ConvertDetail(requestBody));

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
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var equipments = _dbManager.GetEquipments(pageNumber, pageSize);

            var dtoEquipments = equipments.Select(e => _dtoConverter.ConvertEquipment(e)).ToList();

            return dtoEquipments;
        }

        private object CreateEquipment(dynamic parameters)
        {
            var requestBody = this.Bind<EquipmentDto>();
            var equipmentId = _dbManager.CreateEquipment(_dtoConverter.ConvertEquipment(requestBody));

            return Response.AsJson(equipmentId);
        }

        private object DeleteEquipment(dynamic parameters)
        {
            _dbManager.DeleteEquipment(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        #endregion

        #region OrderApi
        private object GetOrders(dynamic parameters)
        {
            var orders = _dbManager.GetOrders();
            var dtoOrders = orders.Select(d => _dtoConverter.ConvertOrder(d)).ToList();

            return dtoOrders;
        }
        private object DeleteOrder(dynamic parameters)
        {
            _dbManager.DeleteOrder(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        private object CreateOrder(dynamic parameters)
        {
            var requestBody = this.Bind<OrderDto>();
            var orderId = _dbManager.CreateOrder(_dtoConverter.ConvertOrder(requestBody));

            return Response.AsJson(orderId);
        }
        #endregion

        #region ProductionItemApi
        private object GetProductionItems(dynamic parameters)
        {
            var productionItems = _dbManager.GetProductionItems();
            var dtoProductionItems = productionItems.Select(d => _dtoConverter.ConvertProductionItem(d)).ToList();

            return dtoProductionItems;
        }

        private object CreateProductionItem(dynamic parameters)
        {
            var requestBody = this.Bind<ProductionItemDto>();
            var productionItemId = _dbManager.CreateProductionItem(_dtoConverter.ConvertProductionItem(requestBody));

            return Response.AsJson(productionItemId);
        }

        private object DeleteProductionItem(dynamic parameters)
        {
            _dbManager.DeleteProductionItem(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        #endregion

        #region Operations

        private object GetOperations(dynamic parameters)
        {
            var operations = _dbManager.GetOperations().ToList();
            var dtoOperations = operations.Select(d => _dtoConverter.ConvertOperation(d)).ToList();

            return dtoOperations;
        }

        private object GetOperationsByDetailId(dynamic parameters)
        {
            int detailId = Request.Query["detailId"];
            var operations = _dbManager.GetOperationsByDetailId(detailId).ToList();
            var dtoOperations = operations.Select(d => _dtoConverter.ConvertOperation(d)).ToList();

            return dtoOperations;
        }

        private object CreateOperation(dynamic parameters)
        {
            var requestBody = this.Bind<OperationDto>();
            var operationId = _dbManager.CreateOperation(_dtoConverter.ConvertOperation(requestBody));

            return Response.AsJson(operationId);
        }

        private object DeleteOperation(dynamic parameters)
        {
            _dbManager.DeleteOperation(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        #endregion

        #region Routes

        private object GetRoutes(dynamic parameters)
        {
            var routes = _dbManager.GetRoutes().ToList();
            var dtoRoutes = routes.Select(d => _dtoConverter.ConvertRoute(d)).ToList();

            return dtoRoutes;
        }

        private object CreateRoute(dynamic parameters)
        {
            var requestBody = this.Bind<RouteDto>();
            var routeId = _dbManager.CreateRoute(_dtoConverter.ConvertRoute(requestBody));

            return Response.AsJson(routeId);
        }

        private object DeleteRoute(dynamic parameters)
        {
            _dbManager.DeleteRoute(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        #endregion

        #region ConveyorsAndWorkshops
        private object GetConveyors(dynamic parameters)
        {
            var conveyors = _dbManager.GetConveyors().ToList();
            var dtoConveyors = conveyors.Select(d => _dtoConverter.ConvertConveyor(d)).ToList();

            return dtoConveyors;
        }

        private object GetWorkshops(dynamic parameters)
        {
            var workshops = _dbManager.GetWorkshops().ToList();
            var dtoWorkshops = workshops.Select(d => _dtoConverter.ConvertWorkshop(d)).ToList();

            return dtoWorkshops;
        }
        #endregion
    }
}
