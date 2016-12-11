using System;
using EasyNetQ;
using EasyQMeetup.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EasyQMeetup
{
    public class Consumer
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var logger = new LoggerFactory()
                .AddConsole(config.GetSection("Logging"))
                .AddDebug()
                .CreateLogger<Consumer>();


            var goingList = new MeetupGoingList();
            var bus = RabbitHutch.CreateBus(GetConnectionString(config));

            logger.LogInformation("Starting Consumer");

            bus.Subscribe<RSVPConfirmedEvent>("MeetupRSVP_Subscription", 
                @event => {
                    goingList.Confirm(@event.UserName, @event.PlusNumber);
                    logger.LogInformation(goingList.ToString());
                });

            bus.Subscribe<RSVPCancelledEvent>("MeetupRSVP_Subscription", 
                @event => {
                    goingList.Cancel(@event.UserName);
                    logger.LogInformation(goingList.ToString());
                });

            Console.ReadLine();
        }

        private static string GetConnectionString(IConfiguration config) => 
            $@"host={config["RABBIT_HOST"]};
            username={config["RABBIT_USER"]};
            password={config["RABBIT_PASSWORD"]}";
    }
}
