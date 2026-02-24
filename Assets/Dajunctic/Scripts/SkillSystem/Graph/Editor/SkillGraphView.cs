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

            // Create Node Views
            foreach (var nodeData in graph.nodes)
            {
                CreateNodeView(nodeData);
            }

            // Create Links
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

        private SkillNodeView GetNodeByGuid(string guid)
        {
            return nodes.ToList().OfType<SkillNodeView>().FirstOrDefault(n => n.nodeData.guid == guid);
        }

        private void CreateNodeView(SkillNode nodeData)
        {
            var nodeView = new SkillNodeView(nodeData);
            AddElement(nodeView);
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

            Vector2 mousePos = evt.mousePosition;
            Vector2 graphMousePos = contentViewContainer.WorldToLocal(mousePos);

            evt.menu.AppendAction("Add Start Node", (a) => CreateNode<Nodes.StartNode>(graphMousePos));
            evt.menu.AppendAction("Add Animation Node", (a) => CreateNode<Nodes.AnimationNode>(graphMousePos));
            evt.menu.AppendAction("Add Wait Node", (a) => CreateNode<Nodes.WaitNode>(graphMousePos));
            evt.menu.AppendAction("Add VFX Node", (a) => CreateNode<Nodes.VFXNode>(graphMousePos));
            evt.menu.AppendAction("Add Damage Node", (a) => CreateNode<Nodes.DamageNode>(graphMousePos));
            evt.menu.AppendAction("Add Target Node", (a) => CreateNode<Nodes.TargetNode>(graphMousePos));
            evt.menu.AppendAction("Add Shoot Node", (a) => CreateNode<Nodes.ShootNode>(graphMousePos));
        }

        private void CreateNode<T>(Vector2 position) where T : SkillNode
        {
            var nodeData = ScriptableObject.CreateInstance<T>();
            nodeData.guid = System.Guid.NewGuid().ToString();
            nodeData.position = position;
            nodeData.name = typeof(T).Name;
            
            AssetDatabase.AddObjectToAsset(nodeData, _window.CurrentGraph);
            _window.CurrentGraph.nodes.Add(nodeData);
            
            EditorUtility.SetDirty(_window.CurrentGraph);
            AssetDatabase.SaveAssets();

            CreateNodeView(nodeData);
        }
    }
}
