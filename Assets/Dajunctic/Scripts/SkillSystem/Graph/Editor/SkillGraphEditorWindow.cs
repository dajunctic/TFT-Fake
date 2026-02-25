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

        private SkillGraph _currentGraph;
        private SkillGraphView _graphView;
        private CombatActor _previewActor;
        private PreviewRenderUtility _previewRenderUtility;
        private GameObject _previewInstance;
        private Vector2 _previewDir = new Vector2(120, -20);
        private float _previewDistance = 6f;
        private Vector3 _previewPivot = Vector3.up * 0.8f;
        private Dictionary<string, int> _nodeTriggerCounts = new Dictionary<string, int>();

        // Custom Proportional Splitter variables
        private VisualElement _leftPane;
        private VisualElement _rightPane;
        private VisualElement _divider;
        private bool _isDraggingDivider = false;
        private float _splitRatio = 0.5f; // Khởi tạo 50/50 để Preview rộng rãi hơn

        [MenuItem("Dajunctic/Skill Graph Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<SkillGraphEditorWindow>();
            window.titleContent = new GUIContent("Skill Graph Editor");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            _previewRenderUtility = new PreviewRenderUtility();
            _previewRenderUtility.camera.fieldOfView = 30f;
            _previewRenderUtility.camera.farClipPlane = 1000;
            _previewRenderUtility.camera.nearClipPlane = 0.1f;
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
            if (_previewRenderUtility != null) _previewRenderUtility.Cleanup();
            if (_previewInstance != null) DestroyImmediate(_previewInstance);
        }

        private void OnEditorUpdate()
        {
            if (_previewInstance != null)
            {
                var animator = _previewInstance.GetComponentInChildren<Animator>();
                if (animator != null && !Application.isPlaying) animator.Update(Time.deltaTime);
            }
            Repaint();
        }

        private void ConstructGraphView()
        {
            rootVisualElement.style.flexDirection = FlexDirection.Column;

            // Main Content Area
            var contentArea = new VisualElement();
            contentArea.style.flexDirection = FlexDirection.Row;
            contentArea.style.flexGrow = 1;
            rootVisualElement.Add(contentArea);

            // Left Pane (Graph) - Uses flexGrow based on ratio
            _leftPane = new VisualElement();
            _leftPane.style.flexGrow = _splitRatio;
            _leftPane.style.flexBasis = 0;

            _graphView = new SkillGraphView(this)
            {
                name = "Skill Graph"
            };
            _graphView.StretchToParentSize();
            _leftPane.Add(_graphView);
            contentArea.Add(_leftPane);

            // Custom Draggable Divider
            _divider = new VisualElement();
            _divider.style.width = 4;
            _divider.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 1f); // Làm sáng thanh chia một chút để dễ thấy
            // _divider.style.cursor = ... (Tạm thời bỏ qua để tránh lỗi biên dịch, quan trọng là phần toán học bên dưới đã được sửa)


            // Divider Interactivity
            _divider.RegisterCallback<MouseDownEvent>(evt => { _isDraggingDivider = true; _divider.CaptureMouse(); evt.StopPropagation(); });
            _divider.RegisterCallback<MouseUpEvent>(evt => { _isDraggingDivider = false; _divider.ReleaseMouse(); evt.StopPropagation(); });
            _divider.RegisterCallback<MouseMoveEvent>(OnDividerDrag);

            contentArea.Add(_divider);

            // Right Pane (Settings & Preview) - Uses flexGrow based on remaining ratio
            _rightPane = new VisualElement();
            _rightPane.style.flexGrow = 1f - _splitRatio;
            _rightPane.style.flexBasis = 0;
            _rightPane.style.backgroundColor = new Color(0.18f, 0.18f, 0.18f, 1f);
            contentArea.Add(_rightPane);

            var rightScroll = new ScrollView(ScrollViewMode.Vertical);
            rightScroll.style.flexGrow = 0; // Chỉ chiếm không gian vừa đủ cho settings
            rightScroll.style.paddingLeft = 10;
            rightScroll.style.paddingRight = 10;
            rightScroll.style.paddingTop = 10;
            _rightPane.Add(rightScroll);

            // Preview Section
            rightScroll.Add(new Label("Preview Settings") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10 } });

            var actorField = new ObjectField("Preview Actor") { objectType = typeof(CombatActor) };
            actorField.RegisterValueChangedCallback(evt =>
            {
                _previewActor = evt.newValue as CombatActor;
                UpdatePreviewInstance();
            });
            rightScroll.Add(actorField);

            // Preview sẽ nằm ngoài ScrollView để có thể scale theo chiều cao của RightPane
            var previewIMGUI = new IMGUIContainer(OnPreviewGUI);
            previewIMGUI.style.flexGrow = 1; // Tự động giãn nở theo chiều cao còn lại
            previewIMGUI.style.marginTop = 10;
            previewIMGUI.style.backgroundColor = new Color(0.12f, 0.12f, 0.12f, 1f);
            _rightPane.Add(previewIMGUI);
        }

        private void OnDividerDrag(MouseMoveEvent evt)
        {
            if (!_isDraggingDivider) return;

            VisualElement parent = _divider.parent;
            float totalWidth = parent.resolvedStyle.width;
            if (totalWidth <= 0) return;

            // Vị trí chuột (evt.mousePosition) trong UI Toolkit mặc định là toạ độ của Panel (Window)
            // Ta chỉ cần trừ đi toạ độ X của parent để có vị trí cục bộ trong vùng chứa
            float mousePositionX = evt.mousePosition.x - parent.worldBound.x;
            float newRatio = mousePositionX / totalWidth;

            // Clamp ratio to prevent panes from disappearing
            _splitRatio = Mathf.Clamp(newRatio, 0.1f, 0.9f);

            // Update UI
            _leftPane.style.flexGrow = _splitRatio;
            _rightPane.style.flexGrow = 1f - _splitRatio;

            evt.StopPropagation();
        }

        public void OnNodeSelectionChanged(SkillNode node)
        {
            if (node != null)
            {
                Selection.activeObject = node;
                EditorGUIUtility.PingObject(node);
            }
        }

        private void UpdatePreviewInstance()
        {
            if (_previewInstance != null) DestroyImmediate(_previewInstance);
            if (_previewActor != null)
            {
                _previewInstance = Instantiate(_previewActor.gameObject);
                _previewInstance.hideFlags = HideFlags.HideAndDontSave;
                _previewInstance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

                var monoScripts = _previewInstance.GetComponentsInChildren<MonoBehaviour>();
                foreach (var script in monoScripts)
                {
                    if (script is not CombatActor) script.enabled = false;
                }

                var animator = _previewInstance.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                }
                _previewRenderUtility.AddSingleGO(_previewInstance);
            }
        }

        private void OnPreviewGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(200, 200, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (_previewInstance == null)
            {
                GUI.Label(new Rect(rect.x + 10, rect.y + 10, 200, 20), "No Preview Actor Selected");
            }

            HandlePreviewInput(rect);
            _previewRenderUtility.BeginPreview(rect, GUIStyle.none);
            _previewRenderUtility.camera.transform.SetPositionAndRotation(_previewPivot + _previewRenderUtility.camera.transform.forward * -_previewDistance, Quaternion.Euler(-_previewDir.y, -_previewDir.x, 0));
            _previewRenderUtility.lights[0].intensity = 2.0f;
            _previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(40f, 40f, 0f);
            // _previewRenderUtility.lights[1].intensity = 1.5f;
            // _previewRenderUtility.lights[1].transform.rotation = Quaternion.Euler(30f, -30f, 0f);

            _previewRenderUtility.Render(true);
            DrawPreviewGrid();
            Texture result = _previewRenderUtility.EndPreview();
            GUI.DrawTexture(rect, result);
        }

        private void HandlePreviewInput(Rect rect)
        {
            int controlID = GUIUtility.GetControlID("PreviewInput".GetHashCode(), FocusType.Passive);
            Event evt = Event.current;
            switch (evt.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                    if (rect.Contains(evt.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        evt.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID) GUIUtility.hotControl = 0;
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        if (evt.button == 1)
                        {
                            _previewDir -= evt.delta * (evt.shift ? 3 : 1) / Mathf.Min(rect.width, rect.height) * 140f;
                            _previewDir.y = Mathf.Clamp(_previewDir.y, -90f, 90f);
                        }
                        else if (evt.button == 0 || evt.button == 2)
                        {
                            Vector3 camRight = _previewRenderUtility.camera.transform.right;
                            Vector3 camUp = _previewRenderUtility.camera.transform.up;
                            _previewPivot -= (camRight * evt.delta.x - camUp * evt.delta.y) * 0.01f * (_previewDistance / 6f);
                        }
                        evt.Use();
                        GUI.changed = true;
                    }
                    break;
                case EventType.ScrollWheel:
                    if (rect.Contains(evt.mousePosition))
                    {
                        _previewDistance += evt.delta.y * 0.05f;
                        _previewDistance = Mathf.Max(0.1f, _previewDistance);
                        evt.Use();
                        GUI.changed = true;
                    }
                    break;
            }
        }

        private void DrawPreviewGrid()
        {
            GL.PushMatrix();
            GL.LoadProjectionMatrix(_previewRenderUtility.camera.projectionMatrix);
            GL.modelview = _previewRenderUtility.camera.worldToCameraMatrix;
            var mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            mat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(new Color(0.7f, 0.7f, 0.7f, 0.5f));
            float gridSize = 10f;
            float step = 1f;
            for (float i = -gridSize; i <= gridSize; i += step)
            {
                GL.Vertex(new Vector3(i, 0, -gridSize));
                GL.Vertex(new Vector3(i, 0, gridSize));
                GL.Vertex(new Vector3(-gridSize, 0, i));
                GL.Vertex(new Vector3(gridSize, 0, i));
            }
            GL.End();
            GL.PopMatrix();
        }

        public SkillGraph CurrentGraph => _currentGraph;

        public void LoadGraph(SkillGraph graph)
        {
            _currentGraph = graph;
            if (_graphView != null) _graphView.PopulateView(_currentGraph);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            toolbar.Add(new Button(() => { SaveData(); }) { text = "Save Graph" });
            toolbar.Add(new Button(() => { LoadData(); }) { text = "Load Graph" });
            toolbar.Add(new Button(() => { PreviewSkill(); }) { text = "Preview Skill" });
            toolbar.Add(new Button(() => { UpdatePreviewInstance(); }) { text = "Reset Preview" });
            toolbar.Add(new Button(() => { _graphView.ClearGraph(); }) { text = "Clear Graph" });
            rootVisualElement.Insert(0, toolbar);
        }

        private void SaveData()
        {
            if (_currentGraph == null) return;
            _graphView.SaveGraph(_currentGraph);
            EditorUtility.SetDirty(_currentGraph);
            AssetDatabase.SaveAssets();
        }

        private void LoadData()
        {
            string path = EditorUtility.OpenFilePanel("Load Skill Graph", "Assets", "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = FileUtil.GetProjectRelativePath(path);
            var graph = AssetDatabase.LoadAssetAtPath<SkillGraph>(path);
            if (graph != null) LoadGraph(graph);
        }

        private void PreviewSkill()
        {
            if (_currentGraph == null || _previewInstance == null) return;
            _nodeTriggerCounts.Clear();
            var actor = _previewInstance.GetComponent<CombatActor>();
            if (actor == null) return;
            var context = new SkillExecutionContext(actor);
            var entryNode = _currentGraph.nodes.OfType<Nodes.EntryNode>().FirstOrDefault();
            if (entryNode != null) ExecuteNode(entryNode, context);
        }

        private void ExecuteNode(SkillNode node, SkillExecutionContext context)
        {
            node.Execute(context, () =>
            {
                var outgoingLinks = _currentGraph.links.Where(l => l.baseNodeGuid == node.guid).ToList();
                foreach (var link in outgoingLinks)
                {
                    var nextNode = _currentGraph.nodes.FirstOrDefault(n => n.guid == link.targetNodeGuid);
                    if (nextNode == null) continue;
                    if (!_nodeTriggerCounts.ContainsKey(nextNode.guid)) _nodeTriggerCounts[nextNode.guid] = 0;
                    _nodeTriggerCounts[nextNode.guid]++;
                    int totalIncoming = _currentGraph.links.Count(l => l.targetNodeGuid == nextNode.guid);
                    if (_nodeTriggerCounts[nextNode.guid] >= totalIncoming) ExecuteNode(nextNode, context);
                }
            });
        }
    }
}
