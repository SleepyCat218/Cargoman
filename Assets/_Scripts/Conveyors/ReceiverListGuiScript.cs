using System.Collections.Generic;
using UnityEngine;

namespace Cargoman
{
    public class ReceiverListGuiScript : MonoBehaviour
    {
        [SerializeField] private List<ReceiverOrderManager> _receivers = new List<ReceiverOrderManager>();

        private void Awake()
        {
            foreach (var item in _receivers)
            {
                item.CreateReceiverGui();
            }
        }
    }
}