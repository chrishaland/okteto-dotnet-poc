function(namespace, istio_enabled) {
  apiVersion: 'v1',
  kind: 'Namespace',
  metadata: {
    name: namespace,
    labels: {
      'istio-injection': if istio_enabled == true then 'enabled' else 'disabled',
    },
  },
}
