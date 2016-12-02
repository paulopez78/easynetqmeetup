using System;
using System.Linq;
using EasyNetQ;
using EasyQMeetup.Domain;
using EasyQMeetup.Domain.Events;

namespace EasyQMeetup
{
    public class Program
    {
        public class Message
        {
            public string Text { get; set; }
        }
        public static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("amqp://guest:guest@localhost:5672");

            var easyMeetup = new Meetup(1, "EasyNetQ Meetup Demo");
            easyMeetup.RSVP("Pau", 2);
            easyMeetup.CancelRSVP("Pau");
    
            bus.Subscribe<RSVPConfirmedEvent>("MeetupRSVP_Subscription", 
                @event => Console.WriteLine($"RSVP confirmed for {@event.UserName} with {@event.PlusNumber}"));

            bus.Subscribe<RSVPCancelledEvent>("MeetupRSVP_Subscription", 
                @event => Console.WriteLine($"RSVP cancelled for {@event.UserName}"));

            Console.WriteLine("Publishing events.");
            
            foreach (dynamic domainEvent in easyMeetup.GetEvents())
                bus.Publish(domainEvent);

            Console.WriteLine("Events published.");
            Console.ReadLine();
        }
    }
}
