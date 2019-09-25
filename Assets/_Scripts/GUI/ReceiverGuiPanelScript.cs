using UnityEngine;
using UnityEngine.UI;

namespace Cargoman
{
    public class ReceiverGuiPanelScript : MonoBehaviour
    {
        [SerializeField] private Image _nextCargoImage, _orderTimerImage;
        [SerializeField] private Text _cargoLeft, _orderTimer;

        public void UpdateOrderTimer(float timer, float startTimeValue)
        {
            _orderTimer.text = ((int)timer).ToString();
            _orderTimerImage.fillAmount = timer / startTimeValue;
        }

        public void UpdateCargoImage(Sprite image)
        {
            var tempColor = _nextCargoImage.color;
            if (image == null)
            {
                tempColor.a = 0f;
            }
            else
            {
                tempColor.a = 1f;
            }
            _nextCargoImage.color = tempColor;
            _nextCargoImage.sprite = image;
        }

        public void UpdateCargoLeftValue(int count)
        {
            _cargoLeft.text = count.ToString();
        }
    }
}