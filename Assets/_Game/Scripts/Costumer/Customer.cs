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

    public void Serve()
    {
        if (IsServed)
            return;

        IsServed = true;

        Debug.Log($"😊 Customer served: {CurrentOrder.Product.ProductName}");
    }
}