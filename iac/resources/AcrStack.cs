using Pulumi;

namespace iac_techchallenge05.resources;

public class AcrStack
{
    public Pulumi.Azure.ContainerService.Registry AcrObj { get; }

    public AcrStack(Pulumi.Azure.Core.ResourceGroup createdResourceGroup)
    {
        var config = new Config();

        var azRegistry = new Pulumi.Azure.ContainerService.Registry(config.Require("acrName"), new()
        {
            Name = config.Require("acrName"),
            ResourceGroupName = createdResourceGroup.Name,
            Location = createdResourceGroup.Location,
            Sku = config.Require("acrSku"),
        });

        this.AcrObj = azRegistry;
    }
}
