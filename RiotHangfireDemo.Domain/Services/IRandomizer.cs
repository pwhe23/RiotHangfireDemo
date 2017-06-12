using System;

namespace RiotHangfireDemo.Domain
{
    public interface IRandomizer
    {
        TimeSpan GetRandomTimeout();
        bool IsRandomError();
    };
}
