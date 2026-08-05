# Report Lab 3 

## Reflection between Week 2 and Week 3

Working through the BankAccount and AppointmentBooking systems this fortnight reinforced something I hadn't fully appreciated before: most of the defects I found weren't caught by the code failing to compile or crashing — they were caught by deliberately writing a test that checked one specific thing. The original Withdraw overdraft bug, Appointment.Id never being assigned despite passing validation, and BookingResult.Booked silently dropping its Appointment argument all "worked" in the sense that they ran without error. Early tests, written directly against stated requirements rather than against what the code happened to already do, were the only thing that surfaced them. This changed how I think about code review too — reviewing Doctor.ReserveSlot() for the race condition before it caused a real overbooking incident was far cheaper than finding it after the fact.

Regular, small commits made this process much easier to follow than I expected. Each defect I fixed this fortnight was small and isolated — one bug, one fix, one passing test — and being able to see exactly which change caused a test to flip from red to green gave me (and would give a teammate) a clear, honest record of progress, rather than a single large "fixed some bugs" commit that hides what actually changed and why.

Test results also gave me something I didn't have before: evidence, not just confidence. Saying "cancellation now restores the doctor's slot" is a claim; a passing CancelAppointment_ExistingAppointment_ReleasesDoctorSlot test is proof. When I second-guessed whether a failing test meant the production code was wrong (as with the transaction-fee boundary test), being able to manually verify the maths against the assertion, rather than just trusting either side blindly, was what let me make a confident, defensible decision about which side to fix.

Although this lab was largely individual work, the back-and-forth review process I used with Claude to walk through each new feature functioned a lot like the peer review a team would do — flagging assumptions I hadn't stated, catching a message-wording bug I wouldn't have noticed on my own, and pushing back on a design choice (throwing exceptions instead of returning a result) before it spread further through the codebase. It made clear why teamwork matters for quality: a second perspective catches things a single author is too close to the code to see, and shared visibility into test results means quality isn't just one person's memory of "I think this works."

Going into the next lab phase, I want to be more deliberate about two things: writing the test before extending a shared class (like BookingResult or Doctor) rather than after, since several of this fortnight's bugs were partial refactors where a new field wasn't wired through everywhere it needed to be; and doing a full regression run after every change, not just running the one test related to the feature I was working on, since that's what would have caught the Doctor.ReleaseSlot() daily-count gap sooner.

## Step 14: Ask Claude for QA Process Suggestions

  - Prompt Used: "Can you make any suggestion regarding about quality governance  checklist for a small student project?"
  - Useful Suggestion: Every suggestion is useful but the thing for me that is really important is this: 
    **Before each commit**
    * Commit is small and does one identifiable thing (one bug, one rule, one refactor) — not a bundle of unrelated changes
    * Commit message states what changed and why, not just "fixed stuff"
    * Full test suite run locally (dotnet test), not just the test related to the change
  
  - Suggestion I modified: Any of the suggestion I haven't applied in this test project. So there's nothing to modify.
  - Suggestion I rejected: Any of the suggestion I haven't applied in this test project. So there's nothing to reject.
  - Why human judgement was required: It is still important as we as a human we must still have a say or judgement on how AI nowadays suggest not just taking everything, but still work alongside with it brainstorm with it and not just letting them do all the work.