using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tools.MatGen
{
    internal class MaterialDuplicatorWindow : EditorWindow
    {
        private string _destinationPath = Application.dataPath + "/Materials";
        private Material[] _materials = new Material[0];
        private string[] _materialPaths = new string[0];
        private Editor _materialEditor;
        private Vector2 _propertyScrollPosition;
        private Vector2 _scrollPosition;
        private string _nameSource = "";
        private string _nameTarget = "";
        private bool _canDrawEditor = true;
        private List<(string, string)> _inputOutput = new List<(string, string)>();

        public void SetMaterials(Material[] materials)
        {
            _materials = materials;            
            for (int i = 0; i < _materials.Length && _canDrawEditor; i++)
                _canDrawEditor = _materials[0].shader.name == _materials[i].shader.name;

            _materialPaths = new string[_materials.Length];
            for (int i = 0; i < _materials.Length; i++)
                _materialPaths[i] = _materials[i].name;
            
            if (_canDrawEditor)
                _materialEditor = Editor.CreateEditor(_materials);

            UpdateInputOutput();
        }

        private void OnGUI()
        {
            MatGenLayout.ChooseDestination(ref _destinationPath);
            MatGenLayout.DrawSeparator(2);

            MatGenLayout.ModifyName(ref _nameSource, ref _nameTarget);
            MatGenLayout.DrawSeparator(2);

            ShowMaterialPropertiesAndOutput();

            GUILayout.Space(4);            
            bool clicked = MatGenLayout.ShowActionButton(
                "Duplicate Materials",
                "Duplicate selected materials",
                () => MatGenService.DuplicateMaterials(
                    _materials,
                    _destinationPath,
                    _nameSource,
                    _nameTarget
                )
            );

            if (clicked)
                Close();
        }

        private void ShowMaterialPropertiesAndOutput()
        {
            UpdateInputOutput();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("Material Properties", EditorStyles.boldLabel);
            if (_canDrawEditor)
            {
                if (_materialEditor != null)
                {
                    _propertyScrollPosition = GUILayout.BeginScrollView(
                        _propertyScrollPosition,
                        EditorStyles.helpBox,
                        GUILayout.Width(310),
                        GUILayout.Height(300)
                    );

                    GUILayout.BeginVertical();
                    _materialEditor.DrawHeader();
                    _materialEditor.OnInspectorGUI();
                    GUILayout.EndVertical();

                    GUILayout.EndScrollView();
                }
            }
            else
            {
                GUILayout.Label("Selected materials must use the same shader.", EditorStyles.helpBox);
            }
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Source Name", EditorStyles.boldLabel, GUILayout.Width(150));
            GUILayout.Label("Target Name", EditorStyles.boldLabel, GUILayout.Width(150));
            GUILayout.EndHorizontal();

            _scrollPosition = GUILayout.BeginScrollView(
                _scrollPosition,
                EditorStyles.helpBox,
                GUILayout.Width(320),
                GUILayout.Height(300)
            );
            foreach (var (input, output) in _inputOutput)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(input, GUILayout.Width(150));
                GUILayout.Label(output, GUILayout.Width(150));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void UpdateInputOutput()
        {
            _inputOutput.Clear();
            foreach (var path in _materialPaths)
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