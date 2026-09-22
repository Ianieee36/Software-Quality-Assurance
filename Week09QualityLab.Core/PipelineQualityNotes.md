# QA in CI/CD Pipeline Activities

## Task 1: Describe the CI Pipeline for this Lab Project

What should happen whenever a developer pushes a code in Github is that it saves the newer version of the codebase whenever there's any changes in the codebase. Restoring dependencies (*dotnet restore*) is essential in CI as libraries or and external packages can't be uploaded in github this action ensures that it always starts and run in a new blank slate so that it compiles properly. Build verification (*dotnet build*) also helps us to identify and catches any compilation errors. Running MSTest Tests ensures that the system behaves as expected and nothing is broken. Collecting test results and reporting helps us to identify what are the test pass, failed , or skipped which also leads us to know if the system is behaving as expected or not.

## Task 2: Identify Pipeline Stages

1. Source Code Checkout:
- The pipeline retrieves the specific commit or branch of the source code in Github and loads it into the clean virtual environment on the build runner. It reduces the risks of version mismatchs or testing the wrong code.

2. Dependency Restore
- The runner downloads all required third-party libraries and NuGet packages specified in the project configuration files. It eliminates the risk of broken external references and environment discrepancies, ensuring the code isn't relying on outdated, mismatched, or locally cached packages that won't exist in production.

3. Build Verification
- The system compiles the C# source code into binaries (DLLs and EXEs) to ensure there are no compilation errors. It mitigates the risk of syntax or structural code defects, catching broken references, typos, or type mismatches early before any time is wasted running heavier downstream tests.

4. Unit Test Execution
- The runner executes the suite of automated MSTest cases against the compiled binaries to validate individual components of code logic. It reduces the risk of functional regressions and logic bugs, preventing developers from accidentally breaking existing features or introducing flawed calculations into the application.

5. Test Result Reporting
- The pipeline aggregates the outputs from the MSTest execution and publishes a highly visible summary directly into the GitHub interface. It mitigates the risk of silent test failures and lack of visibility, ensuring that broken code is never accidentally merged due to overlooked or hidden testing errors.

6. Code Coverage Measurement
- A tool (like Coverlet or Visual Studio Code Coverage) calculates exactly what percentage of your source code statements were actually executed by your unit tests. It minimizes the risk of untested code paths and false confidence, alerting the team if new features were written but completely skipped by the automated test suite.

7. Static Code Analysis
- Automated linters and analyzers (such as Roslyn Analyzers or SonarQube) inspect the raw source code without running it to look for anti-patterns, code smells, or style violations. It mitigates the risk of poor maintainability and technical debt, ensuring the codebase remains clean, readable, and compliant with team engineering standards over time.

8. Dependency Vulnerability Scanning
- The pipeline scans the restored NuGet packages against a database of known security flaws. It reduces the risk of supply chain security breaches, blocking developers from shipping applications that rely on third-party libraries with known, exploitable security holes.

9. Artefact Creation
- The pipeline packages the compiled, verified binaries and assets into a single deployable format, such as a zip file or a NuGet package. It eliminates the risk of configuration drift and packaging errors, guaranteeing that the exact code that was just tested is what gets zipped up for distribution, rather than a raw, manual file export.

10. Optional Deployment to a Test Environment
- The pipeline automatically deploys the created artefact to a non-production hosting environment (like an Azure App Service slot or a test server) for manual or integration testing. It reduces the risk of deployment-day surprises and environmental failures, confirming that the application actually installs, starts up, and interacts correctly with real infrastructure.