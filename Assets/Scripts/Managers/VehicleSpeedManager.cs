using Configs;
using UnityEngine;

namespace Managers
{
    public class VehicleSpeedManager : MonoBehaviour
    {
        private GameConfig _config;
        private int _currentStreak;
        private float _currentSpeed;

        public float CurrentSpeed => _currentSpeed;
        public int CurrentStreak => _currentStreak;

        public void Initialize(GameConfig config)
        {
            _config = config;
            _currentSpeed = _config.initialVehicleSpeed;
            _currentStreak = GameSessionManager.Instance.GetCurrentStreak();
        }

        public void OnSuccessfulJump()
        {
            _currentStreak = GameSessionManager.Instance.GetCurrentStreak();
            UpdateSpeed();
        }

        public void OnJumpFailed()
        {
            _currentStreak = 0;
            ResetToBaseSpeed();
        }

        private void UpdateSpeed()
        {
            float speedMultiplier = Mathf.Pow(_config.speedIncreaseFactor, _currentStreak);
            _currentSpeed = Mathf.Min(
                _config.initialVehicleSpeed * speedMultiplier,
                _config.maxVehicleSpeed
            );
        }

        private void ResetToBaseSpeed()
        {
            _currentSpeed = _config.initialVehicleSpeed;
        }
    }
}