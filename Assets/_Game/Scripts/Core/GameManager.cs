using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action NightStarted;

    [Header("Game Settings")]
    [SerializeField] private float nightDuration = 120f;
    [SerializeField] private int startingMoney = 0;

    public float TimeRemaining { get; private set; }
    public int Money { get; private set; }
    public int CustomersServed { get; private set; }
    public int Mistakes { get; private set; }

    public bool IsPlaying { get; private set; }

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
            EndNight();
        }
    }

    public void StartNight()
    {
        Money = startingMoney;
        CustomersServed = 0;
        Mistakes = 0;

        TimeRemaining = nightDuration;
        IsPlaying = true;

        Debug.Log("🌙 NIGHT STARTED!");

        NightStarted?.Invoke();
    }

    public void AddMoney(int amount)
    {
        Money += amount;

        Debug.Log($"💰 Money: ${Money}");
    }

    public void RegisterCustomer()
    {
        CustomersServed++;

        Debug.Log($"👤 Customers served: {CustomersServed}");
    }

    public void RegisterMistake()
    {
        Mistakes++;

        Debug.Log($"❌ Mistakes: {Mistakes}");
    }

    private void EndNight()
    {
        IsPlaying = false;

        Debug.Log("🌅 NIGHT OVER!");
        Debug.Log($"Customers served: {CustomersServed}");
        Debug.Log($"Money earned: ${Money}");
        Debug.Log($"Mistakes: {Mistakes}");
    }
}