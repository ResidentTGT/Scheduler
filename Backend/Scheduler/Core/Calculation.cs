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
            Logger.Log($"Расчет заказа id: {orderId} начат.", LogLevel.Info);

            Logger.Log($"Получение заказа из базы...", LogLevel.Info);
            var order = _dbManager.GetOrderById(orderId);
            Logger.Log($"Заказ загружен из базы. Id: {order.Id}, название: {order.Name}, описание: {order.Description}.", LogLevel.Info);

            Logger.Log($"Обновление состояния заказа...", LogLevel.Info);
            _dbManager.SetOrderState(order.Id, OrderState.InProcess);
            Logger.Log($"Состояние заказа в базе обновлено. Текущее состояние: в процессе.", LogLevel.Info);


            var assemblingTime = new AssemblingTime(_dbManager);
            Logger.Log($"Начат расчет времен сборок партий изделий на конвейере для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);
            var assemblingTimes = assemblingTime.CalculateAssemblingTimes(order);
            Logger.Log($"Закончен расчет времен сборок партий изделий на конвейере для заказа: id = {order.Id}, название = '{order.Name}'.", LogLevel.Info);

            

        }
    }
}
