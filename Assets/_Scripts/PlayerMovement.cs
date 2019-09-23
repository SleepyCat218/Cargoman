using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private bool _canMove;
        private CharacterController _characterController;

        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            _canMove = true;
            _characterController = GetComponent<CharacterController>();
        }

        private void AnimateMove(float moveForwardValue)
        {
            _animator.SetFloat("MoveSpeed", moveForwardValue);
        }

        public void Move(float moveForwardValue, float timescaleMultiplier = 1)
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
            movementVector = transform.TransformDirection(movementVector);
            _characterController.Move(movementVector * _speed * timescaleMultiplier);
        }

        public void Rotate(float rotation, float timescaleMultiplier = 1)
        {
            if (!_canMove)
            {
                return;
            }
            transform.Rotate(Vector3.up, rotation * _rotationSpeed * timescaleMultiplier);
        }
    }
}