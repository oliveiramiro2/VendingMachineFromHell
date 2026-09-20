using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [SerializeField] private CustomerManager customerManager;

    public void DeliverProduct(ProductData product)
    {
        if (product == null)
            return;

        Customer customer = customerManager.CurrentCustomer;

        if (customer == null)
        {
            Debug.Log("There is no customer to serve.");
            return;
        }

        bool success = customer.TryServe(product);

        if (!success)
            return;

        GameManager.Instance.AddMoney(product.Price);
        GameManager.Instance.RegisterCustomer();

        Debug.Log($"🥤 Delivered: {product.ProductName}");

        customerManager.ServeCurrentCustomer();
    }
}