<ListQueueItems>
    <div if={ result.Items.length == 0 && !result.PageNumber } class="lead">
        <i class="fa fa-spinner fa-pulse fa-fw"></i> Loading
    </div>
    <div>
        <ActionButton action="EnqueueEmail" text="Enqueue Email" />
        <ActionButton action="EnqueueReport" text="Enqueue Report" />
    </div>
    <table if={ result.Items.length > 0 } class="table table-hover">
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
                    <span class="label label-warning" if="{ item.Status == 'Queued' }">{ item.Status }</span>
                    <span class="label label-info" if="{ item.Status == 'Running' }">{ item.Status } <i class="fa fa-spinner fa-pulse fa-fw"></i></span>
                    <span class="label label-danger" if="{ item.Status == 'Error' }">{ item.Status }</span>
                    <span class="label label-success" if="{ item.Status == 'Completed' }">{ item.Status }</span>
                </td>
                <td>
                    <p class="{ small:true, bg-danger:item.Status == 'Error', bg-success:item.Status == 'Completed' }" if={ !!item.Log }>{ item.Log }</p>
                </td>
            </tr>
        </tbody>
    </table>
    <Pager page-number={ result.PageNumber } page-size={ result.PageSize } total-items={ result.TotalItems } />
    <script>
        var vm = this;
        vm.queryQueueItems = { PageNumber:1, PageSize:20 };
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
        p { padding:.5em; }
    </style>
</ListQueueItems>
