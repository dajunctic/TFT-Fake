using UnityEditor;
using UnityEngine;
using XNodeEditor;
using Dajunctic.SkillSystem.Graph;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    [CustomNodeEditor(typeof(SkillNode))]
    public class SkillNodeEditor : NodeEditor
    {
        public override void OnHeaderGUI()
        {
            string title = target.name;
            if (string.IsNullOrEmpty(title)) title = target.GetType().Name;
            
            // Draw a slightly taller header with better alignment
            GUILayout.BeginVertical(GUILayout.Height(40));
            GUILayout.FlexibleSpace();
            GUILayout.Label(title, NodeEditorResources.styles.nodeHeader);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        public override Color GetTint()
        {
            // The [NodeTint] attribute is already handled by the base class, 
            // but we can add logic here if we wanted dynamic tints.
            return base.GetTint();
        }
    }
}
