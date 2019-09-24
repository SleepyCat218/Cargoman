using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(Rigidbody))]
    public class CargoPickable : MonoBehaviour, IPickable
    {
        public bool CanBePickable { get; set; }
        private Transform _transform;
        private Rigidbody _rigidbody;
        [SerializeField] private float _timeToDestroy = 2f;

        public delegate void CleanSubmitterQueueDelegate();
        public CleanSubmitterQueueDelegate cleanSubmitterQueue; 

        public IPickable Pick(Transform cargoParentTransform)
        {
            CanBePickable = false;
            _rigidbody.isKinematic = true;
            _transform.position = cargoParentTransform.position;
            _transform.rotation = cargoParentTransform.rotation;
            _transform.parent = cargoParentTransform;
            if(cleanSubmitterQueue != null)
            {
                cleanSubmitterQueue();
                cleanSubmitterQueue = null;
            }
            return this;
        }

        public void DropCargo()
        {
            CanBePickable = false;
            _transform.parent = null;
            _rigidbody.isKinematic = false;
            DestroyCargo();
        }

        public void PutCargo(Transform cargoTransform)
        {
            CanBePickable = false;
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
            CanBePickable = true;
        }

        private void DestroyCargo()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
                Collider col = child.gameObject.AddComponent<BoxCollider>();
            }
            Destroy(gameObject, _timeToDestroy);
        }
    }
}