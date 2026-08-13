namespace PrescriptionRefill.Tests;

[TestClass]
public sealed class PrescriptionRefillTests
{
    [TestMethod]
    public void SubmitRequest_ValidPatientAndMedicine_ReturnSuccess()
    {
        var patient = new Patient("P001", "Christian Cantos");
        var medicine = new Medicine("M001", "Paracetamol", DateTime.Today.AddDays(10));
        var request = new RefillRequest("R001", patient, medicine);

        var service = new RefillService();

        RefillResult result = service.SubmitRequest(patient, medicine, request);

        Assert.IsTrue(result.Success);
        StringAssert.Contains(result.Message, "your refill request has been processed");
    }

    [TestMethod]
    public void Patient_EmptyId_ThrowsException()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
            new Patient("", "Christian Cantos"));

    }

    [TestMethod]
    public void SubmitRequest_EmptyMedicineName_ReturnsFailure()
    {
        var patient = new Patient("P001", "Christian Cantos");
        var medicine = new Medicine("M001", "", DateTime.Today.AddDays(10));
        var request = new RefillRequest("R001", patient, medicine);

        var service = new RefillService();

        RefillResult result = service.SubmitRequest(patient, medicine, request);

        Assert.IsFalse(result.Success);
        StringAssert.Contains(result.Message, "medicine name is empty.");
    }

    [TestMethod]
    public void SubmitRequest_TwoOrFewerDaysRemaining_MarkRequestAsUrgent()
    {
        var patient = new Patient("P001", "Christian Cantos");
        var medicine = new Medicine("M001", "Paracetamol", DateTime.Today.AddDays(2));
        var request = new RefillRequest("R001", patient, medicine);

        var service = new RefillService();

        RefillResult result = service.SubmitRequest(patient, medicine, request);

        Assert.AreEqual(RequestStatus.URGENT, result.Status);
    }

    [TestMethod]
    public void SubmitRequest_ResultMessage_IsClear()
    {
        var patient = new Patient("P001", "Christian Cantos");
        var medicine = new Medicine("M001", "Paracetamol", DateTime.Today.AddDays(3));
        var request = new RefillRequest("R001", patient, medicine);

        var service = new RefillService();

        RefillResult result = service.SubmitRequest(patient, medicine, request);

        Assert.IsTrue(result.Success);
        StringAssert.Contains(result.Message, "your refill request has been processed");
    }
}
