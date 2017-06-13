using System;
using System.Linq;
using BatMap;
using MediatR;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Query our QueueItems, results are Paged.
    /// </summary>
    public class QueryQueueItems : IRequest<PagedList<QueryQueueItems.QueueItemInfo>>, ICommand, IPageable
    {
        public string Status { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public class QueueItemInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Started { get; set; }
            public DateTime? Completed { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
            public string Log { get; set; }
        };

        internal class Handler : IRequestHandler<QueryQueueItems, PagedList<QueueItemInfo>>
        {
            private readonly IDb _db;

            public Handler(IDb db)
            {
                _db = db;
            }

            public PagedList<QueueItemInfo> Handle(QueryQueueItems cmd)
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
                    .ProjectTo<QueueItemInfo>(checkIncludes: true)
                    .OrderByDescending(x => x.Id)
                    .ToPagedList(cmd);
            }
        };
    };
}