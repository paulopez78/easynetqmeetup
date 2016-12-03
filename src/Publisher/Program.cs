using System;
using EasyNetQ;
using EasyQMeetup.Domain;

namespace EasyQMeetup
{
    public class Publisher
    {
        public static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("amqp://guest:guest@localhost:5672");

            var easyMeetup = new Meetup(1, "EasyNetQ Meetup Demo");
            easyMeetup.RSVP("Pau", 2);
            easyMeetup.CancelRSVP("Pau");
    
            Console.WriteLine("Publishing events.");
            
            foreach (dynamic domainEvent in easyMeetup.GetEvents())
                bus.Publish(domainEvent);

            Console.WriteLine("Events published.");
            Console.ReadLine();
        }
    }
}
