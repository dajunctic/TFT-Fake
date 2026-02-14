using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Panthera/HeroData")]
public class HeroData : CombatActorData
{
    public string heroId; // Unique ID for matching (e.g., "master_yi")
    public string displayName;
    public int rarity = 1; // 1 to 5 cost
    public Sprite shopIcon;
    public List<string> traits = new List<string>();
    public GameObject prefab;
}