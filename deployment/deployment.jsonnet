function(name, namespace, version, port) {
    apiVersion: 'apps/v1',
    kind: 'Deployment',
    metadata: {
        name: name,
        namespace: namespace
    },
    spec: {
        replicas: 1,
        selector: {
            matchLabels: {
                app: name,
                version: version
            }
        },
        template: {
            metadata: {
                labels: {
                    app: name,
                    version: version
                }
            },
            spec: {
                containers: [
                    {
                        name: name,
                        image: 'chrishaland/okteto-dotnet-poc:%s' % version,
                        resources: {
                            requests: { memory: '30Mi', cpu: '10m' },
                            limits: { memory: '60Mi', cpu: '10m' }
                        },
                        ports: [
                            { containerPort: port }
                        ],
                        volumeMounts: [
                            { name: 'secret', mountPath: '/app/secret' },
                            { name: 'configmap', mountPath: '/app/configmap' }
                        ]
                    }
                ],
                volumes: [
                    { name: 'secret', secret: { secretName: name } },
                    { name: 'configmap', configMap: { name: name } }
                ]
            }
        }
    }
}
