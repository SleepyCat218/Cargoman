using UnityEngine;

namespace Cargoman
{
    public interface IPickable
    {
        bool CanBePickable { get; set; }
        IPickable Pick(Transform cargoParentTransform);
        void DropCargo();
        void PutCargo(Transform cargoTransform);
    }
}