namespace RiotHangfireDemo.Domain
{
    public interface ICommand
    {
        // Placeholder interface to find commands
    };

    public interface ICommander
    {
        object Execute(string commandName, string commandJson);
        object Execute(object cmd);
    };
}
