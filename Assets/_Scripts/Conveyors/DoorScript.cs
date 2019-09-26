using UnityEngine;

namespace Cargoman
{
    public class DoorScript : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _needStop = false;

        private void OnTriggerEnter(Collider other)
        {
            IMovable cargo = other.GetComponent<IMovable>();
            if (cargo == null)
            {
                return;
            }
            _animator.SetTrigger("Open");
        }

        private void OnTriggerExit(Collider other)
        {
            IMovable cargo = other.GetComponent<IMovable>();
            if (cargo == null)
            {
                return;
            }
            if (_needStop)
            {
                cargo.SwitchMoving(false);
            }
            _animator.SetTrigger("Close");

        }
    }
}