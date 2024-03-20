using Pulumi;
using Pulumi.Azure.ContainerService;
using Pulumi.Kubernetes.Helm;
using Pulumi.Kubernetes.Helm.V3;
using System.Collections.Generic;

namespace iac_techchallenge05.resources.helm.charts;

public class Wordpress
{
    public Wordpress(KubernetesCluster aks, Pulumi.Kubernetes.Provider provider)
    {
        var config = new Config();

        var wordpress = new Chart("wordpress", new ChartArgs()
        {
            Chart = "wordpress",
            Version = "19.3.0",
            FetchOptions = new ChartFetchArgs()
            {
                Repo = "https://charts.bitnami.com/bitnami"
            },
            Values = new Dictionary<string, object>
            {
                ["wordpressUsername"] = config.Require("wordpressUsername"),
                ["wordpressPassword"] = config.Require("wordpressPassword")
            }
        }, new ComponentResourceOptions()
        {
            Provider = provider,
            DependsOn = new InputList<Resource>(){ aks }
        });
        /*
        var lb_Wordpress = new Pulumi.Kubernetes.Core.V1.Service("lb-wordpress", new ServiceArgs
        {
            Metadata = new ObjectMetaArgs
            {
                Name = "lb-wordpress"
            },
            Spec = new ServiceSpecArgs
            {
                Selector = new InputMap<string>
                {
                    { "app.kubernetes.io/instance", "wordpress" },
                    { "app.kubernetes.io/name", "wordpress" }
                },

                Ports = new ServicePortArgs[]
                {
                    new ServicePortArgs
                    {
                        Name = "wordpress-http",
                        Port = 8080,
                        TargetPort = 8080
                    },
                    new ServicePortArgs
                    {
                        Name = "wordpress-https",
                        Port = 8443,
                        TargetPort = 8443
                    }
                },

                Type = "LoadBalancer"
            }
        }, new CustomResourceOptions()
        {
            Provider = provider,
            DependsOn = new Resource[] { aks, wordpress }
        });*/
    }
}
