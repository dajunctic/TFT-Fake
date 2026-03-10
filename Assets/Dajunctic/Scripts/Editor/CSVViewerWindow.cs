using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace Dajunctic.Editor
{
    public class CSVViewerWindow : EditorWindow
    {
        private TextAsset _csvAsset;
        private string _rawContent;
        private List<string[]> _data = new List<string[]>();
        private string[] _headers;
        private string _searchQuery = "";
        private Vector2 _scrollPos;

        // Sorting
        private int _sortIndex = -1;
        private bool _sortAscending = true;

        // Styles
        private GUIStyle _headerStyle;
        private GUIStyle _cellStyle;
        private GUIStyle _zebraStyle;
        private GUIStyle _searchStyle;

        [MenuItem("Window/Dajunctic/CSV Viewer")]
        public static void ShowWindow()
        {
            GetWindow<CSVViewerWindow>("CSV Viewer");
        }

        private void OnEnable()
        {
            InitStyles();
        }

        private void InitStyles()
        {
            _headerStyle = new GUIStyle(EditorStyles.miniButtonMid)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 13,
                fontStyle = FontStyle.Bold,
                fixedHeight = 30,
                normal = { textColor = new Color(0.4f, 1f, 1f) }, // Cyan neon
                hover = { textColor = Color.white }
            };

            _cellStyle = new GUIStyle(EditorStyles.label)
            {
                padding = new RectOffset(10, 10, 5, 5),
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft
            };

            _zebraStyle = new GUIStyle();
            _zebraStyle.normal.background = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.05f));

            _searchStyle = new GUIStyle(EditorStyles.toolbarSearchField)
            {
                fixedHeight = 22,
                fontSize = 12
            };
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i) pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            DrawHeader();
            EditorGUILayout.EndVertical();

            if (_headers != null && _headers.Length > 0)
            {
                DrawSearch();
                DrawTable();
            }
            else
            {
                EditorGUILayout.HelpBox("Select a CSV file to view.", MessageType.Info);
            }
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            _csvAsset = (TextAsset)EditorGUILayout.ObjectField("CSV Asset", _csvAsset, typeof(TextAsset), false);
            if (EditorGUI.EndChangeCheck() && _csvAsset != null)
            {
                ParseCSV();
            }

            if (GUILayout.Button("Load External", GUILayout.Width(100)))
            {
                string path = EditorUtility.OpenFilePanel("Select CSV", "", "csv");
                if (!string.IsNullOrEmpty(path))
                {
                    _rawContent = File.ReadAllText(path);
                    ParseRawContent();
                }
            }

            if (GUILayout.Button("Refresh", GUILayout.Width(70)))
            {
                if (_csvAsset != null) ParseCSV();
                else ParseRawContent();
            }

            if (GUILayout.Button("Export Filtered", GUILayout.Width(110)))
            {
                ExportFiltered();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ExportFiltered()
        {
            if (_headers == null || _data == null) return;
            string path = EditorUtility.SaveFilePanel("Export CSV", "", "FilteredData.csv", "csv");
            if (string.IsNullOrEmpty(path)) return;

            var filtered = GetFilteredData();
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(",", _headers.Select(h => $"\"{h}\"")));
                foreach (var row in filtered)
                {
                    writer.WriteLine(string.Join(",", row.Select(c => $"\"{c}\"")));
                }
            }
            Debug.Log($"[CSV Viewer] Exported {filtered.Count} rows to {path}");
            EditorUtility.RevealInFinder(path);
        }

        private void DrawSearch()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Search:", GUILayout.Width(50));
            _searchQuery = EditorGUILayout.TextField(_searchQuery, _searchStyle);
            if (GUILayout.Button("X", EditorStyles.toolbarButton, GUILayout.Width(20)))
            {
                _searchQuery = "";
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTable()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            // Table Header
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            for (int i = 0; i < _headers.Length; i++)
            {
                string headerText = _headers[i];
                if (_sortIndex == i)
                {
                    headerText += _sortAscending ? " ▲" : " ▼";
                }

                if (GUILayout.Button(headerText, _headerStyle, GUILayout.MinWidth(120)))
                {
                    SortBy(i);
                }
            }
            EditorGUILayout.EndHorizontal();

            // Table Data
            var filteredData = GetFilteredData();
            for (int i = 0; i < filteredData.Count; i++)
            {
                Rect rect = EditorGUILayout.BeginHorizontal(GUILayout.Height(25));
                if (i % 2 == 0)
                {
                    GUI.Box(rect, "", _zebraStyle);
                }

                foreach (var cell in filteredData[i])
                {
                    // Tint tier cells
                    Color originalColor = GUI.contentColor;
                    if (cell == "5") GUI.contentColor = new Color(1f, 0.8f, 0f); // Gold
                    else if (cell == "4") GUI.contentColor = new Color(1f, 0.4f, 1f); // Purple
                    else if (cell == "3") GUI.contentColor = new Color(0.4f, 0.7f, 1f); // Blue

                    EditorGUILayout.LabelField(cell, _cellStyle, GUILayout.MinWidth(120));
                    GUI.contentColor = originalColor;
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(2);
            }

            EditorGUILayout.EndScrollView();
        }

        private List<string[]> GetFilteredData()
        {
            if (string.IsNullOrEmpty(_searchQuery)) return _data;
            return _data.Where(row => row.Any(cell => cell.IndexOf(_searchQuery, System.StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
        }

        private void ParseCSV()
        {
            if (_csvAsset == null) return;
            _rawContent = _csvAsset.text;
            ParseRawContent();
        }

        private void ParseRawContent()
        {
            _data.Clear();
            _headers = null;
            _sortIndex = -1;

            if (string.IsNullOrEmpty(_rawContent)) return;

            // Simple regex for CSV parsing (handles quotes and commas)
            string pattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
            string[] lines = _rawContent.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 0)
            {
                _headers = Regex.Split(lines[0], pattern).Select(s => s.Trim('\"')).ToArray();
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] row = Regex.Split(lines[i], pattern).Select(s => s.Trim('\"')).ToArray();
                    if (row.Length == _headers.Length)
                    {
                        _data.Add(row);
                    }
                }
            }
        }

        private void SortBy(int index)
        {
            if (_sortIndex == index)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortIndex = index;
                _sortAscending = true;
            }

            if (_sortAscending)
            {
                _data = _data.OrderBy(row => row[index]).ToList();
            }
            else
            {
                _data = _data.OrderByDescending(row => row[index]).ToList();
            }
        }
    }
}
