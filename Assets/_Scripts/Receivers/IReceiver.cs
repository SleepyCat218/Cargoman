using UnityEngine;

namespace Cargoman
{
    public interface IReceiver : IInteractable
    {
        Transform GetCargoTransform();
        void ReceiveCargo(ICargo cargo);
    }
}