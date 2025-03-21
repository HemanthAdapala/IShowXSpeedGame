using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class VehicleRewardData
    {
        public VehicleType vehicleType;
        public int xp;
        public int coins;
    }

    public enum VehicleType
    {
        Tier1,
        Tier2,
        Tier3,
        Tier4,
        Tier5
    }
}