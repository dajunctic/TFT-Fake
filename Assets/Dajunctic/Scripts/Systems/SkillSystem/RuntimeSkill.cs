using UnityEngine;

public class RuntimeSkill
{
    public SkillData Data { get; }
    public float LastUsedTime { get; private set; }
    public int CurrentComboIndex { get; private set; }
    private bool _isComboFinished = false;
    public bool IsReady
    {
        get
        {
            if (Data.IsCombo && CurrentComboIndex == 0)
            {
                if (_isComboFinished)
                {
                    return Time.time >= LastUsedTime + Data.comboTolerance;
                }
            }

            return Time.time >= LastUsedTime + Data.GetFrameTime(Data.cooldownFrames);
        }
    }
    public RuntimeSkill(SkillData data)
    {
        Data = data;
        ResetCooldown();
        ResetCombo(false);
    }

    public void Use()
    {
        LastUsedTime = Time.time;
        if (CurrentComboIndex == 0)
        {
            _isComboFinished = false;
        }
    }

    public void ResetCooldown()
    {
        LastUsedTime = 0f;
    }
    
    public void CheckComboTimeout()
    {
        if (Data.IsCombo && Time.time - LastUsedTime > Data.comboTolerance)
        {
            ResetCombo(false);
        }
    }

    public void AdvanceCombo()
    {
        if (Data.skillSteps == null || Data.skillSteps.Length == 0) return;
            
        CurrentComboIndex++;
            
        if (CurrentComboIndex >= Data.skillSteps.Length)
        {
            ResetCombo(true);
        }
    }

    public void ResetCombo(bool isFinished)
    {
        CurrentComboIndex = 0;
        _isComboFinished = isFinished;
    }
    
    public SkillStep GetCurrentStep()
    {
        if (Data.skillSteps == null || Data.skillSteps.Length == 0) return default;
        if (CurrentComboIndex >= Data.skillSteps.Length) CurrentComboIndex = 0;
        return Data.skillSteps[CurrentComboIndex];
    }
}