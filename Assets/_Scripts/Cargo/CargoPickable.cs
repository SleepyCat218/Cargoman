using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(Rigidbody))]
    public class CargoPickable : MonoBehaviour, ICargo
    {
        [SerializeField] private float _speedModifier = 1f;
        public float SpeedModifier
        {
            get => _speedModifier;
        }

        [SerializeField] private Sprite _cargoImage;
        [SerializeField] private CargoType _cargoType;
        public CargoType cargoType { get => _cargoType; }
        public Sprite CargoImage { get => _cargoImage; }
        public bool CanBePickable { get; set; }
        private Transform _transform;
        private Rigidbody _rigidbody;
        private ParticleSystem _highlightParticles;
        [SerializeField] private float _timeToDestroy = 2f;
        [SerializeField] private string _trashLayer = "Trash";

        public delegate void CleanSubmitterQueueDelegate();
        public CleanSubmitterQueueDelegate cleanSubmitterQueue; 

        public ICargo Pick(Transform cargoParentTransform)
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
            Destroy(gameObject, _timeToDestroy);
        }

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            CanBePickable = true;
            _highlightParticles = GetComponent<ParticleSystem>();
        }

        private void DestroyCargo()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
                Collider col = child.gameObject.AddComponent<BoxCollider>();
            }
            Destroy(gameObject, _timeToDestroy);

            int newLayer = LayerMask.NameToLayer(_trashLayer);
            if (newLayer <= 31 && newLayer >= 0)
            {
                foreach (Transform item in _transform)
                {
                    item.gameObject.layer = newLayer;
                }
                gameObject.layer = newLayer;
            }
        }

        public float GetSqrMagnitude(Transform playerTransform)
        {
            return (_transform.position - playerTransform.position).sqrMagnitude;
        }

        public void HighlightObject()
        {
            _highlightParticles.Play();
        }

        public void StopHightlightObject()
        {
            _highlightParticles.Stop();
        }
    }
}