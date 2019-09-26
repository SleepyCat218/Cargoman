using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f, _cargoHolderModifier = 0.3f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private Animator _animator;

        private const float gravityConst = -9.8f;
        private bool _canMove;
        private CharacterController _characterController;

        private void Awake()
        {
            _canMove = true;
            _characterController = GetComponent<CharacterController>();
        }

        #region "Moving and rotating";
        public void Move(float moveForwardValue, float cargoModifier, bool cargoHolder, float timescaleMultiplier = 1)
        {
            if (_characterController == null || !_canMove)
            {
                return;
            }

            AnimateMove(moveForwardValue);
            Vector3 movementVector = moveForwardValue * Vector3.forward;
            if (movementVector.magnitude > 1f)
            {
                movementVector.Normalize();
            }
            movementVector = transform.TransformDirection(movementVector) * _speed * cargoModifier;
            if(cargoHolder)
            {
                movementVector *= _cargoHolderModifier;
            }
            movementVector += Vector3.up * gravityConst;
            _characterController.Move(movementVector * timescaleMultiplier);
        }

        public void Rotate(float rotation, float timescaleMultiplier = 1)
        {
            if (!_canMove)
            {
                return;
            }
            transform.Rotate(Vector3.up, rotation * _rotationSpeed * timescaleMultiplier);
        }
        #endregion;

        #region "Animations";
        public void SetMovementAnimationSpeed(float animationSpeed)
        {
            _animator.SetFloat("BootsSpeed", animationSpeed);
        }

        private void AnimateMove(float moveForwardValue)
        {
            _animator.SetFloat("MoveSpeed", moveForwardValue);
        }
        #endregion
    }
}