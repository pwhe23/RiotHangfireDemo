namespace RiotHangfireDemo.Domain
{
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

    public interface IRequest<TResponse>
    {
    };

    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest cmd);
    };
}
