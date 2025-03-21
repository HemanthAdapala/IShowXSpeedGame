using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "VehicleDataRewardConfig", menuName = "Configs/VehicleDataRewardConfig")]
    public class VehicleDataRewardConfig : ScriptableObject
    {
        public VehicleRewardData vehicleRewardData;
    }
}
