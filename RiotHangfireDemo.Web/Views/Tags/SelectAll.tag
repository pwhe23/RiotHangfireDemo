<SelectAll>
    <input type="checkbox" onchange={ changed } title="Select All" />
    <script>
        var vm = this;
        vm.selector = vm.opts.selector;

        vm.changed = function (e) {
            var checked = e.currentTarget.checked;
            $(vm.selector).each(function (i, checkbox) {
                if (checkbox.checked !== checked) {
                    checkbox.click();
                }
            });
        };
    </script>
</SelectAll>
