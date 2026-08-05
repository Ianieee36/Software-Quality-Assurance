using ENSE707_AppointmentBooking;

namespace ENSE707_AppointmentBooking.Tests
{
    [TestClass]
    public class CancelAppointmentTests
    {
        private static Doctor CreateDoctor(int slots = 2, int maxDaily = 5) =>
            new Doctor("D001", "Dr. Amelia Ratana", slots, maxDaily);

        private static Patient CreatePatient() =>
            new Patient("P001", "Christian Cantos");

        private static Appointment BookValidAppointment(Doctor doctor, Patient patient, AppointmentBookingService service, DateTime? date = null)
        {
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            BookingResult result = service.BookAppointment(request);
            return result.Appointment;
        }

        [TestMethod]
        public void CancelAppointment_ExistingAppointment_MarksAppointmentAsCancelled()
        {
            var doctor = CreateDoctor();
            var patient = CreatePatient();
            var service = new AppointmentBookingService();
            var appointment = new Appointment("A001", doctor, patient, DateTime.Today.AddDays(1));

            service.CancelAppointment(appointment);

            Assert.IsTrue(appointment.IsCancelled);
        }

        [TestMethod]
        public void CancelAppointment_ExistingAppointment_ReleasesDoctorSlot()
        {
            var doctor = CreateDoctor(slots: 3);
            var patient = CreatePatient();
            var service = new AppointmentBookingService();

            // Book first so there is a real reserved slot to release.
            var appointment = BookValidAppointment(doctor, patient, service);
            Assert.AreEqual(2, doctor.AvailableSlots); // sanity check on the booking step itself

            service.CancelAppointment(appointment);

            // NOTE: this assertion reflects REQ-CAN-02 as specified. It will
            // fail against the current CancelAppointment implementation,
            // since it does not call doctor.ReleaseSlot(). That failure is
            // the intended signal that the release logic is still missing,
            // not a mistake in this test.
            Assert.AreEqual(3, doctor.AvailableSlots);
        }

        [TestMethod]
        public void CancelAppointment_NullAppointment_ThrowsException()
        {
            var service = new AppointmentBookingService();

            Assert.ThrowsExactly<ArgumentNullException>(() => service.CancelAppointment(null));
        }

        [TestMethod]
        public void CancelAppointment_AlreadyCancelledAppointment_ThrowsException()
        {
            var doctor = CreateDoctor();
            var patient = CreatePatient();
            var service = new AppointmentBookingService();
            var appointment = new Appointment("A001", doctor, patient, DateTime.Today.AddDays(1));

            service.CancelAppointment(appointment); // first cancellation succeeds

            // Second cancellation attempt on the same appointment should fail.
            // Appointment.Cancel() throws InvalidOperationException directly;
            // CancelAppointment does not currently wrap or translate it, so
            // that is the exact exception type expected here.
            Assert.ThrowsExactly<InvalidOperationException>(() => service.CancelAppointment(appointment));
        }

        [TestMethod]
        public void BookAppointment_Success_ReturnsAppointmentWithCorrectDetails()
        {
            var doctor = CreateDoctor();
            var patient = CreatePatient();
            var service = new AppointmentBookingService();
            var requestedDate = DateTime.Today.AddDays(1);
            var request = new AppointmentRequest(patient, doctor, requestedDate);

            BookingResult result = service.BookAppointment(request);

            Assert.IsNotNull(result.Appointment);
            Assert.AreEqual(doctor, result.Appointment.Doctor);
            Assert.AreEqual(patient, result.Appointment.Patient);
            Assert.AreEqual(requestedDate, result.Appointment.AppointmentDate);
            Assert.IsFalse(result.Appointment.IsCancelled);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Appointment.Id));
        }

        // =========================================================== //
        // REQ-CAN-04: cancelling restores per-date daily availability,
        // not just the overall slot pool.
        // =========================================================== //
 
        [TestMethod]
        public void CancelAppointment_ExistingAppointment_RestoresDailyAppointmentAvailability()
        {
            // Doctor capped at 1 appointment per day, so booking the one slot
            // for a date fully consumes that date's availability.
            var doctor = CreateDoctor(slots: 5, maxDaily: 1);
            var patient = CreatePatient();
            var service = new AppointmentBookingService();
            var date = DateTime.Today.AddDays(1);
 
            var appointment = BookValidAppointment(doctor, patient, service, date);
            Assert.IsFalse(doctor.IsAvailableOn(date)); // daily cap reached
 
            service.CancelAppointment(appointment);
 
            // Cancelling should reopen this specific date, not just increase
            // the overall pool.
            Assert.IsTrue(doctor.IsAvailableOn(date));
            Assert.AreEqual(0, doctor.BookedCountFor(date));
        }

        [TestMethod]
        public void CancelAppointment_OneOfTwoBookingsOnSameDate_OnlyReleasesOneSlot()
        {
            // Guards against ReleaseSlot over-crediting the daily count -
            // cancelling one of two same-day bookings should leave the date
            // still counted as having one active booking, not zero.
            var doctor = CreateDoctor(slots: 5, maxDaily: 2);
            var date = DateTime.Today.AddDays(1);
            var service = new AppointmentBookingService();
 
            var appointment1 = BookValidAppointment(doctor, new Patient("P001", "Diana William"), service, date);
            BookValidAppointment(doctor, new Patient("P002", "Aroha Ngata"), service, date);
            Assert.AreEqual(2, doctor.BookedCountFor(date));
 
            service.CancelAppointment(appointment1);
 
            Assert.AreEqual(1, doctor.BookedCountFor(date));
        }
 

 
        // =========================================================== //
        // REQ-CAN-05: cancellation failure messages must be clear and
        // actionable, not generic framework defaults.
        // =========================================================== //
 
        [TestMethod]
        public void CancelAppointment_NullAppointment_ExceptionMessageIsClearAndActionable()
        {
            var service = new AppointmentBookingService();
 
            var exception = Assert.ThrowsExactly<ArgumentNullException>(
                () => service.CancelAppointment(null));
 
            // Guards against the .NET default "Value cannot be null.
            // (Parameter 'appointment')" text, which names the parameter
            // but gives the caller no indication of what to do about it.
            StringAssert.Contains(exception.Message, "valid appointment");
        }
 
        [TestMethod]
        public void CancelAppointment_AlreadyCancelledAppointment_ExceptionMessageIsClearAndActionable()
        {
            var doctor = CreateDoctor();
            var patient = CreatePatient();
            var service = new AppointmentBookingService();
            var appointment = new Appointment("A001", doctor, patient, DateTime.Today.AddDays(1));
 
            service.CancelAppointment(appointment);
 
            var exception = Assert.ThrowsExactly<InvalidOperationException>(
                () => service.CancelAppointment(appointment));
 
            StringAssert.Contains(exception.Message, "already been cancelled");
        }
    }
}