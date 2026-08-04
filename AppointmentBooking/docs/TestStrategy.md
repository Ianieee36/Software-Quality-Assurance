# Test Strategy

* Purpose
  - This document defines the approach for testing the AppointmentBookingSystem, a C# class library that models appointment booking patients and doctors   for a community clinic. The purpose of testing is to verify that the system correctly enforces its business rules, behaves reliably under normal and edge-case conditions, and produces clear, actional feedback to the staff and patients who rely on it.

* Scope of Testing
  
  The following components and behaviours are within scope:
   
  - Patient - construction validation such as ID, legal name, DisplayName fallback logic
  - Doctor - construction validation, overall slot availability,, per-date daily appointment cap, slot reservation, and thread-safety of slot reservation
  - AppointmentRequest - construction validation, including rejection of past dates
  - ClinicPolicy - configurable minimum notice period
  - AppointmentBookingService - orchestration of all booking rules: null request handling, patient ID validation, notice-period enforcement, availability checking and successful booking.
  - BookingResult - correctness and clarity of both success and failure messages

* Out of Scope

  - User interface - no UI currently exists for this system; it is a backend domain library only
  - Accessibility testing - not applicable until a UI is built on top of this library
  - Integration with external systems - these do not currently exist in the codebase; only proposed as future design directions
  - Load/performance testing under high concurrent user volume - the current concurrency testing is limited to verifying correctness of the Doctor locking mechanism, not throughput or scalability under production-level load.
  - Security penetration testing - no authentication/authorization layer currently exists to test; this has been identified as a known gap, not a tested feature.
  - Localication/multilingual message testing - message content is currently English-only; no localization mechanism exists yet to test against.

* Test Levels
  
  - Unit Testing - the primary level of testing for this system. Each class is tested in isolation using MSTest, with no external dependencies.
  - Integration testing - not yet formally separated from unit testing in this codebase, since there are no external systems (database, SMS provider, patient records API) to integrate with. If those integrations are added in future, integration tests would be introduced as a distinct level, using fake/mock implementations of any new interfaces to keep unit tests fast and isolated.
  - System Testing - not currently formalised; would apply once a UI or API layer is built on top of this domain library, to verify end-to-end booking workflows from a user's perspective.
 
* Test Types
 
  - Functional Testing - verifying each business rule behaves correctly such as overdraft-stlye overbooking prevention, daily cap enforcement, notice period enforcement
  - Boundary Value Testing - testing values at, just above, and just below defined limits such as booking exactly on the notice-period boundary, exactly at a doctor's daily appointment cap, exactly zero available slots
  - Negative/Invalid Input Testing - null requests, blank patient IDs, past dates, negative or zero values passed to constructors.
  - Regression Testing - re-running the full test suite after any code change to confirm existing behaviour was not broken.
  - Concurrency Testing - verifying Doctor.ReserveSlot() does not oversell slots under simulated concurrent access, exposing and confirming the fix for the check-then-act race condition.
  - Usability Testing - verifying that BookingResult messages are non-empty, mention the relevant doctor/patient by name where appropriate, and avoid leaking technical/exception details to the end user.

* Test Environment
  
  - Language/Framework: C# targeting .NET 10.0
  - Test Framework: MSTest
  - Execution: Local development machine (macOS) via dotnet test from the command line, and via VS Code Test Explorer with the C# Dev Kit extension
  - Data: All test data is created in-memory within each test.

* Tools

  - MSTest
  - dotnet CLI
  - VS Code + C# Dev Kit
  - Git/GitHub

* Defect Management Approach

  - Defects are identified primarily through falling unit tests written before or immediately after a bug is discovered.
  - Each defect is traced back to a specific failing test case with a clear, descriptive tes name.
  - Fixes are verified by re-running the full test suite, not just the single test that originall failed, to confirm no other behaviour was affected.


* Entry Criteria
  
  - Requirements/business rules for the feature under test are documented and unambiguous such as the four extended rules: notice period, daily cap, patient ID validation, message clarity.
  - The relevant classes compile successfully and the test project correctly references the main project.
  - Test project dependencies are restored and compatible with the project's target framework.

* Exit Criteria

  - All unit tests in the relevant test class passed.
  - Boundary values and invalid inputs for each business rule have at least one corresponding test case
  - No known defect remains without either a passing regression test confirming the fix, or an explicitly documented reason it is out of scope.
  - Test names and assertions clearly map back to the requirement they verify, so traceability from requirment to test is possible without additional documentation.

* Risks and Mitigation

| Risk | Impact | Mitigation |
| :--- | :--- | :--- |
| Race condition in Doctor.ReserveSlot() under concurrent bookings | Slots could be oversold, leading to double-booked appointments | Addressed via a lock around the check-then-act sequence; covered by a dedicated concurrency test. Take note that reservation slots are scale dependent so if Doctor objects are ever loaded fresh per request from a database it would need revisiting |
| DateTime.Today depends on server/local time zone | Notice-period and past-date validation could behave inconsistently across regions/deployments | Currently unmitigated flagged as a Portability gap recommended fix is an injectable clock/time provider abstraction, not yet implemented |
| No authentication/authorization | Any caller can book, and potentially cancel, appointments for any patient with any doctor | Currently unmitigitated explicitly out of scope for this lab exercise. but documented as a real gap that would need addressing before production use |
| Ambiguous or changing business rules | Tests may encode an incorrect interpretations of a requirement, passing while not matching actual business intent | Mitigate by treating requirement ambiguity as something to clarify before writing tests, and by explicitly documenting assumptions made |
| Hardcoded values reintroduced elsewhere in future changes | Business rules becomes difficult to change or test without code edits | Mitigated by the precedent set with ClinicPolicy. configuration extracted as an injectable object rather than embedded as a literal value; recommended as the pattern for any future any future configurable rule |
| Test suite become slow or flaky if real external services are introduced later | Developers may begin ignoring failing tests, reducing the suite's value | Mitigated proactively by planning interface-based seams like INotificationService, IPatientRepository this will be for any future integration, allowing fake/mock implementation to keep unit tests fast and deterministic | 