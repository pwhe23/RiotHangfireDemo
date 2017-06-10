function jsonRpc(command, obj, success, error) {
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

//REF: http://stackoverflow.com/a/8649003/366559
function getQuerystringObject(search) {
    if (typeof search === 'undefined') {
        search = location.search.substring(1);
    }
    return search
        ? JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}',
            function (key, value) { return key === "" ? value : decodeURIComponent(value) })
        : {};
}
