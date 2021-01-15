function(name, namespace, db_username, db_password) {
    apiVersion: 'v1',
    kind: 'Secret',
    metadata: {
        name: name,
        namespace: namespace
    },
    type: 'Opaque',
    stringData: {
        'appsettings.json': std.manifestJson({
            Database: {
                Username: db_username,
                Password: db_password
            }
        })
    }
}