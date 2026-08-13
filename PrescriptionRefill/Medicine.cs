namespace PrescriptionRefill
{
    public class Medicine
    {
        public string MedicineId { get; }
        public string MedicineName { get; }
        public DateTime SupplyEndDate { get; }
        public int DaysRemaining => (SupplyEndDate.Date - DateTime.Today).Days;

        public Medicine(string medicineId, string medicineName, DateTime supplyEndDate)
        {   
            MedicineId = medicineId;
            MedicineName = medicineName;
            SupplyEndDate = supplyEndDate;
        }

        public RequestStatus GetRequestStatus()
        {
            return DaysRemaining <= 2 ? RequestStatus.URGENT : RequestStatus.NON_URGENT;
        }
    }
}