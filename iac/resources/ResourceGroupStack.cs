using Pulumi;

namespace iac_techchallenge05.resources;

public class ResourceGroupStack
{
    public Pulumi.Azure.Core.ResourceGroup ResourceGroupObj { get; }

    public ResourceGroupStack()
    {
        var config = new Config();

        var rg = new Pulumi.Azure.Core.ResourceGroup(config.Require("resourceGroupName"), new()
        {
            Name = config.Require("resourceGroupName"),
            Location = config.Require("location")
        });

        this.ResourceGroupObj = rg;
    }
}
