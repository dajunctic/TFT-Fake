using UnityEditor;
using UnityEngine;

namespace Dajunctic
{
    [CustomEditor(typeof(HexAreaView))]
    public class HexAreaEditor : Editor
    {
        private HexAreaView _view;
        private bool _editMode = false;

        private void OnEnable() => _view = (HexAreaView)target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_view.Data == null)
            {
                EditorGUILayout.HelpBox("Attack HexAreaData to start!", MessageType.Warning);
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
                Vector2Int hexCoords = WorldToHex(hitPoint);

                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    if (_view.Data.TryGetTile(hexCoords, out var tile))
                        _view.Data.RemoveTile(hexCoords); 
                    else
                        _view.Data.AddTile(hexCoords); 

                    EditorUtility.SetDirty(_view.Data);
                    e.Use();
                }
            }
        }

        private Vector2Int WorldToHex(Vector3 worldPos)
        {
            Vector3 localPos = worldPos - _view.transform.position;
            float q = (2f/3f * localPos.x) / _view.Data.HexSize;
            float r = (-1f/3f * localPos.x + Mathf.Sqrt(3f)/3f * localPos.z) / _view.Data.HexSize;
            return HexRound(q, r);
        }

        private Vector2Int HexRound(float q, float r)
        {
            float s = -q - r;
            int rq = Mathf.RoundToInt(q);
            int rr = Mathf.RoundToInt(r);
            int rs = Mathf.RoundToInt(s);

            float q_diff = Mathf.Abs(rq - q);
            float r_diff = Mathf.Abs(rr - r);
            float s_diff = Mathf.Abs(rs - s);

            if (q_diff > r_diff && q_diff > s_diff) rq = -rr - rs;
            else if (r_diff > s_diff) rr = -rq - rs;

            return new Vector2Int(rq, rr);
        }
    }
}