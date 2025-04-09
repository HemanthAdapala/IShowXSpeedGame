using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleItemPrefabUI : MonoBehaviour
{
    [SerializeField] private Image vehicleImageUI;
    [SerializeField] private TextMeshProUGUI vehicleNameUI;
    [SerializeField] private TextMeshProUGUI vehiclePriceUI;
    [SerializeField] private Button buyButtonUI;


    public void SetData(VehicleUIItemData vehicleData)
    {
        if(vehicleData != null) 
        {
            if(vehicleImageUI != null)
            {
                vehicleImageUI.sprite = vehicleData.VehicleImage;
            }
            vehicleNameUI.text = vehicleData.VehicleName;
            if(vehicleData.isPurchased)
            {
                buyButtonUI.interactable = false;
                vehiclePriceUI.text = "Purchased";
            }
            else
            {
                buyButtonUI.interactable = true;
                vehiclePriceUI.text = vehicleData.VehiclePrice.ToString();
                buyButtonUI.onClick.AddListener(() => BuyVehicle(vehicleData));
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Vehicle data is null.");
        }
    }

    private void BuyVehicle(VehicleUIItemData vehicleItem)
    {
        var isPurchased = GameManager.Instance.PurchaseVehicleItem(vehicleItem);
        if (isPurchased)
        {
            buyButtonUI.interactable = false;
            vehiclePriceUI.text = "Purchased";
        }
    }

    
}
