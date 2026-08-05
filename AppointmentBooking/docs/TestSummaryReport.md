# Test Summary Report

## Summary

   - This report summarizes the testing carried out for the new appointment cancellation feature, which allows reception staff to cancel an existing appointment and release the associated doctor's slot. Testing covered the Appointment class, the CancelAppointment method added to AppointmentBookingService, and the BookAppointment success path's ability to return a usable Appointment reference for later cancellation. Five test cases were written directly against the requirements defined in the Test Plan. Testing surfaced three real defects in the implementation, all of were identified, explained, and corrected before the full suite passed.

## Features Tested
   
   - Marking an existing appointment as cancelled (CancelAppointment_ExistingAppointment_MarksAppointmentAsCancelled)
   - Releasing a doctor's slot when an appointment is cancelled (CancelAppointment_ExistingAppointment_ReleasesDoctorSlot)
   - Rejecting cancellation of a null appointment reference (CancelAppointment_NullAppointment_ThrowsException)
   - Rejeecting cancellation of an appointment that has already been cancelled (CancelAppointment_AlreadyCancelledAppointment_ThrowsException)
   - Correctness of the Appointment object returned by a successful booking (BookAppointment_Success_Returns AppointmentWithCorrectDetails), including its Id, Doctor, Patient, AppointmentDate, and IsCancelled state

## Features Not Tested

   - Concurrent cancellation.
   - Cancellation of an appointment tied to a doctor with AvailableSlots already at its maximum.

## Test Environment

   - Language/Framework: C# targeting .NET 10.0
   - Test Framework: MSTest 
   - Execution: dotnet test via command line on macOS using VS Code Test Explorer.
   - Test date: All test data constructed in-memory per test.

## Test Results

   | Test Case | Initial Result | Final Result
   | :--- | :--- | :--- |
   | CancelAppointment_ExistingAppointment_MarksAppointmentAsCancelled | Pass | Pass |
   | CancelAppointment_ExistingAppointment_ReleasesDoctorSlot | Fail | Pass |
   | CancelAppointment_NullAppointment_ThrowsException | Pass | Pass |
   | CancelAppointment_AlreadyCancelledAppointment_ThrowsException | Pass | Pass |
   | BookAppointment_Success_ReturnsAppointmentWithCorrectDetails | Fail | Pass |
   | CancelAppointment_ExistingAppointment_RestoresDailyAppointmentAvailability | Fail | Pass |
   | CancelAppointment_OneOfTwoBookingOnSameDate_OnlyReleaseOneSlot | Pass | Pass |
   | CancelAppointment_NullAppointment_ExceptionMessageIsClearAndActionable | Fail | Pass |
   | CancelAppointment_AlreadyCancelledAppointment_ExceptionMessageIsClearAndActionable | Pass | Pass |


## Defects Found

   - `BookingResult.Booked(appointment)` did not attach the `Appointment` to the result. The factory method called `new BookingResult(true, message)` without passing `appointment` as the third constructor argument, so it silently defaulted to `null`. Every successful booking returned a `BookingResult` with `Appointment == null`, making the appointment unrecoverable for later cancellation.
   - `Appointment`'s constructor validated `id` but never assigned it. The constructor checked `id` for blank/whitespace and threw correctly for invalid input, but never executed `Id = id;`. Every constructed `Appointment` therefore had `Id == null`, even though a valid ID string had been passed in and validated.
   - `CancelAppointment` dit not release the doctor's slot. The method called `appointment.Cancel()`, which correctly marks the appointment itself as cancelled, but never called `appointment.Doctor.ReleaseSlot()`. As a result, cancelling an appointment marked it cancelled without actually returning the slot to the doctor's availability - directly violating REQ-CAN-02.
   - `Doctor.ReleaseSlot()` did not restore per-date daily availability. Even once wired up, the original `ReleaseSlot()` only incremented `AvailableSlots`, with no `date` parameter and nod adjustment to `_dailyBookingCounts`. A cancelled appointment on a date that had reached `MaxDailyAppointments` would not reopen that date for new bookings, only increasing overall capacity elsewhere, violating the intent of REQ-CAN-04.
   - `CancelAppointment`'s null check used a generic, non-actionable exception messsage. The original `ArgumentNullException(nameof(appointment))` call relied on .NET's default message text, which identifies the failing parameter but gives no guidance on what the caller should do. failling REQ-CAN-05's requirement that messages be clear and actionable.

