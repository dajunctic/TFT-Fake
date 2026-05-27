using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using GraphProcessor;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic.SkillSystem.Editor
{
    public class SkillGraphWindow : BaseGraphWindow
    {
        [MenuItem("Window/NodeGraphProcessor/Skill Graph")]
        public static BaseGraphWindow OpenWithTmpGraph()
        {
            var graphWindow = CreateWindow<SkillGraphWindow>();

            graphWindow.Show();

            return graphWindow;
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            titleContent = new GUIContent("Skill Graph");

            if (graphView == null)
            {
                graphView = new BaseGraphView(this);
                graphView.Add(new ToolbarView(graphView));
            }
            rootView.Add(graphView);
        }

        [OnOpenAsset(0)]
        public static bool OnBaseGraphOpened(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);

            if (asset is SkillGraph graph)
            {
                var window = GetWindow<SkillGraphWindow>();
                window.InitializeGraph(graph);
                window.Show();
                return true;
            }

            return false;
        }
    }
}
