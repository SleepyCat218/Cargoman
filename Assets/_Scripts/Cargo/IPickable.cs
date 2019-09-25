using UnityEngine;

namespace Cargoman
{
    public interface ICargo
    {
        Sprite CargoImage { get; }
        CargoType cargoType { get; }
        bool CanBePickable { get; set; }
        ICargo Pick(Transform cargoParentTransform);
        void DropCargo();
        void PutCargo(Transform cargoTransform);
    }
}