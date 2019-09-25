using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cargoman
{
    public class ReceiverOrderManager : MonoBehaviour
    {
        
        [SerializeField] private Queue<CargoType> _orderQueue = new Queue<CargoType>();
        [SerializeField] private float _minTimeToGetNewOrder = 5f, _maxTimeToGetNewOrder = 15f, _timerPercentForRedLight = 25, _timePerOneCargo = 60f;

        [SerializeField] private Light _waitLight, _failOrderSoonLight;
        [SerializeField] private bool _redLightTurnedOn = false;
        [SerializeField] private float failLightFreq = 2f;

        private bool _canGetCargo = true;
        private float _orderTimer = 10f;
        private Coroutine _orderTimerCoroutine;
        private Coroutine _failLightCoroutine;
        private float _startIntencity;
        private int _scoreReward;

        public void SetOrderQueue()
        {
            _orderQueue = SetOrder();
        }

        private void Awake()
        {
            StartCoroutine(GetNewOrder());
            _startIntencity = _failOrderSoonLight.intensity;
        }

        public void CheckCargo(IPickable cargo)
        {
            if (!_canGetCargo || _orderQueue.Count == 0)
            {
                return;
            }

            if (cargo.cargoType != _orderQueue.Peek())
            {
                OrderCancel();
                return;
            }

            _orderQueue.Dequeue();
            if (_orderQueue.Count == 0)
            {
                OrderComplete();
            }
        }

        private void StopConveyorWork()
        {
            StopCoroutine(_orderTimerCoroutine);
            StartCoroutine(GetNewOrder());
            _canGetCargo = false;
            TurnOffAllLights();
        }

        private void OrderComplete()
        {
            ScoreManager.Instance.AddScore(_scoreReward);
            Debug.Log("complete");
            StopConveyorWork();
        }

        private void OrderCancel()
        {
            Debug.Log("OrderCancel");
            StopConveyorWork();
        }

        private Queue<CargoType> SetOrder()
        {
            Queue<CargoType> newQueue = new Queue<CargoType>();
            int cargoInOrder = CalculateCargoInOrder(ScoreManager.Instance.Score);
            for (int i = 0; i < cargoInOrder; i++)
            {
                CargoType type = (CargoType)Random.Range(0, System.Enum.GetValues(typeof(CargoType)).Length);
                Debug.Log(type);
                newQueue.Enqueue(type);
            }
            _scoreReward = CalculateOrderReward(cargoInOrder);
            _orderTimer = CalculateOrderTimer(cargoInOrder);
            return newQueue;
        }

        #region "Lights";
        private void TurnOffAllLights()
        {
            _waitLight.enabled = false;
            if (_failOrderSoonLight.enabled)
            {
                TurnOffFailLight();
            }
        }

        private void TurnOffFailLight()
        {
            StopCoroutine(_failLightCoroutine);
            _failOrderSoonLight.enabled = false;
            _redLightTurnedOn = false;
        }

        private void TurnOnFailLight()
        {
            _failLightCoroutine = StartCoroutine(TurnOnFailLightCoroutine());
        }

        private IEnumerator TurnOnFailLightCoroutine()
        {
            _failOrderSoonLight.enabled = true;
            _redLightTurnedOn = true;

            float timer = 0f;
            bool intencityDir = false;
            while (true)
            {
                if (!intencityDir)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        intencityDir = true;
                    }
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer >= failLightFreq)
                    {
                        intencityDir = false;
                    }
                }
                _failOrderSoonLight.intensity = (timer / failLightFreq) * _startIntencity;
                yield return null;
            }
        }
        #endregion;

        private IEnumerator GetNewOrder()
        {
            yield return new WaitForSeconds(Random.Range(_minTimeToGetNewOrder, _maxTimeToGetNewOrder));
            SetOrderQueue();
            _waitLight.enabled = true;
            _canGetCargo = true;
            _orderTimerCoroutine = StartCoroutine(StartOrderTimer());
        }

        private IEnumerator StartOrderTimer()
        {
            float timer = _orderTimer;
            float redLightStart = _orderTimer * _timerPercentForRedLight / 100;
            while (timer >= 0)
            {
                if(!_redLightTurnedOn && timer < redLightStart)
                {
                    TurnOnFailLight();
                }
                timer -= Time.deltaTime;
                Debug.Log(timer);
                yield return null;
            }
            OrderCancel();
        }

        private float CalculateOrderTimer(int cargoCount)
        {
            float timer = cargoCount * _timePerOneCargo;
            return timer;
        }

        private int CalculateOrderReward(int cargoCount)
        {
            int reward = cargoCount * 5;
            return reward;
        }

        private int CalculateCargoInOrder(int scoreValue)
        {
            if (scoreValue <= 10)
            {
                return 1;
            }
            int cargoInOrder = scoreValue / 10;
            return cargoInOrder;
        }
    }
}