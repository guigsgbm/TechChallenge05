using Pulumi;
using Azure = Pulumi.Azure;

namespace iac_techchallenge05.resources;

public class ServiceBusStack
{
    public ServiceBusStack(Pulumi.Azure.Core.ResourceGroup createdResourceGroup)
    {
        var config = new Config();

        var sbNamespace = new Azure.ServiceBus.Namespace(config.Require("sbNamespaceName"), new()
        {
            Name = config.Require("sbNamespaceName"),
            Location = createdResourceGroup.Location,
            ResourceGroupName = createdResourceGroup.Name,
            Sku = config.Require("sbSku")
        });

        var sbQueue = new Azure.ServiceBus.Queue(config.Require("sbQueueName"), new()
        {
            Name = config.Require("sbQueueName"),
            NamespaceId = sbNamespace.Id,
            EnablePartitioning = true
        });
    }
}
