# Continuous Improvement

## What Worked Well

- **Writing tests directly against stated requirements, before trusting that
  the code already satisfied them.** This pattern caught real defects
  consistently throughout the project — from the original `Withdraw`
  overdraft bug, to `Appointment.Id` never being assigned despite passing
  validation, to `BookingResult.Booked` silently dropping the `Appointment`
  argument. In every case, the bug was invisible until a test explicitly
  asserted on the specific value in question.
- **Boundary value analysis** (testing exactly at, just above, and just below
  a limit) reliably surfaced edge-case bugs that a single "happy path" test
  would have missed — e.g. the zero-vs-negative overlap bug in the first
  `Deposit` attempt, and the exact-slot-count boundary in `Withdraw` and
  `Doctor.ReserveSlot`.
- **Treating business-rule failures as expected outcomes (`BookingResult`)
  rather than exceptions**, reserved for cases where that distinction
  genuinely mattered (e.g. insufficient funds, no available slots), made the
  resulting code easier to test and kept failure handling consistent across
  most of the system.
- **Verifying a failing test's expectation before "fixing" the code.** The
  `CalculateTransactionFee_LargeAmount` test failure turned out to be a wrong
  expected value in the test itself, not a bug in production code — checking
  the maths first avoided introducing a real defect just to satisfy an
  incorrect test.
- **Re-running the full regression suite after every fix**, not just the
  single test that originally failed, which caught no unintended
  side-effects across the changes made in this project.

## Root Cause of One Issue

**Issue:** `BookingResult.Booked(appointment)` returned a successful booking
result with `Appointment` always set to `null`, even though a valid
`Appointment` object was passed into the method.

**Root cause analysis:**

- **What happened:** The factory method called
  `new BookingResult(true, message)` — only two of the constructor's three
  parameters were supplied. The third parameter, `appointment`, defaulted to
  `null` because it was never forwarded from the method's own `appointment`
  parameter into the constructor call.
- **Why it happened:** `BookingResult` originally only had two pieces of
  data (`Success`, `Message`). When `Appointment` was added as a third
  property to support the cancellation feature, the constructor was updated,
  but the *existing* `Booked` factory method was not revisited to pass the
  new parameter through — the method still "worked" in the sense that it
  compiled and returned a result with a correct message, so nothing flagged
  it as incomplete.
- **Why it wasn't caught immediately:** No existing test asserted on
  `result.Appointment` at the time this change was made, because no
  requirement had needed that property until the cancellation feature
  introduced a need to retrieve the appointment's ID afterward. The defect
  was only exposed once a new test
  (`BookAppointment_Success_ReturnsAppointmentWithCorrectDetails`) was
  written specifically to check it.
- **Underlying pattern:** This is a case of a **partial refactor** — a data
  model was extended (`BookingResult` gained a new field), but not every
  place that constructs that model was updated to populate the new field.
  This is the same class of risk as the earlier `Appointment.Id` defect:
  validation or construction logic added correctly in one place, without a
  corresponding assignment or pass-through in another.

## Improvement Action

Adopt a rule for this project going forward: **whenever a class gains a new
field or property, search for every existing factory method / constructor
call site for that class before considering the change complete**, rather
than only updating the call site that immediately motivated the change. In
practice, this means treating "add a field to `BookingResult`" and "add a
field to `Appointment`" as two-part tasks — add the field, *then*
deliberately check the class for every existing way an instance already gets
created and confirm each one still makes sense.

As a concrete step, before adding a new field to any shared class in this
codebase, use a project-wide search (e.g. VS Code's "Find All References" on
the class name) to list every construction site, and briefly check each one
against the new field before moving on.

## How We Will Check the Improvement

- **A regression test for the specific defect class**: keep
  `BookAppointment_Success_ReturnsAppointmentWithCorrectDetails` (and
  similar "does the returned object actually carry the data it claims to"
  tests) permanently in the suite, so any future refactor that reintroduces
  this pattern is caught immediately rather than discovered later.
- **A short manual check before marking any "add a field" change as done**:
  confirm every constructor/factory call site for the changed class was
  reviewed, not just the one that prompted the change — this can be verified
  informally each time by re-reading the class's factory methods
  side-by-side after any such change.
- **Continued use of full regression runs** (`dotnet test` across the whole
  suite) after every change, rather than only running the test related to
  the immediate feature, since this is what would catch a similar
  partial-refactor defect in an unrelated class going forward.

## Quality Culture Reflection

The recurring pattern across this project — `Withdraw`'s missing overdraft
check, the zero/negative overlap in the first `Deposit` attempt,
`Appointment.Id` never being assigned, `BookingResult.Booked` dropping its
argument, and `Doctor.ReleaseSlot()` initially not restoring the per-date
count — is that **almost none of these defects were caught by the code
failing to compile or crashing at runtime**. Each one produced code that
looked complete and ran without error, and only became visible once a test
was written that checked a specific, previously-unexamined detail. This
reflects a core lesson carried through this whole lab: quality is not
something a codebase announces on its own — it has to be actively verified
by asking "does this actually do what it claims to do," one deliberate,
specific test at a time, rather than trusting that working code and passing
compilation are the same thing as correct code. A team habit of writing that
next specific test — especially right after extending a shared class — is a
cheap, repeatable way to keep this kind of defect from accumulating silently
as a system grows.


## Agile and DevOps Quality Practices for this project

| Practice | How It Could Be Used in This Project | 
| :--- | :--- |
| Sprint Planning | Select a small set of features and quality tasks for the week |
| Daily stand-up | Discuss progress, blockers, and testing issues |
| Definition of Done | Feature is complete only when coded, reviewed, tested, and documented | 
| Continuous Integration | Automatically run tests when code is pushed |
| Regression Testing | Re-run existing tests after each change |
| Retrospective | Review what went well and what should improve | 
