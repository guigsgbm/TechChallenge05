using iac_techchallenge05.resources;
using Pulumi;

return await Deployment.RunAsync(async () =>
{
    var resourceGroup = new ResourceGroup();
    var rg = await resourceGroup.CreateResourceGroup();

    var aks = new AKS();
    await aks.CreateAks(rg);
});
