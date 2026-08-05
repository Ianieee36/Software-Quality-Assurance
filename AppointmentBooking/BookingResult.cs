using System;

namespace ENSE707_AppointmentBooking
{
    public class BookingResult
    {
        public bool Success { get; }
        public string Message { get; }
        public Appointment Appointment { get; }

        public BookingResult(bool success, string message, Appointment appointment = null)
        {
            Success = success;
            Message = message;
            Appointment = appointment;
        }

        public static BookingResult Booked(Appointment appointment) =>
            new BookingResult(true,
                $"Appointment booked successfully for {appointment.Patient.DisplayName} with {appointment.Doctor.FullName} on {appointment.AppointmentDate:d}.",
                appointment);

        public static BookingResult MissingRequest() =>
            new BookingResult(false,
                "Appointment request is missing. Please provide patient, doctor, and date details and try again.");

        public static BookingResult InvalidPatientId() =>
            new BookingResult(false,
                "A valid patient ID is required to book this appointment. Please provide your patient ID and try again.");

        public static BookingResult InsufficientNotice(int minimumNoticeDays) =>
            new BookingResult(false,
                $"Appointments must be booked at least {minimumNoticeDays} day(s) in advance. Please choose a later date.");

        public static BookingResult NoAvailability(Doctor doctor) =>
            new BookingResult(false,
                $"{doctor.FullName} has no available appointment slots. Please choose another doctor or contact the clinic.");

        public static BookingResult DailyLimitReached(Doctor doctor, DateTime date) =>
            new BookingResult(false,
                $"{doctor.FullName} has reached the maximum of {doctor.MaxDailyAppointments} appointments for {date:d}. Please choose a different date.");
    }
}