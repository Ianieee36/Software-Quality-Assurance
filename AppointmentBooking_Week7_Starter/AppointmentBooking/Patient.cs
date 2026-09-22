namespace ENSE707_AppointmentBooking;

public class Patient
{
    public Patient(string id, string legalName, string preferredName = "")
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Patient ID is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(legalName))
        {
            throw new ArgumentException("Legal name is required.", nameof(legalName));
        }

        Id = id;
        LegalName = legalName;
        PreferredName = preferredName;
    }

    public string Id { get; }

    public string LegalName { get; }

    public string PreferredName { get; }

    public string DisplayName => string.IsNullOrWhiteSpace(PreferredName)
        ? LegalName
        : PreferredName;
}

