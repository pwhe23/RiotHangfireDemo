<ActionButton>
    <button if={ !!action } type="button" class="btn btn-primary" onclick={ clicked }>{ text }</button>
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