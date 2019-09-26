using UnityEngine;

namespace Cargoman
{
    public class CargoReceiver : MonoBehaviour, IReceiver
    {
        [SerializeField] private Transform _cargoTransform;

        private ReceiverOrderManager _orderManager;
        private Transform _transform;
        private ParticleSystem _highlightParticles;

        private void Awake()
        {
            _orderManager = GetComponent<ReceiverOrderManager>();
            _transform = transform;
            _highlightParticles = GetComponent<ParticleSystem>();
        }

        #region "IReceiver realization";
        public Transform GetCargoTransform()
        {
            return _cargoTransform;
        }

        public void ReceiveCargo(ICargo cargo)
        {
            _orderManager.CheckCargo(cargo);
        }
        #endregion;

        #region "IInteractable realization";
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
        #endregion
    }
}