using System;

namespace RiotHangfireDemo
{
    public interface IClock
    {
        DateTime Now();
    };

    public class Clock : IClock
    {
        public DateTime Now() => DateTime.Now;
    };
}