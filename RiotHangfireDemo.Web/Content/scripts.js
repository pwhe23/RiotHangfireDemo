function jsonRpc(command, obj, success, error) {
    //console.log(command, obj);
    $.ajax({
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        url: "/jsonrpc/" + command,
        data: JSON.stringify(obj || {}),
        success: function (data) {
            if ("IsSuccess" in data && !data.IsSuccess) {
                if (data.Messages.length) alert("ERROR: " + data.Messages.join("\n"));
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

/// Riot.js serialize all inputs in the refs collection to an object
function SerializeRefs(refs, data) {
    data = data || {};
    for (var ref in refs) {
        if (refs.hasOwnProperty(ref)) {
            var input = refs[ref];
            if ("value" in input && String(input.value).length > 0) {
                data[ref] = input.value;
            }
        }
    }
    return data;
}
