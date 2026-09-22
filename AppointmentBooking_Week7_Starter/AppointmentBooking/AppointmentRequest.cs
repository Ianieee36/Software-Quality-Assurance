namespace ENSE707_AppointmentBooking;

public class AppointmentRequest
{
    public AppointmentRequest(Patient patient, Doctor doctor, DateTime requestedDate)
    {
        Patient = patient ?? throw new ArgumentNullException(nameof(patient));
        Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));

        if (requestedDate.Date < DateTime.Today)
        {
            throw new ArgumentException("Requested appointment date cannot be in the past.", nameof(requestedDate));
        }

        RequestedDate = requestedDate;
    }

    public Patient Patient { get; }

    public Doctor Doctor { get; }

    public DateTime RequestedDate { get; }
}

