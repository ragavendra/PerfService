using System.Text.Json;
using Confluent.Kafka;
using PerfRunner.Models;

namespace PerfRunner.Services
{
   public class KafkaUserStore : IUserManager
   {

      private readonly ILogger<KafkaUserStore> _logger;

      private readonly string _topic;

      private readonly KafkaDependentProducer<string, string> _producer;

      private readonly IConsumer<string, string> _consumer;

      private readonly int _consumerTimeout;

      public UserFormatInfo UserFormatInfo { get; set; } = new UserFormatInfo();

      public KafkaUserStore(KafkaDependentProducer<string, string> producer, ILogger<KafkaUserStore> logger, IConfiguration configuration)
      {
         _topic = configuration.GetValue<string>("Kafka:UserTopic");
         _producer = producer;
         _logger = logger;

         // InitializeAsync();
         var consumerConfig = new ConsumerConfig();
         configuration.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
         _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

         if (!int.TryParse(configuration.GetValue<string>("KafkaConsumeTimeout"), out _consumerTimeout))
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

      private void _deliveryReportHandler(DeliveryReport<string, string> deliveryReport)
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
         // _consumer.Subscribe(_topic);
         _consumer.Subscribe("perf_user_" + userState.ToString().ToLower());

         var msg = _consumer.Consume(_consumerTimeout);

         if (msg?.Message?.Value == null)
         {
            _logger.LogWarning($"No more {userState.ToString()} users present!");
         }
         else
         {
            var user = JsonSerializer.Deserialize<User>(msg?.Message?.Value);
            return user;
         }

         return default;
      }

      public bool CheckInUser(User user)
      {
         var user_ = JsonSerializer.Serialize(user);

         // no await as method is sync
         // _producer.ProduceAsync(_topic, new Message<string, User> { Key = user.State.ToString(), Value = user });
         _producer.Produce("perf_user_" + user.State.ToString().ToLower(), new Message<string, string> { Key = user.State.ToString(), Value = user_ }, _deliveryReportHandler);

         return true;
      }
   }
}
