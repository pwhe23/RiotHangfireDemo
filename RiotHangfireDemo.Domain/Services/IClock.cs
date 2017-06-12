using System;

namespace RiotHangfireDemo.Domain
{
    public interface IClock
    {
        DateTime Now();
    };
}
