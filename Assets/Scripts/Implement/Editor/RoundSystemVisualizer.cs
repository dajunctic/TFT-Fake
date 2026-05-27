using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic.Editor
{
    public class RoundSystemVisualizer : EditorWindow
    {
        private RoundSystemData _roundSystemData;
        private Vector2 _scrollPos;
        private StageData _selectedStage;

        [MenuItem("Dajunctic/Round System Visualizer")]
        public static void ShowWindow()
        {
            GetWindow<RoundSystemVisualizer>("Round Visualizer");
        }

        private void OnEnable()
        {
            FindData();
        }

        private void FindData()
        {
            string[] guids = AssetDatabase.FindAssets("t:RoundSystemData");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _roundSystemData = AssetDatabase.LoadAssetAtPath<RoundSystemData>(path);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            DrawStageList();

            DrawRoundTimeline();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawStageList()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(200), GUILayout.ExpandHeight(true));
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stages", EditorStyles.boldLabel);
            if (GUILayout.Button("+", GUILayout.Width(25))) AddStage();
            EditorGUILayout.EndHorizontal();

            if (_roundSystemData == null)
            {
                if (GUILayout.Button("Find RoundSystemData")) FindData();
                EditorGUILayout.EndVertical();
                return;
            }

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            for (int i = 0; i < _roundSystemData.stages.Count; i++)
            {
                var stage = _roundSystemData.stages[i];
                if (stage == null) continue;

                EditorGUILayout.BeginHorizontal();
                GUI.color = (_selectedStage == stage) ? Color.cyan : Color.white;
                if (GUILayout.Button($"Stage {stage.stageNumber}", GUILayout.Height(30)))
                {
                    _selectedStage = stage;
                }
                GUI.color = Color.white;

                if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(30)))
                {
                    if (EditorUtility.DisplayDialog("Delete Stage", $"Are you sure you want to remove Stage {stage.stageNumber}?", "Yes", "No"))
                    {
                        _roundSystemData.stages.RemoveAt(i);
                        EditorUtility.SetDirty(_roundSystemData);
                        if (_selectedStage == stage) _selectedStage = null;
                        break;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Refresh Data")) FindData();
            EditorGUILayout.EndVertical();
        }

        private void DrawRoundTimeline()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
            if (_selectedStage == null)
            {
                EditorGUILayout.LabelField("Select a stage to view rounds", EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandHeight(true));
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Stage {_selectedStage.stageNumber} Progression", EditorStyles.boldLabel);
            if (GUILayout.Button("+ Add Round", GUILayout.Width(100))) AddRound();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10);

            float containerWidth = position.width - 220; 
            int cardsPerRow = Mathf.Max(1, Mathf.FloorToInt(containerWidth / 130)); 
            
            _roundScrollPos = EditorGUILayout.BeginScrollView(_roundScrollPos);
            
            for (int i = 0; i < _selectedStage.rounds.Count; i += cardsPerRow)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < cardsPerRow; j++)
                {
                    int index = i + j;
                    if (index >= _selectedStage.rounds.Count) break;

                    var round = _selectedStage.rounds[index];
                    if (round == null)
                    {
                        EditorGUILayout.LabelField("NULL", GUILayout.Width(120), GUILayout.Height(150));
                        continue;
                    }
                    DrawRoundCard(round, index);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(5);
            }
            
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Select Asset", GUILayout.Width(100)))
            {
                Selection.activeObject = _selectedStage;
            }

            EditorGUILayout.EndVertical();
        }

        private Vector2 _roundScrollPos;

        private void DrawRoundCard(RoundData round, int index)
        {
            EditorGUILayout.BeginVertical(GUI.skin.window, GUILayout.Width(120), GUILayout.Height(180));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(round.displayName, EditorStyles.miniBoldLabel);
            if (GUILayout.Button("x", GUILayout.Width(18)))
            {
                if (EditorUtility.DisplayDialog("Remove Round", $"Remove {round.displayName}?", "Yes", "No"))
                {
                    _selectedStage.rounds.RemoveAt(index);
                    EditorUtility.SetDirty(_selectedStage);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    return;
                }
            }
            EditorGUILayout.EndHorizontal();

            Rect rect = GUILayoutUtility.GetRect(100, 50);
            if (round.icon != null)
            {
                GUI.DrawTexture(rect, round.icon.texture, ScaleMode.ScaleToFit);
            }
            else
            {
                EditorGUI.DrawRect(rect, GetColorForType(round.roundType));
                GUI.Label(rect, round.roundType.ToString(), EditorStyles.centeredGreyMiniLabel);
            }

            EditorGUILayout.LabelField($"Type: {round.roundType}", EditorStyles.miniLabel);
            
            EditorGUI.BeginChangeCheck();
            bool hasAug = EditorGUILayout.Toggle("Augment", round.hasAugment);
            float p = EditorGUILayout.FloatField("Plan", round.planningDuration);
            float c = EditorGUILayout.FloatField("Combat", round.combatDuration);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(round, "Edit Round Data");
                round.hasAugment = hasAug;
                round.planningDuration = p;
                round.combatDuration = c;
                EditorUtility.SetDirty(round);
            }

            if (round.hasAugment)
            {
                GUI.color = Color.yellow;
                EditorGUILayout.LabelField("★ AUGMENT", EditorStyles.miniButton);
                GUI.color = Color.white;
            }

            if (GUILayout.Button("View", GUILayout.Height(18)))
            {
                Selection.activeObject = round;
            }

            EditorGUILayout.EndVertical();
        }

        private void AddStage()
        {
            if (_roundSystemData == null) return;

            string path = AssetDatabase.GetAssetPath(_roundSystemData);
            string folder = System.IO.Path.GetDirectoryName(path);
            
            int nextNum = _roundSystemData.stages.Count + 1;
            StageData stage = CreateInstance<StageData>();
            stage.stageNumber = nextNum;
            
            string assetPath = $"{folder}/Stage_{nextNum}.asset";
            AssetDatabase.CreateAsset(stage, assetPath);
            AssetDatabase.SaveAssets();

            _roundSystemData.stages.Add(stage);
            EditorUtility.SetDirty(_roundSystemData);
            _selectedStage = stage;
        }

        private void AddRound()
        {
            if (_selectedStage == null) return;

            string path = AssetDatabase.GetAssetPath(_selectedStage);
            string folder = System.IO.Path.GetDirectoryName(path);
            if (folder.EndsWith("Stages")) folder = folder.Substring(0, folder.Length - 7); 

            string roundsFolder = $"{folder}/Rounds";
            if (!System.IO.Directory.Exists(roundsFolder)) System.IO.Directory.CreateDirectory(roundsFolder);

            int nextNum = _selectedStage.rounds.Count + 1;
            RoundData round = CreateInstance<RoundData>();
            round.roundType = RoundType.PvP;
            round.displayName = $"{_selectedStage.stageNumber}-{nextNum}";
            
            string assetPath = $"{roundsFolder}/Round_{_selectedStage.stageNumber}_{nextNum}.asset";
            AssetDatabase.CreateAsset(round, assetPath);
            AssetDatabase.SaveAssets();

            _selectedStage.rounds.Add(round);
            EditorUtility.SetDirty(_selectedStage);
        }

        private Color GetColorForType(RoundType type)
        {
            switch (type)
            {
                case RoundType.PvP: return new Color(0.8f, 0.2f, 0.2f, 0.5f);
                case RoundType.PvE_Minion: return new Color(0.2f, 0.8f, 0.2f, 0.5f);
                case RoundType.PvE_Boss: return new Color(0.2f, 0.2f, 0.8f, 0.5f);
                case RoundType.Carousel: return new Color(0.8f, 0.8f, 0.2f, 0.5f);
                default: return Color.gray;
            }
        }
    }
}
