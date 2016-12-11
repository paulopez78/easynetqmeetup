using System;
using EasyNetQ;
using EasyQMeetup.Domain.Events;
using Microsoft.Extensions.Configuration;

namespace EasyQMeetup
{
    public class Consumer
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = $@"
                host={config["RABBIT_HOST"]};
                username={config["RABBIT_USER"]};
                password={config["RABBIT_PASSWORD"]}";
            
            var goingList = new MeetupGoingList();
            var bus = RabbitHutch.CreateBus(connectionString);

            bus.Subscribe<RSVPConfirmedEvent>("MeetupRSVP_Subscription", 
                @event => {
                    goingList.Confirm(@event.UserName, @event.PlusNumber);
                    Console.Clear();
                    Console.Write(goingList.ToString());
                });

            bus.Subscribe<RSVPCancelledEvent>("MeetupRSVP_Subscription", 
                @event => {
                    goingList.Cancel(@event.UserName);
                    Console.Clear();
                    Console.Write(goingList.ToString());
                });

            Console.ReadLine();
        }
    }
}
