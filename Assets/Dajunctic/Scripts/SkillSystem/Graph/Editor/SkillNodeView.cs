using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Dajunctic.SkillSystem.Graph;
using System.Text.RegularExpressions;

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
                titleLabel.style.fontSize = 12;
            }
        }

        private void CreateInputPorts()
        {
            if (nodeData is Nodes.EntryNode) return;

            // Execution In
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "In";
            inputPort.AddToClassList("execution-port");
            inputContainer.Add(inputPort);

            // Data Inputs
            var fields = nodeData.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (System.Attribute.IsDefined(field, typeof(Dajunctic.SkillSystem.Graph.NodeInputAttribute)))
                {
                    var port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, field.FieldType);
                    port.portName = field.Name;
                    SetPortLabel(port, FormatPortName(field.Name));
                    inputContainer.Add(port);
                }
            }
        }

        private void CreateOutputPorts()
        {
            if (nodeData is Nodes.ExitNode) return;

            // Execution Out
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.portName = "Out";
            outputPort.AddToClassList("execution-port");
            outputContainer.Add(outputPort);

            // Data Outputs
            var fields = nodeData.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (System.Attribute.IsDefined(field, typeof(Dajunctic.SkillSystem.Graph.NodeOutputAttribute)))
                {
                    var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, field.FieldType);
                    port.portName = field.Name;
                    SetPortLabel(port, FormatPortName(field.Name));
                    outputContainer.Add(port);
                }
            }
        }

        /// <summary>
        /// Override the visible label text on a port without changing portName (which is used for save/load).
        /// </summary>
        private void SetPortLabel(Port port, string displayName)
        {
            port.schedule.Execute(() =>
            {
                var label = port.Q<Label>();
                if (label != null) label.text = displayName;
            });
        }

        private void CreateSettings()
        {
            var serializedObject = new UnityEditor.SerializedObject(nodeData);
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // Skip m_Script

            while (iterator.NextVisible(false))
            {
                // Tránh hiển thị các field đã được dùng làm Port
                var fieldInfo = nodeData.GetType().GetField(iterator.name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (fieldInfo != null && (System.Attribute.IsDefined(fieldInfo, typeof(Dajunctic.SkillSystem.Graph.NodeInputAttribute)) || System.Attribute.IsDefined(fieldInfo, typeof(Dajunctic.SkillSystem.Graph.NodeOutputAttribute))))
                    continue;

                var field = new PropertyField(iterator);
                field.Bind(serializedObject);
                extensionContainer.Add(field);
            }
            RefreshExpandedState();
        }

        /// <summary>
        /// Converts camelCase field names to Title Case display names.
        /// e.g., "targets" -> "Targets", "targetType" -> "Target Type"
        /// </summary>
        private string FormatPortName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return fieldName;

            // Insert space before each uppercase letter (for camelCase splitting)
            string spaced = Regex.Replace(fieldName, "(?<!^)([A-Z])", " $1");

            // Capitalize first letter of each word
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(spaced);
        }

        private static readonly Color HighlightColor = new Color(0.2f, 0.8f, 0.2f, 0.6f);

        public void SetHighlight()
        {
            style.borderBottomColor = HighlightColor;
            style.borderTopColor = HighlightColor;
            style.borderLeftColor = HighlightColor;
            style.borderRightColor = HighlightColor;
            style.borderBottomWidth = 3;
            style.borderTopWidth = 3;
            style.borderLeftWidth = 3;
            style.borderRightWidth = 3;
        }

        public void ClearHighlight()
        {
            style.borderBottomColor = StyleKeyword.Null;
            style.borderTopColor = StyleKeyword.Null;
            style.borderLeftColor = StyleKeyword.Null;
            style.borderRightColor = StyleKeyword.Null;
            style.borderBottomWidth = StyleKeyword.Null;
            style.borderTopWidth = StyleKeyword.Null;
            style.borderLeftWidth = StyleKeyword.Null;
            style.borderRightWidth = StyleKeyword.Null;
        }
    }
}
