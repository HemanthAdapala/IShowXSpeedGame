using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "VehicleDataRewardConfig", menuName = "Configs/VehicleDataRewardConfig")]
    public class VehicleDataRewardConfig : ScriptableObject
    {
        public VehicleRewardData vehicleRewardData;
    }
}
