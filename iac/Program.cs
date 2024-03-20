using iac_techchallenge05.resources;
using iac_techchallenge05.resources.helm;
using Pulumi;

return await Deployment.RunAsync(() =>
{
    var resourceGroup = new ResourceGroupStack();

    var acr = new AcrStack(resourceGroup.ResourceGroupObj);

    var azureKubernetesService = new AksStack(resourceGroup.ResourceGroupObj, acr.AcrObj);

    var sb = new ServiceBusStack(resourceGroup.ResourceGroupObj);
});
