using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cargoman
{
    public class CargoSubmitter : MonoBehaviour
    {
        [SerializeField] private float _cargoStartWait = 3f;
        [SerializeField] private float _cargoSpawnWait = 0f;
        [SerializeField] private Transform _cargoSpawner;
        [SerializeField] private List<GameObject> _cargoList = new List<GameObject>();

        private List<CargoTypeStruct> _cargoTypes = new List<CargoTypeStruct>();
        private Transform _currentCargo;

        public List<CargoTypeStruct> CargoTypes { get => _cargoTypes; }

        public IEnumerator SpawnCargo()
        {
            yield return new WaitForSeconds(_cargoStartWait);

            while(true)
            {
                if (_currentCargo == null)
                {
                    GameObject newObject = Instantiate(_cargoList[Random.Range(0, _cargoList.Count)], _cargoSpawner.position, _cargoSpawner.rotation);
                    _currentCargo = newObject.transform;
                    CargoPickable pickable = newObject.GetComponent<CargoPickable>();
                    pickable.cleanSubmitterQueue = ClearCurrentCargo;
                    yield return new WaitForSeconds(_cargoSpawnWait);
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void Start()
        {
            StartCoroutine(SpawnCargo());
        }

        public void ClearCurrentCargo()
        {
            _currentCargo = null;
        }

        private void Awake()
        {
            foreach (var item in _cargoList)
            {
                CargoPickable pickable = item.GetComponent<CargoPickable>();

                _cargoTypes.Add(new CargoTypeStruct(pickable.cargoType, pickable.CargoImage));
            }
        }
    }
}