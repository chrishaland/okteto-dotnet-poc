local namespace = import 'namespace.jsonnet';
local configmap = import 'configmap.jsonnet';
local secret = import 'secret.jsonnet';
local service = import 'service.jsonnet';
local deployment = import 'deployment.jsonnet';

local PORT = 5000;

function(
    NAME='okteto-dotnet-poc',
    NAMESPACE='default',
    VERSION='0.1',
    ISTIO_ENABLED=false,
    DB_SERVER='mssql.mssql',
    DB_NAME='okteto-dotnet-poc',
    DB_USERNAME='okteto',
    DB_PASSWORD='okteto'
) [
    namespace(NAMESPACE, ISTIO_ENABLED),
    configmap(NAME, NAMESPACE, DB_SERVER, DB_NAME),
    secret(NAME, NAMESPACE, DB_USERNAME, DB_PASSWORD),
    service(NAME, NAMESPACE, PORT),
    deployment(NAME, NAMESPACE, VERSION, PORT)
]
