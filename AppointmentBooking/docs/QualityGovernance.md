# Process Assurance vs Product Assurance

| Area | Process Assurance | Product Assurance |
| :--- | :--- | ---: |
| Main Focus | How the work is performed | Quality of the software product |
| Example in this project | Requirements review, coding, standards, Git commits, test process | Validation logic, working booking feature, passing tests |
| Evidence | Review checklist, commits, test plan, CI results | Test results, defect reports, working prototype |
| Goal | Prevent quality problems | Detect and confirm product quality |

Both are important and needed in software process because Process Assurance examines whether appropriate methods, controls, responsibilities, reviews, and improvement mechanisms exist and operatet effectively while Product Assurance evaluates whether outputs satisfy requirements, quality objectives, user needs, and applicable constraints. A strong process increases the probability of a good product.

## Quality Governance Rules

| Governance Area | Rule | Evidence |
| :--- | :--- | :--- |
| Requirements | Each new feature must have at least one requirement ID | Requirements list |
| Testing | Each requirement must have at least one test case | Traceability matrix |
| Code quality | Code must pass all unit test before commit | Test results |
| Github | Each student must commit meaningful work regularly | Git history |
| AI use | Copilot suggestions must be reviewed and tested | AI reflection notes |
| Defects | Defects mus be recorded with status and severity | Defect log |
| Release | A feature can only be releasesd if exit criteria are met | Test summary report |

- These rules support quality governance as this makes a standard on how to develop quality software. These standards prevents product from any damage during software processes or activity. It make sures that developers is following these standards during development processes.


## Defect Log 

| Defect ID | Description | Severity | Status | Found In | Fixed In |
| :--- | :--- | :--- | :--- | :--- | :--- |
| DEF-001 | The appointment date that has been cancelled does not go back to being available | High | Fixed | Cancellation test | Updated CancelAppointment method 
| DEF-002 | The slot count did not increase after cancellation. | High | Fixed | Cancellation test | Updated CancelAppointment method |
| DEF-003 | A null appointment does not return a clear and actionable exception message | Mid | Fixed | Update CancelAppointment method | 