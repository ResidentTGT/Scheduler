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
            Get[@"^(^(?i)((?!/api/).)*$)$"] = Index;

            Get["/api/details"] = GetDetails;
            Get["/api/details-with-routes"] = GetDetailsWithRoutes;
            Post["/api/create-detail"] = CreateDetail;
            Get["/api/delete-detail"] = DeleteDetail;

            Get["/api/equipments"] = GetEquipments;
            Post["/api/create-equipment"] = CreateEquipment;
            Get["/api/delete-equipment"] = DeleteEquipment;

            Get["/api/orders"] = GetOrders;
            Post["/api/create-order"] = CreateOrder;
            Get["/api/delete-order"] = DeleteOrder;
            Get["/api/calculate-order"] = CalculateOrder;

            Get["/api/production-items"] = GetProductionItems;
            Post["/api/create-production-item"] = CreateProductionItem;
            Get["/api/delete-production-item"] = DeleteProductionItem;

            Get["/api/operations"] = GetOperations;
            Get["/api/detail-operations"] = GetOperationsByDetailId;
            Post["/api/create-operation"] = CreateOperation;
            Get["/api/delete-operation"] = DeleteOperation;

            Get["/api/routes"] = GetRoutes;
            Post["/api/create-route"] = CreateRoute;
            Get["/api/delete-route"] = DeleteRoute;

            Get["/api/conveyors"] = GetConveyors;
            Post["/api/create-conveyor"] = CreateConveyor;
            Get["/api/delete-conveyor"] = DeleteConveyor;

            Get["/api/workshops"] = GetWorkshops;
            Post["/api/create-workshop"] = CreateWorkshop;
            Get["/api/delete-workshop"] = DeleteWorkshop;
        }

        private object Index(dynamic parameters)
        {
            return View["wwwroot/index.html"];
        }

        #region DetailsApi
        private object GetDetails(dynamic parameters)
        {
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);
            int equipmentId = Int32.Parse(Request.Query["equipmentId"].Value);

            var details = _dbManager.GetDetails(pageNumber, pageSize, equipmentId);

            var dtoDetails = details.Select(d => _dtoConverter.ConvertDetail(d)).ToList();

            return dtoDetails;
        }

        private object GetDetailsWithRoutes(dynamic parameters)
        {
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var details = _dbManager.GetDetailsWithRoutes(pageNumber, pageSize);

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
            OperationType? operationType = (OperationType)Int32.Parse(Request.Query["operationType"].Value);

            var equipments = _dbManager.GetEquipments(pageNumber, pageSize, operationType);

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
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var orders = _dbManager.GetOrders(pageNumber, pageSize);
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

        private object CalculateOrder(dynamic parameters)
        {
            var calculation = new Calculation();
            Task.Run(() => { calculation.CalculateOrderById(Request.Query["id"]); });

            return Response.AsJson(HttpStatusCode.OK);
        }
        #endregion

        #region ProductionItemApi
        private object GetProductionItems(dynamic parameters)
        {
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var productionItems = _dbManager.GetProductionItems(pageNumber, pageSize);
            var dtoProductionItems = productionItems.Select(d => _dtoConverter.ConvertProductionItemForView(d)).ToList();

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
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var operations = _dbManager.GetOperations(pageNumber, pageSize).ToList();
            var dtoOperations = operations.Select(d => _dtoConverter.ConvertOperation(d)).ToList();

            return dtoOperations;
        }

        private object GetOperationsByDetailId(dynamic parameters)
        {
            int detailId = Request.Query["detailId"];
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var operations = _dbManager.GetOperationsByDetailId(detailId, pageNumber, pageSize).ToList();
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
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var routes = _dbManager.GetRoutes(pageNumber, pageSize).ToList();
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
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var conveyors = _dbManager.GetConveyors(pageNumber, pageSize).ToList();
            var dtoConveyors = conveyors.Select(d => _dtoConverter.ConvertConveyor(d)).ToList();

            return dtoConveyors;
        }

        private object GetWorkshops(dynamic parameters)
        {
            int pageNumber = Int32.Parse(Request.Query["pageNumber"].Value);
            int pageSize = Int32.Parse(Request.Query["pageSize"].Value);

            var workshops = _dbManager.GetWorkshops(pageNumber, pageSize).ToList();
            var dtoWorkshops = workshops.Select(d => _dtoConverter.ConvertWorkshop(d)).ToList();

            return dtoWorkshops;
        }

        private object CreateWorkshop(dynamic parameters)
        {
            var requestBody = this.Bind<WorkshopDto>();
            var workshopId = _dbManager.CreateWorkshop(_dtoConverter.ConvertWorkshop(requestBody));

            return Response.AsJson(workshopId);
        }

        private object DeleteWorkshop(dynamic parameters)
        {
            _dbManager.DeleteWorkshop(Request.Query["id"]);

            return HttpStatusCode.OK;
        }

        private object CreateConveyor(dynamic parameters)
        {
            var requestBody = this.Bind<ConveyorDto>();
            var conveyorId = _dbManager.CreateConveyor(_dtoConverter.ConvertConveyor(requestBody));

            return Response.AsJson(conveyorId);
        }

        private object DeleteConveyor(dynamic parameters)
        {
            _dbManager.DeleteConveyor(Request.Query["id"]);

            return HttpStatusCode.OK;
        }
        #endregion
    }
}
