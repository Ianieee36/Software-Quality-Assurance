using Microsoft.VisualStudio.TestTools.UnitTesting;
using Week09QualityLab.Core;

namespace Week09QualityLab.Tests;

[TestClass]
public sealed class DiscountCalculatorTests
{
    [TestMethod]
    public void CalculateFinalPrice_ShouldReturnOriginalPrice_ForRegularCustomer()
    {
        var calculator = new DiscountCalculator();
        
        decimal result = calculator.CalculateFinalPrice(100m, "Regular");

        Assert.AreEqual(100m, result);
    }

    [TestMethod]
    public void CalculateFinalPrice_ShouldApplyTenPercentDiscount_ForPremiumCustomer()
    {
        var calculator = new DiscountCalculator();

        decimal result = calculator.CalculateFinalPrice(100m, "Premium");

        Assert.AreEqual(90m, result);
    }

    [TestMethod]
    public void CalculateFinalPrice_ShouldApplyTwentyPercentDiscount_ForVipCustomer()
    {
        var calculator = new DiscountCalculator();

        decimal result = calculator.CalculateFinalPrice(100m, "VIP");

        Assert.AreEqual(80m, result);
    }

    [TestMethod]
    public void CalculateFinalPrice_ShouldThrowException_WhenOriginalPriceIsNegative()
    {
        var calculator = new DiscountCalculator();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => calculator.CalculateFinalPrice(-1, "Regular"));
    }
}
