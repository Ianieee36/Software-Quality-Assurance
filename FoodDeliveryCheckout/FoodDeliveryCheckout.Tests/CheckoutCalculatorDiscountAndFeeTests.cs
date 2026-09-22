using FoodDeliveryCheckout.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoodDeliveryCheckout.Tests;

[TestClass]
public class CheckoutCalculatorDiscountAndFeeTests
{

    [TestMethodAttribute]
    [DataRowAttribute(100, CustomerType.Regular, 0)]
    [DataRowAttribute(100, CustomerType.Premium, 10)]
    [DataRowAttribute(100, CustomerType.Student, 15)]
    [DataRowAttribute(50, CustomerType.Premium, 5)]
    [DataRowAttribute(80, CustomerType.Student, 12)]
    public void CalculateDiscount_ValidSubtotalAndCustomerType_ReturnsExpectedDiscount(double subtotal,
        CustomerType customerType,
        double expectedDiscount)
    {
        var calculate = new CheckoutCalculator();

        var discount = calculate.CalculateDiscount((decimal)subtotal, customerType);

        Assert.AreEqual((decimal)expectedDiscount, discount);
    }

    [TestMethodAttribute]
    [DataRowAttribute(100, 5)]
    [DataRowAttribute(50, 2.5)]
    [DataRowAttribute(20, 1)]
    public void CalculateServiceFee_ValidSubtotal_ReturnsFivePercent(
        double subtotal,
        double expectedFee)
    {
        var calculator = new CheckoutCalculator();

        var serviceFee = calculator.CalculateServiceFee((decimal)subtotal);

        Assert.AreEqual((decimal)expectedFee, serviceFee);
    }

    [TestMethod]
    public void CalculateDiscount_NegativeSubtotal_ThrowsArgumentOutOfRangeException()
    {
        var calculator = new CheckoutCalculator();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>( () => calculator.CalculateDiscount(-1, CustomerType.Regular));
    }

    [TestMethod]
    public void CalculateServiceFee_NegativeSubtotal_ThrowsArgumentOutOfRangeException()
    {
        var calculator = new CheckoutCalculator();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>( () => calculator.CalculateServiceFee(-1));
    }


}