using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Domain;
using Infrastructure.DB.Repository;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging;

public class ItemMessagingConfig
{
    public string? ConnectionString { get; set; }
    public string? QueueName { get; set; }
}

public class ItemMessaging
{
    private readonly ItemRepository _itemRepository;
    private readonly IQueueClient _queueClient;
    private readonly ItemMessagingConfig _itemMessagingConfig;
    private readonly ILogger<ItemMessaging> _logger;

    public ItemMessaging(IOptions<ItemMessagingConfig> itemMessagingConfig, ILogger<ItemMessaging> logger, ItemRepository itemRepository)
    {
        _itemMessagingConfig = itemMessagingConfig.Value;
        _queueClient = new QueueClient(_itemMessagingConfig.ConnectionString, _itemMessagingConfig.QueueName);
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public async Task SendMessageAsync(SimplifiedItem item)
    {
        string messageJson = JsonConvert.SerializeObject(item);
        var message = new Message(Encoding.UTF8.GetBytes(messageJson));

        await _queueClient.SendAsync(message);
    }

    public void StartMessageProcessing()
    {
        var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
            MaxConcurrentCalls = 1,
            AutoComplete = false
        };

        _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
    }

    public void StopMessageProcessing()
    {
        _queueClient.CloseAsync().Wait();
    }

    private async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
        _logger.LogInformation($"Received message: {Encoding.UTF8.GetString(message.Body)}");

        var bodyJson = Encoding.UTF8.GetString(message.Body);
        var item = JsonConvert.DeserializeObject<Item>(bodyJson);

        await _itemRepository.Add(item);
        await _itemRepository.Save();

        await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
        _logger.LogError($"Message handler encountered an exception: {exceptionReceivedEventArgs.Exception}");

        return Task.CompletedTask;
    }

}

