using System;
using EasyNetQ;
using EasyQMeetup.Domain.Events;

namespace EasyQMeetup
{
    public class Consumer
    {
        public static void Main(string[] args)
        {
            var goingList = new MeetupGoingList();
            var bus = RabbitHutch.CreateBus("amqp://guest:guest@bus:5672");

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
