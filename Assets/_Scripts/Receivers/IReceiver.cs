using UnityEngine;

namespace Cargoman
{
    public interface IReceiver
    {
        Transform GetCargoTransform();
        void ReceiveCargo(IPickable cargo);
    }
}