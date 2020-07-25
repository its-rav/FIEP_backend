using Castle.Core.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FirebaseAdmin.Messaging;
using DataTier.UOW;
using BusinessTier.Utilities;

namespace BusinessTier.Services
{
    public class MainWorker : BackgroundService
    {
        private readonly ILogger<MainWorker> _logger;


        public MainWorker(ILogger<MainWorker> logger, Microsoft.Extensions.Configuration.IConfiguration configuration,IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            ggSheetUtil = new GoogleSheetApiUtils(_unitOfWork);

            _logger = logger;
            string connectionString = configuration.GetConnectionString("RedisCacheConnection");
            _redis =ConnectionMultiplexer.Connect(connectionString);

            if (_redis.IsConnected)
            {
                _logger.LogInformation($"Redis successfully connected");
            }
            else
            {
                _logger.LogInformation($"Fail to connect to Redis");
            }

        }
        private GoogleSheetApiUtils ggSheetUtil;
        private ConnectionMultiplexer _redis;
        private IUnitOfWork _unitOfWork;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _redis.GetSubscriber().Subscribe("NotificationChannel", async (channel, message) =>
            {
                _logger.LogInformation($"New notification receiving at channel {channel}: {message}");


                Guid notificationID = new Guid(message.ToString());

                var notification = _unitOfWork.Repository<DataTier.Models.Notification>().FindFirstByProperty(x => x.NotificationID == notificationID);
                List<string> fcmTokens = new List<string>();
                _logger.LogInformation($"Debug {notification}");
                if (notification.UserFCMTokens != null)
                {
                    //var eventRepo = _unitOfWork.Repository<Event>().GetAll().Where(x => x.EventId==notification.EventId);
                }

                if (notification.Event != null)
                {
                    var listOfEventSubcriptions = notification.Event.EventSubscription;
                    foreach (var eventsubs in listOfEventSubcriptions)
                    {
                        var tokens = eventsubs.User.FCMToken;
                        foreach (var token in tokens)
                        {
                           fcmTokens.Add(token.FCMToken);
                        }
                    }
                }

                if (notification.GroupInformation != null)
                {
                    var listOfGroupSubcriptions = notification.GroupInformation.GroupSubscription;
                    foreach (var groupsubs in listOfGroupSubcriptions)
                    {
                        var tokens = groupsubs.User.FCMToken;
                        foreach (var token in tokens)
                        {
                            fcmTokens.Add(token.FCMToken);
                        }
                    }
                }

                if (fcmTokens.Count <= 0)
                {
                    return;
                }

                foreach (var token in fcmTokens)
                {
                    _logger.LogInformation($"Debug {token}");
                }
                var multicastMessage = new MulticastMessage()
                {
                    Tokens = fcmTokens,
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Body = notification.Body,
                        Title = notification.Title,
                        ImageUrl = notification.ImageUrl,
                    },
                };

                var messaging = FirebaseMessaging.DefaultInstance;
                try {

                    BatchResponse response;
                    response = await messaging.SendMulticastAsync(multicastMessage);
                    var failedTokens = new List<string>();

                    if (response.FailureCount > 0)
                    {
                        for (var i = 0; i < response.Responses.Count; i++)
                        {
                            if (!response.Responses[i].IsSuccess)
                            {
                                // The order of responses corresponds to the order of the registration tokens.
                                failedTokens.Add(multicastMessage.Tokens[i]);
                            }
                        }
                        _logger.LogInformation($"List of tokens that caused failures: {failedTokens}");
                    }
                } catch (Exception e)  {
                    _logger.LogInformation(e.ToString());
                }
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("( 10mins ) Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(600000, stoppingToken);

                ggSheetUtil.UpdateDataToSheet();
            }
            _redis.GetSubscriber().Unsubscribe("NotificationChannel");
        }
    }
}
