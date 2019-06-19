using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerClassWork
{
    public class MyTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTimeStart { set; get; }
        public MyEvent Event { set; get; }
        public MyPeriodcity Periodcity { set; get; }
        public String Parameter1 { set; get; }
        public String Parameter2 { set; get; }
    }
}
