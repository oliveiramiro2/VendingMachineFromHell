using UnityEngine;

[CreateAssetMenu(
    fileName = "Customer",
    menuName = "Vending Machine/Customer"
)]
public class CustomerData : ScriptableObject
{
    [SerializeField] private float patience;

    public float Patience => patience;
}