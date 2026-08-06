namespace LibraryReservation
{
    public class Book
    {
        public string Id { get; }
        public string Title { get; }
        public bool IsReserved { get; private set; }

        public Book(string id, string title)
        {
            if(string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Book ID is required");
            
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Book title is required.");
            
            Id = id;
            Title = title;
            IsReserved = false;
            
        }

        public void MarkAsReserved()
        {
            if(IsReserved)
                throw new InvalidOperationException("Book is already reserved.");
            
            IsReserved = true;
        } 
    }
}