using Configs;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class VehicleData
    {
        public int vehicleId;
        public string vehicleName;
        public Sprite indicatorIcon;
        public GameObject prefab;
        public int unlockAtStreak = 0;
        public float spawnWeight = 1f;
        public VehicleEmojiData[] emojiData;
        public VehicleRewardData rewardData;
        public VehicleEmojiData GetRandomEmoji() => emojiData[UnityEngine.Random.Range(0, emojiData.Length)];
    }

    [System.Serializable]
    public class VehicleEmojiData
    {
        public GameObject emoji;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class VehicleRewardData
    {
        public int xp;
        public int coins;
    }
    public enum VehicleType
    {
        Slow,
        Normal,
        Fast,
        Special
    }

    [Serializable]
    public class VehicleRawData
    {
        public VehicleType vehicleType;
        public float minSpeed;
        public float maxSpeed;
        public List<VehicleDataConfig> vehicleDataConfigs;
    }
}
