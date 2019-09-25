using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cargoman
{
    public class ReceiverOrderManager : MonoBehaviour
    {
        
        [SerializeField] private Queue<CargoType> _orderQueue = new Queue<CargoType>();
        [SerializeField] private Orders _orders;
        [SerializeField] private float _timeToGetNewOrder = 3f, _timerPercentForRedLight = 25;

        [SerializeField] private Light _waitLight, _failOrderSoonLight;
        [SerializeField] private bool _redLightTurnedOn = false;
        [SerializeField] private float failLightFreq = 2f;

        private bool _canGetCargo = true;
        private float _orderTimer = 10f;
        private Coroutine orderTimerCoroutine;
        private Coroutine failLightCoroutine;
        private float startIntencity;

        public void SetOrderQueue()
        {
            _orderQueue = _orders.GetOrder();
        }

        private void Awake()
        {
            StartCoroutine(GetNewOrder());
            startIntencity = _failOrderSoonLight.intensity;
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
            StopCoroutine(orderTimerCoroutine);
            StartCoroutine(GetNewOrder());
            _canGetCargo = false;
            TurnOffAllLights();
        }

        private void OrderComplete()
        {
            Debug.Log("complete");
            StopConveyorWork();
        }

        private void OrderCancel()
        {
            Debug.Log("OrderCancel");
            StopConveyorWork();
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
            StopCoroutine(failLightCoroutine);
            _failOrderSoonLight.enabled = false;
            _redLightTurnedOn = false;
        }

        private void TurnOnFailLight()
        {
            failLightCoroutine = StartCoroutine(TurnOnFailLightCoroutine());
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
                _failOrderSoonLight.intensity = (timer / failLightFreq) * startIntencity;
                yield return null;
            }
        }
        #endregion;

        private IEnumerator GetNewOrder()
        {
            yield return new WaitForSeconds(_timeToGetNewOrder);
            SetOrderQueue();
            _waitLight.enabled = true;
            _canGetCargo = true;
            orderTimerCoroutine = StartCoroutine(StartOrderTimer());
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
    }
}