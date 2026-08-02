namespace ENSE707_AppointmentBooking
{
    // Represents clinic-specific booking rules. Kept separate from
    // AppointmentBookingService so different clinics/deployments could
    // configure different policies without touching booking logic.

    public class ClinicPolicy
    {
        public int MinimumNoticeDays { get; }
 
        public ClinicPolicy(int minimumNoticeDays = 1)
        {
            if (minimumNoticeDays < 0)
                throw new ArgumentException("Minimum notice days cannot be negative");
 
            MinimumNoticeDays = minimumNoticeDays;
        }
    }
}