using UnityEngine;

[CreateAssetMenu(
    fileName = "Customer",
    menuName = "Vending Machine/Customer"
)]
public class CustomerData : ScriptableObject
{
    [SerializeField] private float patience;
    [SerializeField] private float rewardMultiplier = 1f;
    [SerializeField] private Sprite sprite;

    public float Patience => patience;
    public float RewardMultiplier => rewardMultiplier;
    public Sprite Sprite => sprite;
}