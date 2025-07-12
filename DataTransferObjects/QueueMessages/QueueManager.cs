using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Pro4Soft.DataTransferObjects.QueueMessages
{
    public class QueueManager
    {
        private ServiceBusClient _client;
        private readonly ConcurrentDictionary<string, ServiceBusProcessor> _receivers = new ConcurrentDictionary<string, ServiceBusProcessor>();
        private readonly ConcurrentDictionary<string, ServiceBusSender> _senders = new ConcurrentDictionary<string, ServiceBusSender>();
        
        public void Initialize(string connectionString)
        {
            if (_client != null && !_client.IsClosed)
                _client.DisposeAsync().AsTask().Wait();

            _client = new ServiceBusClient(connectionString);
        }

        //Sending
        public async Task SendToQueue<T>(T data) where T: BaseQueueItem, new()
        {
            if(_client == null)
                throw new BusinessWebException($"QueueManager has not been Initialized. Make sure to call Initialize before sending or receiving");

            var attr = typeof(T).GetCustomAttribute(typeof(QueueSourceAttribute), false) as QueueSourceAttribute;
            if (attr == null)
                throw new BusinessWebException($"Attribute {nameof(QueueSourceAttribute)} is not setup on {typeof(T).FullName}");

            if (!_senders.ContainsKey(attr.QueueName))
                _senders[attr.QueueName] = _client.CreateSender(attr.QueueName);
            await _senders[attr.QueueName].SendMessageAsync(new ServiceBusMessage(BinaryData.FromObjectAsJson(data)));
        }

        //Receiving
        public async Task ReadFromQueue<T>(Func<string, T, Task> handler) where T : BaseQueueItem, new()
        {
            if (_client == null)
                throw new BusinessWebException($"QueueManager has not been Initialized. Make sure to call Initialize before sending or receiving");

            var attr = typeof(T).GetCustomAttribute(typeof(QueueSourceAttribute), false) as QueueSourceAttribute;
            if (attr == null)
                throw new BusinessWebException($"Attribute {nameof(QueueSourceAttribute)} is not setup on {typeof(T).FullName}");

            var threadName = Thread.CurrentThread.Name;
            var key = $"{Thread.CurrentThread.Name}+{attr.QueueName}";
            if (!_receivers.ContainsKey(key))
            {
                _receivers[key] = _client.CreateProcessor(attr.QueueName, new ServiceBusProcessorOptions());
                _receivers[key].ProcessErrorAsync += args =>
                {
                    Console.WriteLine(args.Exception.ToString());
                    return Task.CompletedTask;
                };
                _receivers[key].ProcessMessageAsync += async args =>
                {
                    try
                    {
                        await handler(threadName, args.Message.Body.ToObjectFromJson<T>());
                    }
                    finally
                    {
                        await args.CompleteMessageAsync(args.Message);
                    }
                };
                await _receivers[key].StartProcessingAsync();
            }
            else if (!_receivers[key].IsClosed)
            {
                _receivers[key].CloseAsync().Wait();
                _receivers.TryRemove(key, out _);
                await ReadFromQueue(handler);
            }
        }
    }
}