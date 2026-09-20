using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    [SerializeField] private ProductData[] availableProducts;

    public Order GenerateOrder()
    {
        if (availableProducts == null || availableProducts.Length == 0)
        {
            Debug.LogError("No products available to generate an order.");
            return null;
        }

        ProductData product = availableProducts[
            Random.Range(0, availableProducts.Length)
        ];

        return new Order(product);
    }
}