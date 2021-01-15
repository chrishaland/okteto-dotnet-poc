function(name, namespace, db_server, db_name) {
    apiVersion: 'v1',
    kind: 'ConfigMap',
    metadata: {
        name: name,
        namespace: namespace
    },
    data: {
        'appsettings.json': std.manifestJson({
            Database: {
                Server: db_server,
                Name: db_name
            }
        })
    }
}