using iac_techchallenge05.resources;
using Pulumi;

return await Deployment.RunAsync(() =>
{
    var resourceGroup = new ResourceGroupStack();

    var azureKubernetesService = new AksStack(resourceGroup.ResourceGroupObj);
});
