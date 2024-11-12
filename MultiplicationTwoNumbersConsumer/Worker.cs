using Confluent.Kafka;
using Core;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MultiplicationTwoNumbersConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumer<string, string> _consumer;
        private readonly IProducer<string, string> _producer;
        private readonly KafkaSettings _kafkaSettings;

        public Worker(ILogger<Worker> logger, IOptions<KafkaSettings> kafkaSettings)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings.Value;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                Acks = Enum.Parse<Acks>(_kafkaSettings.Acks)
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_kafkaSettings.InputTopic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    if (consumeResult != null)
                    {
                        var message = consumeResult.Message.Value;
                        var twoNumbers = JsonSerializer.Deserialize<TwoNumbers>(message);
                        string result = $"Consumed multiplication message: {twoNumbers.FirstNumber} x {twoNumbers.SecondNumber} = {twoNumbers.FirstNumber * twoNumbers.SecondNumber}";
                        _producer.Produce(_kafkaSettings.OutputTopic, new Message<string, string> { Key = consumeResult.Message.Key, Value = result });
                        _logger.LogInformation(result);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Consume error: {ex.Error.Reason}");
                }

                await Task.Delay(1000, stoppingToken);
            }

            _consumer.Close();
        }
    }
}

