using UnityEngine;

namespace Cargoman
{
    public class CargoReceiver : MonoBehaviour, IReceiver
    {
        [SerializeField] private Transform _cargoTransform;

        private ReceiverOrderManager _orderManager;

        private void Awake()
        {
            _orderManager = GetComponent<ReceiverOrderManager>();
        }

        public Transform GetCargoTransform()
        {
            return _cargoTransform;
        }

        public void ReceiveCargo(ICargo cargo)
        {
            _orderManager.CheckCargo(cargo);
        }
    }
}