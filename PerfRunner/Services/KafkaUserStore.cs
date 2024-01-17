using Confluent.Kafka;
using PerfRunner.Models;

namespace PerfRunner.Services
{
   public class KafkaUserStore : IUserManager
   {

      private readonly ILogger<KafkaUserStore> _logger;

      private readonly string _topic;

      private readonly KafkaDependentProducer<string, User> _producer;

      private readonly IConsumer<string, User> _consumer;

      private readonly int _consumerTimeout;

      public UserFormatInfo UserFormatInfo { get; set; } = new UserFormatInfo();

      public KafkaUserStore(KafkaDependentProducer<string, User> producer, ILogger<KafkaUserStore> logger, IConfiguration configuration)
      {
         _topic = configuration.GetValue<string>("Kafka:UserTopic");
         _producer = producer;
         _logger = logger;

         // InitializeAsync();
         var consumerConfig = new ConsumerConfig();
         configuration.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
         _consumer = new ConsumerBuilder<string, User>(consumerConfig).Build();

         if (!int.TryParse(configuration.GetValue<string>("Kafka:ConsumerSettings:ConsumeTimeout"), out _consumerTimeout))
            _consumerTimeout = 6_000;

         LoadUsers();
      }

      public KafkaUserStore(ILogger<KafkaUserStore> logger, IConfiguration configuration)
      {
         _logger = logger;

         // _connectionString = $"{configuration["KAFKA_ADDR"]},ssl=false,allowAdmin=true,abortConnect=false";
         // var connectionString = $"{addr},ssl=false,allowAdmin=true,abortConnect=false";
         // private readonly ProducerConfig _config;
         // _config = new ProducerConfig { BootstrapServers = configuration["KAFKA_ADDR"] };

         // InitializeAsync();
         LoadUsers();
      }

      private void _deliveryReportHandler(DeliveryReport<string, User> deliveryReport)
      {
         if (deliveryReport.Status == PersistenceStatus.NotPersisted)
         {
            // It is common to write application logs to Kafka (note: this project does not provide
            // an example logger implementation that does this). Such an implementation should
            // ideally fall back to logging messages locally in the case of delivery problems.
            _logger.Log(LogLevel.Warning, $"Message delivery failed: {deliveryReport.Message.Value}");
         }
      }

      public Task InitializeAsync()
      {
         // EnsureRedisConnected();
         return Task.CompletedTask;
      }

            // load users to ready state
      private void LoadUsers()
      {
         var totalUsers = UserFormatInfo?.TotalUsers;
         var accountIndex = UserFormatInfo?.UserStartIndex;
         while (totalUsers-- >= 0)
         {
            var user = new User(string.Format(UserFormatInfo!.UserAccountFormat, accountIndex++), UserState.Ready);
            // _logger.LogInformation("Loading user - {user}", user);
            // Console.WriteLine($"Loading user - {user.Email}");
            CheckInUser(user);
         }
      }

      public User CheckOutUser(UserState userState)
      {
         _consumer.Subscribe(_topic);
         var msg = _consumer.Consume(_consumerTimeout);

         if (msg.Message.Value is not User)
         {
            _logger.LogWarning($"No more {userState.ToString()} users present!");
         }
         else
         {
            return msg.Message.Value;
         }

         return default;
      }

      public bool CheckInUser(User user)
      {
         // no await as method is sync
         // _producer.ProduceAsync(_topic, new Message<string, User> { Key = user.State.ToString(), Value = user });
         _producer.Produce(_topic, new Message<string, User> { Key = user.State.ToString(), Value = user }, _deliveryReportHandler);

         return true;
      }
   }
}
