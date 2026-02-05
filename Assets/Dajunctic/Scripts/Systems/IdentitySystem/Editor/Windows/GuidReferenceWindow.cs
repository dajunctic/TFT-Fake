#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

namespace Dajunctic
{
    public class GuidReferenceWindow : EditorWindow
    {
        private SerializedProperty _targetProperty;
        private string[] _allIds;
        private string _selectedId;
        private string _search = "";
        private Vector2 _scroll;
        private int _perPage = 10;
        private int _page = 0;

        private Dictionary<string, Object> _assetMap;

        public static void Open(SerializedProperty prop, string prefix, Type[] assetTypes)
        {

            var ids = GuidReferenceHelper.GetIds(prefix, assetTypes).ToArray();

            var map = GuidReferenceHelper.AssetMap;

            var w = GetWindow<GuidReferenceWindow>(true, "Select Guid", true);
            w._targetProperty = prop;
            w._allIds = ids;
            w._selectedId = prop.stringValue;
            w._assetMap = map;
            w.minSize = new Vector2(650, 430);

            var main = EditorGUIUtility.GetMainWindowPosition();
            w.position = new Rect(
                main.x + main.width/2 - 325,
                main.y + main.height/2 - 215,
                650, 430
            );

            w.ShowUtility();
        }

        private void OnGUI()
        {
            DrawHeader();
            GUILayout.Space(6);
            DrawSearch();
            GUILayout.Space(6);
            DrawList();
        }

        void DrawHeader()
        {
            var r = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(r, new Color(0.17f,0.17f,0.17f,1f));
            GUILayout.Space(2);

            EditorGUILayout.BeginHorizontal();

            GUILayout.Label("Selected Guid:", EditorStyles.boldLabel, GUILayout.Width(90));

            if (!string.IsNullOrEmpty(_selectedId))
            {
                GUIStyle selStyle = new GUIStyle(EditorStyles.label);
                selStyle.normal.textColor = new Color(0.2f,1f,1f,1f);
                selStyle.hover.textColor = selStyle.normal.textColor;   
                selStyle.focused.textColor = selStyle.normal.textColor;
                GUILayout.Label(_selectedId, selStyle);
            }
            else
                GUILayout.Label("<none>", EditorStyles.label);

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2);
            EditorGUILayout.EndVertical();
        }


        void DrawSearch()
        {
            Rect r = GUILayoutUtility.GetRect(0, 26, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(r, new Color(0.13f,0.13f,0.13f,1f));
            r = new Rect(r.x+6, r.y+3, r.width-12, r.height-6);

            Rect searchRect = new Rect(r.x + 20, r.y, r.width - 40, r.height);
            Rect clearRect = new Rect(r.x + r.width - 19, r.y + 1, 24, 24);
            Rect iconRect = new Rect(r.x, r.y + 1, 18, 18);

            GUI.Label(iconRect, EditorGUIUtility.IconContent("Search Icon"));

            _search = EditorGUI.TextField(searchRect, _search);

            if (!string.IsNullOrEmpty(_search))
            {
                if (GUI.Button(clearRect, "x", EditorStyles.miniButton))
                    _search = "";
            }
        }

        void DrawList()
        {
            var filtered = string.IsNullOrEmpty(_search) ?
                _allIds :
                _allIds.Where(x => x.ToLower().Contains(_search.ToLower())).ToArray();

            int total = filtered.Length;
            int totalPages = Mathf.Max(1, Mathf.CeilToInt(total / (float)_perPage));
            _page = Mathf.Clamp(_page, 0, totalPages - 1);

            DrawPaging(filtered);

            int start = _page * _perPage;
            int end = Mathf.Min(start + _perPage, total);

            _scroll = EditorGUILayout.BeginScrollView(_scroll);

            GUIStyle idStyle = new GUIStyle(EditorStyles.label);
            idStyle.normal.textColor = new Color(0.2f,1f,1f,1f);
            idStyle.hover.textColor = idStyle.normal.textColor;   
            idStyle.focused.textColor = idStyle.normal.textColor;

            for (int i = start; i < end; i++)
            {
                string id = filtered[i];

                var h = EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                Rect r = GUILayoutUtility.GetRect(0, 24, GUILayout.ExpandWidth(true));

                if (r.Contains(Event.current.mousePosition))
                    EditorGUI.DrawRect(r, new Color(0.2f,0.2f,0.2f,0.4f));

                Rect labelRect = new Rect(r.x + 8, r.y+1, r.width-160, r.height-2);
                GUI.Label(labelRect, id, idStyle);

                float btnW = 60;
                Rect pingRect = new Rect(r.x + r.width - btnW*2 - 10, r.y+1, btnW, r.height-2);
                Rect useRect = new Rect(r.x + r.width - btnW - 4, r.y+1, btnW, r.height-2);

                using (new EditorGUI.DisabledScope(_assetMap == null || !_assetMap.ContainsKey(id)))
                {
                    if (GUI.Button(pingRect, "Ping"))
                    {
                        if (_assetMap.TryGetValue(id, out var obj))
                            EditorGUIUtility.PingObject(obj);
                    }
                }

                if (GUI.Button(useRect, "Use"))
                {
                    _targetProperty.stringValue = id;
                    _targetProperty.serializedObject.ApplyModifiedProperties();
                    Close();
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.Space(1);
            }

            EditorGUILayout.EndScrollView();
        }

        void DrawPaging(string[] filtered)
        {
            int total = filtered.Length;
            int totalPages = Mathf.Max(1, Mathf.CeilToInt(total / (float)_perPage));

            var h = EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

            GUILayout.Label($"Display {1 + _page * _perPage} to {Mathf.Min((_page+1)*_perPage, total)} of {total}");

            GUILayout.FlexibleSpace();
            _perPage = EditorGUILayout.IntField(_perPage, GUILayout.Width(40));

            GUI.enabled = _page > 0;
            if (GUILayout.Button("<<", GUILayout.Width(30))) _page = 0;
            if (GUILayout.Button("<", GUILayout.Width(30))) _page--;
            GUI.enabled = true;

            GUILayout.Label($"{_page+1}/{totalPages}", GUILayout.Width(50));

            GUI.enabled = _page < totalPages - 1;
            if (GUILayout.Button(">", GUILayout.Width(30))) _page++;
            if (GUILayout.Button(">>", GUILayout.Width(30))) _page = totalPages - 1;
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif