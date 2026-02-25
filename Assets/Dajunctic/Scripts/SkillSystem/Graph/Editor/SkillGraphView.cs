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
                Vector2 screenPos = GUIUtility.GUIToScreenPoint(_lastMousePosition);
                OpenSearchWindow(_lastMousePosition, screenPos, false);
            }

            if (evt.ctrlKey && evt.keyCode == KeyCode.S)
            {
                _window.SaveData();
                evt.StopPropagation();
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
                nodeData.graph = graph;
                CreateNodeView(nodeData);
            }

            foreach (var link in graph.links)
            {
                SkillNodeView baseNodeView = GetNodeByGuid(link.baseNodeGuid);
                SkillNodeView targetNodeView = GetNodeByGuid(link.targetNodeGuid);

                if (baseNodeView != null && targetNodeView != null)
                {
                    var outputPort = baseNodeView.outputContainer.Query<Port>().ToList().FirstOrDefault(p => p.portName == link.portName);
                    var inputPort = targetNodeView.inputContainer.Query<Port>().ToList().FirstOrDefault(p => p.portName == link.targetPortName || (string.IsNullOrEmpty(link.targetPortName) && p.portName == "In"));

                    if (outputPort != null && inputPort != null)
                    {
                        var edge = outputPort.ConnectTo(inputPort);
                        AddElement(edge);
                    }
                }
            }
        }

        public SkillNodeView GetNodeViewByGuid(string guid)
        {
            return nodes.ToList().OfType<SkillNodeView>().FirstOrDefault(n => n.nodeData.guid == guid);
        }

        private new SkillNodeView GetNodeByGuid(string guid)
        {
            return GetNodeViewByGuid(guid);
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
            if (_window.CurrentGraph == null) return graphViewChange;

            if (graphViewChange.elementsToRemove != null ||
                graphViewChange.movedElements != null ||
                graphViewChange.edgesToCreate != null)
            {
                Undo.RecordObject(_window.CurrentGraph, "Change Graph");
                _window.SetDirty(true);

                if (graphViewChange.elementsToRemove != null)
                {
                    foreach (var element in graphViewChange.elementsToRemove)
                    {
                        if (element is SkillNodeView nodeView)
                        {
                            _window.CurrentGraph.nodes.Remove(nodeView.nodeData);

                            // Xoá các link liên quan đến node này
                            _window.CurrentGraph.links.RemoveAll(l =>
                                l.baseNodeGuid == nodeView.nodeData.guid ||
                                l.targetNodeGuid == nodeView.nodeData.guid);

                            AssetDatabase.RemoveObjectFromAsset(nodeView.nodeData);
                            Undo.DestroyObjectImmediate(nodeView.nodeData);
                        }
                        else if (element is Edge edge)
                        {
                            // Xoá link cụ thể khi edge bị xoá (Disconnect hoặc Delete)
                            if (edge.output?.node is SkillNodeView source && edge.input?.node is SkillNodeView target)
                            {
                                _window.CurrentGraph.links.RemoveAll(l =>
                                    l.baseNodeGuid == source.nodeData.guid &&
                                    l.portName == edge.output.portName &&
                                    l.targetNodeGuid == target.nodeData.guid &&
                                    l.targetPortName == edge.input.portName);
                            }
                        }
                    }
                }

                if (graphViewChange.edgesToCreate != null)
                {
                    foreach (var edge in graphViewChange.edgesToCreate)
                    {
                        if (edge.output?.node is SkillNodeView source && edge.input?.node is SkillNodeView target)
                        {
                            _window.CurrentGraph.links.Add(new NodeLink
                            {
                                baseNodeGuid = source.nodeData.guid,
                                portName = edge.output.portName,
                                targetNodeGuid = target.nodeData.guid,
                                targetPortName = edge.input.portName
                            });
                        }
                    }
                }
            }
            return graphViewChange;
        }

        public void ClearGraph()
        {
            if (_window.CurrentGraph == null) return;
            Undo.RecordObject(_window.CurrentGraph, "Clear Graph");
            foreach (var node in _window.CurrentGraph.nodes.ToList())
            {
                AssetDatabase.RemoveObjectFromAsset(node);
                Undo.DestroyObjectImmediate(node);
            }
            _window.CurrentGraph.nodes.Clear();
            _window.CurrentGraph.links.Clear();

            // Tạo các node mặc định
            CreateNode(typeof(Nodes.EntryNode), new Vector2(100, 200));
            CreateNode(typeof(Nodes.ExitNode), new Vector2(500, 200));

            PopulateView(_window.CurrentGraph);
            AssetDatabase.SaveAssets();
            _window.SetDirty(true);
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
                        targetNodeGuid = targetNodeView.nodeData.guid,
                        targetPortName = edge.input.portName
                    });
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (_window.CurrentGraph == null) return;

            // Đưa Create Node lên đầu tiên
            evt.menu.InsertAction(0, "Create Node", (action) =>
            {
                OpenSearchWindow(action.eventInfo.localMousePosition, action.eventInfo.mousePosition, true);
            });

            evt.menu.AppendSeparator(); // Thêm gạch ngang phân cách

            base.BuildContextualMenu(evt); // Giữ lại các lệnh mặc định phía dưới
        }

        private void OpenSearchWindow(Vector2 localMousePosition, Vector2 panelMousePosition, bool isLocal)
        {
            Vector2 graphPos = isLocal ? localMousePosition : contentViewContainer.WorldToLocal(localMousePosition);
            _searchWindow.GraphMousePosition = graphPos;

            // panelMousePosition là toạ độ relative to editor window panel
            // Cần cộng thêm vị trí của cửa sổ trên màn hình
            Vector2 screenPos = _window.position.position + panelMousePosition;
            // Bù trừ một chút cho thanh tiêu đề (title bar) của window
            screenPos.y += 24;

            SearchWindow.Open(new SearchWindowContext(screenPos), _searchWindow);
        }

        public void CreateNode(System.Type type, Vector2 position)
        {
            Undo.RecordObject(_window.CurrentGraph, "Create Node");
            var nodeData = ScriptableObject.CreateInstance(type) as SkillNode;
            nodeData.guid = System.Guid.NewGuid().ToString();
            nodeData.position = position;
            nodeData.name = type.Name.Replace("Node", ""); // "TrackingNode" -> "Tracking"
            nodeData.graph = _window.CurrentGraph;
            AssetDatabase.AddObjectToAsset(nodeData, _window.CurrentGraph);
            _window.CurrentGraph.nodes.Add(nodeData);

            _window.SetDirty(true);
            CreateNodeView(nodeData);
        }
    }
}
