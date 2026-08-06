namespace LibraryReservation
{
    public class ReservationService
    {
        public ReservationResult ReserveBook(Book book, Member member)
        {
            if(book == null)
                return new ReservationResult(false, "Reservation failed: book details are required.");

            if(member == null) 
                return new ReservationResult(false, "Reservation failed: member details are required.");

            if(book.IsReserved)
                return new ReservationResult(false, $"Reservation failed: '{book.Title}' is already reserved.");

            book.MarkAsReserved();

            return new ReservationResult(true, $"Reservation successful: '{book.Title}' has been reserved for {member.FullName}.");    
        }
    }
}