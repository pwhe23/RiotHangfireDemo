using MediatR;
namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Marker interface to be able to find all commands and queries
    /// </summary>
    public interface ICommand
    {
    };
    /// <summary>
    /// Placeholder interface to find Commands
    /// </summary>
    public abstract class Command : IRequest<CommandResponse>, ICommand
    {
    };

    public class CommandResponse
    {
        public static CommandResponse Success()
        {
            return new CommandResponse();
        }
    };

    public class CommandResponse<T>
    {
    };

    public abstract class Query<T> : IRequest<PagedList<T>>, ICommand, IPageable
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    };

    public abstract class CommandHandler<TReq, TResp> : IRequestHandler<TReq, TResp> where TReq : IRequest<TResp>
    {
        public abstract TResp Handle(TReq message);
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
