<ListQueueItems>
    <div if="{ result.Items.length == 0 && !result.PageNumber }" class="lead">
        <i class="fa fa-spinner fa-pulse fa-fw"></i> Loading
    </div>
    <div>
        <ActionButton action="EnqueueEmail" text="Enqueue Email" />
        <ActionButton action="EnqueueReport" text="Enqueue Report" />
    </div>
    <table if={ result.Items.length > 0 } class="table table-hover small">
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>
                <th>Created</th>
                <th>Started</th>
                <th>Completed</th>
                <th>Status</th>
                <th>Log</th>
            </tr>
        </thead>
        <tbody>
            <tr each={ item, index in result.Items }>
                <td>{ item.Id }</td>
                <td>{ item.Name }</td>
                <td>{ moment(item.Created).fromNow(); }</td>
                <td>
                    <span if={ !!item.Started }>{ moment(item.Started).fromNow(); }</span>
                </td>
                <td>
                    <span if={ !!item.Completed }>{ moment(item.Completed).diff(moment(item.Started), 'seconds') } sec(s)</span>
                </td>
                <td>
                    <span if="{ item.Status == 'Queued' }" class="label label-warning">{ item.Status }</span>
                    <span if="{ item.Status == 'Running' }" class="label label-info">{ item.Status } <i class="fa fa-spinner fa-pulse fa-fw"></i></span>
                    <span if="{ item.Status == 'Error' }" class="label label-danger">{ item.Status }</span>
                    <span if="{ item.Status == 'Completed' }" class="label label-success">{ item.Status }</span>
                </td>
                <td>
                    <p if={ !!item.Log } class="{ bg-danger:item.Status == 'Error', bg-success:item.Status == 'Completed' }">{ item.Log }</p>
                </td>
            </tr>
        </tbody>
    </table>
    <Pager page-number={ result.PageNumber } page-size={ result.PageSize } total-items={ result.TotalItems } />
    <script>
        var vm = this;
        vm.queryQueueItems = { PageNumber:1, PageSize:10 };
        vm.result = { Items:[] };

        function load() {
            jsonRpc("QueryQueueItems", vm.queryQueueItems, function (result) {
                vm.result = result;
                vm.update();
            });
        }

        vm.on("mount", function () {
            var pushHub = $.connection.pushHub;
            pushHub.client.push = function (type, data) {
                vm.trigger(type, data);
            };
            $.connection.hub.start();
        });

        vm.on("QueueItems.Changed", function () {
            load();
        });

        vm.on("Pager.Clicked", function (pageNumber) {
            vm.queryQueueItems.PageNumber = pageNumber;
            load();
        });

        load();
    </script>
    <style>
        table p { padding:.5em; margin:0; }
    </style>
</ListQueueItems>
