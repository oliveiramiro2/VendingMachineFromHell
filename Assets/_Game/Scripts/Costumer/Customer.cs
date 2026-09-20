using UnityEngine;
using System;

public class Customer : MonoBehaviour
{
    public float Patience { get; private set; }
    public float MaxPatience { get; private set; }

    public bool IsServed { get; private set; }
    public Order CurrentOrder { get; private set; }

    public event Action PatienceExpired;
    public event Action<float> PatienceChanged;
    public event Action<Order> OrderInitialized;

    private void Update()
    {
        if (IsServed)
            return;

        Patience -= Time.deltaTime;

        if (Patience <= 0f)
        {
            Patience = 0f;

            PatienceChanged?.Invoke(Patience);
            PatienceExpired?.Invoke();

            return;
        }

        PatienceChanged?.Invoke(Patience);
    }

    public void Initialize(float patience, Order order)
    {
        MaxPatience = patience;
        Patience = patience;

        IsServed = false;
        CurrentOrder = order;

        PatienceChanged?.Invoke(Patience);
        OrderInitialized?.Invoke(CurrentOrder);
    }

    public bool TryServe(ProductData product)
    {
        if (IsServed)
            return false;

        if (Patience <= 0f)
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