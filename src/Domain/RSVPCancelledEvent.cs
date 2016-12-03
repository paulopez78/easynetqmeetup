namespace EasyQMeetup.Domain.Events
{
    public class RSVPCancelledEvent : IDomainEvent
    {
        public string UserName { get; }

        public RSVPCancelledEvent(string userName)
        {
            UserName = userName;
        }
    }
}