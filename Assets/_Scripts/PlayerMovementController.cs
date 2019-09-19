using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
        }

        public void Move(float forwardMove, float rotation, float timescaleMultiplier)
        {
            _movement.Rotate(forwardMove, Time.fixedDeltaTime);
            _movement.Move(rotation, Time.fixedDeltaTime);
        }
    }
}