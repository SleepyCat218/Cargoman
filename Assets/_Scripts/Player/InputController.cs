using UnityEngine;

namespace Cargoman
{
    public class InputController : MonoBehaviour
    {
        private PlayerMovementController _movementController;
        private PlayerCargoPicker _cargoPicker;

        private void Awake()
        {
            _movementController = GetComponent<PlayerMovementController>();
            _cargoPicker = GetComponent<PlayerCargoPicker>();
        }

        private void FixedUpdate()
        {
            _movementController?.Move(Input.GetAxis("Horizontal"), Input.GetAxis("MoveForward"), Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _cargoPicker.InteractWithCargo(_movementController);
            }
        }
    }

}