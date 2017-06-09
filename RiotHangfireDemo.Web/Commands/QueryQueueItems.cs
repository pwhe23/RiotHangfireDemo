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
            private readonly DemoDb _db;

            public Handler(DemoDb db)
            {
                _db = db;
            }

            public PagedList<QueueItem> Handle(QueryQueueItems cmd)
            {
                var queueItems = _db
                    .QueueItems
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