using Pulumi;
using Pulumi.Azure.ContainerService;
using Pulumi.Kubernetes.Helm.V3;
using Pulumi.Kubernetes.Helm;
using System.Collections.Generic;

namespace iac_techchallenge05.resources.helm.charts;

public class PostgresDB
{
    public PostgresDB(KubernetesCluster aks, Pulumi.Kubernetes.Provider provider)
    {
        var config = new Config();

        var postgresDB = new Chart("postgresql", new ChartArgs()
        {
            Chart = "postgresql",
            Version = "14.0.1",
            FetchOptions = new ChartFetchArgs()
            {
                Repo = "https://charts.bitnami.com/bitnami"
            },
            Values = new Dictionary<string, object>
            {
                ["auth"] = new Dictionary<string, string>
                {
                    ["username"] = config.Require("postgreUsername"),
                    ["password"] = config.Require("postgrePassword"),
                    ["database"] = config.Require("postgreDatabase"),
                    ["postgresPassword"] = config.Require("postgresAdminPassword"),
                }
            }
        }, new ComponentResourceOptions()
        {
            Provider = provider,
            DependsOn = { aks }
        });
    }
}
