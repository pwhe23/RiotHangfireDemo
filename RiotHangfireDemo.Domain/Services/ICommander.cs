using MediatR;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Placeholder interface to find Commands
    /// </summary>
    public interface ICommand
    {
    };

    /// <summary>
    /// Allow execution of commands using name and json, untyped objects,
    /// and regular MediatR requests.
    /// </summary>
    public interface ICommander
    {
        object Execute(string commandName, string commandJson);
        object Execute(object command);
        TResponse Send<TResponse>(IRequest<TResponse> command);
    };
}
