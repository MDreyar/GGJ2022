using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeDarkMap : MonoBehaviour
{
    const string mapName = "Map";
    const string darkMapName = "Map_Dark";
    const string darkTextureSuffix = "_dark";

    [UnityEditor.MenuItem("GGJ/DuplicateMap")]
    public static void duplicateMap() {
        // don't do this when game is running
        if (Application.isPlaying) return;

        // remove old map
        GameObject map = GameObject.Find(mapName);
        if(map == null) {
            Debug.LogError("No map found!");
        }
        GameObject darkMap = GameObject.Find(darkMapName);
        if (darkMap != null)
            DestroyImmediate(darkMap);

        // duplicate map
        darkMap = Instantiate(map);
        darkMap.name = darkMapName;

        // update renderers
        var renderers = darkMap.GetComponentsInChildren<SpriteRenderer>();
        foreach(var renderer in renderers) {
            renderer.sortingOrder = 1;
            renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            // update sprite with dark sprite
            var spritePath = AssetDatabase.GetAssetPath(renderer.sprite);
            var darkSpritePath = spritePath.Replace(renderer.sprite.name, renderer.sprite.name + darkTextureSuffix);
            var darkSprite = AssetDatabase.LoadAssetAtPath<Sprite>(darkSpritePath);
            if(darkSprite == null) {
                Debug.LogError("Could not find dark sprite for " + renderer.sprite.name + " at " + darkSpritePath);
                continue;
            }
            renderer.sprite = darkSprite;
        }
    }
}
