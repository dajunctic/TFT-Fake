using System;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using Dajunctic.SkillSystem.Graph;
using Dajunctic.SkillSystem.Graph.ActionNodes;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    [CustomNodeGraphEditor(typeof(SkillGraph))]
    public class SkillGraphEditor : NodeGraphEditor
    {
        public override float GetNoodleThickness(XNode.NodePort output, XNode.NodePort input)
        {
            return 4f;
        }

        public override Color GetTypeColor(Type type)
        {
            if (type == typeof(ActionNode) || type.IsSubclassOf(typeof(ActionNode)))
                return new Color(1f, 0.4f, 0.8f); // Pink/Purple for Flow
            
            if (type == typeof(bool))
                return new Color(0.4f, 1f, 0.4f); // Green for Triggers
            
            if (typeof(IDamageTaker).IsAssignableFrom(type) || (type.IsGenericType && type.GetGenericArguments()[0] == typeof(IDamageTaker)))
                return new Color(1f, 0.8f, 0.2f); // Orange/Yellow for Targets
            
            if (type == typeof(Vector3))
                return new Color(0.4f, 0.6f, 1f); // Light Blue for Positions
            
            return base.GetTypeColor(type);
        }

        public override NoodlePath GetNoodlePath(XNode.NodePort output, XNode.NodePort input)
        {
            return NoodlePath.Curvy;
        }
    }
}
