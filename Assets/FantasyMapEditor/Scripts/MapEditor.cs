using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Assets.FantasyMapEditor.Scripts
{
    public class MapEditor : MonoBehaviour
    {
        public Transform[] Layers;
        public Toggle[] LayerToggles;
        public SpriteRenderer Cursor;
        public SpriteRenderer Base;
        public SpriteRenderer Preset;
        public InputField SortingOrder;
        public MapObject Selected;
        public Camera CaptureCamera;
        public int CaptureWidth;
        public int CaptureHeight;

        private Sprite _sprite;
        private int _layer;
        private float _scale = 0.75f;
        private readonly List<KeyCode> _keysPressed = new();

        public static MapEditor Instance { get; private set; }
        public static int Mode { get; private set; }

        public void Start()
        {
            Instance = this;
            SetSize("[M]");
            SwitchLayer(0);
            Popup.Instance.Show("<color=\"yellow\">[RMB]</color> Move camera");
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(origin, Vector2.zero);

                if (hit.collider != null)
                {
                    Debug.Log(hit.transform.name);
                    Debug.Log("hit");
                }
            }

            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                IncSortingOrder(-1);
            }
            else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                IncSortingOrder(+1);
            }

            if ((Mode == 1 || Mode == 2) && Selected)
            {
                var delta = 0.5f * Time.deltaTime;

                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || _keysPressed.Contains(KeyCode.LeftControl))
                {
                    if (Input.GetKey(KeyCode.UpArrow) || _keysPressed.Contains(KeyCode.UpArrow))
                    {
                        Selected.transform.localScale += new Vector3(delta, delta);

                        if (Selected.transform.localScale.x > 1.25f) Selected.transform.localScale = 1.25f * Vector3.one;
                    }
                    else if (Input.GetKey(KeyCode.DownArrow) || _keysPressed.Contains(KeyCode.DownArrow))
                    {
                        Selected.transform.localScale -= new Vector3(delta, delta);

                        if (Selected.transform.localScale.x < 0.75f) Selected.transform.localScale = 0.75f * Vector3.one;
                    }

                    if (Input.GetKey(KeyCode.LeftArrow) || _keysPressed.Contains(KeyCode.LeftArrow))
                    {
                        Selected.transform.Rotate(0, 0, 100 * delta);

                        if (Selected.transform.eulerAngles.z < 180 && Selected.transform.eulerAngles.z > 15) Selected.transform.eulerAngles = new Vector3(0, 0, 15);
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) || _keysPressed.Contains(KeyCode.RightArrow))
                    {
                        Selected.transform.Rotate(0, 0, -100 * delta);

                        if (Selected.transform.eulerAngles.z > 180 && Selected.transform.eulerAngles.z < -15 + 360) Selected.transform.eulerAngles = new Vector3(0, 0, -15);
                    }
                }
                else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || _keysPressed.Contains(KeyCode.LeftShift))
                {
                    var sortingOrder = Selected.transform.parent.GetComponent<SortingGroup>().sortingOrder;

                    if (Input.GetKeyDown(KeyCode.UpArrow) && sortingOrder < 7)
                    {
                        Selected.transform.SetParent(Layers[sortingOrder + 1]);
                        LayerToggles[sortingOrder + 1].isOn = true;
                    }

                    if (Input.GetKeyDown(KeyCode.DownArrow) && sortingOrder > 0)
                    {
                        Selected.transform.SetParent(Layers[sortingOrder - 1]);
                        LayerToggles[sortingOrder - 1].isOn = true;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftArrow) || _keysPressed.Contains(KeyCode.LeftArrow))
                    {
                        Selected.transform.position += new Vector3(-delta, 0);
                    }
                    
                    if (Input.GetKey(KeyCode.RightArrow) || _keysPressed.Contains(KeyCode.RightArrow))
                    {
                        Selected.transform.position += new Vector3(delta, 0);
                    }
                    
                    if (Input.GetKey(KeyCode.UpArrow) || _keysPressed.Contains(KeyCode.UpArrow))
                    {
                        Selected.transform.position += new Vector3(0, delta);
                    }

                    if (Input.GetKey(KeyCode.DownArrow) || _keysPressed.Contains(KeyCode.DownArrow))
                    {
                        Selected.transform.position += new Vector3(0, -delta);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    DeleteObject();
                }
            }
        }

        public string Size { get; private set; }

        public void SetSize(string size)
        {
            if (size == Size) return;

            var prev = GetScale(Size ?? size);

            _scale = GetScale(Size = size);

            foreach (var layer in Layers)
            {
                if (layer.name == "Base" || layer.name == "Preset") continue;

                foreach (Transform t in layer)
                {
                    //t.localPosition /= prev;
                    //t.localPosition *= _scale;

                    t.localScale /= prev;
                    t.localScale *= _scale;

                    if (t.name.Contains("[S]") || t.name.Contains("[M]") || t.name.Contains("[L]"))
                    {
                    }
                }
            }

            ReplaceSize();

            Cursor.transform.localScale = _scale * Vector3.one;

            float GetScale(string s)
            {
                switch (s)
                {
                    case "[S]": return 1f;
                    case "[M]": return 0.75f;
                    case "[L]": return 0.5f;
                    default: throw new NotSupportedException();
                }
            }
        }

        private void ReplaceSize()
        {
            if (Base.sprite != null && (Base.sprite.name.Contains("[S]") || Base.sprite.name.Contains("[M]") || Base.sprite.name.Contains("[L]")))
            {
                var baseName = Base.sprite.name.Replace("[S]", "").Replace("[M]", "").Replace("[L]", "");

                Base.sprite = Inventory.Instance.SpriteCollection.Base.Single(i => i.name == baseName + Size);
            }

            if (Preset.sprite != null && (Preset.sprite.name.Contains("[S]") || Preset.sprite.name.Contains("[M]") || Preset.sprite.name.Contains("[L]")))
            {
                var presetName = Preset.sprite.name.Replace("[S]", "").Replace("[M]", "").Replace("[L]", "");

                Preset.sprite = Inventory.Instance.SpriteCollection.Presets.Single(i => i.name == presetName + Size);
            }
        }

        public void DeleteMode()
        {
            Mode = 0;
            _sprite = Cursor.sprite = null;
            
            if (Selected) Selected.Deselect();

            Selected = null;
        }

        public void SelectMode()
        {
            Mode = 1;
            _sprite = Cursor.sprite = null;
        }

        public void DrawMode(Sprite sprite)
        {
            Mode = 2;
            _sprite = Cursor.sprite = sprite;

            if (Selected) Selected.Deselect();

            Selected = null;
        }

        public void SelectObject(MapObject mapObject)
        {
            if (Selected == mapObject) return;

            if (Selected) Selected.Deselect();

            Selected = mapObject;
            SetSortingOrder(Selected.SpriteRenderer.sortingOrder);
        }

        public void EnableCursor(bool value)
        {
            Cursor.enabled = value;
        }

        public void MoveCursor(Vector2 pointer)
        {
            if (!Cursor.enabled) return;

            var position = Camera.main.ScreenToWorldPoint(pointer);

            Cursor.transform.position = new Vector3(position.x, position.y);
        }

        public void Draw(Vector2 pointer)
        {
            if (!Layers[_layer].gameObject.activeSelf) return;

            var position = Camera.main.ScreenToWorldPoint(pointer);

            switch (Mode)
            {
                case 0: break;
                case 1: break;
                case 2:
                {
                    var mapObject = new GameObject(_sprite.name);
                    var spriteRenderer = mapObject.AddComponent<SpriteRenderer>();

                    spriteRenderer.sprite = _sprite;
                    spriteRenderer.sortingOrder = SortingOrder.text == "" ? 0 : int.Parse(SortingOrder.text);
                    spriteRenderer.transform.SetParent(Layers[_layer]);
                    spriteRenderer.transform.position = new Vector3(position.x, position.y);
                    spriteRenderer.transform.localScale = _scale * Vector3.one;

                    mapObject.AddComponent<PolygonCollider2D>();

                    Selected = mapObject.AddComponent<MapObject>();

                    break;
                }
            }
        }

        public void DeleteObject()
        {
            if (Selected)
            {
                Destroy(Selected.gameObject);
            }
        }

        public void DeleteObject(MapObject mapObject)
        {
            Destroy(mapObject.gameObject);
        }

        public void SetSortingOrder(int value)
        {
            SortingOrder.text = value.ToString();
        }

        public void IncSortingOrder(int value)
        {
            SortingOrder.text = (int.Parse(SortingOrder.text) + value).ToString();

            if ((Mode == 1 || Mode == 2) && Selected)
            {
                Selected.SpriteRenderer.sortingOrder += value;
            }
        }

        public void Flip()
        {
            if ((Mode == 1 || Mode == 2) && Selected)
            {
                Selected.SpriteRenderer.flipX = !Selected.SpriteRenderer.flipX;
            }
        }

        public void OnKeyPressed(int keyCode)
        {
            if (!_keysPressed.Contains((KeyCode) keyCode))
            {
                _keysPressed.Add((KeyCode) keyCode);
            }
        }

        public void OnKeyReleased(int keyCode)
        {
            if (_keysPressed.Contains((KeyCode) keyCode))
            {
                _keysPressed.Remove((KeyCode) keyCode);
            }
        }

        public void OnKeysPressed(string input)
        {
            foreach (var key in input.Split('+').Select(Enum.Parse<KeyCode>))
            {
                if (!_keysPressed.Contains(key))
                {
                    _keysPressed.Add(key);
                }
            }
        }

        public void OnKeysReleased(string input)
        {
            foreach (var key in input.Split('+').Select(Enum.Parse<KeyCode>))
            {
                if (_keysPressed.Contains(key))
                {
                    _keysPressed.Remove(key);
                }
            }
        }

        public void SwitchLayer(int layer)
        {
            _layer = layer;
        }

        public void EnableLayer(int layer)
        {
            Layers[layer].gameObject.SetActive(!Layers[layer].gameObject.activeSelf);
        }

        public void SaveTexture()
        {
            var renderTexture = new RenderTexture(CaptureWidth, CaptureHeight, 24);
            var texture2D = new Texture2D(CaptureWidth, CaptureHeight, TextureFormat.ARGB32, false);

            CaptureCamera.targetTexture = renderTexture;
            CaptureCamera.Render();
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, CaptureWidth, CaptureHeight), 0, 0);
            texture2D.Apply();
            CaptureCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            SaveFileDialog("Save map as texture", "Map", "Image", ".png", texture2D.EncodeToPNG());
        }

        public void SaveJson()
        {
            var json = SerializeMap();

            SaveFileDialog("Save map as JSON", "Map", "JSON", ".json", Encoding.UTF8.GetBytes(json));
        }

        public void LoadJson()
        {
            OpenFileDialog("Open map JSON", "JSON", ".json", bytes => BuildMap(Encoding.UTF8.GetString(bytes)));
        }

        public void Reset()
        {
            var json = SerializeMap();
            var folder = Path.Combine(Application.dataPath, "FantasyMapEditor/Backup");
            var path = Path.Combine(folder, $"{Guid.NewGuid()}.json");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            File.WriteAllText(path, json);

            Debug.LogWarning($"Map backup saved as {path}");

            foreach (var mapObject in FindObjectsOfType<MapObject>())
            {
                Destroy(mapObject.gameObject);
            }
        }

        private string SerializeMap()
        {
            var mapObjects = FindObjectsOfType<MapObject>().Select(i => i.Save()).ToList();

            mapObjects.Insert(0, new Dictionary<string, object> { { "S", Size }, { "B", Base.sprite?.name ?? "" }, { "P", Preset.sprite?.name ?? "" } });

            var json = JsonConvert.SerializeObject(mapObjects);

            return json;
        }

        private void BuildMap(string json)
        {
            foreach (var mapObject in FindObjectsOfType<MapObject>())
            {
                Destroy(mapObject.gameObject);
            }
            
            var jtokens = JArray.Parse(json);

            Base.sprite = Inventory.Instance.SpriteCollection.Base.SingleOrDefault(i => i.name == jtokens[0]["B"].Value<string>());
            Preset.sprite = Inventory.Instance.SpriteCollection.Presets.SingleOrDefault(i => i.name == jtokens[0]["P"].Value<string>());

            for (var i = 1; i < jtokens.Count; i++)
            {
                var jToken = jtokens[i];
                var spriteName = jToken["N"].Value<string>();
                var found = Inventory.Instance.SpriteCollection.All.Where(i => i.name == spriteName).ToList();

                if (found.Count != 1)
                {
                    Debug.LogError($"{found.Count} sprites with name {spriteName}");
                    continue;
                }

                var instance = new GameObject(spriteName);
                var spriteRenderer = instance.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = found[0];
                spriteRenderer.sortingOrder = jToken["O"].Value<int>();
                spriteRenderer.flipX = jToken["F"].Value<bool>();
                instance.transform.localPosition = new Vector3(jToken["X"].Value<float>(), jToken["Y"].Value<float>(), 0);
                instance.transform.localEulerAngles = new Vector3(0, 0, jToken["R"].Value<float>());
                instance.transform.localScale = Vector3.one * jToken["S"].Value<float>();
                instance.transform.SetParent(Layers[jToken["G"].Value<int>()]);

                instance.AddComponent<PolygonCollider2D>();
                instance.AddComponent<MapObject>();
            }

            var size = jtokens[0]["S"] == null ? "[M]" : jtokens[0]["S"].Value<string>();

            SetSize(size);
        }
        
        public void ExportSprite()
        {
            var texture = Cursor.sprite.texture;

            SaveFileDialog("Save sprite", texture.name, "Image", ".png", texture.EncodeToPNG());
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }

        private void SaveFileDialog(string title, string fileName, string fileType, string extension, byte[] bytes)
        {
            #if UNITY_EDITOR

            var path = UnityEditor.EditorUtility.SaveFilePanel(title, null, fileName + extension, extension.Replace(".", ""));

            if (string.IsNullOrEmpty(path)) return;

            File.WriteAllBytes(path, bytes);

            #elif UNITY_STANDALONE

            #if FILE_BROWSER

            StartCoroutine(SimpleFileBrowserForWindows.WindowsFileBrowser.SaveFile(title, "", fileName, fileType, extension, bytes, (success, p) => { }));

            #else

            Debug.LogWarning("Please import this asset: http://u3d.as/2QLg");
            
            #endif
            
            #elif UNITY_WEBGL

            #if FILE_BROWSER

            if (extension == ".png")
            {
                Popup.Instance.Show("This feature is unavailable in the demo version. Please purchase the full app.");
                return;
            }

            SimpleFileBrowserForWebGL.WebFileBrowser.Download(fileName + extension, bytes);

            #else

            Debug.LogWarning("Please import this asset: http://u3d.as/2W52");
            
            #endif

            #endif
        }

        private void OpenFileDialog(string title, string fileType, string extension, Action<byte[]> callback)
        {
            #if UNITY_EDITOR

            var path = UnityEditor.EditorUtility.OpenFilePanel(title, "", extension.Replace(".", ""));

            if (path == "") return;

            callback(File.ReadAllBytes(path));

            #elif UNITY_WEBGL

            #if FILE_BROWSER

            SimpleFileBrowserForWebGL.WebFileBrowser.Upload((fileName, mime, bytes) => callback(bytes), extension);

            #else

            Debug.LogWarning("Please import this asset: http://u3d.as/2W52");
            
            #endif

            #elif UNITY_STANDALONE

            #if FILE_BROWSER

            StartCoroutine(SimpleFileBrowserForWindows.WindowsFileBrowser.OpenFile(title, "", fileType, new[] { extension }, (success, _, bytes) => { if (success) callback(bytes); }));

            #else

            Debug.LogWarning("Please import this asset: http://u3d.as/2QLg");
            
            #endif

            #endif
        }
    }
}