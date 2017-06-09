using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace RiotHangfireDemo
{
    public class QueryTasks : IRequest<List<Task>>
    {
        public bool? Completed { get; set; }

        internal class Handler : IRequestHandler<QueryTasks, List<Task>>
        {
            private readonly DemoDb _db;

            public Handler(DemoDb db)
            {
                _db = db;
            }

            public List<Task> Handle(QueryTasks cmd)
            {
                var tasks = _db.Tasks.AsQueryable();

                if (cmd.Completed.HasValue)
                {
                    tasks = tasks.Where(x => x.Completed == cmd.Completed.Value);
                }

                return tasks.ToList();
            }
        };
    };
}