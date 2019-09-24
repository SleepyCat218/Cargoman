using UnityEngine;

namespace Cargoman
{
    public class CargoReceiver : MonoBehaviour, IReceiver
    {
        [SerializeField] private Transform cargoTransform;

        public Transform GetCargoTransform()
        {
            return cargoTransform;
        }
    }
}