using System.Collections.Generic;

namespace OSScheduler.BackEnd
{
    public class Process
    {
        private LinkedListNode<Process> first;

        public string Name { get; set; }
        public double BurstTime { get; set; }
        public double ArrivalTime { get; set; }
        public int Priority { get; set; }

        public Process() { }

        public Process(string name, double burstTime, double arrivalTime)
        {
            Name = name;
            BurstTime = burstTime;
            ArrivalTime = arrivalTime;
            Priority = 0;
        }

        public Process(string name, double burstTime, double arrivalTime, int priority)
        {
            Name = name;
            BurstTime = burstTime;
            ArrivalTime = arrivalTime;
            Priority = priority;
        }

        public Process(LinkedListNode<Process> first)
        {
            this.first = first;
        }
    }
}
