using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.MatGen
{
    internal class MatGenWindow : EditorWindow
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
            ChooseDestination();
            DrawSeparator(2);
            ModifyName();
            DrawSeparator(2);
            ChooseShaderType();
            DrawSeparator(2);
            ShowInputOutputColumns();

            GUILayout.Space(4);
            ShowGenerateButton();
        }

        private void ChooseDestination()
        {
            GUILayout.Label("Choose Destination", EditorStyles.boldLabel);
            GUILayout.Box(_destinationPath, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Browse", GUILayout.Width(150)))
                _destinationPath = EditorUtility.OpenFolderPanel("Choose Destination", _destinationPath, "");
            _destinationPath = _destinationPath.Replace(Application.dataPath, "Assets");
            GUILayout.EndHorizontal();
        }

        private void ModifyName()
        {
            GUILayout.Label("Modify Name", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Replace: ", GUILayout.Width(55));
            _nameSource = GUILayout.TextField(_nameSource, GUILayout.Width(100));
            GUILayout.Label("With: ", GUILayout.Width(32));
            _nameTarget = GUILayout.TextField(_nameTarget, GUILayout.Width(100));
            GUILayout.EndHorizontal();
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

        private void ShowGenerateButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate Materials", GUILayout.Width(150)))
            {
                MatGenService.CreateMaterialAssets(
                    _texturePaths,
                    _destinationPath,
                    _shaderType,
                    _shaderTextureInput,
                    _nameSource,
                    _nameTarget
                );
                Close();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawSeparator(float padding = 0)
        {
            GUILayout.Space(padding);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(padding);
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
