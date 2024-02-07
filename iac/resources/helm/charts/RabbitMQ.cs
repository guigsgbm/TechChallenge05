using Pulumi;
using Pulumi.Azure.ContainerService;
using Pulumi.Kubernetes.Helm;
using Pulumi.Kubernetes.Helm.V3;
using Pulumi.Kubernetes.Types.Inputs.Core.V1;
using Pulumi.Kubernetes.Types.Inputs.Meta.V1;
using Pulumi.RabbitMQ;
using System.Collections.Generic;

namespace iac_techchallenge05.resources.helm.charts;

public class RabbitMQ
{
    public RabbitMQ(KubernetesCluster aks, Pulumi.Kubernetes.Provider provider)
    {
        var config = new Pulumi.Config();

        var rabbitMQ = new Chart("rabbitmq", new ChartArgs()
        {
            Chart = "rabbitmq",
            Version = "12.9.0",
            FetchOptions = new ChartFetchArgs()
            {
                Repo = "https://charts.bitnami.com/bitnami"
            },
            Values = new Dictionary<string, object>
            {
                ["auth"] = new Dictionary<string, object>
                {
                    ["username"] = config.Require("rabbitUsername"),
                    ["password"] = config.Require("rabbitPassword"),
                }
            }
        }, new ComponentResourceOptions()
        {
            Provider = provider,
            DependsOn = { aks }
        });

        var lbRabbitMqSvc = new Pulumi.Kubernetes.Core.V1.Service("lb-rabbitmq", new ServiceArgs
        {
            Metadata = new ObjectMetaArgs
            {
                Name = "lb-rabbitmq"
            },
            Spec = new ServiceSpecArgs
            {
                Selector = new InputMap<string>
                {
                    { "app.kubernetes.io/instance", "rabbitmq" },
                    { "app.kubernetes.io/name", "rabbitmq" }
                },

                Ports = new ServicePortArgs[]
                {
                    new ServicePortArgs
                    {
                        Name = "rabbitmq",
                        Protocol = "TCP",
                        Port = 5672,
                        TargetPort = 5672
                    },
                    new ServicePortArgs
                    {
                        Name = "rabbitmq-management",
                        Protocol = "TCP",
                        Port = 15672,
                        TargetPort = 15672
                    }
                },

                Type = "LoadBalancer"
            }
        }, new CustomResourceOptions()
        {
            Provider = provider,
            DependsOn = new Resource[] { aks, rabbitMQ }
        });
    }
}
