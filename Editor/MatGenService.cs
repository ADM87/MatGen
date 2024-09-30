using UnityEditor;
using UnityEngine;

namespace Tools.MatGen
{
    public static class MatGenService
    {
        public static void CreateMaterialAssets(
            string[] texturePaths,
            string destinationPath,
            string shaderType,
            string shaderTextureInput,
            string nameSource,
            string nameTarget)
        {
            foreach (var texturePath in texturePaths)
            {
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                var material = new Material(Shader.Find(shaderType));
                material.SetTexture(shaderTextureInput, texture);
                var name = string.IsNullOrEmpty(nameSource)
                    ? texture.name
                    : texture.name.Replace(nameSource, nameTarget);
                AssetDatabase.CreateAsset(material, $"{destinationPath}/{name}.mat");
            }
            AssetDatabase.Refresh();
        }
    }
}
