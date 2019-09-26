using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float _speedModifier = 1f;
        public float SpeedModifier
        {
            get => _speedModifier;
            set
            {
                if(value <= 0)
                {
                    value = 1f;
                }
                _speedModifier = value;
                
                _movement?.SetMovementAnimationSpeed(_speedModifier);
            }
        }

        public bool CargoHolder = false;
        
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
            _movement.Move(rotation, _speedModifier, CargoHolder, Time.fixedDeltaTime);
        }
    }
}