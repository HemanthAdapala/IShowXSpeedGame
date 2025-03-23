using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [System.Serializable]
    public class VehicleData
    {
        public ChallengeTiers tier;
        public string vehicleName;
        public Sprite indicatorIcon;
        public VehicleEmojiData[] emojiData;
    }

    [System.Serializable]
    public class VehicleEmojiData
    {
        public GameObject emoji;
        public AudioClip audioClip;
    }

    public enum ChallengeTiers
    {
        Beginner,
        Skilled,
        Expert,
        Extreme,
        Insane
    }
}
