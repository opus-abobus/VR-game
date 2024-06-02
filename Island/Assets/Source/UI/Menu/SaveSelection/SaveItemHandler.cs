using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SaveItemHandler : MonoBehaviour, IPointerClickHandler
    {
        private float _lastClickTime = 0;
        [SerializeField] private float _doubleClickPeriod = 0.3f;

        public event Action<GameObject> SingleClick, DoubleClick;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnClick();

                if (Time.time - _lastClickTime < _doubleClickPeriod)
                {
                    OnDoubleClick();
                }
                _lastClickTime = Time.time;
            }
        }

        private void OnClick()
        {
            SingleClick?.Invoke(gameObject);
        }

        private void OnDoubleClick()
        {
            DoubleClick?.Invoke(gameObject);
        }
    }
}
