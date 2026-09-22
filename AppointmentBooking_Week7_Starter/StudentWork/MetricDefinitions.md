# Metric definitions

Complete one contract for each required metric. Begin with the quality goal and question, then define the calculation.

## Metric 1 — Valid booking success rate

- **Quality goal:**

Goal: Improves performance during valid booking requests

- **Quality question:**

Question: What is the percentage of successful booking request during normal hours.

- **Metric name and formula:**

Metric Name: Percentage of valid booking request during normal hours

Formula: Valid Booking Success Rate = Numerator/Denominator * 100

- **Numerator:**

Total number of sucessful booking requests

- **Denominator:**

Total number of valid booking requests

- **Included observations:**

  1. received booking requests during normal hours
  2. success requests during normal hours
  3. requests that resulted to unexpected system failures.

- **Excluded observations:**

  1. Request rejected due to users error 
  2. tracking requests during peak hours

- **Release, environment and time scope:**

Release: 2.3.0
Environment: Testing environment
Time Scope: During normal hours everyday

- **Segment or breakdown:**

Time of the day
Client platform
Booking type

- **Baseline:**

There is a 90% baseline success rate based on the records

- **Target or warning threshold:**

Target: >= 95%
Warning threshold: < 90%

- **Decision or action supported:**

If metric drops below 90% there should an immediate engineering investigation.

- **Limitation or possible misuse:**

This metric can mask system degradation such as if peak-hour traffic is mistakenly identified as normal hours and also in client side, if the frontend fails to validate input, or catch user errors it can result to bad data entering server.

## Metric 2 — P95 booking latency

- **Quality goal:**

Goal: Improve response time on each requests

- **Quality question:**

Question: What is the maximum response time experienced by of valid booking requests during 
both normal and peak hours

- **Metric name and formula:**

Metric name: 95th percentile (P95) Booking Latency (Normal vs Peak Hours)

Formula 1: P95(Response Time of all valid requests during normal hours)

Formula 2: P95(Response Time of all valid requests during peak hours)

- **Population and unit:**

Population: All individual with valid HTTP transaction response times for the booking endpoint.

Unit: milliseconds (ms)

- **Percentile convention:**

P95 percentile. this means that 95% of requests are faster than or equal to this value, and only the slowest 5% of requests exceed it.

- **Included observations:**

  1. Completed, valid booking requests that successfully processed or returned a server error
  2. Separated into two distinct buckets: "Normal operational hours" traffic and "Designated peak-traffic hours" traffic.

- **Excluded observations:**

  1. Client-side validation rejections
  2. Network timeouts/dropped connections
  3. Non-production traffic

- **Release, environment and time scope:**

  Release: 2.3.0
  Environment: Testing environment
  Time Scope: Everyday, during normal hours and peak hours traffic

- **Segment or breakdown:**

  1. Time of the day either normal or peak hours
  2. Infrastructure region 
  3. Platform either web browser or mobile applications

- **Baseline:**

Normal hours: 777.5ms 
Peak Hours: 1642.5ms 

- **Target or warning threshold:**

Breach threshold: >= 1500ms

- **Decision or action supported:**

Result status: Breached (Peak Hours)

Action needed: backend investigation, auto-scaling of backend compute nodes or optimize database indexing.

- **Limitation or possible misuse:**

Calculations using standard linear interpolation on small sample sizes. Can overly smooth the impact of sudden spikes. It should be backed by p99 track to catch absolute worst-case anomalies.

## Metric 3 — Peak-hour system-failure rate

- **Quality goal:**

Improve reliability of the appointment booking even in peak hours

- **Quality question:**

What is the percentage of booking requests failed, during peak hours?   

- **Metric name and formula:**
- **Numerator:**
- **Denominator:**
- **Meaning of “peak hour” and “system failure”:**
- **Included observations:**
- **Excluded observations:**
- **Release, environment and time scope:**
- **Baseline:**
- **Target or warning threshold:**
- **Decision or action supported:**
- **Limitation or possible misuse:**




