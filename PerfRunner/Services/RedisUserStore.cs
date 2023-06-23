using System;
using System.Linq;
using System.Threading.Tasks;
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
        private volatile bool _isRedisConnectionOpened = false;

        private readonly object locker = new object();
        private readonly byte[] emptyCartBytes;
        private readonly string _connectionString;

        private readonly ConfigurationOptions _redisConnectionOptions;

        private readonly ILogger<RedisUserStore> _logger;

        public RedisUserStore(ILogger<RedisUserStore> logger)
        {
            _logger = logger;
        }

        public ConnectionMultiplexer GetConnection()
        {
            EnsureRedisConnected();
            return _redis;
        }

        public Task InitializeAsync()
        {
            EnsureRedisConnected();
            return Task.CompletedTask;
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
                    throw new ApplicationException("Wasn't able to connect to redis");
                }

                Console.WriteLine("Successfully connected to Redis");
                var cache = _redis.GetDatabase();

                Console.WriteLine("Performing small test");
                cache.StringSet("someKey", "OK" );
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

      public async Task<User> CheckOutUser(UserState userState)
      {
        var db = _redis.GetDatabase();

        // var value = await db.HashGetAsync("userId", CART_FIELD_NAME);
        var value = await db.HashGetAsync("userId", CART_FIELD_NAME);

        if(value.IsNull)
        {
            _logger.LogWarning("No more users present!");
        }
        else
        {
            return (User)value;
        }

        return default;
      }

      public bool CheckInUser(User user)
      {
         throw new NotImplementedException();
      }
   }
}
