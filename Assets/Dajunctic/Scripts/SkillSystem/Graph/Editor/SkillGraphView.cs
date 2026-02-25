using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    public class SkillGraphView : GraphView
    {
        private readonly SkillGraphEditorWindow _window;
        private SkillSearchWindow _searchWindow;
        private Vector2 _lastMousePosition;

        public SkillGraphView(SkillGraphEditorWindow window)
        {
            _window = window;

            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Dajunctic/Scripts/SkillSystem/Graph/Editor/SkillGraph.uss"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddSearchWindow();

            RegisterCallback<KeyDownEvent>(OnKeyDown);
            RegisterCallback<MouseMoveEvent>(OnMouseMove);
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            _lastMousePosition = evt.mousePosition;
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Space && !evt.ctrlKey && !evt.commandKey)
            {
                OpenSearchWindow(evt.originalMousePosition, false);
            }
        }

        private void AddSearchWindow()
        {
            _searchWindow = ScriptableObject.CreateInstance<SkillSearchWindow>();
            _searchWindow.Init(this, _window);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node).ToList();
        }

        public void PopulateView(SkillGraph graph)
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            foreach (var nodeData in graph.nodes)
            {
                CreateNodeView(nodeData);
            }

            foreach (var link in graph.links)
            {
                SkillNodeView baseNodeView = GetNodeByGuid(link.baseNodeGuid);
                SkillNodeView targetNodeView = GetNodeByGuid(link.targetNodeGuid);

                if (baseNodeView != null && targetNodeView != null)
                {
                    var outputPort = baseNodeView.outputContainer.Query<Port>().ToList().FirstOrDefault(p => p.portName == link.portName);
                    var inputPort = targetNodeView.inputContainer.Query<Port>().ToList().FirstOrDefault();

                    if (outputPort != null && inputPort != null)
                    {
                        var edge = outputPort.ConnectTo(inputPort);
                        AddElement(edge);
                    }
                }
            }
        }

        private new SkillNodeView GetNodeByGuid(string guid)
        {
            return nodes.ToList().OfType<SkillNodeView>().FirstOrDefault(n => n.nodeData.guid == guid);
        }

        private void CreateNodeView(SkillNode nodeData)
        {
            var nodeView = new SkillNodeView(nodeData);
            AddElement(nodeView);
        }

        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            if (selectable is SkillNodeView nodeView)
            {
                _window.OnNodeSelectionChanged(nodeView.nodeData);
            }
        }

        public override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            if (selection.Count == 0)
            {
                _window.OnNodeSelectionChanged(null);
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var element in graphViewChange.elementsToRemove)
                {
                    if (element is SkillNodeView nodeView)
                    {
                        _window.CurrentGraph.nodes.Remove(nodeView.nodeData);
                        AssetDatabase.RemoveObjectFromAsset(nodeView.nodeData);
                        Undo.DestroyObjectImmediate(nodeView.nodeData);
                    }
                }
                AssetDatabase.SaveAssets();
            }
            return graphViewChange;
        }

        public void ClearGraph()
        {
            if (_window.CurrentGraph == null) return;
            foreach (var node in _window.CurrentGraph.nodes.ToList())
            {
                AssetDatabase.RemoveObjectFromAsset(node);
                Undo.DestroyObjectImmediate(node);
            }
            _window.CurrentGraph.nodes.Clear();
            _window.CurrentGraph.links.Clear();
            PopulateView(_window.CurrentGraph);
            AssetDatabase.SaveAssets();
        }

        public void SaveGraph(SkillGraph graph)
        {
            graph.nodes.Clear();
            graph.links.Clear();
            var nodeViews = nodes.ToList().Cast<SkillNodeView>();
            foreach (var nodeView in nodeViews)
            {
                nodeView.nodeData.position = nodeView.GetPosition().position;
                graph.nodes.Add(nodeView.nodeData);
            }
            var edges = this.edges.ToList();
            foreach (var edge in edges)
            {
                if (edge.output.node is SkillNodeView baseNodeView && edge.input.node is SkillNodeView targetNodeView)
                {
                    graph.links.Add(new NodeLink
                    {
                        baseNodeGuid = baseNodeView.nodeData.guid,
                        portName = edge.output.portName,
                        targetNodeGuid = targetNodeView.nodeData.guid
                    });
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_window.CurrentGraph == null) return;
            OpenSearchWindow(evt.mousePosition, true);
        }

        private void OpenSearchWindow(Vector2 mousePosition, bool isLocal)
        {
            Vector2 panelPos = isLocal ? this.LocalToWorld(mousePosition) : mousePosition;
            _searchWindow.GraphMousePosition = contentViewContainer.WorldToLocal(panelPos);
            Vector2 screenPos = _window.position.position + panelPos;
            screenPos.y += 22;
            SearchWindow.Open(new SearchWindowContext(screenPos), _searchWindow);
        }

        public void CreateNode(System.Type type, Vector2 position)
        {
            var nodeData = ScriptableObject.CreateInstance(type) as SkillNode;
            nodeData.guid = System.Guid.NewGuid().ToString();
            nodeData.position = position;
            nodeData.name = type.Name;
            AssetDatabase.AddObjectToAsset(nodeData, _window.CurrentGraph);
            _window.CurrentGraph.nodes.Add(nodeData);
            EditorUtility.SetDirty(_window.CurrentGraph);
            AssetDatabase.SaveAssets();
            CreateNodeView(nodeData);
        }
    }
}