## Defects Fixed
 
   1. `BookingResult.Booked` updated to pass `appointment` as the third
   constructor argument, so `result.Appointment` is correctly populated on
   success.
   2. `Appointment`'s constructor updated to assign `Id = id;` after validation,
   so the appointment's ID is actually retained.
   3. `CancelAppointment` updated to call `appointment.Doctor.ReleaseSlot()` in
   addition to `appointment.Cancel()`, so cancelling an appointment correctly
   restores the doctor's available slot count.
   4. `Doctor.ReleaseSlot()` changed to accept a `DateTime date` parameter and
   decrement `_dailyBookingCounts` for that date (guarded to never go below
   zero), in addition to incrementing `AvailableSlots`. This is a breaking
   signature change to `Doctor`'s public API, consistent with earlier
   signature changes made in this project when a design gap was found.
   `CancelAppointment` was updated to pass `appointment.AppointmentDate`
   through to it.
   5. `CancelAppointment`'s null-check updated to use the
   `ArgumentNullException(string paramName, string message)` overload with an
   explicit, actionable message, rather than relying on the framework
   default. `Appointment.Cancel()`'s already-cancelled message was also
   reworded slightly for clarity. Note: this was a deliberately narrow fix —
   `CancelAppointment` still communicates outcomes via thrown exceptions
   rather than being redesigned to return a `BookingResult`-style object, in
   order to avoid breaking the already-passing exception-based tests. Whether
   to pursue that larger redesign remains an open decision (see Known Issues).

## Known Issues

   - `CancelAppointment` communicates failure only via thrown exceptions(`ArgumentNullException`,`InvalidOperationException`), which is inconsistent with `BookAppointment`'s pattern of returning a `BookingResult` for expected business-rule outcomes. REQ-CAN-05 is currently satisfied by making those exception messages clear and actionable, rather than by resolving this deeper desgin inconsistency. Whether "cancelling an already-cancelled appointment" should instead be treated as an expected business outcome, and returned as a result object has not been formally decided.
   - No mechanism currently exists for `CancelAppointment` to reject an appointment that is well-formed but was never actually returned by `BookAppointment` - REQ-CAN-03 is only partially addressed by rejeecting `null` and already-cancelled appointments, not appointments that were never legitimately booked in the first place.

## Release Recommendation

   **Not yet recommended for release** in its current state. REQ-CAN-01 through REQ-CAN-05 are now all covered by passing tests, including the previously untested per-date restoration and message clarity behavior. However, REQ-CAN-03 is still not fully verifiable. Nothing currently prevents `CancelAppointment` from accepting a manually constructed `Appointment` that was never actually returned by `BookAppointment`, so "the system shall not allow cancellation of an appointment that does not exist" is only partially demonstrated. Recommended resolving this gap, and making an explicit decision on the exception-vs-result design question noted above, before considering this feature complete.


## Lessons Learned

   - **Constructor validation and constructor assignment are two different things, and it's east to write one without the other.** `Appointment.Id` defect is a clear example: the validation logic. was correct and would have passed a code review focused only on "does this reject bad input". The missing assignment only became visible once a test explicitly asserted on the property's value.
   - **A defect can hide behind a passing constructor call.** Both the missing `Id` assignment and the missing `appointment` argument in `BookingResult.Booked` are silent failures. No exeception was thrown, no compiler error occured, and the code "worked" in the sense that it ran without crashing. This reinforces a point raised earlier in this project: passing tests is not the same as correct behaviour, and only a test that asserts on the specific value in question can catch this class of bug.
   - **New features exposed a design inconsistency that pre-existed unnoticed.** `BookAppointment` returns a `BookingResult` specifically so callers don't need `try/catch` for expected outcomes; `CancelAppointment` throws exceptions instead. This inconsistency was tolerable while cancellation logic was simple, but became a real open question once REQ-CAN-05 (clear, actionable messaging) was considered — a good example of how a requirement can retroactively reveal that an earlier, seemingly-independent design decision needs revisiting.
   - **Writing the test before confirming the code was correct was more useful than writing the code and assuming it worked.** All three defects were found because tests were written directly against the stated requirements, not against what the code happened to already do — reinforcing the same TDD-style workflow used throughout this project (`Withdraw`'s overdraft bug being the first example of this pattern, `Appointment.Id` being the most recent).

| Test Area | Number of Tests | Passed | Failed | Notes |
| :--- | ---: | ---: | ---: | :--- |
| Booking Tests | 20 | 20 | 0 | Existing tests are tested carefully and some test failed until it was fixed that lead the test to passed |
| Cancellation Tests | 9 | 9 | 0 | Existing tests are tested carefully and some test failed until it was fixed that lead the test to passed |