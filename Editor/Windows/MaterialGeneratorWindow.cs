using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.MatGen
{
    internal class MaterialGeneratorWindow : EditorWindow
    {
        private string _destinationPath = Application.dataPath + "/Materials";
        private string _shaderType = "Standard";
        private string _shaderTextureInput = "_MainTex";
        private string _nameSource = "";
        private string _nameTarget = "";
        private string[] _texturePaths = new string[0];
        private List<(string, string)> _inputOutput = new List<(string, string)>();
        private Vector2 _scrollPosition;

        public void SetTexturePaths(string[] paths)
        {
            _texturePaths = paths;
            UpdateInputOutput();
        }

        private void OnGUI()
        {
            MatGenLayout.ChooseDestination(ref _destinationPath);
            MatGenLayout.DrawSeparator(2);

            ChooseShaderType();
            MatGenLayout.DrawSeparator(2);

            MatGenLayout.ModifyName(ref _nameSource, ref _nameTarget);
            MatGenLayout.DrawSeparator(2);

            ShowInputOutputColumns();
            GUILayout.Space(4);

            bool clicked = MatGenLayout.ShowActionButton(
                "Generate Materials", 
                "Generate materials from selected textures", 
                () => MatGenService.CreateMaterialAssets(
                    _texturePaths,
                    _destinationPath,
                    _shaderType,
                    _shaderTextureInput,
                    _nameSource,
                    _nameTarget
                )
            );

            if (clicked)
                Close();
        }

        private void ChooseShaderType()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Choose Shader Type", EditorStyles.boldLabel);
            GUILayout.Label("Must accept a texture input", EditorStyles.miniLabel);
            GUILayout.EndHorizontal();

            var shaderNames = ShaderUtil.GetAllShaderInfo().Select(s => s.name).ToArray();
            int selectedIndex = EditorGUILayout.Popup("Shader Type:",
                Array.IndexOf(shaderNames, _shaderType),
                shaderNames);
            _shaderType = shaderNames[selectedIndex];

            GUILayout.BeginHorizontal();
            GUILayout.Label("Texture Input: ", GUILayout.Width(147));
            _shaderTextureInput = GUILayout.TextField(_shaderTextureInput, GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }

        private void ShowInputOutputColumns()
        {
            UpdateInputOutput();

            GUILayout.Label("Input / Output", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Texture Name", EditorStyles.miniBoldLabel, GUILayout.Width(200));
            GUILayout.Label("Material Name", EditorStyles.miniBoldLabel, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            _scrollPosition = GUILayout.BeginScrollView(
                _scrollPosition,
                EditorStyles.helpBox,
                GUILayout.Height(200)
            );
            foreach (var (input, output) in _inputOutput)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(input, GUILayout.Width(200));
                GUILayout.Label(output, GUILayout.Width(200));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void UpdateInputOutput()
        {
            _inputOutput.Clear();
            foreach (var path in _texturePaths)
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(path);
                var newName = string.IsNullOrEmpty(_nameSource)
                    ? name
                    : name.Replace(_nameSource, _nameTarget);
                _inputOutput.Add((name, newName));
            }
        }
    }
}
