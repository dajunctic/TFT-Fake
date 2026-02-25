using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    public class SkillNodeView : UnityEditor.Experimental.GraphView.Node
    {
        public SkillNode nodeData;
        public Port inputPort;
        public Port outputPort;

        public SkillNodeView(SkillNode nodeData)
        {
            this.nodeData = nodeData;
            title = nodeData.name.Replace("Node", "");
            viewDataKey = nodeData.guid;

            style.left = nodeData.position.x;
            style.top = nodeData.position.y;

            AddToClassList("skill-node");

            CreateInputPorts();
            CreateOutputPorts();
            CreateSettings();

            var titleLabel = this.Q<Label>();
            if (titleLabel != null)
            {
                titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                titleLabel.style.fontSize = 14;
            }
        }

        private void CreateInputPorts()
        {
            if (nodeData is Nodes.EntryNode) return;

            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "In";
            inputContainer.Add(inputPort);
        }

        private void CreateOutputPorts()
        {
            if (nodeData is Nodes.ExitNode) return;

            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.portName = "Out";
            outputContainer.Add(outputPort);
        }

        private void CreateSettings()
        {
            // Use Inspector-like fields for node settings
            var serializedObject = new UnityEditor.SerializedObject(nodeData);
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // Skip m_Script

            while (iterator.NextVisible(false))
            {
                var field = new PropertyField(iterator);
                field.Bind(serializedObject);
                extensionContainer.Add(field);
            }
            RefreshExpandedState();
        }
    }
}
