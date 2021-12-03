using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [Serializable]
    public class PrefabConfig
    {
        public string text;
        public string color;
        public string image;

        public string Text => text;
        public Color Color;
        public Texture2D Image;

        public bool Validate(string assetFolder)
        {
            // validating TEXT and ...
            return !string.IsNullOrEmpty(text) &&
                   ValidateImage(assetFolder) &&
                   ValidateColor();
        }

        private bool ValidateImage(string assetFolder)
        {
            // ... IMAGE
            string assetPath = $"{assetFolder}/{image}";
            Image = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

            return Image != null;
        }

        private bool ValidateColor()
        {
            // ... COLOR
            return ColorUtility.TryParseHtmlString(color, out Color);
        }

        public override string ToString()
        {
            return $"text: {text} | color: {color} | image: {image}";
        }
    }
}