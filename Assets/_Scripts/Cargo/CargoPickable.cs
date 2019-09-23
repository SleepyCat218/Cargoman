using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(Rigidbody))]
    public class CargoPickable : MonoBehaviour, IPickable
    {
        private Transform _transform;
        private Rigidbody _rigidbody;

        public IPickable Pick(Transform cargoParentTransform)
        {
            _rigidbody.isKinematic = true;
            _transform.position = cargoParentTransform.position;
            _transform.rotation = cargoParentTransform.rotation;
            _transform.parent = cargoParentTransform;
            return this;
        }

        public void DropCargo()
        {
            _transform.parent = null;
            _rigidbody.isKinematic = false;
        }

        public void PutCargo(Transform cargoTransform)
        {

            _transform.parent = null;
            _transform.position = cargoTransform.position;
            _transform.rotation = cargoTransform.rotation;
            IMovable cargoMovable = GetComponent<IMovable>();
            cargoMovable.SwitchMoving(true);
        }

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}