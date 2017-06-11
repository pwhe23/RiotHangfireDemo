<ActionButton>
    <button type="button" class="btn btn-primary" onclick={ clicked } if={ !!action }>{ text }</button>
    <script>
        var vm = this;

        vm.on("update", function () {
            vm.action = vm.opts.action;
            vm.text = vm.opts.text;
        });

        vm.clicked = function () {
            jsonRpc(vm.action);
        };
    </script>
</ActionButton>