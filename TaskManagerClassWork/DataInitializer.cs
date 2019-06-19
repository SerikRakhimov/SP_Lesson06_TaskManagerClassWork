using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TaskManagerClassWork
{
    public class DataInitializer : CreateDatabaseIfNotExists<TaskManagerContext>
    {
        protected override void Seed(TaskManagerContext context)
        {
            context.MyTasks.AddRange(new List<MyTask>{
                new MyTask
                {
                    Event = MyEvent.DownloadFile,
                    DateTimeStart = new DateTime(2019, 6, 19, 10, 45, 00),
                    Periodcity = MyPeriodcity.OneTime,
                    Parameter1 = "https://tophotels.ru/icache/hotel_photos/83/16/30978/430605_119x119.jpg",
                    Parameter2 = "D:"
                },
                new MyTask
                {
                    Event = MyEvent.MoveFolder,
                    DateTimeStart = new DateTime(2019, 6, 19, 10, 50, 00),
                    Periodcity = MyPeriodcity.EveryDay,
                    Parameter1 = "d:\\src",
                    Parameter2 = "d:\\desc"
                },
                new MyTask
                {
                    Event = MyEvent.SendEmail,
                    DateTimeStart = new DateTime(2019, 6, 19, 10, 55, 00),
                    Periodcity = MyPeriodcity.EveryYear,
                    Parameter1 = "s_astana@mail.ru",
                    Parameter2 = "Тестовое сообщение"
                }
            });
            context.SaveChanges();
        }
    }

}
