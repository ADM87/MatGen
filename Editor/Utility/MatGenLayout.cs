using UnityEditor;
using UnityEngine;

namespace Tools.MatGen
{
    internal static class MatGenLayout
    {
        public static void ChooseDestination(ref string destinationPath)
        {
            GUILayout.Label("Choose Destination", EditorStyles.boldLabel);
            GUILayout.Box(destinationPath, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Browse", GUILayout.Width(150)))
                destinationPath = EditorUtility.OpenFolderPanel("Choose Destination", destinationPath, "");
            destinationPath = destinationPath.Replace(Application.dataPath, "Assets");
            GUILayout.EndHorizontal();
        }

        public static void ModifyName(ref string nameSource, ref string nameTarget)
        {
            GUILayout.Label("Modify Name", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Replace: ", GUILayout.Width(55));
            nameSource = GUILayout.TextField(nameSource, GUILayout.Width(100));
            GUILayout.Label("With: ", GUILayout.Width(32));
            nameTarget = GUILayout.TextField(nameTarget, GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }

        public static bool ShowActionButton(string label, string tooltip, System.Action action)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var clicked = GUILayout.Button(new GUIContent(label, tooltip), GUILayout.Width(150), GUILayout.Height(30));
            if (clicked)
                action();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return clicked;
        }

        public static void DrawSeparator(float padding = 0, float thickness = 2)
        {
            GUILayout.Space(padding);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(thickness));
            GUILayout.Space(padding);
        }
    }
}
