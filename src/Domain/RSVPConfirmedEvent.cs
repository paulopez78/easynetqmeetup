namespace EasyQMeetup.Domain.Events
{
    public class RSVPConfirmedEvent : IDomainEvent
    {
        public string UserName { get; }
        public int PlusNumber { get; }

        public RSVPConfirmedEvent(string userName, int plusNumber)
        {
            UserName = userName;
            PlusNumber = plusNumber;
        }
    }
}