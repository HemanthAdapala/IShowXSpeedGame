using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "VehicleDataConfig", menuName = "Configs/VehicleDataConfig")]
    public class VehicleDataConfig : ScriptableObject
    {
        public VehicleData vehicleData;
    }
}
