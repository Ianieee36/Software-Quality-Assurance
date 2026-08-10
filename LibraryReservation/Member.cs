namespace LibraryReservation
{
    public class Member
    {
        public string Id { get; }
        public string FullName { get; }
        public bool HasReservations { get; private set;}

        public Member(string id, string fullName)
        {
            if(string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Member ID is required.");

            if(string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Member name is required.");

            Id = id;
            FullName = fullName; 
            HasReservations = false;
        }

        public bool HasActiveReservation()
        {
            return HasReservations;
        }

        public void MarkAsReserved()
        {
            HasReservations = true;
        }
    }
}