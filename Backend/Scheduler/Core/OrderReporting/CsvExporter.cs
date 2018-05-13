using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Database;
using Scheduler.Model;

namespace Scheduler.Core.OrderReporting
{
    internal class CsvExporter
    {
        private DbManager _dbManager;

        internal CsvExporter(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        internal void GenerateCsvExportFile(Order order)
        {
            var filePath = $"C://{order.Name}-{DateTime.Now.Ticks}.csv";

            if (File.Exists(filePath))
                throw new Exception("Файл с таким именем уже существует.");

            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                WriteOrderInfo(order, sw);

                sw.WriteLine("\n");

                WriteOrderQuantumsInfo(order, sw);
            }
        }

        private void WriteOrderQuantumsInfo(Order order, StreamWriter sw)
        {
            foreach (var orderQuantum in order.OrderQuantums)
            {
                sw.WriteLine($"Продукция;Количество продукции;Количество групп;Начало производства;Длительность производства;Окончание производства");
                sw.WriteLine($"{orderQuantum.ProductionItem.Title};{orderQuantum.Count};{orderQuantum.MachiningDurations.Count};" +
                    $"{orderQuantum.MachiningStartTimes[0]};{orderQuantum.AssemblingEndTimes.Last() - orderQuantum.MachiningStartTimes[0]};{orderQuantum.AssemblingEndTimes.Last()}");

                sw.WriteLine("\n;Сборка");
                sw.WriteLine(";Начало;Длительность;Окончание");
                sw.WriteLine($";{orderQuantum.AssemblingStartTimes[0]};{orderQuantum.AssemblingEndTimes.Last() - orderQuantum.AssemblingStartTimes[0]};{orderQuantum.AssemblingEndTimes.Last()};");
                sw.WriteLine(";;По группам деталей");
                sw.WriteLine(";;Номер группы;Начало;Длительность;Окончание");
                for (int i = 0; i < orderQuantum.AssemblingDurations.Count; i++)
                    sw.WriteLine($";;Группа {i + 1};{orderQuantum.AssemblingStartTimes[i]};{orderQuantum.AssemblingDurations[i]};{orderQuantum.AssemblingEndTimes[i]};");

                sw.WriteLine("\n;Обработка");
                sw.WriteLine(";Начало;Длительность;Окончание");
                sw.WriteLine($";{orderQuantum.MachiningStartTimes[0]};{orderQuantum.MachiningEndTimes.Last() - orderQuantum.MachiningStartTimes[0]};{orderQuantum.MachiningEndTimes.Last()};");
                sw.WriteLine(";;По группам деталей");
                sw.WriteLine(";;Номер группы;Начало;Длительность;Окончание");
                for (int i = 0; i < orderQuantum.MachiningDurations.Count; i++)
                    sw.WriteLine($";;Группа {i + 1};{orderQuantum.MachiningStartTimes[i]};{orderQuantum.MachiningDurations[i]};{orderQuantum.MachiningEndTimes[i]};");

                WriteGroupsInfo(orderQuantum, sw);
            }
        }

        private void WriteGroupsInfo(OrderQuantum orderQuantum, StreamWriter sw)
        {
            sw.WriteLine(";;Для каждой группы");
            var groups = orderQuantum.ProductionItem.ProductionItemQuantumsGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                sw.WriteLine($";;Группа {i + 1}");
                sw.WriteLine(";;Цех;Начало;Длительность;Окончание;Средняя загруженность станков");
                for (int j = 0; j < groups[i].WorkshopSequence.Count; j++)
                    sw.WriteLine($";;{_dbManager.GetWorkshop(groups[i].WorkshopSequence[j]).Name};{groups[i].WorkshopStartTimes[j]};{groups[i].WorkshopDurations[j]};{groups[i].WorkshopEndTimes[j]};");
            }

            sw.WriteLine(";;;Для деталей");

            for (int i = 0; i < groups.Count; i++)
            {
                sw.WriteLine($";;;Группа;Группа {i + 1}");
                for (int j = 0; j < groups[i].ProductionItemQuantums.Count; j++)
                {
                    sw.WriteLine(";;;Деталь;Количество;Количество операций");

                    sw.WriteLine($";;;{groups[i].ProductionItemQuantums[j].Detail.Title};{groups[i].ProductionItemQuantums[j].Count};" +
                        $"{groups[i].ProductionItemQuantums[j].Detail.Operations.Where(o => o.Type == OperationType.Machining).Count()};");
                    sw.WriteLine(";;;Цех;Станок;Начало;Длительность;Окончание");
                    var productionItemQuantum = groups[i].ProductionItemQuantums[j];
                    for (int k = 0; k < productionItemQuantum.MachiningDurations.Count; k++)
                    {
                        sw.WriteLine($";;;{_dbManager.getWorkshopByEquipmentId(productionItemQuantum.Detail.EquipmentsIdSequence[k]).Name};" +
                            $"{productionItemQuantum.Detail.EquipmentsNameSequence[k]};{productionItemQuantum.StartTimes[k]};{productionItemQuantum.MachiningDurations[k]};{productionItemQuantum.EndTimes[k]}");
                    }
                }
            }

        }

        private void WriteOrderInfo(Order order, StreamWriter sw)
        {
            sw.WriteLine($"Заказ;План. начало;План. окончание");
            sw.WriteLine($"{order.Name};{order.PlannedBeginDate};{order.PlannedEndDate}");
        }
    }
}
