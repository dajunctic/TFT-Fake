using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Dajunctic.Editor
{
    public class TFTRoundInitializer : EditorWindow
    {
        private string rootPath = "Assets/_TFT/Data/Rounds";

        [MenuItem("Dajunctic/Initialize TFT Round Structure")]
        public static void ShowWindow()
        {
            GetWindow<TFTRoundInitializer>("TFT Round Initializer");
        }

        private void OnGUI()
        {
            rootPath = EditorGUILayout.TextField("Root Path", rootPath);
            if (GUILayout.Button("Generate TFT Rounds & Stages"))
            {
                Generate();
            }
        }

        private void Generate()
        {
            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
            if (!Directory.Exists(rootPath + "/Rounds")) Directory.CreateDirectory(rootPath + "/Rounds");
            if (!Directory.Exists(rootPath + "/Stages")) Directory.CreateDirectory(rootPath + "/Stages");

            RoundSystemData systemData = CreateInstance<RoundSystemData>();
            AssetDatabase.CreateAsset(systemData, $"{rootPath}/RoundSystemData.asset");

            // --- STAGE 1 (PvE) ---
            StageData stage1 = CreateStage(1);
            stage1.rounds.Add(CreateRound("1-1", RoundType.Carousel, 0, 0, "Initial Carousel"));
            stage1.rounds.Add(CreateRound("1-2", RoundType.PvE_Minion, 30, 30, "Minions"));
            stage1.rounds.Add(CreateRound("1-3", RoundType.PvE_Minion, 30, 30, "Minions"));
            stage1.rounds.Add(CreateRound("1-4", RoundType.PvE_Minion, 30, 30, "Minions"));
            systemData.stages.Add(stage1);

            // --- STAGE 2 (PvP/Carousel/PvE) ---
            StageData stage2 = CreateStage(2);
            stage2.rounds.Add(CreateRound("2-1", RoundType.PvP, 45, 30, "PvP (Augment)", true));
            stage2.rounds.Add(CreateRound("2-2", RoundType.PvP, 30, 30));
            stage2.rounds.Add(CreateRound("2-3", RoundType.PvP, 30, 30));
            stage2.rounds.Add(CreateRound("2-4", RoundType.Carousel, 0, 30));
            stage2.rounds.Add(CreateRound("2-5", RoundType.PvP, 30, 30));
            stage2.rounds.Add(CreateRound("2-6", RoundType.PvP, 30, 30));
            stage2.rounds.Add(CreateRound("2-7", RoundType.PvE_Boss, 30, 30, "Krugs"));
            systemData.stages.Add(stage2);

            // --- STAGE 3 ---
            StageData stage3 = CreateStage(3);
            stage3.rounds.Add(CreateRound("3-1", RoundType.PvP, 30, 30));
            stage3.rounds.Add(CreateRound("3-2", RoundType.PvP, 45, 30, "PvP (Augment)", true));
            stage3.rounds.Add(CreateRound("3-3", RoundType.PvP, 30, 30));
            stage3.rounds.Add(CreateRound("3-4", RoundType.Carousel, 0, 30));
            stage3.rounds.Add(CreateRound("3-5", RoundType.PvP, 30, 30));
            stage3.rounds.Add(CreateRound("3-6", RoundType.PvP, 30, 30));
            stage3.rounds.Add(CreateRound("3-7", RoundType.PvE_Boss, 30, 30, "Wolves"));
            systemData.stages.Add(stage3);

            // --- STAGE 4 ---
            StageData stage4 = CreateStage(4);
            stage4.rounds.Add(CreateRound("4-1", RoundType.PvP, 30, 30));
            stage4.rounds.Add(CreateRound("4-2", RoundType.PvP, 45, 30, "PvP (Augment)", true));
            stage4.rounds.Add(CreateRound("4-3", RoundType.PvP, 30, 30));
            stage4.rounds.Add(CreateRound("4-4", RoundType.Carousel, 0, 30));
            stage4.rounds.Add(CreateRound("4-5", RoundType.PvP, 30, 30));
            stage4.rounds.Add(CreateRound("4-6", RoundType.PvP, 30, 30));
            stage4.rounds.Add(CreateRound("4-7", RoundType.PvE_Boss, 30, 30, "Raptors"));
            systemData.stages.Add(stage4);

            // --- STAGE 5 ---
            StageData stage5 = CreateStage(5);
            stage5.rounds.Add(CreateRound("5-1", RoundType.PvP, 30, 30));
            stage5.rounds.Add(CreateRound("5-2", RoundType.PvP, 30, 30));
            stage5.rounds.Add(CreateRound("5-3", RoundType.PvP, 30, 30));
            stage5.rounds.Add(CreateRound("5-4", RoundType.Carousel, 0, 30));
            stage5.rounds.Add(CreateRound("5-5", RoundType.PvP, 30, 30));
            stage5.rounds.Add(CreateRound("5-6", RoundType.PvP, 30, 30));
            stage5.rounds.Add(CreateRound("5-7", RoundType.PvE_Boss, 30, 30, "Dragon"));
            systemData.stages.Add(stage5);

            // --- STAGE 6 ---
            StageData stage6 = CreateStage(6);
            stage6.rounds.Add(CreateRound("6-1", RoundType.PvP, 30, 30));
            stage6.rounds.Add(CreateRound("6-2", RoundType.PvP, 30, 30));
            stage6.rounds.Add(CreateRound("6-3", RoundType.PvP, 30, 30));
            stage6.rounds.Add(CreateRound("6-4", RoundType.Carousel, 0, 30));
            stage6.rounds.Add(CreateRound("6-5", RoundType.PvP, 30, 30));
            stage6.rounds.Add(CreateRound("6-6", RoundType.PvP, 30, 30));
            stage6.rounds.Add(CreateRound("6-7", RoundType.PvE_Boss, 30, 30, "Elder Dragon"));
            systemData.stages.Add(stage6);

            // --- STAGE 7 ---
            StageData stage7 = CreateStage(7);
            stage7.rounds.Add(CreateRound("7-1", RoundType.PvP, 30, 30));
            stage7.rounds.Add(CreateRound("7-2", RoundType.PvP, 30, 30));
            stage7.rounds.Add(CreateRound("7-3", RoundType.PvP, 30, 30));
            stage7.rounds.Add(CreateRound("7-4", RoundType.Carousel, 0, 30));
            stage7.rounds.Add(CreateRound("7-5", RoundType.PvP, 30, 30));
            stage7.rounds.Add(CreateRound("7-6", RoundType.PvP, 30, 30));
            stage7.rounds.Add(CreateRound("7-7", RoundType.PvE_Boss, 30, 30, "Rift Herald"));
            systemData.stages.Add(stage7);

            // --- STAGE 8 ---
            StageData stage8 = CreateStage(8);
            stage8.rounds.Add(CreateRound("8-1", RoundType.PvP, 30, 30));
            stage8.rounds.Add(CreateRound("8-2", RoundType.PvP, 30, 30));
            stage8.rounds.Add(CreateRound("8-3", RoundType.PvP, 30, 30));
            stage8.rounds.Add(CreateRound("8-4", RoundType.Carousel, 0, 30));
            stage8.rounds.Add(CreateRound("8-5", RoundType.PvP, 30, 30));
            stage8.rounds.Add(CreateRound("8-6", RoundType.PvP, 30, 30));
            stage8.rounds.Add(CreateRound("8-7", RoundType.PvE_Boss, 30, 30, "Boss"));
            systemData.stages.Add(stage8);

            AssetDatabase.SaveAssets();
            Debug.Log("TFT Round Structure Generated Successfully!");
        }

        private StageData CreateStage(int num)
        {
            StageData stage = CreateInstance<StageData>();
            stage.stageNumber = num;
            AssetDatabase.CreateAsset(stage, $"{rootPath}/Stages/Stage_{num}.asset");
            return stage;
        }

        private RoundData CreateRound(string id, RoundType type, float planning = 30, float combat = 30, string name = "", bool augment = false)
        {
            RoundData round = CreateInstance<RoundData>();
            round.roundType = type;
            round.planningDuration = planning;
            round.combatDuration = combat;
            round.displayName = string.IsNullOrEmpty(name) ? id : name;
            round.hasAugment = augment;
            AssetDatabase.CreateAsset(round, $"{rootPath}/Rounds/Round_{id.Replace("-", "_")}.asset");
            return round;
        }
    }
}
