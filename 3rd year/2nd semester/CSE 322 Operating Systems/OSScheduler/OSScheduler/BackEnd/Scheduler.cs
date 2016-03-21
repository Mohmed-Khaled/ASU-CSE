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
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime);
            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
        }

        private void Sjfp()
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime);
            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
        }

        private void Pnp()
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime);

            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
        }

        private void Pp()
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime);
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
