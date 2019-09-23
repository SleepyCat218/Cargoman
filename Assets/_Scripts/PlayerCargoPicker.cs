using System.Collections;
using UnityEngine;

namespace Cargoman
{
    public class PlayerCargoPicker : MonoBehaviour
    {
        [SerializeField] private Transform cargoParentTransform;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _pickDelay = 0.75f, _putDelay = 0.75f, _dropDelay = 0.1f;
        [SerializeField] private float _pickAnimationDelay = 1.06f, _putAnimationDelay = 1.06f, _dropAnimationDelay = 0.2f;


        private IPickable _possiblePick;
        private IPickable _pickedCargo;
        private IReceiver _cargoReceiver;
        private bool _canInteract = true;

        private void OnTriggerEnter(Collider other)
        {
            IReceiver cargoReceiver = other.GetComponent<IReceiver>();
            IPickable cargo = other.GetComponent<IPickable>();

            if (cargoReceiver != null)
            {
                _cargoReceiver = cargoReceiver;
            }

            if (cargo != null && _possiblePick == null)
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

        private IEnumerator PickCargo(PlayerMovementController playerMovement, IPickable pickableObject)
        {
            yield return new WaitForSeconds(_pickDelay);
            _pickedCargo = pickableObject.Pick(cargoParentTransform);
            yield return new WaitForSeconds(Mathf.Abs(_pickAnimationDelay - _pickDelay));
            playerMovement.CanMove = true;
            _canInteract = true;
        }

        private IEnumerator DropCargo(PlayerMovementController playerMovement, IPickable objectToDrop)
        {
            yield return new WaitForSeconds(_dropDelay);
            objectToDrop.DropCargo();
            _pickedCargo = null;
            yield return new WaitForSeconds(Mathf.Abs(_dropAnimationDelay - _dropDelay));
            playerMovement.CanMove = true;
            _canInteract = true;
        }

        private IEnumerator PutCargo(PlayerMovementController playerMovement, IPickable objectToPut)
        {
            yield return new WaitForSeconds(_putDelay);
            objectToPut.PutCargo(_cargoReceiver.GetCargoTransform());
            _pickedCargo = null;
            yield return new WaitForSeconds(Mathf.Abs(_putAnimationDelay - _putDelay));
            playerMovement.CanMove = true;
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
                if (_cargoReceiver == null)
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
            else if(_possiblePick != null && _possiblePick.CanBePickable)
            {
                playerMovement.CanMove = false;
                _canInteract = false;
                PickAnimation();
                StartCoroutine(PickCargo(playerMovement, _possiblePick));
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