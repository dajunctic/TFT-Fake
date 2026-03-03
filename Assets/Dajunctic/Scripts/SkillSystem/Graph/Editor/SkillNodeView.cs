using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Dajunctic.SkillSystem.Graph;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    public class SkillNodeView : UnityEditor.Experimental.GraphView.Node
    {
        public SkillNode nodeData;
        public Port inputPort;
        public Port outputPort;

        private static readonly Color ActionPortColor = new Color(1.0f, 0.3f, 0.6f); // Magenta/Pink for Action Flow

        public SkillNodeView(SkillNode nodeData)
        {
            this.nodeData = nodeData;
            title = nodeData.name.Replace("Node", "");
            viewDataKey = nodeData.guid;

            style.left = nodeData.gridPosition.x;
            style.top = nodeData.gridPosition.y;

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
            // Execution In
            if (!(nodeData is Nodes.EntryNode || nodeData is ActionNode))
            {
                inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
                inputPort.portName = "In";
                inputPort.AddToClassList("execution-port");
                inputContainer.Add(inputPort);
            }

            // Data Inputs
            var fields = nodeData.GetType().GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (System.Attribute.IsDefined(field, typeof(NodeInputAttribute)) ||
                    System.Attribute.IsDefined(field, typeof(ActionInput)))
                {
                    var capacity = field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)
                        ? Port.Capacity.Multi
                        : Port.Capacity.Single;

                    var port = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, field.FieldType);
                    port.portName = field.Name;

                    if (System.Attribute.IsDefined(field, typeof(ActionInput)))
                    {
                        port.portColor = ActionPortColor;
                    }

                    SetPortLabel(port, FormatPortName(field.Name));
                    inputContainer.Add(port);
                }
            }
        }

        private void CreateOutputPorts()
        {
            // Execution Out
            if (!(nodeData is Nodes.ExitNode || nodeData is ActionNode))
            {
                outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
                outputPort.portName = "Out";
                outputPort.AddToClassList("execution-port");
                outputContainer.Add(outputPort);
            }

            // Data Outputs
            var fields = nodeData.GetType().GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (System.Attribute.IsDefined(field, typeof(NodeOutputAttribute)))
                {
                    var port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, field.FieldType);
                    port.portName = field.Name;

                    if (field.FieldType == typeof(ActionNode) || field.Name == "self")
                    {
                        port.portColor = ActionPortColor;
                    }

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
            var label = port.Q<Label>();
            if (label != null) label.text = displayName;
        }

        private void CreateSettings()
        {
            var serializedObject = new UnityEditor.SerializedObject(nodeData);
            var iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // Skip m_Script

            while (iterator.NextVisible(false))
            {
                // Tránh hiển thị các field đã được dùng làm Port
                var fieldInfo = nodeData.GetType().GetField(iterator.name,
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

                if (fieldInfo != null && (
                    System.Attribute.IsDefined(fieldInfo, typeof(NodeInputAttribute)) ||
                    System.Attribute.IsDefined(fieldInfo, typeof(NodeOutputAttribute)) ||
                    System.Attribute.IsDefined(fieldInfo, typeof(ActionInput))))
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
