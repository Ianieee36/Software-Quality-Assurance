using System.Runtime.CompilerServices;

namespace ENSE707_AppointmentBooking
{
    public class Doctor
    {
        public string Id { get; }
        public string FullName { get; }
        public int AvailableSlots { get; private set; }

        // 
        public Doctor(string id, string fullName, int availableSlots)
        {
            // Prevents Id become empty
            if(string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Doctor ID is required");

            // Prevents FullName become empty
            if(string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Doctor name is required");

            // Prevents having negative slot counts.
            if(availableSlots < 0)
                throw new ArgumentException("Available slots cannot be negative");

            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
        }

        /* 
        
        Method: HasAvailableSlot()
                It controls and handles availability of slots 

        */
        public bool HasAvailableSlot()
        {
            return AvailableSlots > 0; 
        }

        /* 
        
        Method: ReserveSlot()
                It handles reservation of slots which basically give out appropriate message
                if the slots are full, and decrement slot count if the reservation is successful. 
        
        */
        public void ReserveSlot()
        {
            if(!HasAvailableSlot())
                throw new InvalidOperationException("No appointment slots are available");

            AvailableSlots--;
        }
    }

    /* Detailed Improvement Analysis

        The original code does not properly handles reservations slot count which can have invalid 
        slot counts or it can become negative counts. for Maintainability this newly improved Doctor
        class only handles any reservation behaviors which if there's any new business rules they can 
        easily find it in the Doctor's class instead of running through different classes. We apply 
        Encapsulation as before every important attribute of the Doctor's class such as Id and fullname
        is can be set publicly and outside the class which makes it vulnerable for any modication that's
        why instead of making it publicly available for reading and writing we only use get for reading only.
        It is easier now for testing as we handle reservations slot behavior inside one class and create different
        methods for different behaviors.
         
    */
}