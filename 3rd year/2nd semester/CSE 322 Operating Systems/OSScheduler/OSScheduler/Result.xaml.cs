using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OSScheduler.BackEnd;
using OSScheduler.GUI;

namespace OSScheduler
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result
    {
        private LinkedList<TimeInfo> _timeInfos = new LinkedList<TimeInfo>();
         
        public Result()
        {
            InitializeComponent();
            StackPanel.Children.Add(new TimeMarker(0));
            var chartBar1 = new ChartBar("P1", 4) {VerticalAlignment = VerticalAlignment.Top};
            StackPanel.Children.Add(chartBar1);
            StackPanel.Children.Add(new TimeMarker(4));
            var chartBar2 = new ChartBar("P2", 2) { VerticalAlignment = VerticalAlignment.Top };
            StackPanel.Children.Add(chartBar2);
            StackPanel.Children.Add(new TimeMarker(6));
        }

        public Result(LinkedList<Process> list, int numberOfProcesses)
        {
            double avwt = 0;
            InitializeComponent();
            var itrProcess = list.First;
            while (itrProcess != null)
            {
                var process = itrProcess.Value;
                double lastMark;
                LinkedListNode<TimeInfo> itrTime;
                if (process == list.First())
                {
                    StackPanel.Children.Add(new TimeMarker(process.ArrivalTime));
                }
                else
                {
                    lastMark = double.Parse(StackPanel.Children.OfType<TimeMarker>().Last().Time.Content.ToString());
                    if (Math.Abs(process.ArrivalTime - (-1)) < 0.0001)
                    {
                        process.ArrivalTime = lastMark;
                        itrTime = _timeInfos.First;
                        while (itrTime != null)
                        {
                            if (itrTime.Value.ProccessName != process.Name)
                            {
                                itrTime = itrTime.Next;
                                continue;
                            }
                            avwt += process.ArrivalTime - itrTime.Value.ProcessLastTime;
                            break;
                        }
                    }
                    else if (process.ArrivalTime > lastMark)
                    {
                        var chartBarIdle = new ChartBar("Idle", process.ArrivalTime - lastMark)
                        { VerticalAlignment = VerticalAlignment.Top };
                        StackPanel.Children.Add(chartBarIdle);
                        StackPanel.Children.Add(new TimeMarker(process.ArrivalTime));
                    }
                    else
                    {
                        avwt += lastMark - process.ArrivalTime;
                    }
                }
                var chartBar = new ChartBar(process.Name, process.BurstTime)
                { VerticalAlignment = VerticalAlignment.Top };
                StackPanel.Children.Add(chartBar);
                lastMark = double.Parse(StackPanel.Children.OfType<TimeMarker>().Last().Time.Content.ToString());
                StackPanel.Children.Add(new TimeMarker(lastMark + process.BurstTime));
                var flag = true;
                itrTime = _timeInfos.First;
                while (itrTime != null)
                {
                    if (itrTime.Value.ProccessName != process.Name)
                    {
                        itrTime = itrTime.Next;
                        continue;
                    }
                    itrTime.Value.ProcessLastTime = lastMark + process.BurstTime;
                    flag = false;
                    break;
                }
                if (flag)
                    _timeInfos.AddLast(new TimeInfo(process.Name, lastMark + process.BurstTime));
                itrProcess = itrProcess.Next;
            }
            Avwt.Content = "Average waiting time = " + avwt / numberOfProcesses;
        }
    }
}
