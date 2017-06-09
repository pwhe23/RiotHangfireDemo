using System;

namespace RiotHangfireDemo
{
    public interface ITime
    {
        DateTime Now();
    };

    public class Time : ITime
    {
        public DateTime Now() => DateTime.Now;
    };
}