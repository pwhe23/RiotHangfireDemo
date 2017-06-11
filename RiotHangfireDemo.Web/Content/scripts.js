﻿function jsonRpc(command, obj, success, error) {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        url: "/jsonrpc/" + command,
        data: JSON.stringify(obj || {}),
        success: function (data) {
            if (data.Errors && data.Errors.length) {
                alert("ERROR: " + data.Errors.join("\n"));
                if (error) error(data);
            } else {
                if (success) success(data);
            }
        }
    });
}

//REF: https://stackoverflow.com/a/11616993/366559
function stringify(o) {
    var cache = [];
    return JSON.stringify(o, function (key, value) {
        if (typeof value === "object" && value !== null) {
            if (cache.indexOf(value) !== -1) {
                return; // Circular reference found, discard key
            }
            cache.push(value); // Store value in our collection
        }
        return value;
    });
}
