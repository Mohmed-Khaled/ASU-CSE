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

        #region Algorithms
        private void Fcfs()
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.Name);
            SortedProcesses = new LinkedList<Process>(orderedEnumerable);
        }

        private void Sjfnp()
        {
            SortedProcesses = new LinkedList<Process>();
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.BurstTime).ThenBy(x => x.Name);
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
                    var orderedEnumerable2 = intrProcesses.OrderBy(x => x.BurstTime).ThenBy(x => x.ArrivalTime).ThenBy(x => x.Name);
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
            SortedProcesses = new LinkedList<Process>();
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.BurstTime).ThenBy(x => x.Name);
            var tmpProcesses = new LinkedList<Process>(orderedEnumerable);
            var currentTime = tmpProcesses.First.Value.ArrivalTime;
            var lastTime = tmpProcesses.Last.Value.ArrivalTime;
            var nextTime = GetNextTime(tmpProcesses.First,currentTime);
            while (tmpProcesses.First != null)
            {
                if (currentTime < tmpProcesses.First.Value.ArrivalTime)
                {
                    currentTime = tmpProcesses.First.Value.ArrivalTime;
                    nextTime = GetNextTime(tmpProcesses.First, currentTime);
                }
                else if (currentTime >= lastTime)
                {
                    var orderedEnumerable2 = tmpProcesses.OrderBy(x => x.BurstTime).ThenBy(x => x.ArrivalTime).ThenBy(x => x.Name);
                    tmpProcesses = new LinkedList<Process>(orderedEnumerable2);
                    while (tmpProcesses.First != null)
                    {
                        var tmp = tmpProcesses.First.Value;
                        tmpProcesses.Remove(tmp);
                        if (SortedProcesses.Last != null && SortedProcesses.Last.Value.Name == tmp.Name)
                        {
                            SortedProcesses.Last.Value.BurstTime += tmp.BurstTime;
                        }
                        else
                        {
                            SortedProcesses.AddLast(tmp);
                        }
                        currentTime += tmp.BurstTime;
                    }
                }
                else if (tmpProcesses.First.Value.BurstTime <= nextTime - currentTime)
                {
                    var tmp = tmpProcesses.First.Value;
                    tmpProcesses.Remove(tmp);
                    if (SortedProcesses.Last != null && SortedProcesses.Last.Value.Name == tmp.Name)
                    {
                        SortedProcesses.Last.Value.BurstTime += tmp.BurstTime;
                    }
                    else
                    {
                        SortedProcesses.AddLast(tmp);
                    }
                    currentTime += tmp.BurstTime;
                }
                else if (tmpProcesses.First.Value.BurstTime > nextTime - currentTime)
                {
                    PushPartOfProcess(tmpProcesses.First.Value, currentTime, nextTime - currentTime);
                    currentTime = nextTime;
                    SortAccordingToTime(tmpProcesses, currentTime);
                    nextTime = GetNextTime(tmpProcesses.First, currentTime);
                }
            }
        }

        private void Pnp()
        {
            SortedProcesses = new LinkedList<Process>();
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.Priority).ThenBy(x => x.Name);
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
                    var orderedEnumerable2 = intrProcesses.OrderBy(x => x.Priority).ThenBy(x => x.ArrivalTime).ThenBy(x => x.Name);
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
            SortedProcesses = new LinkedList<Process>();
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.Priority).ThenBy(x => x.Name);
            var tmpProcesses = new LinkedList<Process>(orderedEnumerable);
            var currentTime = tmpProcesses.First.Value.ArrivalTime;
            var lastTime = tmpProcesses.Last.Value.ArrivalTime;
            var nextTime = GetNextTime(tmpProcesses.First, currentTime);
            while (tmpProcesses.First != null)
            {
                if (currentTime < tmpProcesses.First.Value.ArrivalTime)
                {
                    currentTime = tmpProcesses.First.Value.ArrivalTime;
                    nextTime = GetNextTime(tmpProcesses.First, currentTime);
                }
                else if (currentTime >= lastTime)
                {
                    var orderedEnumerable2 = tmpProcesses.OrderBy(x => x.Priority).ThenBy(x => x.ArrivalTime).ThenBy(x => x.Name);
                    tmpProcesses = new LinkedList<Process>(orderedEnumerable2);
                    while (tmpProcesses.First != null)
                    {
                        var tmp = tmpProcesses.First.Value;
                        tmpProcesses.Remove(tmp);
                        if (SortedProcesses.Last != null && SortedProcesses.Last.Value.Name == tmp.Name)
                        {
                            SortedProcesses.Last.Value.BurstTime += tmp.BurstTime;
                        }
                        else
                        {
                            SortedProcesses.AddLast(tmp);
                        }
                        currentTime += tmp.BurstTime;
                    }
                }
                else if (tmpProcesses.First.Value.BurstTime <= nextTime - currentTime)
                {
                    var tmp = tmpProcesses.First.Value;
                    tmpProcesses.Remove(tmp);
                    if (SortedProcesses.Last != null && SortedProcesses.Last.Value.Name == tmp.Name)
                    {
                        SortedProcesses.Last.Value.BurstTime += tmp.BurstTime;
                    }
                    else
                    {
                        SortedProcesses.AddLast(tmp);
                    }
                    currentTime += tmp.BurstTime;
                }
                else if (tmpProcesses.First.Value.BurstTime > nextTime - currentTime)
                {
                    PushPartOfProcess(tmpProcesses.First.Value, currentTime, nextTime - currentTime);
                    currentTime = nextTime;
                    SortAccordingToPriority(tmpProcesses, currentTime);
                    nextTime = GetNextTime(tmpProcesses.First, currentTime);
                }
            }
        }

        private void Rr(double q)
        {
            var orderedEnumerable = Processes.OrderBy(x => x.ArrivalTime).ThenBy(x => x.Name);
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
        #endregion Algorithms

        #region helper functions
        private void PushPartOfProcess(Process p,double start, double time)
        {
            if (SortedProcesses.Last != null && SortedProcesses.Last.Value.Name == p.Name)
            {
                SortedProcesses.Last.Value.BurstTime += time;
            }
            else
            {
                SortedProcesses.AddLast(new Process(p.Name, time, p.ArrivalTime));
            }
            p.ArrivalTime = start + time;
            p.BurstTime -= time;
        }

        private static void SortAccordingToTime(LinkedList<Process> inList, double currentTime)
        {
            var intrProcesses = new LinkedList<Process>();
            var itr = inList.First;
            while (itr != null && currentTime >= itr.Value.ArrivalTime)
            {
                var tmp = itr.Value;
                inList.Remove(tmp);
                intrProcesses.AddLast(tmp);
                itr = inList.First;
            }
            var orderedEnumerable = intrProcesses.OrderByDescending(x => x.BurstTime).ThenByDescending(x => x.ArrivalTime).ThenByDescending(x => x.Name);
            intrProcesses = new LinkedList<Process>(orderedEnumerable);
            while (intrProcesses.First != null)
            {
                var tmp = intrProcesses.First.Value;
                intrProcesses.Remove(tmp);
                inList.AddFirst(tmp);
            }
        }

        private static void SortAccordingToPriority(LinkedList<Process> inList, double currentTime)
        {
            var intrProcesses = new LinkedList<Process>();
            var itr = inList.First;
            while (itr != null && currentTime >= itr.Value.ArrivalTime)
            {
                var tmp = itr.Value;
                inList.Remove(tmp);
                intrProcesses.AddLast(tmp);
                itr = inList.First;
            }
            var orderedEnumerable = intrProcesses.OrderByDescending(x => x.Priority).ThenByDescending(x => x.ArrivalTime).ThenByDescending(x => x.Name);
            intrProcesses = new LinkedList<Process>(orderedEnumerable);
            while (intrProcesses.First != null)
            {
                var tmp = intrProcesses.First.Value;
                intrProcesses.Remove(tmp);
                inList.AddFirst(tmp);
            }
        }

        private static double GetNextTime(LinkedListNode<Process> pNode, double currentTime)
        {
            double nextTime;
            if (pNode.Next != null)
            {
                nextTime = pNode.Next.Value.ArrivalTime;
                var itr = pNode.Next;
                while (nextTime <= currentTime && itr != null)
                {
                    nextTime = itr.Value.ArrivalTime;
                    itr = itr.Next;
                }
            }
            else
            {
                nextTime = pNode.Value.ArrivalTime;
            }
            if (nextTime < currentTime)
            {
                nextTime = currentTime;
            }
            return nextTime;
        }
        #endregion
    }
}
