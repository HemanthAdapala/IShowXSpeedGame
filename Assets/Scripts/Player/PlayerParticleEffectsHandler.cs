using System;
using Configs;
using UnityEngine;

namespace Player
{
    public class PlayerParticleEffectsHandler : MonoBehaviour
    {
        [SerializeField] private Transform explosionTextEffectTransform;
        [SerializeField] private Transform bloodEffectTransform;
        
        [SerializeField] private PlayerParticleEffectsConfig particleEffectsConfig;

        private void Awake()
        {
            if (particleEffectsConfig is null)
            {
                Resources.Load("ScriptableObjects/PlayerParticleEffectsConfig", typeof(PlayerParticleEffectsConfig));
            }
        }
    }
}
