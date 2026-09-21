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

    [Header("Customer Order")]
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private Image orderIcon;

    [Header("Customer Patience")]
    [SerializeField] private Slider patienceSlider;
    [SerializeField] private TMP_Text patienceWarningText;

    [Header("Feedback")]
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float feedbackDuration = 0.75f;

    private CustomerManager customerManager;
    private Customer currentCustomer;

    private float feedbackTimer;

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

        ClearFeedback();

        if (customerManager.CurrentCustomer != null)
        {
            HandleCurrentCustomerChanged(customerManager.CurrentCustomer);
        }
    }

    private void Update()
    {
        if (feedbackTimer <= 0f)
            return;

        feedbackTimer -= Time.deltaTime;

        if (feedbackTimer <= 0f)
        {
            ClearFeedback();
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
        currentCustomer.ProductServed += HandleProductServed;
        currentCustomer.WrongProductAttempted += HandleWrongProduct;

        UpdatePatience(currentCustomer.Patience);
        UpdateOrder(currentCustomer.CurrentOrder);
    }

    private void UnsubscribeFromCustomer()
    {
        if (currentCustomer == null)
            return;

        currentCustomer.PatienceChanged -= UpdatePatience;
        currentCustomer.OrderInitialized -= UpdateOrder;
        currentCustomer.ProductServed -= HandleProductServed;
        currentCustomer.WrongProductAttempted -= HandleWrongProduct;

        currentCustomer = null;
    }

    private void UpdateMoney(int money)
    {
        moneyText.text = $"${money}";
    }

    private void UpdateCustomersServed(int customersServed)
    {
        customersServedText.text = $"{customersServed}";
    }

    private void UpdateMistakes(int mistakes)
    {
        mistakesText.text = $"{mistakes}";
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

        float patiencePercent =
            patience / currentCustomer.MaxPatience;

        patienceWarningText.enabled = patiencePercent <= 0.25f;

        if (patiencePercent <= 0.25f)
        {
            patienceWarningText.text = "HURRY!";
        }
        else
        {
            patienceWarningText.text = string.Empty;
        }
    }

    private void UpdateOrder(Order order)
    {
        if (order == null || order.Product == null)
        {
            orderText.text = "Order: ---";
            orderIcon.enabled = false;
            return;
        }

        orderText.text = order.Product.ProductName;

        if (order.Product.Icon != null)
        {
            orderIcon.sprite = order.Product.Icon;
            orderIcon.enabled = true;
        }
        else
        {
            orderIcon.enabled = false;
        }
    }

    private void HandleProductServed(ProductData product)
    {
        ShowFeedback($"+${product.Price}  CORRECT!", feedbackDuration);
    }

    private void HandleWrongProduct(ProductData product)
    {
        ShowFeedback("WRONG PRODUCT!", feedbackDuration);
    }

    private void ShowFeedback(string message, float duration)
    {
        feedbackText.text = message;
        feedbackText.enabled = true;

        feedbackTimer = duration;
    }

    private void ClearFeedback()
    {
        feedbackText.text = string.Empty;
        feedbackText.enabled = false;
        feedbackTimer = 0f;
    }

    private void ClearCustomerUI()
    {
        orderText.text = "Order: ---";
        orderIcon.enabled = false;
        patienceSlider.value = 0f;
        patienceWarningText.enabled = false;
    }
}