using Data;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "VehicleDataConfig", menuName = "Configs/VehicleDataConfig")]
    public class VehicleDataConfig : ScriptableObject
    {
        public VehicleData vehicleData;
        
        //Get Random emoji data
        public VehicleEmojiData GetRandomEmoji() => vehicleData.emojiData[Random.Range(0, vehicleData.emojiData.Length)];
    }
}
