using System.Collections.Generic;
using UnityEngine;

namespace Cargoman
{
    public class Orders : MonoBehaviour
    {
        public Queue<CargoType> GetOrder()
        {
            Queue<CargoType> newQueue = new Queue<CargoType>();
            for (int i = 0; i < 1; i++)
            {
                CargoType type = (CargoType)Random.Range(0, System.Enum.GetValues(typeof(CargoType)).Length);
                //Debug.Log(type);
                newQueue.Enqueue(type);
            }
            return newQueue;
        }
    }
}