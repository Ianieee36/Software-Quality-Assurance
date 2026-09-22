# ENSE707 Week 7 starter - quality metrics and evidence

This project extends the Week 6 Medical Appointment Booking System without changing its behaviour. It adds a metrics reporter, synthetic measurement data, metric and loader tests, and student evidence templates.

The data is fictional and contains no real patient information.

## Requirements

- .NET 10 SDK
- Visual Studio 2022 with .NET 10 support, or Visual Studio Code with C# support

## Project structure

```text
AppointmentBooking_Week7_Starter/
|-- AppointmentBooking/                 Week 6 domain library
|-- AppointmentBooking.Tests/           Week 6 MSTest suite
|-- AppointmentBooking.Metrics/         Metrics console application
|-- AppointmentBooking.Metrics.Tests/   Loader and metric tests
|-- LabData/                             Synthetic observations and targets
|-- StudentWork/                         Documents students complete
`-- AppointmentBooking.slnx
```

## Baseline check

Run these commands from the extracted project root:

```bash
dotnet restore AppointmentBooking.slnx
dotnet test AppointmentBooking.slnx
dotnet run --project AppointmentBooking.Metrics -- \
  --events LabData/booking-events.csv \
  --targets LabData/quality-targets.json \
  --output artifacts/quality-summary.md
```

Before Activity 3 is completed:

- the Week 6 domain tests and the new loader tests should pass;
- three metric-calculation tests should be reported as inconclusive/skipped;
- the reporter should print the raw counts and create a summary whose calculated values say `not calculated`.

This is intentional. The starter is executable and does not invent plausible values for unfinished metrics.

## Activity 3 completion point

Replace the three placeholder methods in `AppointmentBooking.Metrics/MetricsCalculator.cs` with the implementations in the lab guide. Then rerun the baseline commands.

The expected current-release values are:

| Indicator | Expected value |
|---|---:|
| Valid-request success rate | 90% |
| P50 latency using nearest rank | 700 ms |
| P95 latency using nearest rank | 1450 ms |
| Normal-hour valid success | 100% |
| Peak-hour valid success | 80% |

The current release meets the three supplied targets. However, the release-history data shows a deteriorating trend, so students should interpret the values rather than treating a threshold result as a complete quality conclusion.

## Files students complete

- `StudentWork/MetricDefinitions.md`: goal-question-metric definitions, scope, formulas, comparisons and limitations.
- `StudentWork/QualityEvidenceNote.md`: trend and segment interpretation, qualitative evidence, actions and uncertainties.
- `AppointmentBooking.Metrics/MetricsCalculator.cs`: the three calculation methods supplied in Activity 3.

Generated `bin`, `obj`, `TestResults` and `artifacts` folders should not be submitted.
