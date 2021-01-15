function(name, namespace, port) {
    apiVersion: 'v1',
    kind: 'Service',
    metadata: {
        name: name,
        namespace: namespace,
        labels: {
            app: name
        },
    },
    spec: {
        type: 'ClusterIP',
        selector: {
            app: name
        },
        ports: [
            { name: 'http', port: port }
        ],
    },
}
