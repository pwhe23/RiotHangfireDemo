﻿<CommandButton>
    <button if={ !!command && !confirm } type="button" class="{ cls }" onclick={ clicked }>{ text } <yield /></button>
    <button if={ !!command && !!confirm } type="button" class="{ cls }" onclick={ clicked }>{ text } <yield /></button>
    <script>
        var vm = this;

        vm.command = vm.opts.command;
        vm.text = vm.opts.text;
        vm.confirm = vm.opts.confirm;
        vm.data = vm.opts.data;
        vm.cls = vm.opts.cls || (!vm.opts.confirm ? "btn btn-primary" : "btn btn-danger");

        vm.clicked = function () {
            if (!!vm.confirm && !confirm(vm.confirm)) {
                return;
            }

            jsonRpc(vm.command, vm.data);
        };
    </script>
    <style>
        button { margin-right:25px; }
    </style>
</CommandButton>
