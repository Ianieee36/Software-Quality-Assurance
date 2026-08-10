# Requirements Traceability Matrix 

| Requirement ID | Requirement Summary | Acceptance Criteria | Test Case | Status |
| :--- | :--- | :--- | :--- | :--- |
| REQ-LIB-01 | Reserve only available book | AC-01 | ReserveBook_BookIsAvailableAndValidMember_ReservationSucceeds | Passed |
| REQ-LIB-02 | Reserve empty member ID | AC-02 | ReserveBook_EmptyMemberId_ThrowsException | Passed | 
| REQ-LIB-03 | Reject already reserved book | AC-03 | ReserveBook_BookAlreadyReserved_ReservationFails | Passed |
| REQ-LIB-04 | Reserve only available book | AC-04 | ReserveBook_NullBook_ReservationFailsWithClearMessage | Passed |

- Traceability helps the team check whether each requirement has test evidence. It also supports change management
  because if a requirement changes, the related test cases can be identified, reviewed, and updated.


## Managing Requirement Change

Adds a new library rule:
REQ-LIB-05: A member cannot reserve more than one book at the same time.

**Which class may need to change?**

  - If this new rule gets implemented that possible class that might need to change is Member and ReservationResult.
    though it really depends on how will I design it if i want to make reservation validation I can create a new class for that
    or if I want to stick on my existing classes is that Member needs to be modified or add a method which can check whether it has existing reservation or not and add a fail result in ReservationResult.

**Which test cases need to be added?**

  - If this rule gets added test cases like ReserveBook_MemberCannotReserveMoreThanOneBook_ReservationFails or something can check 
    if a member tries to book more than one book it receives a clear fail message like ReserveBook_MemberCannotReserveMoreThanOneBook_ReservationFailsReturnClearMessage.

**What should be added to the RTM**

  - Maybe the two test cases I mentioned above can be added in the RTM with an acceptance criteria of AC-05 Member reserve another book after making a reservation with another book, reservation must be rejected. 