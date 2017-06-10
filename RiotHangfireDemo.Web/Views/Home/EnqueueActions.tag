<EnqueueActions>
    <div>
        <button type="button" class="btn btn-primary" onclick={ EnqueueEmail }>Enqueue Email</button>
        <button type="button" class="btn btn-primary" onclick={ EnqueueReport }>Enqueue Report</button>
    </div>
    <script>
        var vm = this;

        vm.EnqueueEmail = function () {
            jsonRpc("EnqueueEmail", null, function () {
                vm.parent.trigger("QueueItems.Changed");
            });
        };

        vm.EnqueueReport = function () {
            jsonRpc("EnqueueReport", null, function () {
                vm.parent.trigger("QueueItems.Changed");
            });
        };
    </script>
</EnqueueActions>
