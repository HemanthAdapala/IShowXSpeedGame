using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour,IScreenBase
{
    [SerializeField] private VehicleItemPrefabUI vehicleItemUIPrefab;
    [SerializeField] private Transform vehicleItemUIParent;
    [SerializeField] private Button backButton;

    //Scriptable object data
    [SerializeField] private ShopVehicleItemUIDataConfig ShopVehicleItemUIDataConfig;

    private List<VehicleUIItemData> vehicleUIItemDatas;
    private List<BaseVehicleDataUI> baseVehicleData;

    private void Awake()
    {
        // Initialize the shop UI with vehicle items
        InitializeShopUI();
    }

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnClickBackButton);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnClickBackButton);
    }

    private void OnClickBackButton()
    {
        UIManager.Instance.GoBack();
    }

    private void Start()
    {
        // Set the data for the shop UI
        SetShopUIData();
    }

    private void InitializeShopUI()
    {
        //JSON Data
        baseVehicleData = GameManager.Instance.GetBaseVehicleDataUI();
        vehicleUIItemDatas = ShopVehicleItemUIDataConfig.GetVehicleUIItemDatas();
    }

    private void SetShopUIData()
    {
        foreach (var vehicleData in vehicleUIItemDatas)
        {
            var data = vehicleData.Clone(); // Safe runtime clone

            var jsonData = GetVehicleJsonDataById(data.vehicleId.ToString());

            if (jsonData.vehicleId == -1)
            {
                Debug.LogWarning($"Vehicle with ID {data.vehicleId} not found in JSON data.");
            }
            else
            {
                data.isPurchased = jsonData.isPurchased;
                data.isUnlocked = jsonData.isUnlocked;
            }

            if (vehicleItemUIPrefab == null)
            {
                Debug.LogError("VehicleItemUIPrefab is not assigned in ShopUI!");
                continue;
            }

            var vehicleItemUI = Instantiate(vehicleItemUIPrefab, vehicleItemUIParent);
            var shopVehicleItemUI = vehicleItemUI.GetComponent<VehicleItemPrefabUI>();
            if (shopVehicleItemUI != null)
            {
                shopVehicleItemUI.SetData(data);
            }
            else
            {
                Debug.LogWarning("VehicleItemUIPrefab is missing VehicleItemPrefabUI component.");
            }
        }
    }



    private (int vehicleId, bool isPurchased, bool isUnlocked) GetVehicleJsonDataById(string id)
    {
        foreach (var jsonData in baseVehicleData)
        {
            if (jsonData.vehicleId.ToString() == id)
            {
                return (jsonData.vehicleId, jsonData.isPurchased, jsonData.isUnlocked);
            }
        }
        Debug.LogWarning($"Vehicle with ID {id} not found.");
        return (-1, false, false); // Use -1 as a default value for vehicleId to match the expected int type  
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
