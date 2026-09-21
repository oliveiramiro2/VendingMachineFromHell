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
    [SerializeField] private float feedbackPopScale = 1.25f;
    [SerializeField] private float feedbackScaleSpeed = 8f;

    [Header("Patience Warning")]
    [SerializeField] private float warningPulseSpeed = 6f;
    [SerializeField] private float warningPulseAmount = 0.15f;

    [Header("Night Result")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultMoneyText;
    [SerializeField] private TMP_Text resultCustomersText;
    [SerializeField] private TMP_Text resultMistakesText;

    private CustomerManager customerManager;
    private Customer currentCustomer;

    private float feedbackTimer;
    private Vector3 feedbackOriginalScale;
    private Vector3 warningOriginalScale;

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

        feedbackOriginalScale = feedbackText.transform.localScale;
        warningOriginalScale = patienceWarningText.transform.localScale;

        gameManager.MoneyChanged += UpdateMoney;
        gameManager.CustomersServedChanged += UpdateCustomersServed;
        gameManager.MistakesChanged += UpdateMistakes;
        gameManager.TimeRemainingChanged += UpdateTimer;

        gameManager.NightEnded += HandleNightEnded;

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

        resultPanel.SetActive(!gameManager.IsPlaying);
    }

    private void Update()
    {
        UpdateFeedbackAnimation();
        UpdatePatienceWarningAnimation();
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

            gameManager.NightEnded -= HandleNightEnded;
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
            patienceWarningText.transform.localScale =
                warningOriginalScale;
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
        ShowFeedback(
            $"+${product.Price}  CORRECT!",
            feedbackDuration
        );
    }

    private void HandleWrongProduct(ProductData product)
    {
        ShowFeedback(
            "WRONG PRODUCT!",
            feedbackDuration
        );
    }

    private void ShowFeedback(string message, float duration)
    {
        feedbackText.text = message;
        feedbackText.enabled = true;

        feedbackTimer = duration;

        feedbackText.transform.localScale =
            feedbackOriginalScale * feedbackPopScale;
    }

    private void UpdateFeedbackAnimation()
    {
        if (!feedbackText.enabled)
            return;

        feedbackTimer -= Time.deltaTime;

        if (feedbackTimer <= 0f)
        {
            ClearFeedback();
            return;
        }

        feedbackText.transform.localScale = Vector3.Lerp(
            feedbackText.transform.localScale,
            feedbackOriginalScale,
            Time.deltaTime * feedbackScaleSpeed
        );
    }

    private void UpdatePatienceWarningAnimation()
    {
        if (!patienceWarningText.enabled)
            return;

        float pulse = Mathf.Sin(
            Time.time * warningPulseSpeed
        ) * warningPulseAmount;

        patienceWarningText.transform.localScale =
            warningOriginalScale * (1f + pulse);
    }

    private void HandleNightEnded()
    {
        resultMoneyText.text =
            $"Money: ${GameManager.Instance.Money}";

        resultCustomersText.text =
            $"Served: {GameManager.Instance.CustomersServed}";

        resultMistakesText.text =
            $"Mistakes: {GameManager.Instance.Mistakes}";

        resultPanel.SetActive(true);

        ClearCustomerUI();
        ClearFeedback();
    }

    public void HideNightEnded()
    {
        resultMoneyText.text =
            $"";

        resultCustomersText.text =
            $"";

        resultMistakesText.text =
            $"";

        resultPanel.SetActive(false);
    }

    private void ClearFeedback()
    {
        feedbackText.text = string.Empty;
        feedbackText.enabled = false;
        feedbackTimer = 0f;

        feedbackText.transform.localScale =
            feedbackOriginalScale;
    }

    private void ClearCustomerUI()
    {
        orderText.text = "Order: ---";
        orderIcon.enabled = false;
        patienceSlider.value = 0f;

        patienceWarningText.enabled = false;
        patienceWarningText.transform.localScale =
            warningOriginalScale;
    }
}