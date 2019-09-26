using UnityEngine;

namespace Cargoman
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerMovementController : MonoBehaviour
    {
        public bool CargoHolder = false;

        [SerializeField] private float _speedModifier = 1f;

        private PlayerMovement _movement;
        private bool _canMove = true;

        #region "Properties";
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

        public float SpeedModifier
        {
            get => _speedModifier;
            set
            {
                if (value <= 0)
                {
                    value = 1f;
                }
                _speedModifier = value;

                _movement?.SetMovementAnimationSpeed(_speedModifier);
            }
        }
        #endregion;

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