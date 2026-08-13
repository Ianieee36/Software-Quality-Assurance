namespace PrescriptionRefill
{
    public class RefillService
    {
        public RefillResult SubmitRequest(Patient patient, Medicine medicine, RefillRequest request)
        {
            if(request == null) 
                return new RefillResult(false, "Request Failed: request details is required.");

            if(medicine == null)
                return new RefillResult(false, "Request Failed: medicine details is required.");

            if(string.IsNullOrWhiteSpace(medicine.MedicineName))
                return new RefillResult(false, "Request Failed: medicine name is empty.");

            if(patient == null)
                return new RefillResult(false, "Request Failed: patient details is required.");

            RequestStatus status = medicine.GetRequestStatus();

            string message = status == RequestStatus.URGENT
                ? "Request Successful: your refill request has been processed - marked URGENT."
                : "Request Successful: your refill request has been processed.";

            return new RefillResult(true, message, status);
                
            
        }
    }
}