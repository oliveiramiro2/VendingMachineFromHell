using UnityEngine;
using System;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer Settings")]
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private OrderGenerator orderGenerator;
    [SerializeField] private CustomerData[] availableCustomers;

    public Customer CurrentCustomer { get; private set; }

    public event Action<Customer> CurrentCustomerChanged;
    public event Action<ProductData> ProductServed;
    public event Action<ProductData> WrongProductAttempted;
    public event Action CustomerSpawned;
    public event Action CustomerPatienceExpired;

    private void Start()
    {
        GameManager.Instance.NightStarted += SpawnCustomer;
        GameManager.Instance.NightEnded += HandleNightEnded;

        if (GameManager.Instance.IsPlaying)
        {
            SpawnCustomer();
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.NightStarted -= SpawnCustomer;
        GameManager.Instance.NightEnded -= HandleNightEnded;
    }

    public void SpawnCustomer()
    {
        if (CurrentCustomer != null)
            return;

        Order order = orderGenerator.GenerateOrder();

        if (order == null)
            return;

        CurrentCustomer = Instantiate(
            customerPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        float patience = 10f;

        if (availableCustomers.Length > 0)
        {
            CustomerData customerData = availableCustomers[
                UnityEngine.Random.Range(0, availableCustomers.Length)
            ];
            patience = customerData.Patience > 0f ? customerData.Patience : patience;
        }

        CurrentCustomer.Initialize(patience, order);

        CurrentCustomer.PatienceExpired += HandleCustomerPatienceExpired;
        CurrentCustomer.ProductServed += HandleProductServed;
        CurrentCustomer.WrongProductAttempted += HandleWrongProduct;

        CurrentCustomerChanged?.Invoke(CurrentCustomer);
        CustomerSpawned?.Invoke();

        Debug.Log(
            $"👤 New customer arrived! Order: {order.Product.ProductName} - {patience}"
        );
    }

    public void RemoveCurrentCustomer()
    {
        if (CurrentCustomer == null)
            return;

        Customer customer = CurrentCustomer;

        customer.PatienceExpired -= HandleCustomerPatienceExpired;
        customer.ProductServed -= HandleProductServed;
        customer.WrongProductAttempted -= HandleWrongProduct;

        CurrentCustomer = null;

        CurrentCustomerChanged?.Invoke(null);

        Destroy(customer.gameObject);
    }

    public void ServeCurrentCustomer()
    {
        if (CurrentCustomer == null)
            return;

        RemoveCurrentCustomer();
        SpawnCustomer();
    }

    private void HandleCustomerPatienceExpired()
    {
        if (CurrentCustomer == null)
            return;

        CustomerPatienceExpired?.Invoke();

        Debug.Log("😡 Customer lost patience!");

        GameManager.Instance.RegisterMistake();

        RemoveCurrentCustomer();
        SpawnCustomer();
    }

    private void HandleProductServed(ProductData product)
    {
        ProductServed?.Invoke(product);
    }

    private void HandleWrongProduct(ProductData product)
    {
        WrongProductAttempted?.Invoke(product);
    }

    private void HandleNightEnded()
    {
        Debug.Log("🚪 CustomerManager shutting down current customer.");

        RemoveCurrentCustomer();
    }
}