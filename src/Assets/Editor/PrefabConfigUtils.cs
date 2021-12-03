using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    public static class PrefabConfigUtils
    {
        public static void GetAllPrefabsWithTextComponent(Object[] objs,
            ref List<GameObject> allAssetsWithTextComponent)
        {
            foreach (Object obj in objs)
            {
                if (!(obj is GameObject go))
                {
                    continue;
                }

                var comp = go.GetComponent(typeof(Text));
                if (comp != null)
                {
                    allAssetsWithTextComponent.Add(go);
                }
                else
                {
                    var comps = go.GetComponentsInChildren(typeof(Text));
                    if (comps.Length > 0)
                    {
                        allAssetsWithTextComponent.Add(go);
                    }
                }
            }
        }

        public static void ApplyConfigToGameObjectWithTextComponent(PrefabConfig prefabConfig, GameObject go)
        {
            go.GetComponent<Text>().text = prefabConfig.Text;

            SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Undo.AddComponent(go, typeof(SpriteRenderer));
                spriteRenderer = go.GetComponent<SpriteRenderer>();
            }

            spriteRenderer.color = prefabConfig.Color;
            spriteRenderer.sprite = Sprite.Create(prefabConfig.Image,
                new Rect(0f, 0f, prefabConfig.Image.width, prefabConfig.Image.height), new Vector2(0.5f, 0.5f),
                100f);
        }
    }
}