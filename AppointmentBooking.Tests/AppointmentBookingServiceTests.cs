using ENSE707_AppointmentBooking;

namespace ENSE707_AppointmentBooking.Tests
{
    [TestClass]
    public class AppointmentBookingServiceTests 
    {
        [TestMethod]
        public void BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2, 5);
 
            var patient = new Patient("P001", "Dianna William");
 
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
 
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0, 5);
 
            var patient = new Patient("P001", "Dianna William");
 
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
 
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            Assert.IsFalse(result.Success);
        }
        
        [TestMethod]
        public void BookAppointment_WhenSuccessful_DecreasesAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2, 5);
 
            var patient = new Patient("P001", "Dianna William");
 
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
 
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            Assert.AreEqual(1, doctor.AvailableSlots);
        }

        [TestMethod]
        public void BookAppointment_WhenFailed_DoesNotDecreaseAvailableSlots()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0, 5);
 
            var patient = new Patient("P001", "Dianna William");
 
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
 
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            Assert.AreEqual(0, doctor.AvailableSlots);
        }

        [TestMethod]
        public void Doctor_WhenIdIsEmpty_ThrowsException()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new Doctor("", "Dr Mark", 2, 5));
        }

        [TestMethod]
        public void Doctor_WhenAvailableSlotsIsNegative_ThrowsException()
        {
            Assert.ThrowsExactly<ArgumentException>(() => new Doctor("D001", "Dr Mark", -1, 5));
        }

        [TestMethod]
        public void Doctor_WhenMaxDailyAppointmentsIsZeroOrLess_ThrowsException()
        {
            // New rule: MaxDailyAppointments must be greater than zero.
            Assert.ThrowsExactly<ArgumentException>(() => new Doctor("D001", "Dr Mark", 2, 0));
        }

        [TestMethod]
        public void Patient_WhenIdIsEmpty_ThrowsException()
        {
            // This previously constructed a Doctor instead of a Patient - fixed
            // to actually exercise the class the test name claims to cover.
            Assert.ThrowsExactly<ArgumentException>(() => new Patient("", "Diana William"));
        }

        [TestMethod]
        public void Patient_WhenPreferredNameMissing_DisplayNameUsesLegalName()
        {
            var patient = new Patient("P001", "Diana William", "");
 
            Assert.AreEqual("Diana William", patient.DisplayName);
        }

        [TestMethod]
        public void AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2, 5);
            var patient = new Patient("P001", "Diana William");
            Assert.ThrowsExactly<ArgumentException>(() =>
                new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(-1)));
        }

        [TestMethod]
        public void BookAppointment_WhenSuccessful_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 2, 5);
            var patient = new Patient("P001", "Diana William", "Aroha");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            BookingResult result = service.BookAppointment(request);
            StringAssert.Contains(result.Message, "Appointment booked successfully");
            StringAssert.Contains(result.Message, "Aroha");
        }

        [TestMethod]
        public void BookAppointment_WhenNoSlots_ReturnsHelpfulMessage()
        {
            var doctor = new Doctor("D001", "Dr Mark", 0, 5);
            var patient = new Patient("P001", "Diana William");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
            BookingResult result = service.BookAppointment(request);
 
            // Wording changed with the new BookingResult.NoAvailability factory
            // method: "no available slots" -> "no available appointment slots"
            StringAssert.Contains(result.Message, "no available appointment slots");
        }

        // New Business Rules Tests

        private static Doctor CreateDoctor(int slots = 2, int maxDaily = 5) =>
            new Doctor("D001", "Dr. Amelia Ratana", slots, maxDaily);
 
        private static Patient CreatePatient(string preferredName = "Aroha") =>
            new Patient("P001", "Christian Cantos", preferredName);

        
        // 1. Booking for today is rejected
        [TestMethod]
        public void BookAppointment_ForToday_IsRejected()
        {
            var doctor = CreateDoctor();
            var patient = CreatePatient();
            var request = new AppointmentRequest(patient, doctor, DateTime.Today);
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            Assert.IsFalse(result.Success);
        }

        // 2. Booking for tomorrow is accepted
        [TestMethod]
        public void BookAppointment_ForTomorrow_IsAccepted()
        {
            var doctor = CreateDoctor();
            var patient = CreatePatient();
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            Assert.IsTrue(result.Success);
        }

        // 3. Booking fails when doctor has no slots
        [TestMethod]
        public void BookAppointment_DoctorHasNoSlots_Fails()
        {
            var doctor = CreateDoctor(slots: 0);
            var patient = CreatePatient();
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            Assert.IsFalse(result.Success);
        }

        // 4. Booking message includes the doctor's name
        [TestMethod]
        public void BookAppointment_Message_IncludesDoctorsName()
        {
            var doctor = CreateDoctor(slots: 0); // failure path — no-slots message names the doctor
            var patient = CreatePatient();
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            StringAssert.Contains(result.Message, doctor.FullName);
        }
 
        // 5. Booking message includes the patient's display name
        [TestMethod]
        public void BookAppointment_Message_IncludesPatientsDisplayName()
        {
            var doctor = CreateDoctor(); // success path — booked message names the patient
            var patient = CreatePatient(preferredName: "Aroha");
            var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
            var service = new AppointmentBookingService();
 
            BookingResult result = service.BookAppointment(request);
 
            StringAssert.Contains(result.Message, patient.DisplayName);
        }
 
        // 6. Invalid patient details are rejected
        [TestMethod]
        public void Patient_WithBlankId_ThrowsArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
                new Patient("", "Christian Cantos"));
        }
 
        [TestMethod]
        public void Patient_WithBlankLegalName_ThrowsArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
                new Patient("P001", ""));
        }
 
        // 7. Slot count remains unchanged when booking fails
        [TestMethod]
        public void BookAppointment_WhenBookingFails_SlotCountRemainsUnchanged()
        {
            var doctor = CreateDoctor(slots: 3);
            var patient = CreatePatient();
 
            // Deliberately trigger a failure via the notice-period rule
            // (booking for today), not a lack of slots — isolates that the
            // slot count guard applies even when the failure reason is
            // unrelated to availability.
            var request = new AppointmentRequest(patient, doctor, DateTime.Today);
            var service = new AppointmentBookingService();
 
            service.BookAppointment(request);
 
            Assert.AreEqual(3, doctor.AvailableSlots);
        }
        
    }
}