using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSScheduler.BackEnd
{
    public class TimeInfo
    {
        public string ProccessName { get; set; }
        public double ProcessLastTime { get; set; }

        public TimeInfo(string proccessName, double processLastTime)
        {
            ProccessName = proccessName;
            ProcessLastTime = processLastTime;
        }
    }
}
