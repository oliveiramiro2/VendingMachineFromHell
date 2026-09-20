using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer Settings")]
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private Transform spawnPoint;

    public Customer CurrentCustomer { get; private set; }

    public void SpawnCustomer()
    {
        if (CurrentCustomer != null)
            return;

        CurrentCustomer = Instantiate(
            customerPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        CurrentCustomer.Initialize(10f);

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