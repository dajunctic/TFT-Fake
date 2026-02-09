using UnityEditor;
using UnityEngine;

namespace Dajunctic
{
    [CustomEditor(typeof(SquareAreaView))]
    public class SquareAreaEditor : Editor
    {
        private SquareAreaView _view;
        private bool _editMode = false;

        private void OnEnable() => _view = (SquareAreaView)target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_view.Data == null)
            {
                EditorGUILayout.HelpBox("Attach SquareAreaData to start!", MessageType.Warning);
                return;
            }

            _editMode = GUILayout.Toggle(_editMode, "Edit Mode", "Button");

            if (GUILayout.Button("Delete All"))
            {
                _view.Data.Clear();
                EditorUtility.SetDirty(_view.Data);
            }
        }

        private void OnSceneGUI()
        {
            if (!_editMode || _view.Data == null) return;

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Event e = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, _view.transform.position);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector2Int squareCoords = WorldToSquare(hitPoint);

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    if (_view.Data.TryGetTile(squareCoords, out var tile))
                        _view.Data.RemoveTile(squareCoords);
                    else
                        _view.Data.AddTile(squareCoords);

                    EditorUtility.SetDirty(_view.Data);
                    e.Use();
                }
            }
        }

        private Vector2Int WorldToSquare(Vector3 worldPos)
        {
            return _view.Data.WorldToSquare(worldPos, _view.transform.position);
        }
    }
}
