# Requirements Traceability Matrix 

| Acceptance Criterion ID | Acceptance Criterion |
| :--- | :--- |
| AC-01 | Given a patient and medicine is valid, when the patient submit a refill request, then request must succeed. |
| AC-02 | Given an empty patient Id, when a request is attempted, then the request should fail and display clear message |
| AC-03 | Given an empty medicine name, when a request is attempted, then the request should fail and display clear message |
| AC-04 | Given a patient with two or fewer days of medicine, when the patient submit a refill request, then the request must be marked as urgent. |
| AC-05 | Given a patient submit a request, either the request is successful/fails, then it must display a clear and meaninful message |

| Requirement ID | Requirement Summary | Acceptance Criteria | Test Case | Status |
| :--- | :--- | :--- | :--- | :--- |
| REQ-RX-01 | Submit a refill request | AC-01 | SubmitRequest_ValidPatientAndMedicine_ReturnSuccess | Passed |
| REQ-RX-02 | Reject request for empty patient Id | AC-02 | Patient_EmptyId_ThrowsException | Passed | 
| RREQ-RX-03 | Reject request for empty medicine name | AC-03 | SubmitRequest_EmptyMedicineName_ReturnsFailure | Passed |
| REQ-RX-04 | Request marked as urgent | AC-04 | SubmitRequest_TwoOrFewerDaysRemaining_MarkRequestAsUrgent | Passed |
| REQ-RX-05 | Submit request with success/failure clear message | AC-05 | SubmitRequest_ResultMessage_IsClear | Passed |