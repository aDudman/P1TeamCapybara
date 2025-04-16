using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RoomCoordinateWindow : EditorWindow
    {
        public static Vector2Int coordinates;
        public static string sceneName;
        private static bool isSubmitted = false;
        private static Action<Vector2Int, string> onSubmitCallback;

        public static void ShowWindow(Action<Vector2Int, string> callback)
        {
            coordinates = Vector2Int.zero;
            isSubmitted = false;
            onSubmitCallback = callback;
            EditorWindow.GetWindow(typeof(RoomCoordinateWindow), true, "Enter Room Coordinates");
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter Room Name", EditorStyles.boldLabel);
            sceneName = EditorGUILayout.TextField("Name", sceneName);

            GUILayout.Label("Enter Room Coordinates", EditorStyles.boldLabel);
            coordinates = EditorGUILayout.Vector2IntField("Coordinates", coordinates);

            if (GUILayout.Button("Submit"))
            {
                isSubmitted = true;
                onSubmitCallback?.Invoke(coordinates, sceneName);
                Close();
            }
        }

        public static bool IsSubmitted()
        {
            return isSubmitted;
        }
    }
}
