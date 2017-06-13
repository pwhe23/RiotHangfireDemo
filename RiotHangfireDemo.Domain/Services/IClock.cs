using System;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Abstract out the DateTime stuff to make it easier for testing.
    /// </summary>
    public interface IClock
    {
        DateTime Now();
    };
}
