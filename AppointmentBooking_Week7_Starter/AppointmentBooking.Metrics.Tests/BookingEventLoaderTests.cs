using AppointmentBooking.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppointmentBooking.Metrics.Tests;

[TestClass]
public class BookingEventLoaderTests
{
    [TestMethod]
    public void Load_WithControlledCsv_ParsesExpectedFields()
    {
        string path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(
                path,
                "timestampUtc,release,requestId,isValidRequest,outcome,timeBand,channel,latencyMs\n"
                + "2026-08-01T00:00:00Z,2.3.0,R001,true,Success,Normal,Web,420\n");

            IReadOnlyList<BookingEvent> events = BookingEventLoader.Load(path);

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("R001", events[0].RequestId);
            Assert.IsTrue(events[0].IsValidRequest);
            Assert.AreEqual(420.0, events[0].LatencyMs);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void Load_WhenRequiredHeaderIsMissing_ThrowsInvalidDataException()
    {
        string path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "release,requestId\n2.3.0,R001\n");

            Assert.Throws<InvalidDataException>(() => BookingEventLoader.Load(path));
        }
        finally
        {
            File.Delete(path);
        }
    }
}
