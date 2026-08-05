namespace ENSE707_AppointmentBooking
{
    public class Appointment
    {
        public string Id { get; }
        public Doctor Doctor { get; }
        public Patient Patient { get; }
        public DateTime AppointmentDate { get; }
        public bool IsCancelled { get; private set; }

        public Appointment(string id, Doctor doctor, Patient patient, DateTime appointmentDate)
        {

            if(string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Appointment ID is required.");
            
            Id = id;
            Doctor = doctor ?? throw new ArgumentNullException(nameof(doctor));
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            AppointmentDate = appointmentDate;
            IsCancelled = false;

        }
        
        // Method: Cancel() it cancels any existing appointment.
        public void Cancel()
        {   
            // It validates if an appointment is already cancelled or not.
            if(IsCancelled)
                throw new InvalidOperationException("This appointment has already been cancelled and cannot be cancelled again.");
            
            // if an appointment is not yet cancelled it execute the cancellation. 
            IsCancelled = true;
        }
        
    }
}