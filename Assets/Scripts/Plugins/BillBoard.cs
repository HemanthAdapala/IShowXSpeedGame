using UnityEngine;

namespace Plugins
{
    public class BillBoard : MonoBehaviour
    {
        private Quaternion _initialRotation;

        private void Start()
        {
            _initialRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            transform.rotation = _initialRotation;
        }
    }
}
