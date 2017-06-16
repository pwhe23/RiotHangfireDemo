using System;
using System.Linq;
using BatMap;

namespace RiotHangfireDemo.Domain
{
    /// <summary>
    /// Query our QueueItems, results are Paged using PagedList. We map the Entity Framework QueueItem model to our
    /// QueueItemInfo DTO using BapMap ProjectTo.
    /// </summary>
    public class QueryQueueItems : Query<QueryQueueItems.QueueItemInfo>
    {
        public string Status { get; set; }

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

        internal class Handler : CommandHandler<QueryQueueItems, PagedList<QueueItemInfo>>
        {
            private readonly IDb _db;

            public Handler(IDb db)
            {
                _db = db;
            }

            public override PagedList<QueueItemInfo> Handle(QueryQueueItems cmd)
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