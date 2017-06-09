using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace RiotHangfireDemo
{
    public class QueryQueueItems : IRequest<List<QueueItem>>
    {
        public string Status { get; set; }

        internal sealed class Handler : IRequestHandler<QueryQueueItems, List<QueueItem>>
        {
            private readonly DemoDb _db;

            public Handler(DemoDb db)
            {
                _db = db;
            }

            public List<QueueItem> Handle(QueryQueueItems cmd)
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
                    .ToList();
            }
        };
    };
}