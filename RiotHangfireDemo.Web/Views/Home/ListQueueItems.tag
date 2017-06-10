<ListQueueItems>
    <EnqueueActions></EnqueueActions>
    <table class="table table-hover">
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
                <td>{ item.Created }</td>
                <td>{ item.Started }</td>
                <td>{ item.Completed }</td>
                <td>
                    <span class="label label-warning" if={ item.Status == "Queued" }>{ item.Status }</span>
                    <span class="label label-info" if={ item.Status == "Running" }>{ item.Status }</span>
                    <span class="label label-danger" if={ item.Status == "Error" }>{ item.Status }</span>
                    <span class="label label-success" if={ item.Status == "Completed" }>{ item.Status }</span>
                </td>
                <td>
                    <pre if={ !!item.Log }>{ item.Log }</pre>
                </td>
            </tr>
        </tbody>
    </table>
    <Pager page-number={ result.PageNumber } page-size={ result.PageSize } total-items={ result.TotalItems }></Pager>
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
                switch (type) {
                    case "Refresh": load(); break;
                }
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
</ListQueueItems>
