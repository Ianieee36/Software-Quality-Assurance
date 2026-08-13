namespace PrescriptionRefill
{
    public class RefillResult
    {
        public bool Success { get; }
        public string Message { get; }
        public RequestStatus Status { get; private set;}
        

        public RefillResult(bool success, string message, RequestStatus status = RequestStatus.NON_URGENT)
        {
            Success = success;
            Message = message;
            Status = status;
        }


        
    }
}