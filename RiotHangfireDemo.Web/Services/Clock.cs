using System;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    public class Clock : IClock
    {
        public DateTime Now() => DateTime.Now;
    };
}