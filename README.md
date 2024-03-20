# Documentação do Projeto TechChallenge05

## Visão Geral
Este projeto é uma aplicação .NET 8 que consiste em uma API (que funciona como um Producer tambem), e um Worker (Consumer). O guia a seguir fornece instruções detalhadas para configurar e implantar o projeto em um ambiente Azure.
Todos os recursos foram "deployados" por Iac utilizando o Pulumi, vale lembrar que o banco de dados roda dentro do cluster AKS, com um Helm Chart.

## Recursos
Os seguintes recursos foram utilizados:
- Banco de dados PostgreSql
- Azure Kubernetes Service (AKS)
- Azure Container Registry (ACR)
- Azure Service Bus

Antes de começar, certifique-se de ter o seguinte:
- Conta no Azure com as permissões necessárias.
- .NET 8 SDK instalado localmente.
- Git instalado localmente.

## Passos para Configuração e Implantação

### 1. Clone o Repositório

- Abra um terminal e execute o seguinte comando para clonar o repositório do GitHub:
	```
  git clone https://github.com/guigsgbm/TechChallenge05.git
	```

### 2. Deploy dos recursos no Azure.
- Navegue para a pasta Iac e digite o comando:
```
pulumi up
```

### 3. Build e Deploy dos recursos

- Navegue ate o diretorio raiz do projeto clonado. Ex:
	```
	cd .\TechChallenge05\
	```
- Rode os comandos de Migrations
	```
	dotnet ef migrations add "Initial" -p.\src\Infrastructure\Infrastructure.csproj -c AppDbContext -s.\src\Services\ItemApi\ItemApi.csproj -o.\DB\Migrations\ --verbose
  
	dotnet ef database update -p .\src\Infrastructure\Infrastructure.csproj -c AppDbContext -s .\src\Services\ItemApi\ItemApi.csproj --verbose
	```

- Build e push da imagem
  ```
	docker build . -f .\src\Services\ItemApi\Dockerfile -t nomeacr.azurecr.io/itemapi:latest
 
	docker build . -f .\src\Services\CreatedItemsConsumer\Dockerfile -t nomeacr.azurecr.io/createditemsconsumer:latest

	az login
 
	az acr login --name "NomeACR"

	docker push nomeacr.azurecr.io/itemapi:latest

	docker push nomeacr.azurecr.io/createditemsconsumer:latest
	```
	*As imagens foram baseadas para realizar o build em Linux

### 4. Realizar deploy das imagens no AKS

- Nos iremos utilizar os manifestos yaml para realizar o deploy das aplicacoes no AKS.

- Basta navegar ate a pasta onde estao os arquivos yaml e rodar os comandos, ex:

```
az aks get-credentials --resource-group rgName --name aksName --overwrite-existing

kubectl apply -f .\deploy-api.yaml

kubectl apply -f .\svc-lb-api.yaml

kubectl apply -f .\deploy-consumer.yaml
```

### Conclusão

- Ao final, o aplicativo será implantado, assim como todo o resto do fluxo terá sido executado e as aplicacoes estaram disponiveis no AKS.