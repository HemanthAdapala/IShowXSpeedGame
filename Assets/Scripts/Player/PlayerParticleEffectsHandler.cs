using System;
using Configs;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerParticleEffectsHandler : MonoBehaviour
    {
        [Header("Effect Transforms")]
        [SerializeField] private Transform hitEffectTransform;
        [SerializeField] private Transform bloodEffectTransform;

        [Header("Particle Effects Config")]
        [SerializeField] private PlayerParticleEffectsConfig particleEffectsConfig;
        
        
        private void Awake()
        {
            if (particleEffectsConfig == null)
            {
                particleEffectsConfig = Resources.Load<PlayerParticleEffectsConfig>("ScriptableObjects/PlayerParticleEffectsConfig");
            }
        }

        private void OnEnable()
        {
            GameEventManager.OnFailedJump += OnFailedJump_GameEventManager;
        }

        private void OnFailedJump_GameEventManager()
        {
            Debug.Log("Player collision detected_PlayerEffectsHandler!");
            PlayBloodEffect();
            PlayHitEffect();
        }

        /// <summary>
        /// Spawns and plays a particle effect at a given position.
        /// </summary>
        private void PlayEffect(GameObject effectPrefab, Vector3 position)
        {
            if (effectPrefab == null) return;

            GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);
            ParticleSystem ps = effectInstance.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();
                Destroy(effectInstance, ps.main.duration + 0.5f); // Destroy after playing
            }
            else
            {
                Destroy(effectInstance, 2f); // Fallback if no particle system found
            }
        }

        /// <summary>
        /// Plays a random blood effect (e.g., on collision).
        /// </summary>
        private void PlayBloodEffect()
        {
            PlayEffect(particleEffectsConfig.GetRandomBloodEffect(), bloodEffectTransform.position);
        }

        /// <summary>
        /// Plays a random explosion/hit effect (e.g., on streak loss).
        /// </summary>
        private void PlayHitEffect()
        {
            PlayEffect(particleEffectsConfig.GetRandomHitEffect(), hitEffectTransform.position);
        }

        /// <summary>
        /// Plays a random jump effect (e.g., when player jumps).
        /// </summary>
        private void PlayJumpEffect()
        {
            PlayEffect(particleEffectsConfig.GetRandomJumpEffect(), transform.position);
        }

        private void OnDisable()
        {
            GameEventManager.OnFailedJump -= OnFailedJump_GameEventManager;
        }
    }
}
