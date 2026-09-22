using FoodDeliveryCheckout.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoodDeliveryCheckout.Tests;

[TestClass]
public class CheckoutCalculatorTotalTests
{
    
    [TestMethod]
    public void CalculateTotal_RegularCustomerLocalDelivery_ReturnsExpectedTotal()
    {
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Burger", 20.00m, 1),
            new OrderItem("Drink", 5.00m, 1),
        };

        var total = calculator.CalculateTotal(
            items,
            DeliveryZone.Local,
            CustomerType.Regular
        );

        Assert.AreEqual(34.78m, total);

    }

    [TestMethod]
    public void CalculateTotal_PremiumCustomerSuburbanDelivery_ReturnsExpectedTotal()
    {
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Pizza", 30.00m, 2),
        };

        var total = calculator.CalculateTotal(
            items,
            DeliveryZone.Suburban,
            CustomerType.Premium
        );

        Assert.AreEqual(73.59m, total);

    }

    [TestMethod]
    public void CalculateTotal_StudentCustomerRuralDelivery_ReturnsExpectedTotal()
    {
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Sushi Pack", 15.00m, 2),
            new OrderItem("Juice", 4.00m, 1)
        };

        var total = calculator.CalculateTotal(
            items,
            DeliveryZone.Rural,
            CustomerType.Student
        );

        Assert.AreEqual(50.13m, total);

    }
}