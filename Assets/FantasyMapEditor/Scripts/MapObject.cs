using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.FantasyMapEditor.Scripts
{
    public class MapObject : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        private Vector2 _position, _mousePosition;

        public void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void OnMouseEnter()
        {
            switch (MapEditor.Mode)
            {
                case 0:
                    SpriteRenderer.color = new Color(1f, 0.7f, 0.7f);
                    break;
                case 1:
                    SpriteRenderer.color = new Color(0.7f, 1f, 0.7f);
                    break;
            }
        }

        public void OnMouseExit()
        {
            if (this != MapEditor.Instance.Selected)
            {
                SpriteRenderer.color = Color.white;
            }
        }

        public void OnMouseDown()
        {
            switch (MapEditor.Mode)
            {
                case 0:
                    MapEditor.Instance.DeleteObject(this);
                    break;
                case 1:
                    SpriteRenderer.color = Color.green;
                    _position = transform.position;
                    _mousePosition = Input.mousePosition;
                    MapEditor.Instance.SelectObject(this);
                    break;
                case 2:
                    SpriteRenderer.color = Color.red;
                    break;
            }
        }

        public void OnMouseDrag()
        {
            if (MapEditor.Mode == 1)
            {
                transform.position = (Vector3) _position + Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(_mousePosition);
            }
        }

        public void Deselect()
        {
            SpriteRenderer.color = Color.white;
        }

        public Dictionary<string, object> Save()
        {
            var dict = new Dictionary<string, object>
            {
                { "N", SpriteRenderer.sprite.name },
                { "O", SpriteRenderer.sortingOrder },
                { "F", SpriteRenderer.flipX },
                { "G", transform.parent.GetComponent<SortingGroup>().sortingOrder },
                { "X", transform.localPosition.x },
                { "Y", transform.localPosition.y },
                { "R", transform.localEulerAngles.z },
                { "S", transform.localScale.x }
            };

            return dict;
        }
    }
}