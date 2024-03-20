using iac_techchallenge05.resources.helm.charts;
using Pulumi;
using Pulumi.Azure.ContainerService;

namespace iac_techchallenge05.resources;

public class AksStack
{
    public AksStack(Pulumi.Azure.Core.ResourceGroup createdResourceGroup, Registry createdAcr)
    {
        var config = new Config();

        var aks = new KubernetesCluster(config.Require("aksName"), new KubernetesClusterArgs()
        {
            ResourceGroupName = createdResourceGroup.Name,
            Location = config.Require("location"),
            Name = config.Require("aksName"),
            
            DefaultNodePool = new Pulumi.Azure.ContainerService.Inputs.KubernetesClusterDefaultNodePoolArgs()
            {
                Name = config.Require("defaultNodePoolName"),
                VmSize = config.Require("vmSize"),
                NodeCount = config.RequireInt32("nodeCount"),
                EnableAutoScaling = true,
                MinCount = 1,
                MaxCount = 2
            },

            SkuTier = config.Require("skuTier"),
            KubernetesVersion = config.Require("kubernetesVersion"),
            AutomaticChannelUpgrade = config.Require("AutomaticChannelUpgrade"),
            DnsPrefix = config.Require("dnsPrefix"),

            Identity = new Pulumi.Azure.ContainerService.Inputs.KubernetesClusterIdentityArgs()
            {
                Type = config.Require("identityType"),
            }
        },

        new CustomResourceOptions()
        {
            DependsOn = new Resource[] { createdResourceGroup, createdAcr },
            IgnoreChanges = { "defaultNodePool.nodeCount" } 
        });

        var k8sProvider = new Pulumi.Kubernetes.Provider("k8s-provider", new Pulumi.Kubernetes.ProviderArgs
        {
            KubeConfig = aks.KubeConfigRaw.Apply(c => c)
        }, new CustomResourceOptions
        {
            DependsOn = { aks }
        });

        var acrAssignment = new Pulumi.Azure.Authorization.Assignment("acrAssignment", new()
        {
            PrincipalId = aks.KubeletIdentity.Apply(kubeletIdentity => kubeletIdentity.ObjectId),
            RoleDefinitionName = "AcrPull",
            Scope = createdAcr.Id,
            SkipServicePrincipalAadCheck = true,
        },new CustomResourceOptions()
        {
            DependsOn = new Resource[] { aks, createdAcr }
        });

        //var rabbitMQ = new RabbitMQ(aks, k8sProvider);
        var postgresDB = new PostgresDB(aks, k8sProvider);
        var wordpress = new Wordpress(aks, k8sProvider);
    }
}
