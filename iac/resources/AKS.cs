using System.Collections.Generic;
using System.Threading.Tasks;

namespace iac_techchallenge05.resources;

public class AKS
{
    public async Task<Pulumi.Azure.ContainerService.KubernetesCluster> CreateAks(Pulumi.Azure.Core.ResourceGroup rg)
    {
        var config = new Pulumi.Config();

        var aks = new Pulumi.Azure.ContainerService.KubernetesCluster(config.Get("aksName"), new()
        {
            ResourceGroupName = rg.Name,
            Location = rg.Location,
            Name = config.Get("aksName"),

            DefaultNodePool = new Pulumi.Azure.ContainerService.Inputs.KubernetesClusterDefaultNodePoolArgs()
            {
                Name = config.Get("defaultNodePoolName"),
                VmSize = config.Get("vmSize"),
                NodeCount = int.Parse(config.Get("nodeCount"))
            },

            SkuTier = config.Get("skuTier"),
            KubernetesVersion = config.Get("kubernetesVersion"),
            AutomaticChannelUpgrade = config.Get("AutomaticChannelUpgrade"),
            DnsPrefix = config.Get("dnsPrefix"),

            Identity = new Pulumi.Azure.ContainerService.Inputs.KubernetesClusterIdentityArgs()
            {
                Type = config.Get("identityType"),
            }
        },
        
        new()
        {
            IgnoreChanges = { "NodeCount" }
        });

        return aks;
    }
}
