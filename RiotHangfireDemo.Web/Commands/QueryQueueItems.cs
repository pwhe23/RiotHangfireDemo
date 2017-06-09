using System.Linq;
using MediatR;

namespace RiotHangfireDemo
{
    public class QueryQueueItems : IRequest<PagedList<QueueItem>>, IPageable
    {
        public string Status { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        internal sealed class Handler : IRequestHandler<QueryQueueItems, PagedList<QueueItem>>
        {
            private readonly IDb _db;

            public Handler(IDb db)
            {
                _db = db;
            }

            public PagedList<QueueItem> Handle(QueryQueueItems cmd)
            {
                var queueItems = _db
                    .Query<QueueItem>()
                    .AsQueryable();

                if (cmd.Status != null)
                {
                    queueItems = queueItems
                        .Where(x => x.Status == cmd.Status);
                }

                return queueItems
                    .OrderByDescending(x => x.Id)
                    .ToPagedList(cmd);
            }
        };
    };
}