using Scheduler.Database;
using Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Model;
using Scheduler.Core.Assembling;
using Scheduler.Log;
using Scheduler.Core.Grouping;
using Scheduler.Core.DeterminingOrder;
using Scheduler.Core.CountingTime;
using System.Diagnostics;
using Scheduler.Core.OrderReporting;

namespace Scheduler.Core
{
    internal class Calculation
    {
        private DbManager _dbManager;
        private DtoConverter _dtoConverter;

        public Calculation()
        {
            _dbManager = new DbManager();
            _dtoConverter = new DtoConverter();
        }

        internal void CalculateOrderById(int orderId)
        {
            var timer = new Stopwatch();
            timer.Start();

            Logger.Log($"Расчет заказа id: {orderId} начат.", LogLevel.Info);

            Logger.Log($"Получение заказа из базы...", LogLevel.Info);
            var order = _dbManager.GetOrderById(orderId);
            Logger.Log($"Заказ загружен из базы. Id: {order.Id}, название: {order.Name}, описание: {order.Description}.", LogLevel.Info);

            Logger.Log($"Обновление состояния заказа...", LogLevel.Info);
            _dbManager.SetOrderState(order.Id, OrderState.InProcess);
            Logger.Log($"Состояние заказа в базе обновлено. Текущее состояние: в процессе.", LogLevel.Info);


            var assemblingTime = new AssemblingTime(_dbManager);
            Logger.Log($"Начат расчет времен сборок партий изделий на конвейере для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);
            assemblingTime.CalculateAssemblingTimes(order);
            Logger.Log($"Закончен расчет времен сборок партий изделий на конвейере для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);

            var grouping = new GroupingDetails(_dbManager);
            //Сортировка операций согласно маршруту и группировка по цехам
            grouping.SetWorkshopSequenceForDetails(order);

            Logger.Log($"Начато группирование деталей по маршрутам их следования для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);
            grouping.GroupDetails(order);
            Logger.Log($"Закончено группирование деталей по маршрутам их следования для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);

            var determiningDetailsOrder = new DeterminingDetailsOrder(_dbManager);
            Logger.Log($"Начато определение порядка обработки деталей на станках для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);
            determiningDetailsOrder.DetermineDetailsOrderInGroups(order);
            Logger.Log($"Закончено определение порядка обработки деталей на станках для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);

            var determiningGroupsOrder = new DeterminingGroupsOrder(_dbManager);
            Logger.Log($"Начато определение порядка обработки групп для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);
            determiningGroupsOrder.DetermineGroupsOrder(order);
            Logger.Log($"Закончено определение порядка обработки групп для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);

            Logger.Log($"Начат подсчет времен для части заказов (партий изделий) для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);
            OrderQuantumsTiming.CountTimeForOrderQuantums(order);
            Logger.Log($"Закончен подсчет времен для части заказов (партий изделий) для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);

            _dbManager.SetOrderState(order.Id, OrderState.Ready);

            timer.Stop();
            Logger.Log($"Расчет заказа завершен. Занятое время: {timer.Elapsed}.", LogLevel.Info);

            Logger.Log($"Начато формирование отчета.", LogLevel.Info);
            var reporting = new Reporting(_dbManager);
            var report = reporting.CreateReport(order);
            _dbManager.CreateReport(report);
            Logger.Log($"Закончено формирование отчета.", LogLevel.Info);
        }
    }
}
