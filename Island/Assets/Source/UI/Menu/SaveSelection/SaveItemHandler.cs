using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SaveItemHandler : MonoBehaviour, IPointerClickHandler
    {
        private float _lastClickTime = 0;
        [SerializeField] private float _doubleClickPeriod = 0.3f;

        public event Action<GameObject> DoubleClick;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (Time.time - _lastClickTime < _doubleClickPeriod)
                {
                    OnDoubleClick();
                }
                _lastClickTime = Time.time;
            }
        }

        private void OnDoubleClick()
        {
            DoubleClick?.Invoke(gameObject);
        }
    }
}
