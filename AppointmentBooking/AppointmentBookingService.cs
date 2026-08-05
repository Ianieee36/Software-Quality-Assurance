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
 
            if (string.IsNullOrWhiteSpace(request.Patient.Id))
                return BookingResult.InvalidPatientId();
 
            DateTime earliestAllowedDate = DateTime.Today.AddDays(_policy.MinimumNoticeDays);

            if (request.RequestedDate.Date < earliestAllowedDate)
                return BookingResult.InsufficientNotice(_policy.MinimumNoticeDays);
 
            if (!request.Doctor.IsAvailableOn(request.RequestedDate))
            {
                if (request.Doctor.AvailableSlots <= 0)
                    return BookingResult.NoAvailability(request.Doctor);
 
                return BookingResult.DailyLimitReached(request.Doctor, request.RequestedDate);
            }
 
            request.Doctor.ReserveSlot(request.RequestedDate);

            var appointment = new Appointment(
                Guid.NewGuid().ToString(),
                request.Doctor,
                request.Patient,
                request.RequestedDate
            );
 
            return BookingResult.Booked(appointment);
        }

        public void CancelAppointment(Appointment appointment)
        {
            if(appointment == null) 
                throw new ArgumentNullException(nameof(appointment), "A valid appointment is required to cancel a booking. Please provide an existing appointment.");
            
            appointment.Cancel();
            
            appointment.Doctor.ReleaseSlot(appointment.AppointmentDate);
        }
    }
}