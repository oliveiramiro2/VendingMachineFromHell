using UnityEngine;
using UnityEngine.InputSystem;

public class CustomerTester : MonoBehaviour
{
    [SerializeField] private ProductData testProduct;

    private void Update()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        CustomerManager customerManager =
            FindAnyObjectByType<CustomerManager>();

        if (customerManager == null)
            return;

        Customer customer = customerManager.CurrentCustomer;

        if (customer == null)
            return;

        customer.TryServe(testProduct);
    }
}