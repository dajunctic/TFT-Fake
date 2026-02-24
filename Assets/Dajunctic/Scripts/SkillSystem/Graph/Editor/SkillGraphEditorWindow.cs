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
        private VisualElement _previewContainer;
        private CombatActor _previewActor;
        private PreviewRenderUtility _previewRenderUtility;
        private GameObject _previewInstance;
        private Vector2 _previewDir = new Vector2(120, -20);
        private float _previewDistance = 6f;
        private Vector3 _previewPivot = Vector3.up * 0.8f;
        private Dictionary<string, int> _nodeTriggerCounts = new Dictionary<string, int>();

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
            rootVisualElement.Remove(_graphView);
            if (_previewRenderUtility != null)
            {
                _previewRenderUtility.Cleanup();
                _previewRenderUtility = null;
            }
            if (_previewInstance != null)
            {
                DestroyImmediate(_previewInstance);
            }
        }

        private void OnEditorUpdate()
        {
            if (_previewInstance != null)
            {
                // Manually update animator in editor
                var animator = _previewInstance.GetComponentInChildren<Animator>();
                if (animator != null && !Application.isPlaying)
                {
                    animator.Update(Time.deltaTime);
                }
            }
            Repaint();
        }

        private void ConstructGraphView()
        {
            _graphView = new SkillGraphView(this)
            {
                name = "Skill Graph"
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);

            // Split View
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(splitView);

            var leftPane = new VisualElement();
            leftPane.Add(_graphView);
            splitView.Add(leftPane);

            var rightPane = new VisualElement();
            rightPane.style.paddingLeft = 10;
            rightPane.style.paddingRight = 10;
            rightPane.style.paddingTop = 10;

            _previewContainer = new VisualElement();
            rightPane.Add(new Label("Preview Settings") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginBottom = 10 } });
            
            var actorField = new ObjectField("Preview Actor") { objectType = typeof(CombatActor) };
            actorField.RegisterValueChangedCallback(evt => 
            {
                _previewActor = evt.newValue as CombatActor;
                UpdatePreviewInstance();
            });
            rightPane.Add(actorField);

            var previewIMGUI = new IMGUIContainer(OnPreviewGUI);
            previewIMGUI.style.flexGrow = 1;
            previewIMGUI.style.minHeight = 200;
            rightPane.Add(previewIMGUI);

            rightPane.Add(_previewContainer);
            splitView.Add(rightPane);
            
            _graphView.StretchToParentSize();
        }

        private void UpdatePreviewInstance()
        {
            if (_previewInstance != null)
            {
                DestroyImmediate(_previewInstance);
            }

            if (_previewActor != null)
            {
                _previewInstance = Instantiate(_previewActor.gameObject);
                _previewInstance.hideFlags = HideFlags.HideAndDontSave;
                _previewInstance.transform.position = Vector3.zero;
                _previewInstance.transform.rotation = Quaternion.identity;
                
                // Disable scripts that might interfere with preview, but keep Animator and CombatActor
                var scripts = _previewInstance.GetComponentsInChildren<MonoBehaviour>();
                foreach (var script in scripts)
                {
                    if (!(script is Animator) && !(script is CombatActor)) script.enabled = false;
                }

                // Ensure animator is in a state where it can play
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

            _previewRenderUtility.camera.transform.rotation = Quaternion.Euler(-_previewDir.y, -_previewDir.x, 0);
            _previewRenderUtility.camera.transform.position = _previewPivot + _previewRenderUtility.camera.transform.forward * -_previewDistance;

            _previewRenderUtility.lights[0].intensity = 1.4f;
            _previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(40f, 40f, 0f);
            _previewRenderUtility.lights[1].intensity = 1.4f;

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
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                    }
                    EditorGUIUtility.SetWantsMouseJumping(0);
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        if (evt.button == 1) // Right click - Rotate
                        {
                            _previewDir -= evt.delta * (evt.shift ? 3 : 1) / Mathf.Min(rect.width, rect.height) * 140f;
                            _previewDir.y = Mathf.Clamp(_previewDir.y, -90f, 90f);
                        }
                        else if (evt.button == 0 || evt.button == 2) // Left click or Middle click - Pan
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
            
            // Draw Grid
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

        public SerializedObject GetSerializedObject()
        {
            return new SerializedObject(_currentGraph);
        }

        public void LoadGraph(SkillGraph graph)
        {
            _currentGraph = graph;
            _graphView.PopulateView(_currentGraph);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var saveBtn = new Button(() => { SaveData(); }) { text = "Save Graph" };
            toolbar.Add(saveBtn);

            var loadBtn = new Button(() => { LoadData(); }) { text = "Load Graph" };
            toolbar.Add(loadBtn);

            var previewBtn = new Button(() => { PreviewSkill(); }) { text = "Preview Skill" };
            toolbar.Add(previewBtn);

            var resetBtn = new Button(() => { UpdatePreviewInstance(); }) { text = "Reset Preview" };
            toolbar.Add(resetBtn);

            var clearBtn = new Button(() => { _graphView.ClearGraph(); }) { text = "Clear Graph" };
            toolbar.Add(clearBtn);

            rootVisualElement.Add(toolbar);
        }

        private void SaveData()
        {
            if (_currentGraph == null) return;
            // Implementation for saving graph data back to ScriptableObject
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
            if (graph != null)
            {
                LoadGraph(graph);
            }
        }

        private void PreviewSkill()
        {
            if (_currentGraph == null || _previewInstance == null)
            {
                Debug.LogWarning("Please select a Skill Graph and a Preview Actor.");
                return;
            }

            Debug.Log("Previewing Skill...");
            _nodeTriggerCounts.Clear();
            
            var actor = _previewInstance.GetComponent<CombatActor>();
            if (actor == null) return;

            var context = new SkillExecutionContext(actor);
            context.onSpawnVFX = (vfx) =>
            {
                _previewRenderUtility.AddSingleGO(vfx);
            };

            var startNode = _currentGraph.nodes.OfType<Nodes.StartNode>().FirstOrDefault();
            if (startNode != null)
            {
                ExecuteNode(startNode, context);
            }
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

                    if (!_nodeTriggerCounts.ContainsKey(nextNode.guid))
                        _nodeTriggerCounts[nextNode.guid] = 0;
                    
                    _nodeTriggerCounts[nextNode.guid]++;

                    int totalIncoming = _currentGraph.links.Count(l => l.targetNodeGuid == nextNode.guid);

                    if (_nodeTriggerCounts[nextNode.guid] >= totalIncoming)
                    {
                        ExecuteNode(nextNode, context);
                    }
                }
            });
        }
    }
}
