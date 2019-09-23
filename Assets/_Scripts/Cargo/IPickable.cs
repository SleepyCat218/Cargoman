using UnityEngine;

namespace Cargoman
{
    public interface IPickable
    {
        IPickable Pick(Transform cargoParentTransform);
        void DropCargo();
        void PutCargo(Transform cargoTransform);
    }
}