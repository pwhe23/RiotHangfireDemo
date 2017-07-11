using System;
using System.Collections.Generic;

namespace RiotHangfireDemo.Domain
{
    public abstract class Query<T> : IRequest<PagedList<T>>, IPageable
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    };

    /// <summary>
    /// Placeholder interface to find Commands
    /// </summary>
    public abstract class Command : IRequest<CommandResponse>
    {
    };

    public class CommandResponse
    {
        public bool IsSuccess { get; set; }
        public HashSet<string> Messages { get; set; }

        public static CommandResponse Error(Exception ex)
        {
            return Error(ex.ToString());
        }

        public static CommandResponse Error(params string[] messages)
        {
            return new CommandResponse
            {
                IsSuccess = false,
                Messages = new HashSet<string>(messages),
            };
        }

        public static CommandResponse Success(params string[] messages)
        {
            return new CommandResponse
            {
                IsSuccess = true,
                Messages = new HashSet<string>(messages),
            };
        }
    };

    public abstract class CommandHandler<TReq, TResp> : IRequestHandler<TReq, TResp> where TReq : IRequest<TResp>
    {
        public abstract TResp Handle(TReq cmd);
    };
}
