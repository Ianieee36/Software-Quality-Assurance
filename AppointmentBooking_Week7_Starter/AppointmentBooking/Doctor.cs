namespace ENSE707_AppointmentBooking;

public class Doctor
{
    public Doctor(string id, string fullName, int availableSlots)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Doctor ID is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Doctor name is required.", nameof(fullName));
        }

        if (availableSlots < 0)
        {
            throw new ArgumentException("Available slots cannot be negative.", nameof(availableSlots));
        }

        Id = id;
        FullName = fullName;
        AvailableSlots = availableSlots;
    }

    public string Id { get; }

    public string FullName { get; }

    public int AvailableSlots { get; private set; }

    public bool HasAvailableSlot()
    {
        return AvailableSlots > 0;
    }

    public void ReserveSlot()
    {
        if (!HasAvailableSlot())
        {
            throw new InvalidOperationException("No appointment slots are available.");
        }

        AvailableSlots--;
    }
}

