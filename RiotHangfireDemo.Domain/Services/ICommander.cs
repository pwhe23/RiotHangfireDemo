namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Placeholder interface to find Commands
    /// </summary>
    public interface ICommand
    {
    };

    public interface ICommander
    {
        object Execute(string commandName, string commandJson);
        object Execute(object command);
    };
}
