using Pulumi;
using Pulumi.Azure.ContainerService;
using Pulumi.Kubernetes.Helm.V3;
using Pulumi.Kubernetes.Helm;
using System.Collections.Generic;
using Pulumi.Kubernetes.Types.Inputs.Core.V1;
using Pulumi.Kubernetes.Types.Inputs.Meta.V1;

namespace iac_techchallenge05.resources.helm.charts;

public class PostgresDB
{
    public PostgresDB(KubernetesCluster aks, Pulumi.Kubernetes.Provider provider)
    {
        var config = new Config();

        var postgresDB = new Chart("postgresql", new ChartArgs()
        {
            Chart = "postgresql",
            Version = "14.1.3",
            FetchOptions = new ChartFetchArgs()
            {
                Repo = "https://charts.bitnami.com/bitnami"
            },
            Values = new Dictionary<string, object>
            {
                ["auth"] = new Dictionary<string, object>
                {
                    ["username"] = config.Require("postgreUsername"),
                    ["password"] = config.Require("postgrePassword"),
                    ["database"] = config.Require("postgreDatabase"),
                    ["postgresPassword"] = config.Require("postgrePassword"),
                }
            }
        }, new ComponentResourceOptions()
        {
            Provider = provider,
            DependsOn = { aks }
        });

        var lb_postgresDB = new Pulumi.Kubernetes.Core.V1.Service("lb-postgresql", new ServiceArgs
        {
            Metadata = new ObjectMetaArgs
            {
                Name = "lb-postgresql"
            },
            Spec = new ServiceSpecArgs
            {
                Selector = new InputMap<string>
                {
                    { "app.kubernetes.io/instance", "postgresql" },
                    { "app.kubernetes.io/name", "postgresql" }
                },

                Ports = new ServicePortArgs[]
                {
                    new ServicePortArgs
                    {
                        Name = "postgresql",
                        Protocol = "TCP",
                        Port = 5432,
                        TargetPort = 5432
                    },
                },

                Type = "LoadBalancer"
            }
        }, new CustomResourceOptions()
        {
            Provider = provider,
            DependsOn = new Resource[] { aks, postgresDB }
        });
    }
}
