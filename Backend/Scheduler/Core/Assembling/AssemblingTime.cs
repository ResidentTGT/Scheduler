using Scheduler.Database;
using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Assembling
{

    public class AssemblingTime
    {
        private DbManager _dbManager;

        public AssemblingTime(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public List<OrderQuantumAssemblingTime> CalculateAssemblingTimes(Order order)
        {
          
            var orderQuantumAssemblingTimes = new List<OrderQuantumAssemblingTime>();

            foreach (var orderQuantum in order.OrderQuantums)
            {
                Logger.Log($"Начат расчет времен сборки партии изделий типа '{orderQuantum.ProductionItem.Title}' в кол-ве {orderQuantum.Count} штук.", LogLevel.Info);

                Logger.Log($"Начат поиск операций на сборочных местах конвейера для сборки изделия.", LogLevel.Trace);
                var operations = _dbManager.GetOperationsByProductionItemId(orderQuantum.ProductionItem.Id)
                    .Where(o => o.Type == OperationType.Assembling)
                    .ToList();
                Logger.Log($"Поиск операций закончен. Кол-во: {operations.Count}", LogLevel.Trace);

                Logger.Log($"Начат расчет времен сборок для всей партии изделий, передаточной части партии, а также оставшейся части изделий в случае неполной части партии.", LogLevel.Info);
                var orderQuantumAssemblingTime = new OrderQuantumAssemblingTime()
                {
                    ProductionItem = orderQuantum.ProductionItem,
                    FullBatchTime = CalculateAssemblingTimeForProductsCount(operations, orderQuantum.Count),
                    ProductionsItemsPartTime = CalculateAssemblingTimeForProductsCount(operations, orderQuantum.ItemsCountInOnePart)
                };

                if (orderQuantum.Count % orderQuantum.ItemsCountInOnePart != 0)
                    orderQuantumAssemblingTime.RemainingFromPartsTime = CalculateAssemblingTimeForProductsCount(operations, orderQuantum.Count % orderQuantum.ItemsCountInOnePart);
                Logger.Log($"Закончен расчет времен. Для всей партии: {orderQuantumAssemblingTime.FullBatchTime}, для части партии: {orderQuantumAssemblingTime.ProductionsItemsPartTime}, " +
                    $" для неполной части: {orderQuantumAssemblingTime.RemainingFromPartsTime}", LogLevel.Info);

                Logger.Log($"Закончен расчет времен сборки партии изделий типа '{orderQuantum.ProductionItem.Title}'. Суммарное время сборки для всей партии: {orderQuantumAssemblingTime.FullBatchTime}", LogLevel.Info);

                orderQuantumAssemblingTimes.Add(orderQuantumAssemblingTime);

            }           

            return orderQuantumAssemblingTimes;
        }

        public TimeSpan CalculateAssemblingTimeForProductsCount(List<Operation> operations, int count)
        {
            var conveyor = _dbManager.GetEquipmentByOperationId((int)operations.First().Id).Conveyor; 

            var currentOperations = new List<OperationProcess>();
            foreach (var oper in operations)
                currentOperations.Add(new OperationProcess()
                {
                    Operation = oper,
                    ServedCount = 0,
                    State = OperationState.NotStarted
                });

            var sumTime = new TimeSpan(0);

            while (currentOperations.Last().ServedCount != count)
            {
                var notStartedOperation = currentOperations.FirstOrDefault(o => o.State == OperationState.NotStarted && o.ServedCount != count);
                if (notStartedOperation != null)
                {
                    notStartedOperation.State = OperationState.InProcess;
                }

                var finishedOperations = currentOperations.Where(o => o.State == OperationState.Finished);
                foreach (var o in finishedOperations)
                    o.State = (o.ServedCount == count)
                        ? OperationState.NotStarted
                        : OperationState.InProcess;

                var inProcessOperations = currentOperations.Where(o => o.State == OperationState.InProcess).ToList();
                sumTime += (GetMaxTime(inProcessOperations) + conveyor.TransportTime);

                foreach (var oper in inProcessOperations)
                {
                    oper.State = OperationState.Finished;
                    oper.ServedCount++;
                }
            }
            return sumTime;
        }

        private TimeSpan GetMaxTime(IEnumerable<OperationProcess> operations)
        {
            return operations.Max(op => op.Operation.MainTime);
        }
    }
}
