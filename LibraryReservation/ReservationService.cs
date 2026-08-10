namespace LibraryReservation
{
    public class ReservationService
    {
        public ReservationResult ReserveBook(Book book, Member member)
        {
            // checks if a book exists.
            if(book == null)
                return new ReservationResult(false, "Reservation failed: book details are required.");

            // checks if a member is registered 
            if(member == null) 
                return new ReservationResult(false, "Reservation failed: member details are required.");

            // checks if a member has an active reservation
            if(member.HasActiveReservation())
                return new ReservationResult(false, "Reservation failed: member has an active reservation");

            // checks if a book is already reserved.
            if(book.IsReserved)
                return new ReservationResult(false, $"Reservation failed: '{book.Title}' is already reserved.");

            book.MarkAsReserved(); // book is Reserved
            member.MarkAsReserved(); // member has an active reservation

            return new ReservationResult(true, $"Reservation successful: '{book.Title}' has been reserved for {member.FullName}.");    
        }
    }
}