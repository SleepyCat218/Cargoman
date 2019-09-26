using UnityEngine;

namespace Cargoman
{
    public struct CargoTypeStruct
    {
        public CargoTypeStruct(CargoType type, Sprite image)
        {
            cargoType = type;
            cargoImage = image;
        }
        public CargoType cargoType;
        public Sprite cargoImage;
    }
}