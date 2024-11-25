using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.FantasyMapEditor.Scripts
{
    public class ToggleButton : MonoBehaviour
    {
        public UnityEvent Callback;

        public void Awake()
        {
            GetComponent<Toggle>().onValueChanged.AddListener(OnSelect);
        }

        public void OnSelect(bool value)
        {
            if (value) Callback.Invoke();
        }
    }
}