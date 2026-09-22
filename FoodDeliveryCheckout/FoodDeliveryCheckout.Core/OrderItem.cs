namespace FoodDeliveryCheckout.Core;

public class OrderItem
{
    public string Name { get; }
    public decimal UnitPrice { get; }
    public int Quantity { get; }

    public OrderItem(string name, decimal unitPrice, int quantity)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Item name is required!", nameof(name));

        if(unitPrice <= 0)
            throw new ArgumentOutOfRangeException("Unit price must be greater than 0", nameof(unitPrice));

        if(quantity <= 0) 
            throw new ArgumentOutOfRangeException("Quantity must be greater than 0", nameof(quantity));

        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public decimal GetLineTotal()
    {
        return UnitPrice * Quantity;
    }

}