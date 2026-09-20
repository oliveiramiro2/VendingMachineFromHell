using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float nightDuration = 120f;
    [SerializeField] private int startingMoney = 0;

    public float TimeRemaining { get; private set; }
    public int Money { get; private set; }
    public int CustomersServed { get; private set; }
    public int Mistakes { get; private set; }

    public bool IsPlaying { get; private set; }

    public event Action NightStarted;
    public event Action NightEnded;

    public event Action<int> MoneyChanged;
    public event Action<int> CustomersServedChanged;
    public event Action<int> MistakesChanged;
    public event Action<float> TimeRemainingChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!IsPlaying)
            return;

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            TimeRemainingChanged?.Invoke(TimeRemaining);

            EndNight();
            return;
        }

        TimeRemainingChanged?.Invoke(TimeRemaining);
    }

    public void StartNight()
    {
        Money = startingMoney;
        CustomersServed = 0;
        Mistakes = 0;

        TimeRemaining = nightDuration;
        IsPlaying = true;

        MoneyChanged?.Invoke(Money);
        CustomersServedChanged?.Invoke(CustomersServed);
        MistakesChanged?.Invoke(Mistakes);
        TimeRemainingChanged?.Invoke(TimeRemaining);

        Debug.Log("🌙 NIGHT STARTED!");

        NightStarted?.Invoke();
    }

    public void AddMoney(int amount)
    {
        Money += amount;

        MoneyChanged?.Invoke(Money);

        Debug.Log($"💰 Money: ${Money}");
    }

    public void RegisterCustomer()
    {
        CustomersServed++;

        CustomersServedChanged?.Invoke(CustomersServed);

        Debug.Log($"👤 Customers served: {CustomersServed}");
    }

    public void RegisterMistake()
    {
        Mistakes++;

        MistakesChanged?.Invoke(Mistakes);

        Debug.Log($"❌ Mistakes: {Mistakes}");
    }

    private void EndNight()
    {
        IsPlaying = false;

        Debug.Log("🌅 NIGHT OVER!");
        Debug.Log($"Customers served: {CustomersServed}");
        Debug.Log($"Money earned: ${Money}");
        Debug.Log($"Mistakes: {Mistakes}");

        NightEnded?.Invoke();
    }
}