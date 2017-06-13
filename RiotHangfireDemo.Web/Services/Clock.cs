using System;
using RiotHangfireDemo.Domain;

namespace RiotHangfireDemo.Web
{
    /// <summary>
    /// Abstract out the DateTime stuff to make it easier for testing.
    /// </summary>
    public class Clock : IClock
    {
        public DateTime Now() => DateTime.Now;
    };
}