using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cargoman
{
    public class PlayerCargoPicker : MonoBehaviour
    {
        [SerializeField] private Transform cargoParentTransform;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _pickDelay = 0.75f, _putDelay = 0.75f, _dropDelay = 0.1f;
        [SerializeField] private float _pickAnimationDelay = 1.06f, _putAnimationDelay = 1.06f, _dropAnimationDelay = 0.2f;


        private List<ICargo> _possiblePicks = new List<ICargo>();
        private ICargo _currentPick;
        private ICargo CurrentPick
        {
            get => _currentPick;
            set
            {
                if (_currentPick != value)
                {
                    _currentPick?.StopHightlightObject();
                    _currentPick = value;
                    if (_pickedCargo == null)
                    {
                        _currentPick?.HighlightObject();
                    }
                }
            }
        }
        private ICargo _pickedCargo;

        private IReceiver _cargoReceiver;
        private IReceiver CargoReceiver
        {
            get => _cargoReceiver;
            set
            {
                if (_cargoReceiver != value)
                {
                    _cargoReceiver?.StopHightlightObject();
                    _cargoReceiver = value;
                    _cargoReceiver?.HighlightObject();
                }
            }
        }
        private List<IReceiver> _possibleReceivers = new List<IReceiver>();

        private bool _canInteract = true;

        private void OnTriggerEnter(Collider other)
        {
            IReceiver cargoReceiver = other.GetComponent<IReceiver>();
            ICargo cargo = other.GetComponent<ICargo>();

            if (cargoReceiver != null)
            {
                _possibleReceivers.Add(cargoReceiver);
            }

            if (cargo != null && cargo.CanBePickable)
            {
                _possiblePicks.Add(cargo);
            }
        }

        private void Update()
        {
            if(_possiblePicks.Count > 0 && _pickedCargo == null)
            {
                CurrentPick = GetClosiestObject<ICargo>(_possiblePicks);
            }
            if(_possibleReceivers.Count > 0)
            {
                CargoReceiver = GetClosiestObject<IReceiver>(_possibleReceivers);
            }
        }

        private T GetClosiestObject<T>(List<T> possibleObjects) where T : IInteractable
        {
            T closiestCargo = default(T);
            float minSqrDistance = Mathf.Infinity;
            foreach (var item in possibleObjects)
            {
                float sqrDistance = item.GetSqrMagnitude(transform);
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closiestCargo = item;
                }
            }
            return closiestCargo;
        }

        private void OnTriggerExit(Collider other)
        {
            IReceiver cargoReceiver = other.GetComponent<IReceiver>();
            ICargo cargo = other.GetComponent<ICargo>();

            if (cargoReceiver != null)
            {
                _possibleReceivers.Remove(cargoReceiver);
                CargoReceiver = null;
            }

            if (cargo != null)
            {
                _possiblePicks.Remove(cargo);
                CurrentPick = null;
            }

        }

        private IEnumerator PickCargo(PlayerMovementController playerMovement, ICargo pickableObject)
        {
            _possiblePicks.Remove(pickableObject);
            yield return new WaitForSeconds(_pickDelay);
            _pickedCargo = pickableObject.Pick(cargoParentTransform);
            CurrentPick = null;
            yield return new WaitForSeconds(Mathf.Abs(_pickAnimationDelay - _pickDelay));
            playerMovement.CanMove = true;
            playerMovement.SpeedModifier = pickableObject.SpeedModifier;
            playerMovement.CargoHolder = true;
            _canInteract = true;
        }

        private IEnumerator DropCargo(PlayerMovementController playerMovement, ICargo objectToDrop)
        {
            _pickedCargo = null;
            _possiblePicks.Remove(objectToDrop);
            yield return new WaitForSeconds(_dropDelay);
            objectToDrop.DropCargo();
            yield return new WaitForSeconds(Mathf.Abs(_dropAnimationDelay - _dropDelay));
            playerMovement.CanMove = true;
            playerMovement.SpeedModifier = 1f;
            playerMovement.CargoHolder = false;
            _canInteract = true;
        }

        private IEnumerator PutCargo(PlayerMovementController playerMovement, ICargo objectToPut)
        {
            _pickedCargo = null;
            _possiblePicks.Remove(objectToPut);
            yield return new WaitForSeconds(_putDelay);
            objectToPut.PutCargo(CargoReceiver.GetCargoTransform());
            CargoReceiver.ReceiveCargo(objectToPut);
            yield return new WaitForSeconds(Mathf.Abs(_putAnimationDelay - _putDelay));
            playerMovement.CanMove = true;
            playerMovement.SpeedModifier = 1f;
            playerMovement.CargoHolder = false;
            _canInteract = true;
        }

        public void InteractWithCargo(PlayerMovementController playerMovement)
        {
            if(!_canInteract)
            {
                return;
            }

            if (_pickedCargo != null)
            {
                _canInteract = false;
                playerMovement.CanMove = false;
                if (CargoReceiver == null)
                {
                    DropAnimation();
                    StartCoroutine(DropCargo(playerMovement, _pickedCargo));
                }
                else
                {
                    PutAnimation();
                    StartCoroutine(PutCargo(playerMovement, _pickedCargo));
                }
            }
            else if(CurrentPick != null && CurrentPick.CanBePickable)
            {
                playerMovement.CanMove = false;
                _canInteract = false;
                PickAnimation();
                StartCoroutine(PickCargo(playerMovement, CurrentPick));
            }

        }

        private void PickAnimation()
        {
            _animator.SetInteger("ActionMode", 0);
            _animator.SetTrigger("ActionPickObject");
            _animator.SetInteger("MoveMode", 1);
        }

        private void PutAnimation()
        {
            _animator.SetInteger("ActionMode", 1);
            _animator.SetTrigger("ActionReplaceObject");
            _animator.SetInteger("MoveMode", 0);
        }

        private void DropAnimation()
        {
            _animator.SetInteger("ActionMode", 1);
            _animator.SetTrigger("ActionDropObject");
            _animator.SetInteger("MoveMode", 0);
        }
    }
}