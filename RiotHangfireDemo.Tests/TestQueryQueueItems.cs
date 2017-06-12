using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using RiotHangfireDemo.Domain;
using Shouldly;
using Xunit;

namespace RiotHangfireDemo.Tests
{
    public class TestQueryQueueItems
    {
        [Fact]
        public void Test_QueryQueueItems_Sort()
        {
            var db = A.Fake<IDb>();
            A.CallTo(() => db.Query<QueueItem>()).Returns(_queryItems);

            var cmd = new QueryQueueItems();
            var result = new QueryQueueItems.Handler(db).Handle(cmd);

            result.Items[0].Id.ShouldBe(4);
            result.Items[1].Id.ShouldBe(3);
        }

        [Fact]
        public void Test_QueryQueueItems_NullStatus()
        {
            var db = A.Fake<IDb>();
            A.CallTo(() => db.Query<QueueItem>()).Returns(_queryItems);

            var cmd = new QueryQueueItems();
            var result = new QueryQueueItems.Handler(db).Handle(cmd);

            result.Items.Length.ShouldBe(4);
        }

        [Fact]
        public void Test_QueryQueueItems_SingleStatus()
        {
            var db = A.Fake<IDb>();
            A.CallTo(() => db.Query<QueueItem>()).Returns(_queryItems);

            var cmd = new QueryQueueItems
            {
                Status = QueueItem.COMPLETED,
            };
            var result = new QueryQueueItems.Handler(db).Handle(cmd);

            result.Items.Length.ShouldBe(1);
            result.Items[0].Id.ShouldBe(4);
        }

        private static readonly IQueryable<QueueItem> _queryItems = new List<QueueItem>
        {
            new QueueItem { Id = 1, Status = QueueItem.QUEUED},
            new QueueItem { Id = 2, Status = QueueItem.RUNNING},
            new QueueItem { Id = 3, Status = QueueItem.ERROR},
            new QueueItem { Id = 4, Status = QueueItem.COMPLETED},
        }.AsQueryable();
    };
}
