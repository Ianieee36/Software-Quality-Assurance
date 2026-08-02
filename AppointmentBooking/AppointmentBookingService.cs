namespace ENSE707_AppointmentBooking
{
    public class AppointmentBookingService
    {
        private readonly ClinicPolicy _policy;

        public AppointmentBookingService(ClinicPolicy policy = null)
        {
            _policy = policy ?? new ClinicPolicy();
        }

        public BookingResult BookAppointment(AppointmentRequest request)
        {
            if (request == null)
                return BookingResult.MissingRequest();
 
            // Defense in depth: Patient already guards against a blank ID at
            // construction time, but re-checking here means this rule is
            // enforced by the booking process itself, not only by assuming
            // every caller constructs Patient correctly.
            if (string.IsNullOrWhiteSpace(request.Patient.Id))
                return BookingResult.InvalidPatientId();
 
            DateTime earliestAllowedDate = DateTime.Today.AddDays(_policy.MinimumNoticeDays);
            if (request.RequestedDate.Date < earliestAllowedDate)
                return BookingResult.InsufficientNotice(_policy.MinimumNoticeDays);
 
            if (!request.Doctor.IsAvailableOn(request.RequestedDate))
            {
                // Distinguish WHY it's unavailable so the message stays actionable
                // rather than a generic "can't book" response.
                if (request.Doctor.AvailableSlots <= 0)
                    return BookingResult.NoAvailability(request.Doctor);
 
                return BookingResult.DailyLimitReached(request.Doctor, request.RequestedDate);
            }
 
            request.Doctor.ReserveSlot(request.RequestedDate);
 
            return BookingResult.Booked(request.Patient, request.Doctor, request.RequestedDate);
        }
    }
}