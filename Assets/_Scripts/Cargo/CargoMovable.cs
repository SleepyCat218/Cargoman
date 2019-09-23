using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(Rigidbody))]
    public class CargoMovable : MonoBehaviour, IMovable
    {
        private Rigidbody _rigidbody;
        private Transform _transform;
        private Collider _collider;

        [SerializeField] private bool _needMove;
        [SerializeField] private float _speed = 1f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _transform = transform;
        }

        private void StopMoving()
        {
            _needMove = false;
            _collider.isTrigger = false;
            _rigidbody.isKinematic = true;
        }

        private void StartMoving()
        {
            _needMove = true;
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;
        }

        public void SwitchMoving(bool needMove)
        {
            if (needMove)
            {
                StartMoving();
            }
            else
            {
                StopMoving();
            }
        }

        void FixedUpdate()
        {
            if (_needMove)
            {
                _rigidbody.MovePosition(_transform.position + _transform.right * _speed * Time.fixedDeltaTime);
            }
        }
    }
}