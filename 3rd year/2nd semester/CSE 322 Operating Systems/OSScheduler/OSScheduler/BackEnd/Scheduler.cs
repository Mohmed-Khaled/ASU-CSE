using System;
using System.Collections.Generic;
using System.Linq;

namespace OSScheduler.BackEnd
{
    public class Scheduler
    {
        public LinkedList<Process> Processes { get; set; }
        private LinkedList<Process> SortedProcesses { get; set; }

        public Scheduler(LinkedList<Process> processes)
        {
            Processes = processes;
        }

        public LinkedList<Process> Sort(string alg,double q = 0)
        {
            switch (alg)
            {
                case "FCFS":
                    Fcfs();
                    break;
                case "SJFNP":
                    Sjfnp();
                    break;
                case "SJFP":
                    Sjfp();
                    break;
                case "PNP":
                    Pnp();
                    break;
                case "PP":
                    Pp();
                    break;
                case "RR":
                    Rr(q);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return SortedProcesses;
        }

        private void Fcfs()
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime);
            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
        }

        private void Sjfnp()
        {
            SortedProcesses = new LinkedList<Process>();
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.BurstTime);
            var tmpProcesses = new LinkedList<Process>(orderedEnumerable);
            var intrProcesses = new LinkedList<Process>();
            var currentTime = tmpProcesses.First.Value.ArrivalTime;
            while (tmpProcesses.First != null)
            {
                if (Math.Abs(currentTime - tmpProcesses.First.Value.ArrivalTime) < 0.0001)
                {
                    var tmp = tmpProcesses.First.Value;
                    tmpProcesses.Remove(tmp);
                    SortedProcesses.AddLast(tmp);
                    currentTime += tmp.BurstTime;
                }
                else if (currentTime > tmpProcesses.First.Value.ArrivalTime)
                {
                    var itr = tmpProcesses.First;
                    while (itr != null && currentTime > itr.Value.ArrivalTime)
                    {
                        var tmp = itr.Value;
                        tmpProcesses.Remove(tmp);
                        intrProcesses.AddLast(tmp);
                        itr = tmpProcesses.First;
                    }
                    var orderedEnumerable2 = intrProcesses.OrderBy(x => x.BurstTime).ThenBy(x => x.ArrivalTime);
                    intrProcesses = new LinkedList<Process>(orderedEnumerable2);
                    while (intrProcesses.First != null)
                    {
                        var tmp = intrProcesses.First.Value;
                        intrProcesses.Remove(tmp);
                        SortedProcesses.AddLast(tmp);
                        currentTime += tmp.BurstTime;
                    }

                }
                else if (currentTime < tmpProcesses.First.Value.ArrivalTime)
                {
                    currentTime = tmpProcesses.First.Value.ArrivalTime;
                }
            }
        }

        private void Sjfp()
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.BurstTime);
            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
        }

        private void Pnp()
        {
            SortedProcesses = new LinkedList<Process>();
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.Priority);
            var tmpProcesses = new LinkedList<Process>(orderedEnumerable);
            var intrProcesses = new LinkedList<Process>();
            var currentTime = tmpProcesses.First.Value.ArrivalTime;
            while (tmpProcesses.First != null)
            {
                if (Math.Abs(currentTime - tmpProcesses.First.Value.ArrivalTime) < 0.0001)
                {
                    var tmp = tmpProcesses.First.Value;
                    tmpProcesses.Remove(tmp);
                    SortedProcesses.AddLast(tmp);
                    currentTime += tmp.BurstTime;
                }
                else if (currentTime > tmpProcesses.First.Value.ArrivalTime)
                {
                    var itr = tmpProcesses.First;
                    while (itr != null && currentTime > itr.Value.ArrivalTime)
                    {
                        var tmp = itr.Value;
                        tmpProcesses.Remove(tmp);
                        intrProcesses.AddLast(tmp);
                        itr = tmpProcesses.First;
                    }
                    var orderedEnumerable2 = intrProcesses.OrderBy(x => x.Priority).ThenBy(x => x.ArrivalTime);
                    intrProcesses = new LinkedList<Process>(orderedEnumerable2);
                    while (intrProcesses.First != null)
                    {
                        var tmp = intrProcesses.First.Value;
                        intrProcesses.Remove(tmp);
                        SortedProcesses.AddLast(tmp);
                        currentTime += tmp.BurstTime;
                    }

                }
                else if (currentTime < tmpProcesses.First.Value.ArrivalTime)
                {
                    currentTime = tmpProcesses.First.Value.ArrivalTime;
                }
            }
        }

        private void Pp()
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.Priority);
            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
        }

        private void Rr(double q)
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime);
            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
            var itrProcess = SortedProcesses.First;
            while (itrProcess != null )
            {
                var process = itrProcess.Value;
                if (process.BurstTime <= q)
                {
                    itrProcess = itrProcess.Next;
                    continue;
                }
                SortedProcesses.AddLast(new Process(process.Name, process.BurstTime - q, -1));
                process.BurstTime = q;
                itrProcess = itrProcess.Next;
            }
        }

    }
}
