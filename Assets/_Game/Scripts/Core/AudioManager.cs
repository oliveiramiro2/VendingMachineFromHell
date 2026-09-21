using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource effectsSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip nightStartedClip;
    [SerializeField] private AudioClip customerSpawnedClip;
    [SerializeField] private AudioClip correctProductClip;
    [SerializeField] private AudioClip wrongProductClip;
    [SerializeField] private AudioClip customerPatienceExpiredClip;
    [SerializeField] private AudioClip nightEndedClip;
    [SerializeField] private AudioClip resultClip;
    [SerializeField] private AudioClip productClickClip;

    private CustomerManager customerManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        customerManager = FindAnyObjectByType<CustomerManager>();

        if (customerManager == null)
        {
            Debug.LogError("AudioManager could not find CustomerManager.");
            return;
        }

        GameManager.Instance.NightStarted += HandleNightStarted;
        GameManager.Instance.NightEnded += HandleNightEnded;

        customerManager.CustomerSpawned += HandleCustomerSpawned;
        customerManager.ProductServed += HandleProductServed;
        customerManager.WrongProductAttempted += HandleWrongProduct;
        customerManager.CustomerPatienceExpired += HandleCustomerPatienceExpired;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NightStarted -= HandleNightStarted;
            GameManager.Instance.NightEnded -= HandleNightEnded;
        }

        if (customerManager != null)
        {
            customerManager.CustomerSpawned -= HandleCustomerSpawned;
            customerManager.ProductServed -= HandleProductServed;
            customerManager.WrongProductAttempted -= HandleWrongProduct;
            customerManager.CustomerPatienceExpired -= HandleCustomerPatienceExpired;
        }
    }

    private void HandleNightStarted()
    {
        PlayEffect(nightStartedClip);
    }

    private void HandleCustomerSpawned()
    {
        PlayEffect(customerSpawnedClip);
    }

    private void HandleProductServed(ProductData product)
    {
        PlayEffect(correctProductClip);
    }

    private void HandleWrongProduct(ProductData product)
    {
        PlayEffect(wrongProductClip);
    }

    private void HandleCustomerPatienceExpired()
    {
        PlayEffect(customerPatienceExpiredClip);
    }

    private void HandleNightEnded()
    {
        PlayEffect(nightEndedClip);
    }

    private void PlayEffect(AudioClip clip)
    {
        if (clip == null)
            return;

        effectsSource.PlayOneShot(clip);
    }

    public void PlayProductClick()
    {
        PlayEffect(productClickClip);
    }
}