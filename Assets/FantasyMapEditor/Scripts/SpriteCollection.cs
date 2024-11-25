using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.FantasyMapEditor.Scripts
{
    /// <summary>
    /// Scriptable object that automatically grabs all required images.
    /// </summary>
    [CreateAssetMenu(fileName = "SpriteCollection", menuName = "Pixel Map Engine/SpriteCollection")]
    public class SpriteCollection : ScriptableObject
    {
        public Object SpriteFolder;

        public List<Sprite> Size;
        public List<Sprite> Base;
        public List<Sprite> Presets;
        public List<Sprite> Islands;
        public List<Sprite> Landscape;
        public List<Sprite> Water;
        public List<Sprite> Trees;
        public List<Sprite> Buildings;
        public List<Sprite> Roads;
        public List<Sprite> Other;
        public List<Sprite> UI;
        public List<Sprite> All;

        public Sprite DeleteSprite;
        public Sprite SelectSprite;

        #if UNITY_EDITOR

        public void Refresh()
        {
            Size = LoadSprites(SpriteFolder, "Size");
            Base = LoadSprites(SpriteFolder, "Base");
            Presets = LoadSprites(SpriteFolder, "Presets");
            Landscape = LoadSprites(SpriteFolder, "Landscape");
            Water = LoadSprites(SpriteFolder, "Water");
            Trees = LoadSprites(SpriteFolder, "Trees");
            Buildings = LoadSprites(SpriteFolder, "Buildings");
            Roads = LoadSprites(SpriteFolder, "Roads");
            UI = LoadSprites(SpriteFolder, "UI");
            Other = LoadSprites(SpriteFolder, "Other");
            All = Base.Union(Presets).Union(Landscape).Union(Water).Union(Trees).Union(Buildings).Union(Roads).Union(Other).Union(UI).ToList();
            UnityEditor.EditorUtility.SetDirty(this);

            foreach (var sprite in All)
            {
                if (All.Count(i => i.name == sprite.name) > 1)
                {
                    Debug.LogError($"Multiple sprites with the same name found: {sprite.name}");
                }
            }

            Debug.Log("Refresh done!");
        }

        private static List<Texture2D> LoadTextures(Object folder, string subFolder)
        {
            var files = Directory.GetFiles(UnityEditor.AssetDatabase.GetAssetPath(folder) + $"/{subFolder}", "*.png", SearchOption.AllDirectories).ToList();
            var textures = files.Select(UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>).ToList();

            return textures;
        }

        private static List<Sprite> LoadSprites(Object folder, string subFolder)
        {
            var files = Directory.GetFiles(UnityEditor.AssetDatabase.GetAssetPath(folder) + $"/{subFolder}", "*.png", SearchOption.AllDirectories).ToList();
            var sprites = files.SelectMany(UnityEditor.AssetDatabase.LoadAllAssetsAtPath).OfType<Sprite>().ToList();

            return sprites;
        }

        #endif
    }
}