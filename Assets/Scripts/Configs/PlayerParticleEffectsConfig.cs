using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayerParticleEffectsConfig",fileName = "PlayerParticleEffectsConfig")]
    public class PlayerParticleEffectsConfig : ScriptableObject
    {
        public GameObject[] bloodEffects;
        public GameObject[] hitEffects;
        public GameObject[] jumpEffects;
        
        
        
        public GameObject GetRandomBloodEffect() => bloodEffects[Random.Range(0, bloodEffects.Length)];
        
        public GameObject GetRandomHitEffect() => hitEffects[Random.Range(0, hitEffects.Length)];
        
        public GameObject GetRandomJumpEffect() => jumpEffects[Random.Range(0, jumpEffects.Length)];
        
    }
}
