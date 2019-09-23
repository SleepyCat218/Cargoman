using UnityEngine;

namespace Cargoman
{
    public class PlayerCargoPicker : MonoBehaviour
    {
        [SerializeField] private Transform cargoParentTransform;

        private IPickable _possiblePick;
        private IPickable _pickedCargo;
        private IReceiver _cargoReceiver;

        private void OnTriggerEnter(Collider other)
        {
            IReceiver cargoReceiver = other.GetComponent<IReceiver>();
            IPickable cargo = other.GetComponent<IPickable>();

            if (cargoReceiver != null)
            {
                _cargoReceiver = cargoReceiver;
            }

            if (cargo != null || _possiblePick == null)
            {
                _possiblePick = cargo;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            IReceiver cargoReceiver = other.GetComponent<IReceiver>();
            IPickable cargo = other.GetComponent<IPickable>();

            if (cargoReceiver != null)
            {
                _cargoReceiver = null;
            }

            if (cargo != null)
            {
                _possiblePick = null;
            }

        }

        private void PickCargo()
        {
            if (_possiblePick != null && _pickedCargo == null)
            {
                _pickedCargo = _possiblePick.Pick(cargoParentTransform);
            }
        }

        private void DropCargo()
        {
            _pickedCargo.DropCargo();
            _pickedCargo = null;
        }

        private void PutCargo()
        {
            _pickedCargo.PutCargo(_cargoReceiver.GetCargoTransform());
            _pickedCargo = null;
        }

        public void InteractWithCargo()
        {
            if (_pickedCargo == null)
            {
                PickCargo();
            }
            else
            {
                if (_cargoReceiver == null)
                {
                    DropCargo();
                }
                else
                {
                    PutCargo();
                }
            }
        }
    }
}