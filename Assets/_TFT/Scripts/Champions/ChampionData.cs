using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChampionData", menuName = "Dajunctic/ChampionData")]
public class ChampionData : CombatActorData
{
    public string displayName;
    public int rarity = 1; // 1 to 5 cost
    public Sprite shopIcon;
    public List<string> traits = new List<string>();
    public GameObject prefab;
}