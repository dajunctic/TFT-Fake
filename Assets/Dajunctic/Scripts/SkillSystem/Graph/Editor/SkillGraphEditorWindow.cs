using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

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
                Repaint();
            }
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
            if (_previewInstance == null)
            {
                GUI.Label(new Rect(10, 10, 200, 20), "No Preview Actor Selected");
                return;
            }

            Rect rect = GUILayoutUtility.GetRect(200, 200, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            _previewDir = Drag2D(_previewDir, rect);

            _previewRenderUtility.BeginPreview(rect, GUIStyle.none);

            _previewRenderUtility.camera.transform.position = Vector3.zero;
            _previewRenderUtility.camera.transform.rotation = Quaternion.Euler(-_previewDir.y, -_previewDir.x, 0);
            _previewRenderUtility.camera.transform.position = _previewRenderUtility.camera.transform.forward * -6f + Vector3.up * 1.5f;

            _previewRenderUtility.lights[0].intensity = 1.4f;
            _previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(40f, 40f, 0f);
            _previewRenderUtility.lights[1].intensity = 1.4f;

            _previewRenderUtility.Render(true);

            Texture result = _previewRenderUtility.EndPreview();
            GUI.DrawTexture(rect, result);
        }

        private Vector2 Drag2D(Vector2 scrollPos, Rect rect)
        {
            int controlID = GUIUtility.GetControlID("Slider".GetHashCode(), FocusType.Passive);
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
                        scrollPos -= evt.delta * (evt.shift ? 3 : 1) / Mathf.Min(rect.width, rect.height) * 140f;
                        scrollPos.y = Mathf.Clamp(scrollPos.y, -90f, 90f);
                        evt.Use();
                        GUI.changed = true;
                    }
                    break;
            }
            return scrollPos;
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
                var link = _currentGraph.links.FirstOrDefault(l => l.baseNodeGuid == node.guid);
                if (link != null)
                {
                    var nextNode = _currentGraph.nodes.FirstOrDefault(n => n.guid == link.targetNodeGuid);
                    if (nextNode != null)
                    {
                        ExecuteNode(nextNode, context);
                    }
                }
            });
        }
    }
}
