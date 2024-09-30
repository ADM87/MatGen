using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools.MatGen
{
    internal static class MatGenUtility
    {
        public static bool TryGetSelected<T>(out List<T> selected) where T : Object
        {
            selected = new List<T>();
            foreach (var obj in Selection.objects)
            {
                if (obj is T)
                    selected.Add(obj as T);
            }
            return selected.Count > 0;
        }
    }
}
