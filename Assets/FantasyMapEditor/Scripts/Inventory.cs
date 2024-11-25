using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyMapEditor.Scripts
{
    public class Inventory : MonoBehaviour
    {
        public SpriteCollection SpriteCollection;
        public MapEditor MapEditor;
        public Transform Grid;
        public GameObject InventoryItem;
        public Text Selection;

        private readonly List<Toggle> _toggles = new();

        public static Inventory Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            SwitchTab(7);
        }

        public void SwitchTab(int tab)
        {
            foreach (var toggle in _toggles)
            {
                Destroy(toggle.gameObject);
            }

            _toggles.Clear();

            Selection.text = null;

            switch (tab)
            {
                case 0:
                    break;
                case 1:
                    CreateInventoryItem("Delete", SpriteCollection.DeleteSprite, () => MapEditor.Instance.Base.sprite = null);
                    break;
                case 2:
                    CreateInventoryItem("Delete", SpriteCollection.DeleteSprite, () => MapEditor.Instance.Preset.sprite = null);
                    break;
                default:
                    CreateInventoryItem("Delete", SpriteCollection.DeleteSprite, MapEditor.DeleteMode);
                    CreateInventoryItem("Select", SpriteCollection.SelectSprite, MapEditor.SelectMode);
                    break;
            }

            var sprites = new List<Sprite>();

            switch (tab)
            {
                case 0: sprites = SpriteCollection.Size; break;
                case 1: sprites = SpriteCollection.Base; break;
                case 2: sprites = SpriteCollection.Presets; break;
                case 3: sprites = SpriteCollection.Islands; break;
                case 4: sprites = SpriteCollection.Landscape; break;
                case 5: sprites = SpriteCollection.Water; break;
                case 6: sprites = SpriteCollection.Trees; break;
                case 7: sprites = SpriteCollection.Buildings; break;
                case 8: sprites = SpriteCollection.Roads; break;
                case 9: sprites = SpriteCollection.Other; break;
                case 10: sprites = SpriteCollection.UI; break;
            }

            foreach (var sprite in sprites)
            {
                if (tab > 0 && sprite.name.EndsWith("[S]") && MapEditor.Instance.Size != "[S]") continue;
                if (tab > 0 && sprite.name.EndsWith("[M]") && MapEditor.Instance.Size != "[M]") continue;
                if (tab > 0 && sprite.name.EndsWith("[L]") && MapEditor.Instance.Size != "[L]") continue;

                switch (tab)
                {
                    case 0:
                        CreateInventoryItem(sprite.name, sprite, () => MapEditor.Instance.SetSize(sprite.name));
                        break;
                    case 1:
                        CreateInventoryItem(sprite.name, sprite, () => MapEditor.Instance.Base.sprite = sprite);
                        break;
                    case 2:
                        CreateInventoryItem(sprite.name, sprite, () => MapEditor.Instance.Preset.sprite = sprite);
                        break;
                    default:
                        CreateInventoryItem(sprite.name, sprite, () => MapEditor.DrawMode(sprite));
                        break;
                }
            }

            switch (tab)
            {
                case 0:
                    _toggles[SpriteCollection.Size.FindIndex(i => i.name == MapEditor.Instance.Size)].isOn = true;
                    break;
                case 1:
                    if (MapEditor.Instance.Base.sprite == null)
                    {
                        _toggles[0].isOn = true;
                    }
                    else
                    {
                        _toggles.Single(i => i.targetGraphic.GetComponent<Image>().sprite == MapEditor.Instance.Base.sprite).isOn = true;
                    }
                    break;
                case 2:
                    if (MapEditor.Instance.Preset.sprite == null)
                    {
                        _toggles[0].isOn = true;
                    }
                    else
                    {
                        _toggles.Single(i => i.targetGraphic.GetComponent<Image>().sprite == MapEditor.Instance.Preset.sprite).isOn = true;
                    }
                    break;
                default:
                    _toggles[2].isOn = true;
                    break;
            }
        }

        private void CreateInventoryItem(string itemName, Sprite sprite, Action onSelect)
        {
            var item = Instantiate(InventoryItem, Grid);
            var toggle = item.GetComponent<Toggle>();

            item.name = itemName;
            item.transform.Find("Icon").GetComponent<Image>().sprite = sprite;
            toggle.onValueChanged.AddListener(value => { if (value) onSelect(); });
            toggle.onValueChanged.AddListener(value => { if (value) Selection.text = itemName; });
            toggle.group = Grid.GetComponent<ToggleGroup>();
            toggle.isOn = false;

            _toggles.Add(toggle);
        }
    }
}