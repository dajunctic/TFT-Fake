using System;
using UnityEngine;
using Dajunctic.SkillSystem.Data;

namespace Dajunctic
{
    public static class PhFormula
    {
        public static float CalculateRegen(float baseRegen) => baseRegen;

        public static float CalculateDamageByDamageScale(params object[] args)
        {
            if (args == null || args.Length < 2) return 0f;

            float baseVal = Convert.ToSingle(args[0]);
            DamageScale scale = (DamageScale)args[1];

            float scaleMultiplier = 1f;

            switch (scale)
            {
                case DamageScale.Raw:
                    scaleMultiplier = 1f;
                    break;
                case DamageScale.DamageSourceAtk:
                    if (args.Length > 2) scaleMultiplier = Convert.ToSingle(args[2]);
                    break;
                case DamageScale.DamageSourceArmor:
                    if (args.Length > 3) scaleMultiplier = Convert.ToSingle(args[3]);
                    break;
                case DamageScale.DamageSourceMagicResist:
                    if (args.Length > 4) scaleMultiplier = Convert.ToSingle(args[4]);
                    break;
                case DamageScale.DamageSourceMaxHp:
                    if (args.Length > 5) scaleMultiplier = Convert.ToSingle(args[5]);
                    break;
                case DamageScale.DamageSourceRemainHp:
                    if (args.Length > 6) scaleMultiplier = Convert.ToSingle(args[6]);
                    break;
                case DamageScale.DamageTakerMaxHp:
                    if (args.Length > 7) scaleMultiplier = Convert.ToSingle(args[7]);
                    break;
                case DamageScale.DamageTakerRemainHp:
                    if (args.Length > 8) scaleMultiplier = Convert.ToSingle(args[8]);
                    break;
                case DamageScale.DamageTakerLostHp:
                    if (args.Length > 8)
                    {
                        float maxHp = Convert.ToSingle(args[7]);
                        float currentHp = Convert.ToSingle(args[8]);
                        scaleMultiplier = maxHp - currentHp;
                    }
                    break;
                case DamageScale.DamageSourceAp:
                    // Usually we pass the source's AP value in the 10th argument or we handle it in callers
                    if (args.Length > 9) scaleMultiplier = Convert.ToSingle(args[9]);
                    break;
                default:
                    scaleMultiplier = 1f;
                    break;
            }

            return baseVal * scaleMultiplier;
        }

        public static float CalculateShieldOrHeal(params object[] args)
        {
            if (args == null || args.Length == 0) return 0f;
            float baseVal = Convert.ToSingle(args[0]);
            float buffPower = args.Length > 1 ? Convert.ToSingle(args[1]) : 1f;
            return baseVal * buffPower;
        }

        public static float CalculateCooldown(float baseCd, float haste) => baseCd;
    }
}
