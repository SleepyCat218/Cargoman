using UnityEngine;

namespace Cargoman
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private Transform _transform;
        private Vector3 _offset;

        private void Awake()
        {
            _transform = transform;
            if(_player != null)
            {
                _offset = _transform.position - _player.position;
            }
            else
            {
                this.enabled = false;
            }
        }

        private void LateUpdate()
        {
            Vector3 position = _player.position + _offset;
            _transform.position = position;
        }
    }
}