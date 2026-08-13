namespace PrescriptionRefill
{
    
    public class RefillRequest
    {
        public string RequestId { get; }
        public Patient Patient { get; }
        public Medicine Medicine { get; }
        public DateTime RequestDate { get; private set;}
        public RequestStatus Status { get; private set;}

        public RefillRequest(string requestId, Patient patient, Medicine medicine)
        {
            if(string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentException("Prescription Refill Request Id is required.");

            RequestId = requestId;
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            Medicine = medicine ?? throw new ArgumentNullException(nameof(medicine));
            RequestDate = DateTime.Now;
        }


    }
}