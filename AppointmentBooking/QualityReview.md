# Step 16

**One useful suggestion**

- One useful suggestion that claude has given is that instead of constructing message in
  the service class it can be refactored to using factory method which handles BookingResults.
  It improves maintainability and it is now safer to isolate behaviors which makes it better for 
  testing.

**One suggestion I modified**

- The one suggestion that I modified is that based on the codebase lock is a good, appropriate suggestion.
  It matches the actual complexity of the system rather than over engineering for a persistence architecture that doesn't exists yet. At this scale, lock is a good choice, but would need to be revisited if/ when this system grows to include a database or distributed deployment.

**One suggestion I rejected**

- I considered adding the recommended authorization or authentication directly into AppointementBookingService. I rejected this because it violates separation of concerns. auth belongs in a layer above the domain/business logic, not baked into the booking logic. Suggesting it be added inside BookAppointment would itself be a quality regression, even though "add security" sounds correct on the surface.

**Why human judgement was required**

- Reflecting to my answers from the previous questions is that first, the lock recommendation looked correct in isolation , but only a human who knows the actual deployment/persistence context can judge whether it's the right fix. Second is that deciding where authorization should belongs. This is the core lesson: AI-generated suggestions can be locally correct be locally correct 