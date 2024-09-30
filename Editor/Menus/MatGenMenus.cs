using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools.MatGen
{
    internal static class MatGenMenus
    {
        [MenuItem("Assets/MatGen")]
        [MenuItem("Tools/MatGen")]
        private static void RunMatGen()
        {
            var selected = new List<string>();
            foreach (var obj in Selection.objects)
            {
                if (obj is Texture2D)
                    selected.Add(AssetDatabase.GetAssetPath(obj));
            }

            if (selected.Count == 0)
            {
                EditorUtility.DisplayDialog(
                    "No Textures Selected",
                    "Please select one or more textures to generate materials.",
                    "OK"
                );
                return;
            }

            var window = EditorWindow.GetWindow<MatGenWindow>(
                title: "Material Generator",
                focus: true,
                utility: true
            );
            window.minSize = new Vector2(500, 470);
            window.maxSize = new Vector2(500, 470);
            window.SetTexturePaths(selected.ToArray());
            window.Show();
        }
    }
}
