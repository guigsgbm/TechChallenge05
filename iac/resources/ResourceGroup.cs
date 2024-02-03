using Pulumi;
using System.Threading.Tasks;

namespace iac_techchallenge05.resources;

public class ResourceGroup
{
    public async Task<Pulumi.Azure.Core.ResourceGroup> CreateResourceGroup()
    {
        var config = new Config();

        var rg = new Pulumi.Azure.Core.ResourceGroup(config.Get("resourceGroupName"), new()
        {
            Name = config.Get("resourceGroupName"),
            Location = config.Get("location")
        });

        return rg;
    }
}
