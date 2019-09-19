using UnityEngine;

namespace Cargoman
{
    public class InputController : MonoBehaviour
    {
        private PlayerMovementController _movementController;

        private void Awake()
        {
            _movementController = GetComponent<PlayerMovementController>();
        }

        private void FixedUpdate()
        {
            _movementController?.Move(Input.GetAxis("Horizontal"), Input.GetAxis("MoveForward"), Time.fixedDeltaTime);
        }
    }

}