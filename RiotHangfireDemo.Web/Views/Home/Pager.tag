<Pager>
    <div if={ !pageNumber }>
        <div class="alert alert-warning" role="alert">No items found</div>
    </div>
    <nav aria-label="Page navigation" if={ !!pageNumber }>
        <ul class="pagination">
            <li if={ hasPrevious }>
                <span class="button" aria-label="Previous" onClick={ previousClicked }>
                    <span aria-hidden="true">&larr; Previous</span>
                </span>
            </li>
            <li if={ !hasPrevious } class="disabled">
                <span aria-label="Previous">
                    <span aria-hidden="true">&larr; Previous</span>
                </span>
            </li>
            <li class="active">
                <span>Page { pageNumber } / { totalPages } <span class="sr-only">(current)</span></span>
            </li>
            <li class="disabled">
                <span>
                    <span aria-hidden="true">{ totalItems } Total Item(s)</span>
                </span>
            </li>
            <li if={ hasNext }>
                <span class="button" aria-label="Next" onClick={ nextClicked }>
                    <span aria-hidden="true">Next &rarr;</span>
                </span>
            </li>
            <li if={ !hasNext } class="disabled">
                <span aria-label="Next">
                    <span aria-hidden="true">Next &rarr;</span>
                </span>
            </li>
        </ul>
    </nav>
    <script>
        var vm = this;

        vm.on("update", function () {
            vm.pageNumber = Number(vm.opts.pageNumber || "1");
            vm.pageSize = Number(vm.opts.pageSize || "10");
            vm.totalItems = Number(vm.opts.totalItems || "0");

            if (vm.totalItems == 0) vm.pageNumber = 0;
            if (vm.pageSize < 1) vm.pageSize = 1;
            vm.totalPages = Math.ceil(vm.totalItems / vm.pageSize);

            vm.hasPrevious = vm.pageNumber > 1;
            vm.hasNext = vm.pageNumber < vm.totalPages;
        });

        vm.previousClicked = function () {
            vm.parent.trigger("Pager.Clicked", vm.pageNumber - 1);
        };

        vm.nextClicked = function () {
            vm.parent.trigger("Pager.Clicked", vm.pageNumber + 1);
        };
    </script>
    <style>
        div.alert { margin:1em 0; }
        span.button { cursor:pointer; }
    </style>
</Pager>