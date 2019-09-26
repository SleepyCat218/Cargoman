using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cargoman
{
    public class ReceiverOrderManager : MonoBehaviour
    {
        [SerializeField] private CargoSubmitter _submitter;
        [SerializeField] private GameObject _receiverGui;
        [SerializeField] private Transform _receiversParentGui;
        [SerializeField] private float _minTimeToGetNewOrder = 5f, _maxTimeToGetNewOrder = 15f, _timerPercentForRedLight = 25, _timePerOneCargo = 60f;
        [SerializeField] private Light _waitLight, _failOrderSoonLight;
        [SerializeField] private bool _redLightTurnedOn = false;
        [SerializeField] private float failLightFreq = 2f;

        private Queue<CargoTypeStruct> _orderQueue = new Queue<CargoTypeStruct>();
        private bool _canGetCargo = true;
        private float _orderTimer = 10f;
        private Coroutine _orderTimerCoroutine;
        private Coroutine _failLightCoroutine;
        private float _startIntencity;
        private int _scoreReward;
        private ReceiverGuiPanelScript _guiScript;

        private void Awake()
        {
            StartCoroutine(GetNewOrder());
            _startIntencity = _failOrderSoonLight.intensity;
        }

        #region "GUI"
        public void CreateReceiverGui()
        {
            GameObject gui = Instantiate(_receiverGui, _receiversParentGui);
            _guiScript = gui.GetComponent<ReceiverGuiPanelScript>();
        }
        #endregion;

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

        #region "Order functions";
        private Queue<CargoTypeStruct> SetOrder()
        {
            Queue<CargoTypeStruct> newQueue = new Queue<CargoTypeStruct>();
            int cargoInOrder = CalculateCargoInOrder(ScoreManager.Instance.Score);
            for (int i = 0; i < cargoInOrder; i++)
            {
                CargoTypeStruct type = _submitter.CargoTypes[Random.Range(0, _submitter.CargoTypes.Count)];
                newQueue.Enqueue(type);
            }

            _guiScript.UpdateCargoImage(newQueue.Peek().cargoImage);
            _guiScript.UpdateOrderTimer(_orderTimer, _orderTimer);
            _guiScript.UpdateCargoLeftValue(cargoInOrder);
            _scoreReward = CalculateOrderReward(cargoInOrder);
            _orderTimer = CalculateOrderTimer(cargoInOrder);
            return newQueue;
        }

        private IEnumerator GetNewOrder()
        {
            yield return new WaitForSeconds(Random.Range(_minTimeToGetNewOrder, _maxTimeToGetNewOrder));
            _orderQueue = SetOrder();
            _waitLight.enabled = true;
            _canGetCargo = true;
            _orderTimerCoroutine = StartCoroutine(StartOrderTimer());
        }

        private IEnumerator StartOrderTimer()
        {
            float timer = _orderTimer, oldTimer = timer;
            float redLightStart = _orderTimer * _timerPercentForRedLight / 100;
            while (timer >= 0)
            {
                if(!_redLightTurnedOn && timer < redLightStart)
                {
                    TurnOnFailLight();
                }
                timer -= Time.deltaTime;
                if ((int)timer != (int)oldTimer)
                {
                    _guiScript.UpdateOrderTimer(timer, _orderTimer);
                }
                oldTimer = timer;

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

        private void OrderComplete()
        {
            ScoreManager.Instance.AddScore(_scoreReward);
            StopConveyorWork();
        }

        private void OrderCancel()
        {
            StopConveyorWork();
        }

        public void CheckCargo(ICargo cargo)
        {
            if (!_canGetCargo || _orderQueue.Count == 0)
            {
                return;
            }

            if (cargo.cargoType != _orderQueue.Peek().cargoType)
            {
                OrderCancel();
                return;
            }

            _orderQueue.Dequeue();
            _guiScript.UpdateCargoLeftValue(_orderQueue.Count);
            if (_orderQueue.Count == 0)
            {
                OrderComplete();
                return;
            }
            _guiScript.UpdateCargoImage(_orderQueue.Peek().cargoImage);
        }

        private void StopConveyorWork()
        {
            _guiScript.UpdateCargoImage(null);
            _guiScript.UpdateCargoLeftValue(0);
            _guiScript.UpdateOrderTimer(0, 0);
            StopCoroutine(_orderTimerCoroutine);
            StartCoroutine(GetNewOrder());
            _canGetCargo = false;
            TurnOffAllLights();
        }
        #endregion;
    }
}