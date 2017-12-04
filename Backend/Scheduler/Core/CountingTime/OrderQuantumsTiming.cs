using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Model;
using Scheduler.Log;

namespace Scheduler.Core.CountingTime
{
    public static class OrderQuantumsTiming
    {
        internal static void CountTimeForOrderQuantums(Order order)
        {
            var orderQuantums = order.OrderQuantums;

            for (var i = 0; i < orderQuantums.Count; i++)
            {
                Logger.Log($"Начат расчет времен частей партий для изделия: {orderQuantums[i].ProductionItem.Title}, кол-во: {orderQuantums[i].Count}.", LogLevel.Trace);
                var blocksCount = Math.Round((double)orderQuantums[i].Count / orderQuantums[i].ItemsCountInOnePart);
                Logger.Log($"Количество получившихся частей партий изделий: {blocksCount}.", LogLevel.Trace);

                orderQuantums[i].MachiningDurations = new List<TimeSpan>();
                orderQuantums[i].MachiningStartTimes = new List<TimeSpan>();
                orderQuantums[i].MachiningEndTimes = new List<TimeSpan>();
                orderQuantums[i].AssemblingDurations = new List<TimeSpan>();
                orderQuantums[i].AssemblingStartTimes = new List<TimeSpan>();
                orderQuantums[i].AssemblingEndTimes = new List<TimeSpan>();

                for (var j = 0; j < blocksCount; j++)
                {
                    orderQuantums[i].MachiningDurations.Add(orderQuantums[i].MachiningFullPartTime);
                    orderQuantums[i].MachiningStartTimes.Add(new TimeSpan(orderQuantums[i].MachiningDurations[j].Ticks * j));
                    orderQuantums[i].MachiningEndTimes.Add(new TimeSpan(orderQuantums[i].MachiningDurations[j].Ticks * (j + 1)));
                }

                for (var j = 0; j < blocksCount; j++)
                {
                    if (orderQuantums[i].AssemblingFullPartTime <= orderQuantums[i].MachiningFullPartTime)
                    {
                        var dividedFullTime = new TimeSpan((long)Math.Round(orderQuantums[i].AssemblingFullBatchTime.Ticks / blocksCount));
                        orderQuantums[i].AssemblingEndTimes.Add(orderQuantums[i].MachiningEndTimes[(int)blocksCount - 1] + dividedFullTime
                            - new TimeSpan((int)(blocksCount - j - 1) * dividedFullTime.Ticks));

                        orderQuantums[i].AssemblingDurations.Add(dividedFullTime);

                        orderQuantums[i].AssemblingStartTimes.Add(orderQuantums[i].AssemblingEndTimes[j] - dividedFullTime);
                    }
                    else
                    {
                        orderQuantums[i].AssemblingEndTimes.Add(orderQuantums[i].MachiningStartTimes[j] + orderQuantums[i].AssemblingFullPartTime);

                        orderQuantums[i].AssemblingDurations.Add(orderQuantums[i].AssemblingFullPartTime);

                        orderQuantums[i].AssemblingStartTimes.Add(orderQuantums[i].AssemblingEndTimes[j] - orderQuantums[i].AssemblingFullPartTime);
                    }
                    
                }

                if (i != 0)
                {
                    var diff = new TimeSpan(Math.Max((orderQuantums[i].MachiningStartTimes.First() - orderQuantums[i - 1].MachiningEndTimes.Last()).Ticks
                        , (orderQuantums[i].AssemblingStartTimes.First() - orderQuantums[i - 1].AssemblingEndTimes.Last()).Ticks));
                    for (var j = 0; j < blocksCount; i++)
                    {
                        orderQuantums[i].AssemblingStartTimes[j] += diff;
                        orderQuantums[i].AssemblingEndTimes[j] += diff;
                        orderQuantums[i].MachiningStartTimes[j] += diff;
                        orderQuantums[i].MachiningEndTimes[j] += diff;
                    }
                }

                Logger.Log($"Закончен расчет времен частей партий для изделия: {orderQuantums[i].ProductionItem.Title}, кол-во: {orderQuantums[i].Count}." +
                    $"Начало времени обработки: {orderQuantums[i].MachiningStartTimes.First()}, окончание времени обработки: {orderQuantums[i].MachiningEndTimes.Last()}, " +
                    $"Начало времени сборки: {orderQuantums[i].AssemblingStartTimes.First()}, окончание времени обработки: {orderQuantums[i].AssemblingEndTimes.Last()}", LogLevel.Trace);
            }
        }
    }
}
