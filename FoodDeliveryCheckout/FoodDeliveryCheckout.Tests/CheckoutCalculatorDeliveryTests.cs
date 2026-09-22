using FoodDeliveryCheckout.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoodDeliveryCheckout.Tests;

[TestClass]
public class CheckoutCalculatorDeliveryTests
{
    [TestMethodAttribute]
    [DataRowAttribute(DeliveryZone.Local, 3.99)]
    [DataRowAttribute(DeliveryZone.Suburban, 6.99)]
    [DataRowAttribute(DeliveryZone.Rural, 12.99)]
    public void CalculateDeliveryFee_ValidZone_ReturnsExpectedFee(DeliveryZone zone, double expectedFee)
    {
        var calculate = new CheckoutCalculator();

        var deliveryFee = calculate.CalculateDeliveryZone(zone);

        Assert.AreEqual((decimal)expectedFee, deliveryFee);
    }

}