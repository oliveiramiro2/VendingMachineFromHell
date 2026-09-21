using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Customer : MonoBehaviour
{
    public float Patience { get; private set; }
    public float MaxPatience { get; private set; }

    public bool IsServed { get; private set; }
    public Order CurrentOrder { get; private set; }
    private CustomerData customerData;
    public int Reward { get; private set; }

    [Header("Custom customer")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action PatienceExpired;
    public event Action<float> PatienceChanged;
    public event Action<Order> OrderInitialized;

    public event Action<ProductData> ProductServed;
    public event Action<ProductData> WrongProductAttempted;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    public void Initialize(Order order, CustomerData data)
    {
        MaxPatience = data.Patience;
        Patience = data.Patience;

        IsServed = false;
        CurrentOrder = order;
        customerData = data;

        spriteRenderer.sprite = data.Sprite;

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

            WrongProductAttempted?.Invoke(product);

            return false;
        }

        Reward = Mathf.RoundToInt(
            product.Price * customerData.RewardMultiplier
        );

        IsServed = true;

        Debug.Log($"😊 Customer served: {product.ProductName}");

        ProductServed?.Invoke(product);

        return true;
    }
}