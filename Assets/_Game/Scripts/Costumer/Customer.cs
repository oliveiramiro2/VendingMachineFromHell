using UnityEngine;

public class Customer : MonoBehaviour
{
    public float Patience { get; private set; }
    public bool IsServed { get; private set; }
    public Order CurrentOrder { get; private set; }

    public void Initialize(float patience, Order order)
    {
        Patience = patience;
        IsServed = false;
        CurrentOrder = order;
    }

    public bool TryServe(ProductData product)
    {
        if (IsServed)
            return false;

        if (!CurrentOrder.IsSatisfiedBy(product))
        {
            Debug.Log("❌ Wrong product!");
            return false;
        }

        IsServed = true;

        Debug.Log($"😊 Customer served: {product.ProductName}");

        return true;
    }
}