using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.MatGen
{
    internal static class MatGenMenus
    {
        [MenuItem("Assets/MatGen/Generate Material")]
        [MenuItem("Tools/MatGen/Generate Material")]
        private static void RunMaterialGenerator()
        {
            if (MatGenUtility.TryGetSelected<Texture2D>(out var selected))
            {
                var window = EditorWindow.GetWindow<MaterialGeneratorWindow>(
                    title: "Material Generator",
                    focus: true,
                    utility: true
                );
                window.minSize = new Vector2(500, 480);
                window.maxSize = new Vector2(500, 480);
                window.SetTexturePaths(selected
                    .Select(obj => AssetDatabase.GetAssetPath(obj))
                    .ToArray());
                window.Show();
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "No Textures Selected",
                    "Please select one or more textures to generate materials.",
                    "OK"
                );
            }
        }

        [MenuItem("Assets/MatGen/Duplicate Material")]
        [MenuItem("Tools/MatGen/Duplicate Material")]
        private static void RunMaterialDuplicator()
        {
            if (MatGenUtility.TryGetSelected<Material>(out var selected))
            {
                var window = EditorWindow.GetWindow<MaterialDuplicatorWindow>(
                    title: "Material Duplicator",
                    focus: true,
                    utility: true
                );
                window.minSize = new Vector2(640, 485);
                window.maxSize = new Vector2(640, 485);
                window.SetMaterials(selected
                    .Select(mat => new Material(mat))
                    .ToArray());
                window.Show();
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "No Materials Selected",
                    "Please select one or more materials to duplicate.",
                    "OK"
                );
            }
        }
    }
}
