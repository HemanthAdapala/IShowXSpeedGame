using System;
using System.Collections.Generic;
using UnityEngine;

// This class is used to store the configuration data for the shop vehicle item UI
[CreateAssetMenu(fileName = "ShopVehicleItemUIDataConfig", menuName = "Configs/ShopVehicleItemUIDataConfig")]
public class ShopVehicleItemUIDataConfig : ScriptableObject
{
    public List<VehicleUIItemData> VehicleUIItemDataList; // Array of vehicle UI item data

    public List<VehicleUIItemData> GetVehicleUIItemDatas()
    {
        return VehicleUIItemDataList; // Returns the list of vehicle UI item data
    }
}

[System.Serializable]
public class VehicleUIItemData : BaseVehicleDataUI
{
    public Sprite VehicleImage;
    public string VehicleName;
    public int VehiclePrice;
    public int UnlockAtLevel;

    public VehicleUIItemData Clone()
    {
        return new VehicleUIItemData
        {
            vehicleId = this.vehicleId,
            isPurchased = this.isPurchased,
            isUnlocked = this.isUnlocked,
            VehicleImage = this.VehicleImage,
            VehicleName = this.VehicleName,
            VehiclePrice = this.VehiclePrice,
            UnlockAtLevel = this.UnlockAtLevel
        };
    }

}


[System.Serializable]
public class BaseVehicleDataUI
{
    public int vehicleId;
    public bool isPurchased;
    public bool isUnlocked;
}

[System.Serializable]
public class ShopDataWrapper
{
    public List<BaseVehicleDataUI> vehicles;
}
