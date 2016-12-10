using System.Collections.Generic;
using System.Linq;

namespace EasyQMeetup
{
    public class MeetupGoingList
    {
        private struct GoingPerson
        {
            public string Name { get; set; }
            public int InvitedPeople { get; set; }

            public override string ToString() =>
                $"{Name} {(InvitedPeople > 0 ? $"plus {InvitedPeople}" : string.Empty)}";
        }

        private ICollection<GoingPerson> _goingList = new HashSet<GoingPerson>();

        public void Confirm(string userName, int plusNumber) =>
            _goingList.Add(new GoingPerson { Name = userName, InvitedPeople = plusNumber});

        public void Cancel(string userName) =>
            _goingList.Remove(_goingList.FirstOrDefault(x => x.Name == userName));

        public override string ToString() =>
            string.Join("\n", _goingList);
    }
}
