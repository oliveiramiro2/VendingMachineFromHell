using UnityEngine;

public class Customer : MonoBehaviour
{
    public float Patience { get; private set; }
    public bool IsServed { get; private set; }

    public void Initialize(float patience)
    {
        Patience = patience;
        IsServed = false;
    }

    public void Serve()
    {
        if (IsServed)
            return;

        IsServed = true;

        Debug.Log("😊 Customer served!");
    }
}