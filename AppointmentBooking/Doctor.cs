using System.Runtime.CompilerServices;

namespace ENSE707_AppointmentBooking
{
    public class Doctor
    {
        public string Id { get; }
        public string FullName { get; }
        public int AvailableSlots { get; private set; }
        public int MaxDailyAppointments { get; }
 
        // Tracks how many appointments have been booked per calendar date.
        // Fixes the earlier gap where the system had no concept of "which day"
        // a slot belonged to.
        private readonly Dictionary<DateTime, int> _dailyBookingCounts = new();
 
        // Guards check-then-act sequences below against race conditions.
        // See earlier discussion: this is appropriate at this scale because
        // Doctor is a plain in-memory object with no external persistence layer.
        private readonly object _slotLock = new object();

        public Doctor(string id, string fullName, int availableSlots, int maxDailyAppointments)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Doctor ID is required");
 
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Doctor name is required");
 
            if (availableSlots < 0)
                throw new ArgumentException("Available slots cannot be negative");
 
            if (maxDailyAppointments <= 0)
                throw new ArgumentException("Maximum daily appointments must be greater than zero");
 
            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
            MaxDailyAppointments = maxDailyAppointments;
        }

        // Checks both overall capacity AND the per-day cap for the given date.
        public bool IsAvailableOn(DateTime date)
        {
            lock (_slotLock)
            {
                return AvailableSlots > 0 && BookedCountFor(date) < MaxDailyAppointments;
            }
        }

        public int BookedCountFor(DateTime date)
        {
            lock (_slotLock)
            {
                var day = date.Date;
                return _dailyBookingCounts.TryGetValue(day, out var count) ? count : 0;
            }
        }

        public void ReserveSlot(DateTime date)
        {
            lock (_slotLock)
            {
                var day = date.Date;
                int bookedToday = _dailyBookingCounts.TryGetValue(day, out var count) ? count : 0;
 
                if (AvailableSlots <= 0)
                    throw new InvalidOperationException("No appointment slots are available");
 
                if (bookedToday >= MaxDailyAppointments)
                    throw new InvalidOperationException(
                        $"Maximum daily appointments ({MaxDailyAppointments}) reached for {day:d}");
 
                AvailableSlots--;
                _dailyBookingCounts[day] = bookedToday + 1;
            }
        }

        public void ReleaseSlot(DateTime date)
        {
            lock (_slotLock)
            {
                var day = date.Date;
                int bookedToday = _dailyBookingCounts.TryGetValue(day, out var count) ? count : 0;

                AvailableSlots++;

                if(bookedToday > 0)
                    _dailyBookingCounts[day] = bookedToday - 1;

            }
        }
        
    }

    /* Detailed Improvement Analysis (updated)
 
        Doctor is now date-aware: slot availability is tracked both as an overall
        pool (AvailableSlots) and per calendar day (_dailyBookingCounts), so a
        "maximum daily appointments" business rule can actually be enforced.
        Previously, AvailableSlots had no concept of which day a slot belonged to,
        which was a functional suitability gap - two patients could book the same
        doctor for the same day even if that day was meant to be capped.
 
        The _slotLock addition closes a check-then-act race condition: without it,
        two threads could both pass IsAvailableOn() before either had recorded a
        booking, allowing overbooking under concurrent access. This is scoped
        appropriately here because Doctor is a plain in-memory object with no
        database/persistence layer in this codebase - see prior discussion on why
        a `lock` would NOT be sufficient if Doctor objects were reloaded fresh
        per request from a database (that would require database-level
        concurrency control instead, e.g. optimistic concurrency tokens).
    */
}