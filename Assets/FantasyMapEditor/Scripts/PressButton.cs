using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.FantasyMapEditor.Scripts
{
    public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent OnPress;
        public UnityEvent OnRelease;
        public UnityEvent OnHold;
        public KeyCode Hotkey;

        public bool Pressed { get; set; }

        private float _pressTime;

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            _pressTime = Time.time;
            OnPress?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
            OnRelease?.Invoke();
        }

        public void OnDisable()
        {
            if (Pressed)
            {
                Pressed = false;
                OnRelease?.Invoke();
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(Hotkey) && Input.touchCount == 0)
            {
                OnPress?.Invoke();
            }
            else if ((Input.GetKey(Hotkey) && Input.touchCount == 0 || Pressed) && Time.time - _pressTime > 0.15f)
            {
                OnHold?.Invoke();
            }
            else if (Input.GetKeyUp(Hotkey) && Input.touchCount == 0)
            {
                OnRelease?.Invoke();
            }
        }
    }
}