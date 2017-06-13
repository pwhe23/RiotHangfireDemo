using System;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Abstract System.Random for use by our simulation commands.
    /// </summary>
    public interface IRandomizer
    {
        TimeSpan GetRandomTimeout();
        bool IsRandomError();
    };
}
