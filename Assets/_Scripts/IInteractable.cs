using UnityEngine;

namespace Cargoman
{
    public interface IInteractable
    {
        float GetSqrMagnitude(Transform playerTransform);
        void HighlightObject();
        void StopHightlightObject();
    }
}