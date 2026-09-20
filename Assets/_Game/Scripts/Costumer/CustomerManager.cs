using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer Settings")]
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform spawnPoint;

    public Customer CurrentCustomer { get; private set; }

    private void Start()
    {
        GameManager.Instance.NightStarted += SpawnCustomer;
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

        CurrentCustomer = Instantiate(
            customerPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        CurrentCustomer.Initialize(10f, new Order(new ProductData()));

        Debug.Log("👤 New customer arrived!");
    }

    public void RemoveCurrentCustomer()
    {
        if (CurrentCustomer == null)
            return;

        Destroy(CurrentCustomer.gameObject);
        CurrentCustomer = null;
    }
}