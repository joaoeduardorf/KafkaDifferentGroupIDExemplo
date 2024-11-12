using Confluent.Kafka;
using Core;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace TwoNumbersProducer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProducer<string, string> _producer;
        private readonly KafkaSettings _kafkaSettings;

        public Worker(ILogger<Worker> logger, IOptions<KafkaSettings> kafkaSettings)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings.Value;

            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                Acks = Enum.Parse<Acks>(_kafkaSettings.Acks)
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                var twoNumbers = new TwoNumbers().GenerateTwoRandomNumbers();
                var message = new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = JsonSerializer.Serialize(twoNumbers) };
                try
                {
                    var deliveryResult = await _producer.ProduceAsync(_kafkaSettings.Topic, message, stoppingToken);
                    _logger.LogInformation($"Delivered message to {deliveryResult.TopicPartitionOffset}");
                }
                catch (ProduceException<string, string> e)
                {
                    _logger.LogError($"Delivery failed: {e.Error.Reason}");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
