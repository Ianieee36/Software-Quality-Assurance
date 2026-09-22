# Quality evidence note

## Current observations

Summarise the valid-request success rate, P50 and P95 latency, normal-hour success and peak-hour success. State the scope and dataset.

## Comparisons and interpretation

- What does the release trend reveal that the latest snapshot does not?
- What does segmentation reveal that the aggregate result conceals?
- Why could average latency appear acceptable while some users still experience long waits?
- How does the qualitative feedback complement the numerical evidence?

## Decision-oriented indicators

List four indicators you would retain and explain the decision supported by each.

## Action, inference and unknown

- **Evidence-supported action:**
- **Reasonable inference:**
- **Important unknown:**

## Limitations

Explain the limitations of the synthetic data, the metric definitions and the feedback sample. Do not claim that a metric identifies a cause unless there is separate causal evidence.

## Vanity metric

Identify one attractive but weak metric for this project and explain why it could mislead.

Activities (Lab Work)

## Activity 1 

- Which values are raw observations or measures?

After inspection of the LabData, I identify what are raw observations and what are measures.
For booking-events.csv the only thing that can be measure is the latencyMS which produces how numerical values on how 
fast a booking request event go through and rest are only observations as values can be categorized into classes 
including isValidRequest, outcome, timeBand, and channel while the others such as timestampUtc, release, and requestId
can be treated as identifiers.

- Which values are targets rather than observations?

Based on the quality-targets.json where there are guardrails or thresholds on how a system should perform if it performs
worse than any of the set standard numbers means that system is unhealthy and breaches its own contracts. 
For minimumValidSuccessRatePercent, maximumP95LatencyMs, minimumPeakHourSuccessRatePercent. The values that are targets rather than observations are that for minimumValidSuccessRatePercent atleast 90% of all request must succeed overall. maximumP95LatencyMS means that 95th percentile latency should not go over 1500ms or 1.5seconds which means that system response time should not go over 1.5 seconds if the response time go over the set threshold it fails its own requirement, and last is the minimumPeakHourSuccessRatePercent which has a 80 percent success rate threshold in the busiest hours. This are the target values rather than observations because
there is a threshold which is a basic metric and a target on how well the system should perform.

- Is a request rejected because mandatory input is missing a system failure? Explain.

mandatory input which is missing is not a system failure as this as more a user-error or a system's validation failure
which is a preventive action of the system from ay security breach or any bad data entering the database. A system failure is
that if the system got shutdown or crashes or if the system encounters a unexpected bug which results to internal server error 

- Why is the phrase booking success rate incomplete?

The phrase booking success rate is incomplete because it does not have any measurable rate whether it is 50%, 60%, or 90%.
The phrase is unambiguous as why it is treated as an incompleted phrase.

- What important context would be lost if all releases and time bands were combined?

If all releases and time bands are combined the important context that might get lost is that
it can almost impossible to identify what releases performs well and does not performs well
during peak and normal hours. Release acts an identifier which represents the version of the system
if it still the same version of the system or a new improved one while timeband is an observation where 
we can identify on what time the system performs well or does it performs well during peak hours and where
we can use both of these features to create conclusions on how the system can improve in terms of this results.

## Activity 3

- Why would returning 0% when there are no valid requests be misleading?

This can be misleading as returning 0% implies that the system received a requests but failed to process a single one succesfully which would create a false alarm for engineers.

- What different question would successful requests divided by all requests answer? 

"What percentage of overall incoming traffic results in a complete, successful business transaction"

## Activity 4

- What does the trend reveal that the latest snapshot does not?

Base on my observations, the latest snapshot which is a success valid request is on 800ms but if we look on the actual trend from where the first successful valid request start from 350ms which says alot that slowly the system is degrading as time goes by.

- What does segmentation reveal that the overall result conceals?

Segmentation reveals that risks or failures are concentrated to a specific context like high traffic periods.

- Why could average latency appear acceptable while some users still experience long waits?

Because an average mathematicall smooth out extreme values. Like if 90% of users experience a smooth
50ms response time while 10% of users experience a severe lag of 5000ms, the average will look perfectly healthy while a signifant portion of your users are facing severe lag.

- How does qualitative feedback complement quantitative evidence?

Quantitative data tells us what is happening while qualitative feed back provides text and "Why" behind the numbers, which helps engineers understand the real-world impact and user frustation.

- Which four signals would you retain on a small decision-oriented dashboard, and why?

  1. Valid Success Rate: To ensure core server stability and monitor code health.
  2. P95 Latency: To guard user experience against worst-case slowness.
  3. Time-band Segment: To instantly distinguish between normal operation and capacity constraints under heavy load.
  4. User Feedback: To catch usability friction or edge cases that telemtry numbers fail to capture.

- Identify one vanity metric for this project and explain why it is weak.

  Vanity Metric: Total numbers of Requests processed
  
  Why it is weak: A high volume of requests looks impressive on a chart, but it is weak because it does not measure quality or stability.

- State one action supported by the evidence, one inference and one important unknown.

  Action: Trigger auto-scaling rules or optimize database queries specifically during the designated Peak time-band windows.

  Inference: The server infrastructure is likely running out of memory or connection pool capacity under heavy concurrent load, causing response times to scale up systematically.

  Important unknown: The exact root cause of the SystemFailures.
