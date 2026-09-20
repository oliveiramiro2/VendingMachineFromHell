using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Game Info")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text customersServedText;
    [SerializeField] private TMP_Text mistakesText;
    [SerializeField] private TMP_Text timerText;

    [Header("Customer")]
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private Slider patienceSlider;

    private CustomerManager customerManager;
    private Customer currentCustomer;

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        customerManager = FindAnyObjectByType<CustomerManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameUI could not find GameManager.");
            return;
        }

        if (customerManager == null)
        {
            Debug.LogError("GameUI could not find CustomerManager.");
            return;
        }

        gameManager.MoneyChanged += UpdateMoney;
        gameManager.CustomersServedChanged += UpdateCustomersServed;
        gameManager.MistakesChanged += UpdateMistakes;
        gameManager.TimeRemainingChanged += UpdateTimer;

        customerManager.CurrentCustomerChanged += HandleCurrentCustomerChanged;

        UpdateMoney(gameManager.Money);
        UpdateCustomersServed(gameManager.CustomersServed);
        UpdateMistakes(gameManager.Mistakes);
        UpdateTimer(gameManager.TimeRemaining);

        if (customerManager.CurrentCustomer != null)
        {
            HandleCurrentCustomerChanged(customerManager.CurrentCustomer);
        }
    }

    private void OnDestroy()
    {
        GameManager gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            gameManager.MoneyChanged -= UpdateMoney;
            gameManager.CustomersServedChanged -= UpdateCustomersServed;
            gameManager.MistakesChanged -= UpdateMistakes;
            gameManager.TimeRemainingChanged -= UpdateTimer;
        }

        if (customerManager != null)
        {
            customerManager.CurrentCustomerChanged -= HandleCurrentCustomerChanged;
        }

        UnsubscribeFromCustomer();
    }

    private void HandleCurrentCustomerChanged(Customer customer)
    {
        UnsubscribeFromCustomer();

        currentCustomer = customer;

        if (currentCustomer == null)
        {
            ClearCustomerUI();
            return;
        }

        currentCustomer.PatienceChanged += UpdatePatience;
        currentCustomer.OrderInitialized += UpdateOrder;

        UpdatePatience(currentCustomer.Patience);
        UpdateOrder(currentCustomer.CurrentOrder);
    }

    private void UnsubscribeFromCustomer()
    {
        if (currentCustomer == null)
            return;

        currentCustomer.PatienceChanged -= UpdatePatience;
        currentCustomer.OrderInitialized -= UpdateOrder;

        currentCustomer = null;
    }

    private void UpdateMoney(int money)
    {
        moneyText.text = $"Money: ${money}";
    }

    private void UpdateCustomersServed(int customersServed)
    {
        customersServedText.text = $"Served: {customersServed}";
    }

    private void UpdateMistakes(int mistakes)
    {
        mistakesText.text = $"Mistakes: {mistakes}";
    }

    private void UpdateTimer(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdatePatience(float patience)
    {
        if (currentCustomer == null)
            return;

        patienceSlider.maxValue = currentCustomer.MaxPatience;
        patienceSlider.value = patience;
    }

    private void UpdateOrder(Order order)
    {
        if (order == null || order.Product == null)
        {
            orderText.text = "Order: ---";
            return;
        }

        orderText.text = $"Order: {order.Product.ProductName}";
    }

    private void ClearCustomerUI()
    {
        orderText.text = "Order: ---";
        patienceSlider.value = 0f;
    }
}