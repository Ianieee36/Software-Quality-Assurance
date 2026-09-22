namespace FoodDeliveryCheckout.Core;

public class CheckoutCalculator
{
    public decimal CalculateSubtotal(List<OrderItem> items)
    {
        if(items == null || items.Count == 0) 
            throw new ArgumentException("Order must contain at least one item.", nameof(items));


        return items.Sum(item => item.GetLineTotal());
    }

    public decimal CalculateDeliveryZone(DeliveryZone zone) 
    {
        switch(zone)
        {
            case DeliveryZone.Local:
                return 3.99m;
            
            case DeliveryZone.Suburban:
                return 6.99m;
            
            case DeliveryZone.Rural:
                return 12.99m;

            default:
                throw new ArgumentOutOfRangeException(nameof(zone));
        }
    }

    public decimal CalculateDiscount(decimal subTotal, CustomerType customerType)
    {   
        decimal rate;

        if(subTotal < 0 )
            throw new ArgumentOutOfRangeException(nameof(subTotal));


        switch(customerType)
        {
            
            case CustomerType.Regular:
                rate = 0.00m;
                break;

            case CustomerType.Premium:
                rate = 0.10m;
                break;
            
            case CustomerType.Student:
                rate = 0.15m;
                break; 

            default:
                throw new ArgumentOutOfRangeException(nameof(customerType));
        };

        return subTotal * rate;
    }

    public decimal CalculateServiceFee(decimal subTotal)
    {
        if(subTotal < 0) 
            throw new ArgumentOutOfRangeException(nameof(subTotal));
        
        return subTotal * 0.05m;
    }

    public decimal CalculateTotal(List<OrderItem> items, DeliveryZone zone, CustomerType customerType)
    {
        decimal subTotal = CalculateSubtotal(items); // Calculate the subtotal for all the items

        if(subTotal < 10)
            throw new ArgumentOutOfRangeException("Minimum order subtotal is $10."); // Business rule: Minimum of $10 order

        decimal discount = CalculateDiscount(subTotal, customerType); // Calculate the discount based on customerType.
        decimal serviceFee = CalculateServiceFee(subTotal); // Calculate serviceFee based on the subTotal.
        decimal deliveryFee = CalculateDeliveryZone(zone); // Calculate deliveryFee based on the deliveryZone.

        decimal taxableAmount = subTotal - discount + serviceFee + deliveryFee; // Calculate the taxable amount
        decimal gst = taxableAmount * 0.15m; // calculate gst from taxable amount to get the 15% gst of the total taxable amount.

        decimal total = taxableAmount + gst; // Calculate the total by adding taxable amount + the gst

        return Math.Round(total, 2); // The total must be displayed and rounded up on two decimal points.
    }


}