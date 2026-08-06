namespace LibraryReservation.Tests;

[TestClass]
public sealed class ReservationServiceTests
{
    // AC-01
    [TestMethod]
    public void ReserveBook_BookIsAvailableAndValidMember_ReservationSucceeds()
    {
        var book = new Book("B001", "Software Testing Basics");
        var member = new Member("M001", "Christian Cantos");
        var service = new ReservationService();

        ReservationResult result = service.ReserveBook(book, member);

        Assert.IsTrue(result.Success);
        StringAssert.Contains(result.Message, "Reservation successful");
    }

    // AC-02
    [TestMethod]
    public void ReserveBook_EmptyMemberId_ThrowsException()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
            new Member("", "Christian Cantos"));
    }

    // AC-03
    [TestMethod]
    public void ReserveBook_BookAlreadyReserved_ReservationFails()
    {
        var book = new Book("B001", "Software Testing Basics");
        var member1 = new Member("M001", "Christian Cantos");
        var member2 = new Member("M002", "Chris Cantos");
        var service = new ReservationService();

        service.ReserveBook(book, member1);
        ReservationResult result = service.ReserveBook(book, member2);

        Assert.IsFalse(result.Success);
        StringAssert.Contains(result.Message, "already reserved");
    }

    // AC-04
    [TestMethod]
    public void ReserveBook_NullBook_ReservationFailsWithClearMessage()
    {
        var member = new Member("M001", "Christian Cantos");
        var service = new ReservationService();

        ReservationResult result = service.ReserveBook(null, member);

        Assert.IsFalse(result.Success);
        StringAssert.Contains(result.Message, "book details are required");
    }

    [TestMethod]
    public void ReserveBook_NullMember_ReservationFails()
    {
        var book = new Book("B001", "Software Testing Basics");
        var service = new ReservationService();

        ReservationResult result = service.ReserveBook(book, null);

        Assert.IsFalse(result.Success);
        StringAssert.Contains(result.Message, "member details are required.");
    }
}
