public class Order
{
    public ProductData Product { get; }

    public Order(ProductData product)
    {
        Product = product;
    }

    public bool IsSatisfiedBy(ProductData product)
    {
        return Product == product;
    }
}