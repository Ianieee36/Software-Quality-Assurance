
namespace Week09QualityLab.Core;

public class ShippingCalculator
{
    public decimal CalculateShippingFee(decimal subtotal, string shippingType)
    {
        decimal shippingFee;

        if(subtotal < 0) 
            throw new ArgumentOutOfRangeException(nameof(subtotal));

        switch(shippingType)
        {
            case "Standard":
                shippingFee = 10m;
                break;
            
            case "Express":
                shippingFee = 20m;
                break;
            
            case "FreeShipping":
                shippingFee = 0m;
                break;
            default:
                throw new ArgumentException(nameof(shippingType));
            
        }

        if(subtotal > 100)
        {
            return subtotal;
        }

        return subtotal + shippingFee;
    }
}
