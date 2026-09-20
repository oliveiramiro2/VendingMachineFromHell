using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer Settings")]
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private OrderGenerator orderGenerator;

    public Customer CurrentCustomer { get; private set; }

    private void Start()
    {
        GameManager.Instance.NightStarted += SpawnCustomer;

        if (GameManager.Instance.IsPlaying)
        {
            SpawnCustomer();
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.NightStarted -= SpawnCustomer;
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

        CurrentCustomer.Initialize(10f, order);
        CurrentCustomer.PatienceExpired += HandleCustomerPatienceExpired;

        Debug.Log(
            $"👤 New customer arrived! Order: {order.Product.ProductName}"
        );
    }

    public void RemoveCurrentCustomer()
    {
        if (CurrentCustomer == null)
            return;

        CurrentCustomer.PatienceExpired -= HandleCustomerPatienceExpired;

        Destroy(CurrentCustomer.gameObject);
        CurrentCustomer = null;
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

        Debug.Log("😡 Customer lost patience!");

        GameManager.Instance.RegisterMistake();
        RemoveCurrentCustomer();
        SpawnCustomer();
    }
}