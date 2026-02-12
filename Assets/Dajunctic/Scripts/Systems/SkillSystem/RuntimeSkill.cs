using UnityEngine;

public class RuntimeSkill
{
    public SkillData Data { get; }
    public float LastUsedTime { get; private set; }
    
    public bool IsReady
    {
        get
        {
            return Time.time >= LastUsedTime + Data.cooldown;
        }
    }

    public RuntimeSkill(SkillData data)
    {
        Data = data;
        ResetCooldown();
    }

    public void Use()
    {
        LastUsedTime = Time.time;
    }

    public void ResetCooldown()
    {
        LastUsedTime = 0f;
    }
}
