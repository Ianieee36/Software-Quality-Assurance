# Test Plan

## Feature Under Test

  - Appointment Cancellation - allowing reception staff to cancel an existing, previously booked appointment and release the associated doctor's slot back into availability.

## Test Objective

  - To verify that cancelling an appointment correctly reverses the effects of booking it. releasing the doctor's slot while preventing cancellation of appointments that were never booked or do not exist, and that the system communicates the outcome of a cancellation attempt clearly to reception staff.

## Requirements to be tested
  
  - REQ-CAN-01: The system shall allow an existing appointment to be cancelled.
  - REQ-CAN-02: When an appointment is cancelled, the doctor's available slot count shall increase by one.
  - REQ-CAN-03: The system shall not allow cancellation of an appointment that does not exist.
  - REQ-CAN-04: When an appointment is cancelled, the doctor's daily appointment count for that date shall also decrease by one, so the release slot becomes available for booking on that same date again.
  - REQ-CAN-05: The cancellation outcome message shall be clear and state whether the cancellation succeeded or failed, and why.

## Test Items

  - AppointmentBookingService.CancelAppointment() - not yet implemented; this test plan assumes a new method will be added, taking some form of appointment reference.
  - Doctor.ReleaseSlot(DateTime date) - proposed new method, the inverse of ReserveSlot(DateTime date), to be added to Doctor.
  - BookingResult - reused for cancellation outcomes or a new result type such as CancellationResult if cancellation-specific message is needed.

## Test Approach

  - Requirement-driven testing - each test case is written to map directly to one of the requirements listed, so pass/fail results can be traced back to a specific requirement.
  - Unit testing with MSTest, consistent with the existing test suite for AppointmentBookingService, Doctor, Patient, and AppointmentRequest.
  - Boundary and negative testing - cancelling an appointment that exists, cancelling one that doesn't exist (negative case), and cancelling the same appointment twice (boundary case)
  Regression Testing - re-run the full existing booking test suite after the cancellation feature is added, to confirm booking behaviour is unaffected by the change.
  - State-verification testing - after cancellation, directly asser on Doctor.AvailableSlots and the per-date booking count, not just the returned result object, to confirm the slot was genuinely release rather than just reporting success.

## Test Data
  
  | Scenario | Doctor Setup | Patient | Appointment State |
  | :--- | :--- | :--- |
  | Successful Cancellation | 1 available slot remaining after booking, lets say MaxDailyAppointments = 5 | Valid patient (P001) | A real, previously booked appointment for tomorrow |
  | Cancel non-existent appointment | Any valid doctor | Valid patient | An appointment reference/ID that was never booked | 
  | Cancel already-cancelled appointment | Same doctor as successful case | Valid patient | An appointment that was booked, then cancelled once already |
  | Slot count verification | Doctor starts with 3 slots, books 1, then cancels it | Valid Patient | Assert AvailableSlots returns to 3 after cancellation |
  | Daily count verification | Doctor with MaxDailyAppointments = 1, fully booked for a date then cancelled | Valid patient | Assert a second patient can now book the same date after cancellation |

## Responsibilities
  
  Every testing is performed by me:

  - Test design and test case authorship
  - Implementation of the cancellation feature
  - Test execution and defect logging 
  - Review of requirement ambiguity

## Schedule
  
  | Activity | Estimated Effort |
  | :--- | :--- |
  | Confirm/clarify requirements | 0.5 session |
  | Design Cancel Appointment API and appointment-identity approach | 0.5 session |
  | Write failing test cases (red) for REQ-CAN-01 through REQ-CAN-05 | 1 session |
  | Implement CancelAppointment and Doctor.ReleaseSlot | 1 session |
  | Run tests, fix defects until green | 1 session |
  | Full regression run of exising booking test suite | 0.5 session |

## Pass and Fail Criteria
  
  
  **Pass Criteria:**
  - All test cases mapped to REQ-CAN-01, REQ-CAN-02, and REQ-CAN-03 pass
  - If REQ-CAN-04 and REQ-CAN-05 are confirmed as in-scope, their test cases also pass
  - The full existing regression suite continue to pass unchanged
  - Cancelling a non-existent appointment returns a clear failure result rather than throwing an unhandled exception

  **Fail Criteria:**
  - Any REQ-CAN test case fails without an approved, documented reason
  - Cancelling an appointment does not restore the doctor's slot count
  (violates REQ-CAN-02)
  - Cancelling a non-existent appointment either crashes the system or is
  incorrectly reported as successful
  - Existing booking tests break as a result of changes made to support
  cancellation
 
## Risks

  | Risk | Impact | Mitigation |
  |---|---|---|
  | The system currently has no concept of an individual, identifiable appointment record — only aggregate slot counts | `CancelAppointment` cannot know *which* booking to reverse without this being designed first | Must be resolved before test cases can be finalized; likely requires introducing an appointment identifier (e.g. a returned `AppointmentId` from `BookAppointment`) as a prerequisite change, not just an addition |
  | Cancelling and booking could both mutate `Doctor`'s slot state concurrently | Same class of race condition previously found in `ReserveSlot()` could reappear in a new `ReleaseSlot()` method if not guarded consistently | Reuse the existing `_slotLock` pattern already in `Doctor` for any new slot-mutating method, and include a concurrency test similar to the existing one for `ReserveSlot()` |
  | REQ-CAN-04 and REQ-CAN-05 are proposed, not confirmed by the clinic/course brief | Building and testing against assumed requirements that turn out to be wrong wastes effort and may not match what's actually expected | Confirm these with the lab brief or instructor before treating them as binding; test cases for them should be clearly labelled as based on a proposed extension, not the original requirement list |
  | Double-cancellation (cancelling the same appointment twice) is not explicitly covered by REQ-CAN-01–03 | Ambiguous whether a second cancellation attempt should fail gracefully or be a no-op | Treated in this plan as equivalent to REQ-CAN-03 (appointment no longer exists once cancelled), but this interpretation should be confirmed rather than assumed |