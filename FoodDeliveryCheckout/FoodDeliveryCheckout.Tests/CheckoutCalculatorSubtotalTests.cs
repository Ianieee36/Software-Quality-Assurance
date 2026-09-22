using FoodDeliveryCheckout.Core;

namespace FoodDeliveryCheckout.Tests;

[TestClass]
public class CheckoutCalculatorSubtotalTests
{
    [TestMethod]
    public void GetLineTotal_Valid_Item_CalculateSubtotal()
    {
        OrderItem item = new OrderItem("Ramen", 25.99m, 1);

        decimal result = item.GetLineTotal();

        Assert.AreEqual(25.99m, result);
    }
    
    [TestMethod]
    public void CalculateSubtotal_MultipleItems_ReturnsCorrectSubtotal()
    {
        var calculator = new CheckoutCalculator();

        var items = new List<OrderItem>
        {
            new OrderItem("Fries", 5.99m, 2),
            new OrderItem("Big Mac", 16.99m, 1),
            new OrderItem("Ice Cream", 5.99m, 2)
        };

        var subtotal = calculator.CalculateSubtotal(items);

        Assert.AreEqual(40.95m, subtotal);
    }

    [TestMethod]
    public void CalculateSubtotal_EmptyItemList_ThrowsArgumentException()
    {
        var calculator = new CheckoutCalculator();
        var items = new List<OrderItem> {};

        Assert.ThrowsExactly<ArgumentException>(() => calculator.CalculateSubtotal(items));
    }

    [TestMethod]
    public void CalculateSubtotal_NullItemList_ThrowsArgumentException()
    {
        var calculator = new CheckoutCalculator();

        Assert.ThrowsExactly<ArgumentException>(() => calculator.CalculateSubtotal(null));
    }

    [TestMethodAttribute]
    [DataRowAttribute("")]
    [DataRowAttribute(" ")]
    [DataRowAttribute(null)]
    public void Constructor_InvalidItemName_ThrowsArgumentException(string name)
    {
        Assert.ThrowsExactly<ArgumentException>(() => new OrderItem(name, 5.99m, 1));
    }

    [TestMethodAttribute]
    [DataRowAttribute(0)]
    [DataRowAttribute(-1)]
    public void Constructor_InvalidUnitPrice_ThrowsArgumentOutOfRangeException(double unitPrice)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>( () => new OrderItem("Burger", (decimal)unitPrice, 1));
    }

    [TestMethodAttribute]
    [DataRowAttribute(0)]
    [DataRowAttribute(-1)]
    public void Constructor_InvalidQuantity_ThrowsArgumentOutOfRangeException(int quantity)
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>( () => new OrderItem("Burger", 5.99m, quantity));
    }
    
}