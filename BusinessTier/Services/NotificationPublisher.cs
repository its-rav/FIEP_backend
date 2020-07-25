using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BusinessTier.Services
{
    public class NotificationPublisher
    {
        public NotificationPublisher(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("RedisCacheConnection"); 
            _redis = ConnectionMultiplexer.Connect(connectionString);
        }

        private ConnectionMultiplexer _redis;
        public void Publish(string message)
        {
            var subscriber=_redis.GetSubscriber();
            subscriber.PublishAsync("NotificationChannel",message,CommandFlags.FireAndForget);
        }
        
    }
}
