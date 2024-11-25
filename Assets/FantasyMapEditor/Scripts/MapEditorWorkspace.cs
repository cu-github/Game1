using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.FantasyMapEditor.Scripts
{
    public class MapEditorWorkspace : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public MapEditor MapEditor;

        private Vector3 _pointerDown, _camPosition;

        public void Start()
        {
            MapEditor.EnableCursor(false);
        }

        public void Update()
        {
            MapEditor.MoveCursor(Input.mousePosition);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            MapEditor.EnableCursor(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MapEditor.EnableCursor(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0))
            {
                MapEditor.Draw(eventData.position);
            }
            else if (Input.GetMouseButton(1))
            {
                _pointerDown = eventData.position;
                _camPosition = Camera.main.transform.position;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.GetMouseButton(0))
            {
                //MapEditor.Draw(eventData.position);
            }
            else if (Input.GetMouseButton(1))
            {
                Camera.main.transform.position = _camPosition + Camera.main.ScreenToWorldPoint(_pointerDown) - Camera.main.ScreenToWorldPoint(eventData.position);
            }
        }
    }
}