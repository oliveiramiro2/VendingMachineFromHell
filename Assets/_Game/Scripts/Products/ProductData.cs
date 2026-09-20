using UnityEngine;

[CreateAssetMenu(
    fileName = "Product",
    menuName = "Vending Machine/Product"
)]
public class ProductData : ScriptableObject
{
    [SerializeField] private string productName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price;

    public string ProductName => productName;
    public Sprite Icon => icon;
    public int Price => price;
}