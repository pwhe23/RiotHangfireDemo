namespace RiotHangfireDemo
{
    /// <summary>
    /// Strongly-typed configuration class which can be injected into services.
    /// </summary>
    public class DemoConfig
    {
        public int HangfireWorkerCount { get; set; }
        public int RandomizerErrorMax { get; set; }
        public int RandomizerTimeoutMax { get; set; }
    };
}