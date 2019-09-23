using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerMovement _movement;
        private bool _canMove = true;
        public bool CanMove
        {
            get
            {
                return _canMove;
            }
            set
            {
                _canMove = value;
            }
        }

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
        }

        public void Move(float forwardMove, float rotation, float timescaleMultiplier)
        {
            if(!CanMove)
            {
                return;
            }
            _movement.Rotate(forwardMove, Time.fixedDeltaTime);
            _movement.Move(rotation, Time.fixedDeltaTime);
        }
    }
}