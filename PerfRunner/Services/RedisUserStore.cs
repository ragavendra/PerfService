using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Grpc.Core;
using StackExchange.Redis;
using Google.Protobuf;
using PerfRunner.Models;

namespace PerfRunner.Services
{
   public class RedisUserStore : IUserManager
   {
      private const string CART_FIELD_NAME = "cart";

      private const int REDIS_RETRY_NUM = 30;

      private volatile ConnectionMultiplexer _redis;

      private volatile bool _isRedisConnectionOpened;

      private readonly object locker = new object();

      private readonly string _connectionString;

      private readonly ConfigurationOptions _redisConnectionOptions;

      private readonly ILogger<RedisUserStore> _logger;

      private readonly IConfiguration _configuration;

      public UserFormatInfo UserFormatInfo { get; set; } = new UserFormatInfo();

      public RedisUserStore(ILogger<RedisUserStore> logger, IConfiguration configuration)
      {
         _logger = logger;
         _configuration = configuration;

         // _redisConnectionOptions = new ConfigurationOptions() { };
         // _redisConnectionOptions.
         // "localhost";
         // object asyncState = new object();
         // Task.AsyncState;

         _connectionString = $"{_configuration["REDIS_ADDR"]},ssl=false,allowAdmin=true,abortConnect=false";
         // var connectionString = $"{addr},ssl=false,allowAdmin=true,abortConnect=false";

         _redisConnectionOptions = ConfigurationOptions.Parse(_connectionString);

         // Try to reconnect multiple times if the first retry fails.
         _redisConnectionOptions.ConnectRetry = REDIS_RETRY_NUM;
         _redisConnectionOptions.ReconnectRetryPolicy = new ExponentialRetry(1000);

         _redisConnectionOptions.KeepAlive = 180;

         InitializeAsync();

         LoadUsers();
      }


      // public RedisUserStore(ILogger<RedisUserStore> logger, IConfiguration configuration)
      public RedisUserStore(string addr)
      {
         // _logger = logger;
         // _configuration = configuration;

         // _redisConnectionOptions = new ConfigurationOptions() { };
         // _redisConnectionOptions.
         // "localhost";
         // object asyncState = new object();
         // Task.AsyncState;

         // var connectionString = $"{_configuration["REDIS_ADDR"]},ssl=false,allowAdmin=true,abortConnect=false";
         _connectionString = $"{addr},ssl=false,allowAdmin=true,abortConnect=false";

         _redisConnectionOptions = ConfigurationOptions.Parse(_connectionString);

         // Try to reconnect multiple times if the first retry fails.
         _redisConnectionOptions.ConnectRetry = REDIS_RETRY_NUM;
         _redisConnectionOptions.ReconnectRetryPolicy = new ExponentialRetry(1000);

         _redisConnectionOptions.KeepAlive = 180;

         InitializeAsync();

         LoadUsers();
      }

      public Task InitializeAsync()
      {
         EnsureRedisConnected();
         return Task.CompletedTask;
      }

      public ConnectionMultiplexer GetConnection()
      {
         EnsureRedisConnected();
         return _redis;
      }

      private void EnsureRedisConnected()
      {
         if (_isRedisConnectionOpened)
         {
            return;
         }

         // Connection is closed or failed - open a new one but only at the first thread
         lock (locker)
         {
            if (_isRedisConnectionOpened)
            {
               return;
            }

            Console.WriteLine("Connecting to Redis: " + _connectionString);
            _redis = ConnectionMultiplexer.Connect(_redisConnectionOptions);

            if (_redis == null || !_redis.IsConnected)
            {
               Console.WriteLine("Wasn't able to connect to redis");

               // We weren't able to connect to Redis despite some retries with exponential backoff.
               // throw new ApplicationException("Wasn't able to connect to redis");
            }

            Console.WriteLine("Successfully connected to Redis");
            var cache = _redis.GetDatabase();

            Console.WriteLine("Performing small test");
            cache.StringSet("someKey", "OK");
            object res = cache.StringGet("someKey");
            Console.WriteLine($"Small test result: {res}");

            _redis.InternalError += (o, e) => { Console.WriteLine(e.Exception); };
            _redis.ConnectionRestored += (o, e) =>
            {
               _isRedisConnectionOpened = true;
               Console.WriteLine("Connection to redis was restored successfully.");
            };
            _redis.ConnectionFailed += (o, e) =>
            {
               Console.WriteLine("Connection failed. Disposing the object");
               _isRedisConnectionOpened = false;
            };

            _isRedisConnectionOpened = true;
         }
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

      public bool Ping()
      {
         try
         {
            var cache = _redis.GetDatabase();
            var res = cache.Ping();
            return res != TimeSpan.Zero;
         }
         catch (Exception)
         {
            return false;
         }
      }

      public User CheckOutUser(UserState userState)
      {
         EnsureRedisConnected();
         var db = _redis.GetDatabase();

         // var value = await db.HashGetAsync("userId", CART_FIELD_NAME);
         var user_ = db.ListRightPop(userState.ToString().ToLower());
         // var value = await db.HashGetAsync("userId", CART_FIELD_NAME);

         if (user_.IsNull)
         {
            // _logger.LogWarning("No more users present!");
            Console.WriteLine($"No more {userState.ToString()} users present!");
         }
         else
         {
            var user = JsonSerializer.Deserialize<User>(user_);
            // Console.WriteLine($"UserRedisStore is {user.State} .");
            return user;
         }

         return default;
      }

      public bool CheckInUser(User user)
      {
         EnsureRedisConnected();
         var db = _redis.GetDatabase();

         var user_ = JsonSerializer.Serialize(user);
         var length = db.ListLeftPush(user.State.ToString().ToLower(), user_);
         if(length > 0)
         {
            return true;
         }

         return default;
      }
   }
}
