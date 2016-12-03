using System;
using EasyNetQ;
using EasyQMeetup.Domain.Events;

namespace EasyQMeetup
{
    public class Consumer
    {
        public static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("amqp://guest:guest@localhost:5672");

            bus.Subscribe<RSVPConfirmedEvent>("MeetupRSVP_Subscription", 
                @event => Console.WriteLine($"RSVP confirmed for {@event.UserName} with {@event.PlusNumber}"));

            bus.Subscribe<RSVPCancelledEvent>("MeetupRSVP_Subscription", 
                @event => Console.WriteLine($"RSVP cancelled for {@event.UserName}"));

            Console.ReadLine();
        }
    }
}
