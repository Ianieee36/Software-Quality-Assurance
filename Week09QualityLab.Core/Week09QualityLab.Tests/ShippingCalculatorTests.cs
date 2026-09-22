using Microsoft.VisualStudio.TestTools.UnitTesting;
using Week09QualityLab.Core;

namespace Week09QualityLab.Tests;

[TestClass]
public sealed class ShippingCalculatorTests
{
    [TestMethod]
    public void ShippingCalculator_ShouldApplyStandardShippingFee_ForStandardShipping()
    {
        var calculator = new ShippingCalculator();

        decimal result = calculator.CalculateShippingFee(50m, "Standard");

        Assert.AreEqual(60m, result);
    }

    [TestMethod]
    public void ShippingCalculator_ShouldApplyExpressShippingFee_ForExpressShipping()
    {
        var calculator = new ShippingCalculator();

        decimal result = calculator.CalculateShippingFee(50m, "Express");

        Assert.AreEqual(70m, result);
    }

    [TestMethod]
    public void ShippingCalculator_ShouldApplyFreeShipping_ForOverOneHundredDollarsOrder()
    {
        var calculator = new ShippingCalculator();

        decimal result = calculator.CalculateShippingFee(100m, "FreeShipping");

        Assert.AreEqual(100m, result);
    }

    [TestMethod]
    public void ShippingCalculator_ShouldThrowOutOfRangeException_WhenSubtotalIsNegative()
    {
        var calculator = new ShippingCalculator();

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => calculator.CalculateShippingFee(-1, "Standard"));
    }
}