namespace PrescriptionRefill;

public class Patient
{
    public string Id { get; }
    public string FullName { get; }

    public Patient(string id, string fullName)
    {
        if(string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Patient ID is required.");
        
        if(string.IsNullOrWhiteSpace(fullName)) 
            throw new ArgumentException("Patient Full Name is required");
        
        Id = id;
        FullName = fullName;
    }

}
