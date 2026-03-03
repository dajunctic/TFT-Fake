using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    public class SkillGraphEditorWindow : EditorWindow
    {
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is SkillGraph graph)
            {
                OpenWindow();
                var window = GetWindow<SkillGraphEditorWindow>();
                window.LoadGraph(graph);
                return true;
            }
            return false;
        }

        [SerializeField] private SkillGraph _currentGraph;
        private SkillGraphView _graphView;
        [SerializeField] private CombatActor _previewActor;
        private PreviewRenderUtility _previewRenderUtility;
        private GameObject _previewInstance;
        [SerializeField] private Vector2 _previewDir = new Vector2(120, -20);
        [SerializeField] private float _previewDistance = 6f;
        [SerializeField] private Vector3 _previewPivot = Vector3.up * 0.8f;
        private Dictionary<string, int> _nodeTriggerCounts = new();

        // Dummy state
        [SerializeField] private List<DummySpawnEntry> _dummyEntries = new();
        private List<GameObject> _dummyInstances = new();
        private PreviewSkillServiceProvider _previewServices;

        // Dummy rotation drag
        private int _selectedDummyIndex = -1;   // which dummy is selected for rotate
        private bool _isDraggingDummy = false;

        // Splitter
        private VisualElement _leftPane, _rightPane, _divider;
        private bool _isDraggingDivider;
        [SerializeField] private float _splitRatio = 0.5f;
        [SerializeField] private bool _isDirty;

        private VisualElement _dummyListContainer;

        public static void OpenWindow()
        {
            var window = GetWindow<SkillGraphEditorWindow>();
            window.titleContent = new GUIContent("Skill Graph Editor");
        }

        private void OnEnable()
        {
            rootVisualElement.Clear();
            ConstructGraphView();
            GenerateToolbar();

            _previewRenderUtility = new PreviewRenderUtility();
            _previewRenderUtility.camera.fieldOfView = 30f;
            _previewRenderUtility.camera.farClipPlane = 1000;
            _previewRenderUtility.camera.nearClipPlane = 0.1f;

            _previewServices = new PreviewSkillServiceProvider(_previewRenderUtility);

            EditorApplication.update += OnEditorUpdate;
            Undo.undoRedoPerformed += OnUndoRedoPerformed;

            if (_currentGraph != null)
            {
                bool wasDirty = _isDirty;
                LoadGraph(_currentGraph);
                if (wasDirty) SetDirty(true);
            }
            if (_previewActor != null) UpdatePreviewInstance();
            RebuildDummyInstances();
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            _previewServices?.Cleanup();
            if (_previewRenderUtility != null) _previewRenderUtility.Cleanup();
            if (_previewInstance != null) DestroyImmediate(_previewInstance);
            ClearDummyInstances();
        }

        private void OnEditorUpdate()
        {
            if (_previewInstance != null)
            {
                var anim = _previewInstance.GetComponentInChildren<Animator>();
                if (anim != null && !Application.isPlaying) anim.Update(Time.deltaTime);
            }
            foreach (var dummy in _dummyInstances)
            {
                if (dummy == null) continue;
                var anim = dummy.GetComponentInChildren<Animator>();
                if (anim != null && !Application.isPlaying) anim.Update(Time.deltaTime);
            }

            if (!Application.isPlaying)
            {
                _previewServices?.Update();
            }

            Repaint();
        }

        // ─── UI CONSTRUCTION ────────────────────────────────────────────────────

        private void ConstructGraphView()
        {
            rootVisualElement.style.flexDirection = FlexDirection.Column;

            var contentArea = new VisualElement();
            contentArea.style.flexDirection = FlexDirection.Row;
            contentArea.style.flexGrow = 1;
            rootVisualElement.Add(contentArea);

            // Left: Graph
            _leftPane = new VisualElement();
            _leftPane.style.flexGrow = _splitRatio;
            _leftPane.style.flexBasis = 0;
            _graphView = new SkillGraphView(this) { name = "Skill Graph" };
            _graphView.StretchToParentSize();
            _leftPane.Add(_graphView);
            contentArea.Add(_leftPane);

            // Divider
            _divider = new VisualElement();
            _divider.style.width = 4;
            _divider.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 1f);
            _divider.RegisterCallback<MouseDownEvent>(evt => { _isDraggingDivider = true; _divider.CaptureMouse(); evt.StopPropagation(); });
            _divider.RegisterCallback<MouseUpEvent>(evt => { _isDraggingDivider = false; _divider.ReleaseMouse(); evt.StopPropagation(); });
            _divider.RegisterCallback<MouseMoveEvent>(OnDividerDrag);
            contentArea.Add(_divider);

            // Right: Settings + Preview
            _rightPane = new VisualElement();
            _rightPane.style.flexGrow = 1f - _splitRatio;
            _rightPane.style.flexBasis = 0;
            _rightPane.style.backgroundColor = new Color(0.18f, 0.18f, 0.18f, 1f);
            contentArea.Add(_rightPane);

            var rightScroll = new ScrollView(ScrollViewMode.Vertical);
            rightScroll.style.flexGrow = 0;
            rightScroll.style.paddingLeft = rightScroll.style.paddingRight = 10;
            rightScroll.style.paddingTop = rightScroll.style.paddingBottom = 10;
            _rightPane.Add(rightScroll);

            AddSectionLabel(rightScroll, "▶ Preview Settings");

            var actorField = new ObjectField("Caster Actor (Prefab)") { objectType = typeof(CombatActor) };
            actorField.value = _previewActor;
            actorField.RegisterValueChangedCallback(evt => { _previewActor = evt.newValue as CombatActor; UpdatePreviewInstance(); });
            rightScroll.Add(actorField);

            // ─── Dummy Targets ─────────────────────────────────────────────
            AddSectionLabel(rightScroll, "▶ Dummy Targets");

            var rotateHint = new Label("Tip: Click dummy marker in preview → drag left/right to rotate");
            rotateHint.style.fontSize = 9;
            rotateHint.style.color = new Color(0.5f, 0.8f, 0.5f, 1f);
            rotateHint.style.marginBottom = 4;
            rightScroll.Add(rotateHint);

            _dummyListContainer = new VisualElement();
            rightScroll.Add(_dummyListContainer);
            RebuildDummyUI();

            var addBtn = new Button(() =>
            {
                _dummyEntries.Add(new DummySpawnEntry());
                RebuildDummyUI();
                RebuildDummyInstances();
            })
            { text = "+ Add Dummy Target" };
            addBtn.style.marginTop = 5;
            rightScroll.Add(addBtn);

            // Preview IMGUI
            var previewIMGUI = new IMGUIContainer(OnPreviewGUI);
            previewIMGUI.style.flexGrow = 1;
            previewIMGUI.style.marginTop = 10;
            previewIMGUI.style.backgroundColor = new Color(0.12f, 0.12f, 0.12f, 1f);
            _rightPane.Add(previewIMGUI);
        }

        private void AddSectionLabel(VisualElement parent, string text)
        {
            var lbl = new Label(text);
            lbl.style.unityFontStyleAndWeight = FontStyle.Bold;
            lbl.style.marginTop = 12;
            lbl.style.marginBottom = 4;
            lbl.style.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            parent.Add(lbl);
        }

        // ─── DUMMY UI ────────────────────────────────────────────────────────

        private void RebuildDummyUI()
        {
            _dummyListContainer.Clear();
            for (int i = 0; i < _dummyEntries.Count; i++)
            {
                int idx = i;
                var entry = _dummyEntries[i];

                var card = new VisualElement();
                card.style.backgroundColor = new Color(0.22f, 0.22f, 0.22f, 1f);
                card.style.marginBottom = 4;
                card.style.paddingLeft = card.style.paddingRight = 6;
                card.style.paddingTop = card.style.paddingBottom = 5;
                card.style.borderTopLeftRadius = card.style.borderTopRightRadius =
                    card.style.borderBottomLeftRadius = card.style.borderBottomRightRadius = 4;

                // Header
                var header = new VisualElement();
                header.style.flexDirection = FlexDirection.Row;
                header.style.justifyContent = Justify.SpaceBetween;

                bool isSelected = _selectedDummyIndex == idx;
                var lbl = new Label($"Dummy #{idx + 1}" + (isSelected ? "  ◀ SELECTED" : ""));
                lbl.style.unityFontStyleAndWeight = FontStyle.Bold;
                lbl.style.color = isSelected ? new Color(0.4f, 1f, 0.4f, 1f) : new Color(1f, 0.7f, 0.2f, 1f);
                lbl.style.fontSize = 11;

                var removeBtn = new Button(() =>
                {
                    _dummyEntries.RemoveAt(idx);
                    if (_selectedDummyIndex >= _dummyEntries.Count) _selectedDummyIndex = -1;
                    RebuildDummyUI();
                    RebuildDummyInstances();
                })
                { text = "✕" };
                removeBtn.style.color = new Color(1f, 0.4f, 0.4f, 1f);
                removeBtn.style.width = 22; removeBtn.style.height = 22;

                header.Add(lbl); header.Add(removeBtn);
                card.Add(header);

                // Prefab
                var prefabField = new ObjectField("Prefab") { objectType = typeof(CombatActor) };
                prefabField.value = entry.actorPrefab;
                prefabField.RegisterValueChangedCallback(evt =>
                {
                    _dummyEntries[idx].actorPrefab = evt.newValue as CombatActor;
                    RebuildDummyInstances();
                });
                card.Add(prefabField);

                // Team
                var teamField = new EnumField("Team", entry.team);
                teamField.RegisterValueChangedCallback(evt =>
                {
                    _dummyEntries[idx].team = (Team)evt.newValue;
                    RebuildDummyInstances();
                });
                card.Add(teamField);

                // Position
                var posField = new Vector3Field("Spawn Position");
                posField.value = entry.spawnPosition;
                posField.RegisterValueChangedCallback(evt =>
                {
                    _dummyEntries[idx].spawnPosition = evt.newValue;
                    RebuildDummyInstances();
                });
                card.Add(posField);

                // Y Rotation slider
                var rotRow = new VisualElement();
                rotRow.style.flexDirection = FlexDirection.Row;
                rotRow.style.alignItems = Align.Center;
                rotRow.style.marginTop = 2;

                var rotLbl = new Label("Y Rotation");
                rotLbl.style.width = 80;
                rotLbl.style.fontSize = 10;

                var rotSlider = new Slider(0f, 360f) { value = entry.yRotation };
                rotSlider.style.flexGrow = 1;
                rotSlider.RegisterValueChangedCallback(evt =>
                {
                    _dummyEntries[idx].yRotation = evt.newValue;
                    ApplyDummyRotation(idx);
                });

                var rotValLbl = new Label($"{entry.yRotation:F0}°");
                rotValLbl.style.width = 32;
                rotValLbl.style.fontSize = 10;
                rotValLbl.style.unityTextAlign = TextAnchor.MiddleRight;
                rotSlider.RegisterValueChangedCallback(evt => rotValLbl.text = $"{evt.newValue:F0}°");

                rotRow.Add(rotLbl); rotRow.Add(rotSlider); rotRow.Add(rotValLbl);
                card.Add(rotRow);

                // Click to select
                card.RegisterCallback<MouseDownEvent>(_ =>
                {
                    _selectedDummyIndex = idx;
                    RebuildDummyUI();
                });

                _dummyListContainer.Add(card);
            }
        }

        // ─── DUMMY INSTANCES ─────────────────────────────────────────────────

        private void ClearDummyInstances()
        {
            foreach (var go in _dummyInstances)
                if (go != null) DestroyImmediate(go);
            _dummyInstances.Clear();
        }

        private void RebuildDummyInstances()
        {
            ClearDummyInstances();
            if (_previewRenderUtility == null) return;

            foreach (var entry in _dummyEntries)
            {
                if (entry.actorPrefab == null) continue;

                var inst = Instantiate(entry.actorPrefab.gameObject);
                inst.hideFlags = HideFlags.HideAndDontSave;
                inst.transform.SetPositionAndRotation(entry.spawnPosition, Quaternion.Euler(0, entry.yRotation, 0));

                foreach (var mb in inst.GetComponentsInChildren<MonoBehaviour>())
                    if (mb is not CombatActor) mb.enabled = false;

                var anim = inst.GetComponentInChildren<Animator>();
                if (anim != null) { anim.enabled = true; anim.cullingMode = AnimatorCullingMode.AlwaysAnimate; }

                // Set team
                var actor = inst.GetComponent<CombatActor>();
                if (actor != null)
                {
                    var tf = typeof(CombatActor).GetField("team",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    tf?.SetValue(actor, entry.team);
                }

                _previewRenderUtility.AddSingleGO(inst);
                _dummyInstances.Add(inst);
            }
        }

        /// <summary>Only updates rotation of an existing dummy instance without full rebuild.</summary>
        private void ApplyDummyRotation(int idx)
        {
            if (idx < 0 || idx >= _dummyInstances.Count) return;
            var inst = _dummyInstances[idx];
            if (inst == null) return;
            inst.transform.rotation = Quaternion.Euler(0, _dummyEntries[idx].yRotation, 0);
        }

        // ─── PREVIEW GUI ─────────────────────────────────────────────────────

        private void OnPreviewGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(200, 200, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            if (_previewInstance == null)
                GUI.Label(new Rect(rect.x + 8, rect.y + 8, 300, 20), "Set Caster Actor prefab →", EditorStyles.miniLabel);

            // Bottom overlay: dummy info
            float yOff = 8;
            for (int i = 0; i < _dummyEntries.Count; i++)
            {
                var e = _dummyEntries[i];
                if (e.actorPrefab == null) continue;
                bool sel = i == _selectedDummyIndex;
                string info = $"{(sel ? "► " : "")}Dummy #{i + 1}  [{e.team}]  {e.actorPrefab.name}  @ {e.spawnPosition}  ↻{e.yRotation:F0}°";
                GUI.Label(new Rect(rect.x + 8, rect.y + rect.height - 20 - yOff, 600, 18), info, EditorStyles.miniLabel);
                yOff += 18;
            }

            // Selection hint
            if (_selectedDummyIndex >= 0 && _selectedDummyIndex < _dummyEntries.Count)
            {
                var style = new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = new Color(0.4f, 1f, 0.4f, 1f) } };
                GUI.Label(new Rect(rect.x + rect.width - 220, rect.y + 8, 210, 18),
                    $"Dragging: Dummy #{_selectedDummyIndex + 1}  ↻{_dummyEntries[_selectedDummyIndex].yRotation:F0}°", style);
            }

            HandlePreviewInput(rect);

            _previewRenderUtility.BeginPreview(rect, GUIStyle.none);
            _previewRenderUtility.camera.transform.SetPositionAndRotation(
                _previewPivot + _previewRenderUtility.camera.transform.forward * -_previewDistance,
                Quaternion.Euler(-_previewDir.y, -_previewDir.x, 0));
            _previewRenderUtility.lights[0].intensity = 2f;
            _previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(40f, 40f, 0f);
            _previewRenderUtility.Render(true);
            DrawGrid();
            DrawDummyMarkers();
            Texture result = _previewRenderUtility.EndPreview();
            GUI.DrawTexture(rect, result);
        }

        private void DrawGrid()
        {
            GL.PushMatrix();
            GL.LoadProjectionMatrix(_previewRenderUtility.camera.projectionMatrix);
            GL.modelview = _previewRenderUtility.camera.worldToCameraMatrix;
            var mat = new Material(Shader.Find("Hidden/Internal-Colored")); mat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(new Color(0.6f, 0.6f, 0.6f, 0.4f));
            for (float i = -10f; i <= 10f; i++)
            {
                GL.Vertex(new Vector3(i, 0, -10)); GL.Vertex(new Vector3(i, 0, 10));
                GL.Vertex(new Vector3(-10, 0, i)); GL.Vertex(new Vector3(10, 0, i));
            }
            GL.End();
            GL.PopMatrix();
        }

        private void DrawDummyMarkers()
        {
            GL.PushMatrix();
            GL.LoadProjectionMatrix(_previewRenderUtility.camera.projectionMatrix);
            GL.modelview = _previewRenderUtility.camera.worldToCameraMatrix;
            var mat = new Material(Shader.Find("Hidden/Internal-Colored")); mat.SetPass(0);

            // Caster marker — blue circle + forward arrow
            DrawCircleGL(Vector3.zero, 0.35f, new Color(0.2f, 0.6f, 1f, 1f));
            DrawArrowGL(Vector3.zero, Vector3.forward, 0.5f, new Color(0.2f, 0.6f, 1f, 1f));

            // Dummy markers
            for (int i = 0; i < _dummyEntries.Count; i++)
            {
                var entry = _dummyEntries[i];
                if (entry.actorPrefab == null) continue;

                bool selected = i == _selectedDummyIndex;
                Color col = selected ? new Color(0.4f, 1f, 0.4f, 1f) : new Color(1f, 0.5f, 0.1f, 1f);
                float radius = selected ? 0.42f : 0.35f;

                DrawCircleGL(entry.spawnPosition, radius, col);

                // X cross
                GL.Begin(GL.LINES);
                GL.Color(col);
                var p = entry.spawnPosition + Vector3.up * 0.01f;
                float s = 0.2f;
                GL.Vertex(p + new Vector3(-s, 0, -s)); GL.Vertex(p + new Vector3(s, 0, s));
                GL.Vertex(p + new Vector3(-s, 0, s)); GL.Vertex(p + new Vector3(s, 0, -s));
                GL.End();

                // Forward arrow showing yRotation
                Vector3 fwd = Quaternion.Euler(0, entry.yRotation, 0) * Vector3.forward;
                DrawArrowGL(entry.spawnPosition, fwd, 0.6f, col);
            }

            GL.PopMatrix();
        }

        private void DrawCircleGL(Vector3 center, float radius, Color color)
        {
            int segs = 24;
            GL.Begin(GL.LINES);
            GL.Color(color);
            for (int j = 0; j < segs; j++)
            {
                float a1 = j * Mathf.PI * 2f / segs;
                float a2 = (j + 1) * Mathf.PI * 2f / segs;
                GL.Vertex(center + new Vector3(Mathf.Cos(a1) * radius, 0.01f, Mathf.Sin(a1) * radius));
                GL.Vertex(center + new Vector3(Mathf.Cos(a2) * radius, 0.01f, Mathf.Sin(a2) * radius));
            }
            GL.End();
        }

        private void DrawArrowGL(Vector3 origin, Vector3 dir, float length, Color color)
        {
            dir.y = 0; dir.Normalize();
            if (dir == Vector3.zero) return;
            Vector3 tip = origin + dir * length + Vector3.up * 0.01f;
            Vector3 from = origin + Vector3.up * 0.01f;

            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(from); GL.Vertex(tip);
            // arrowhead
            Vector3 right = Vector3.Cross(Vector3.up, dir).normalized * 0.15f;
            GL.Vertex(tip); GL.Vertex(tip - dir * 0.2f + right);
            GL.Vertex(tip); GL.Vertex(tip - dir * 0.2f - right);
            GL.End();
        }

        // ─── INPUT ────────────────────────────────────────────────────────────

        private void HandlePreviewInput(Rect rect)
        {
            int cid = GUIUtility.GetControlID("PreviewInput".GetHashCode(), FocusType.Passive);
            Event evt = Event.current;

            switch (evt.GetTypeForControl(cid))
            {
                case EventType.MouseDown when rect.Contains(evt.mousePosition):
                    GUIUtility.hotControl = cid;
                    EditorGUIUtility.SetWantsMouseJumping(1);

                    if (evt.button == 0)
                    {
                        // Check if clicking near a dummy marker → select + start rotate drag
                        int hit = HitTestDummyMarker(rect, evt.mousePosition);
                        if (hit >= 0)
                        {
                            _selectedDummyIndex = hit;
                            _isDraggingDummy = true;
                            RebuildDummyUI(); // refresh highlight
                        }
                        else
                        {
                            _isDraggingDummy = false;
                        }
                    }
                    evt.Use();
                    break;

                case EventType.MouseUp when GUIUtility.hotControl == cid:
                    GUIUtility.hotControl = 0;
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    _isDraggingDummy = false;
                    break;

                case EventType.MouseDrag when GUIUtility.hotControl == cid:
                    if (evt.button == 0 && _isDraggingDummy && _selectedDummyIndex >= 0 &&
                        _selectedDummyIndex < _dummyEntries.Count)
                    {
                        // Horizontal drag → rotate dummy
                        float sensitivity = 1.2f;
                        _dummyEntries[_selectedDummyIndex].yRotation =
                            (_dummyEntries[_selectedDummyIndex].yRotation + evt.delta.x * sensitivity + 360f) % 360f;
                        ApplyDummyRotation(_selectedDummyIndex);
                        RebuildDummyUI(); // update slider label
                    }
                    else if (evt.button == 1)
                    {
                        // Right drag → orbit camera
                        _previewDir -= evt.delta * (evt.shift ? 3 : 1) / Mathf.Min(rect.width, rect.height) * 140f;
                        _previewDir.y = Mathf.Clamp(_previewDir.y, -90f, 90f);
                    }
                    else if (!_isDraggingDummy && (evt.button == 0 || evt.button == 2))
                    {
                        // Pan camera
                        var cam = _previewRenderUtility.camera.transform;
                        _previewPivot -= (cam.right * evt.delta.x - cam.up * evt.delta.y) * 0.01f * (_previewDistance / 6f);
                    }
                    evt.Use(); GUI.changed = true;
                    break;

                case EventType.ScrollWheel when rect.Contains(evt.mousePosition):
                    _previewDistance = Mathf.Max(0.1f, _previewDistance + evt.delta.y * 0.05f);
                    evt.Use(); GUI.changed = true;
                    break;
            }
        }

        /// <summary>Returns index of dummy whose marker was clicked (screen-space hit test), or -1.</summary>
        private int HitTestDummyMarker(Rect previewRect, Vector2 mousePos)
        {
            var cam = _previewRenderUtility.camera;
            float hitPixelRadius = 18f; // pixels

            for (int i = 0; i < _dummyEntries.Count; i++)
            {
                if (_dummyEntries[i].actorPrefab == null) continue;

                Vector3 worldPos = _dummyEntries[i].spawnPosition;
                // Transform to viewport then to GUI rect
                Vector3 vp = cam.WorldToViewportPoint(worldPos);
                if (vp.z <= 0) continue;

                Vector2 screenPos = new Vector2(
                    previewRect.x + vp.x * previewRect.width,
                    previewRect.y + (1f - vp.y) * previewRect.height);

                if (Vector2.Distance(mousePos, screenPos) <= hitPixelRadius)
                    return i;
            }
            return -1;
        }

        private void OnDividerDrag(MouseMoveEvent evt)
        {
            if (!_isDraggingDivider) return;
            float totalWidth = _divider.parent.resolvedStyle.width;
            if (totalWidth <= 0) return;
            _splitRatio = Mathf.Clamp((evt.mousePosition.x - _divider.parent.worldBound.x) / totalWidth, 0.1f, 0.9f);
            _leftPane.style.flexGrow = _splitRatio;
            _rightPane.style.flexGrow = 1f - _splitRatio;
            evt.StopPropagation();
        }

        // ─── CASTER PREVIEW ──────────────────────────────────────────────────

        private void UpdatePreviewInstance()
        {
            if (_previewInstance != null) DestroyImmediate(_previewInstance);
            if (_previewActor == null) return;

            _previewInstance = Instantiate(_previewActor.gameObject);
            _previewInstance.hideFlags = HideFlags.HideAndDontSave;
            _previewInstance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            foreach (var mb in _previewInstance.GetComponentsInChildren<MonoBehaviour>())
                if (mb is not CombatActor) mb.enabled = false;

            var anim = _previewInstance.GetComponentInChildren<Animator>();
            if (anim != null) { anim.enabled = true; anim.cullingMode = AnimatorCullingMode.AlwaysAnimate; }

            _previewRenderUtility.AddSingleGO(_previewInstance);
        }

        // ─── GRAPH EXECUTION ─────────────────────────────────────────────────

        public void OnNodeSelectionChanged(SkillNode node)
        {
            if (node != null) { Selection.activeObject = node; EditorGUIUtility.PingObject(node); }
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            toolbar.Add(new Button(SaveData) { text = "Save Graph" });
            toolbar.Add(new Button(PreviewSkill) { text = "▶ Preview Skill" });
            toolbar.Add(new Button(() =>
            {
                _previewServices?.Cleanup();
                UpdatePreviewInstance();
                RebuildDummyInstances();
                ClearAllNodeHighlights();
            })
            { text = "Reset Preview" });
            toolbar.Add(new Button(() => _graphView.ClearGraph()) { text = "Clear Graph" });
            rootVisualElement.Insert(0, toolbar);
        }

        public void SaveData()
        {
            if (_currentGraph == null) return;
            _graphView.SaveGraph(_currentGraph);
            EditorUtility.SetDirty(_currentGraph);
            AssetDatabase.SaveAssets();
            SetDirty(false);
            Debug.Log("Graph Saved: " + _currentGraph.name);
        }

        private void PreviewSkill()
        {
            if (_currentGraph == null || _previewInstance == null) return;
            _previewServices?.Cleanup();
            _nodeTriggerCounts.Clear();
            ClearAllNodeHighlights();

            var actor = _previewInstance.GetComponent<CombatActor>();
            if (actor == null) return;

            var context = new SkillExecutionContext(actor, _previewServices);
            InjectPreviewDummies(context);

            var entryNode = _currentGraph.nodes.OfType<Nodes.EntryNode>().FirstOrDefault();
            if (entryNode != null) ExecuteNodePreview(entryNode, context);
        }

        private void InjectPreviewDummies(SkillExecutionContext context)
        {
            var dummies = new List<IDamageTaker>();
            foreach (var inst in _dummyInstances)
            {
                if (inst == null) continue;
                var ca = inst.GetComponent<CombatActor>();
                if (ca != null) dummies.Add(ca);
            }
            context.nodeOutputs["__preview_dummies__"] = dummies;
        }

        private void ClearAllNodeHighlights()
        {
            if (_graphView == null) return;
            foreach (var nv in _graphView.nodes.ToList().OfType<SkillNodeView>())
                nv.ClearHighlight();
        }

        private void ExecuteNodePreview(SkillNode node, SkillExecutionContext context)
        {
            _graphView?.GetNodeViewByGuid(node.guid)?.SetHighlight();

            node.Init(context, () =>
            {
                var outgoing = _currentGraph.links
                    .Where(l => l.baseNodeGuid == node.guid && l.portName == "Out").ToList();

                // Slow down the preview flow so highlights are visible
                UnityEditor.EditorApplication.CallbackFunction nextCall = null;
                float waitStart = (float)UnityEditor.EditorApplication.timeSinceStartup;
                float stepDelay = 0f;

                nextCall = () =>
                {
                    if ((float)UnityEditor.EditorApplication.timeSinceStartup - waitStart < stepDelay) return;
                    UnityEditor.EditorApplication.update -= nextCall;

                    foreach (var link in outgoing)
                    {
                        var next = _currentGraph.nodes.FirstOrDefault(n => n.guid == link.targetNodeGuid);
                        if (next == null) continue;

                        if (!_nodeTriggerCounts.ContainsKey(next.guid)) _nodeTriggerCounts[next.guid] = 0;
                        _nodeTriggerCounts[next.guid]++;

                        int totalIn = _currentGraph.links.Count(l =>
                            l.targetNodeGuid == next.guid && l.targetPortName == "In");
                        if (_nodeTriggerCounts[next.guid] >= totalIn)
                            ExecuteNodePreview(next, context);
                    }
                };
                UnityEditor.EditorApplication.update += nextCall;
            });

            node.Execute();
        }

        // ─── GRAPH MANAGEMENT ────────────────────────────────────────────────

        public SkillGraph CurrentGraph => _currentGraph;

        public void SetDirty(bool dirty)
        {
            if (_isDirty == dirty) return;
            _isDirty = dirty;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            string n = (_currentGraph != null && !string.IsNullOrEmpty(_currentGraph.name))
                ? _currentGraph.name : "Skill Graph Editor";
            titleContent = new GUIContent(n + (_isDirty ? "*" : ""));
        }

        public void LoadGraph(SkillGraph graph)
        {
            _currentGraph = graph;
            _graphView?.PopulateView(_currentGraph);
            _isDirty = false;
            UpdateTitle();
        }

        private void OnUndoRedoPerformed()
        {
            if (_currentGraph != null) LoadGraph(_currentGraph);
        }
    }

    // ─── DATA ─────────────────────────────────────────────────────────────────

    [System.Serializable]
    public class DummySpawnEntry
    {
        public CombatActor actorPrefab;
        public Team team = Team.Opponent;
        public Vector3 spawnPosition = new Vector3(3f, 0f, 0f);
        [Range(0f, 360f)]
        public float yRotation = 180f; // mặc định quay mặt về phía caster
    }
}
