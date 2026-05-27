using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic {
    public interface IStatusEffectOwner {
        List<object> StatusEffects { get; }
    }
    public interface IAreaActor {
        Vector3 GetAreaCenter();
    }
    public interface ITeamMember {
        ICombatTeam EnemyTeam { get; }
    }
    public interface IHexMovable {
        IHexGrid GetHexGrid();
    }
    public interface IHexGrid {
        object GetAllMoveableHexes();
    }
    
    public class SkillGroup {
        public List<SkillSystem.Logic.ISkillEntity> Skills;
    }

    public interface IActionNodeSystem { 
        void Despawn(SkillSystem.Logic.IActionNode node);
        SkillSystem.Logic.IActionNode[] CreateActionNodes(object graph, object nodes = null);
    }
    public interface IVariableOwner {
        object GetVariable(string name);
        T GetVariable<T>(string name);
        void SetVariable(string name, object val);
    }
    public interface ICombatStatOwner {
        float AtkSpd { get; }
        float BuffPower { get; }
        float MoveSpeed { get; }
        float Energy { get; }
        float Haste { get; }
    }
    
    public enum ShieldType { Normal, Physical, Magical }

    public interface ICombatActorEntity {
        bool IsCombat { get; }
    }
    public interface ICombatTeam {
        bool IsInitialized { get; }
        List<IDamageTaker> Members { get; }
    }

    public class CalculatedDamage {
        public CalculatedDamage(IDamageDealer source, IDamageTaker target, float baseDmg, float ratio, DamageType type, object extra1 = null, object extra2 = null, object extra3 = null, object extra4 = null) {}
        public CalculatedDamage(params object[] args) {}
        public float TotalIntDamage { get; set; }
        public IDamageDealer DamageDealer;
        public IDamageTaker DamageTaker;
        public float FloatNormalDamage;
        public float FloatTrueDamage;
        public DamageType DamageType;
        public DamageAttribute Attributes;
    }
    public class DamageCombined {
        public object[] Args;
        public DamageCombined(params object[] args) {
            Args = args;
        }
        public static implicit operator CalculatedDamage(DamageCombined d) {
            if (d == null) return null;
            var calc = new CalculatedDamage();
            if (d.Args != null && d.Args.Length >= 3)
            {
                var source = d.Args[1] as DamageSource;
                var configObj = d.Args[2];
                calc.DamageDealer = source?.damageDealer;

                float rawDmg = 0f;
                string scaleStr = "Raw";
                bool isTrueDmg = false;
                DamageType dmgType = DamageType.PhysicalDamage;

                if (configObj != null)
                {
                    var typeObj = configObj.GetType();
                    var damageField = typeObj.GetField("damage", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var damageScaleField = typeObj.GetField("damageScale", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var trueDamageField = typeObj.GetField("trueDamage", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var damageTypeField = typeObj.GetField("damageType", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (damageField != null) rawDmg = Convert.ToSingle(damageField.GetValue(configObj));
                    if (damageScaleField != null) scaleStr = damageScaleField.GetValue(configObj).ToString();
                    if (trueDamageField != null) isTrueDmg = Convert.ToBoolean(trueDamageField.GetValue(configObj));
                    if (damageTypeField != null) dmgType = (DamageType)damageTypeField.GetValue(configObj);
                }

                calc.DamageType = dmgType;

                if (scaleStr == "Raw")
                {
                    calc.FloatNormalDamage = rawDmg;
                }
                else if (scaleStr == "DamageSourceAtk" && source != null)
                {
                    calc.FloatNormalDamage = source.atk * rawDmg;
                }
                else
                {
                    calc.FloatNormalDamage = rawDmg;
                }

                if (isTrueDmg)
                {
                    calc.FloatTrueDamage = calc.FloatNormalDamage;
                    calc.FloatNormalDamage = 0;
                }
            }
            return calc;
        }
    }
    public class DamageSource {
        public float atk;
        public float armor;
        public float magicResist;
        public float maxHp;
        public float currentHp;
        public IDamageDealer damageDealer;
        public object debuffFocus;
        
        public DamageSource(IDamageDealer d) {
            if (d != null) {
                atk = d.GetTotalAtk();
                damageDealer = d;
                if (d is CombatActor actor) {
                    armor = actor.Stats?.Armor?.Value ?? 0f;
                    magicResist = actor.Stats?.MagicResist?.Value ?? 0f;
                    maxHp = actor.MaxHp;
                    currentHp = actor.Hp;
                }
            }
        }
        public DamageSource() {}
    }
    public class BuffSingle {}
    public class ShieldSingle {}
    public class Stat {}
    public class Stats {}
    
    public enum Role { Default }
    public enum StatusEffect { Default }
    public enum StatusEffectType { Default }
    public enum ActorAnchorPoint { Root }
    public enum FxStopCondition { OnEnd }
    
    public class EnemyConfig {}
    public class CombatConfig {}
    public class GameConfig {
        public const float MIN_HEIGH_CHECK = -10f;
        public const float HIGH_HEIGH_CHECK = 10f;
    }
    
    public interface IEnemyEntity {}
    public interface ISummoner {
        List<ICombatActor> SummonedActors { get; }
        int SummonCount { get; }
    }
    
    public interface IMissile {}
    public interface IMissileSystem {}

    public static class AttackIdGenerator {
        public static int GetAttackId() => 0;
    }

    public static class PhFormula {
        public static float CalculateRegen(float baseRegen) => baseRegen;
        public static float CalculateDamageByDamageScale(params object[] args) => 0f;
        public static float CalculateShieldOrHeal(params object[] args) => 0f;
        public static float CalculateCooldown(float baseCd, float haste) => baseCd;
    }

    public static class CryptoRandom {
        public static float Range(float min, float max) => min;
        public static int Range(int min, int max) => min;
        public static float value => 0f;
    }

    public class BeforeTakeCalculatedDamageEvent {
        public CalculatedDamage Data;
    }
    public class TakeDamageEvent {}
    public class HealEvent {}
    public class TakeCriticalHitEvent {}
    public class BeginUseUltimateEvent {}
    public class TakingBuffEvent {}
    public class UseSkillEvent {
        public object Data;
    }
    public class CombatActorDieEvent {}
    public class BeforeApplyDefendOnDamageEvent {}
    public class BeginUseSkillEvent {}
    public class BasicAttackDealDamageEvent {}
    public class UseUltimateEvent {}
    public class UpdateSkillIndicatorEvent {}
    public class ClearSkillIndicatorEvent {}

    public static class Extensions {
        public static Vector2 ToV2(this Vector3 v) => new Vector2(v.x, v.z);
        public static (Rect, Rect) HFixedSplit(this Rect r, float w) => (r, r);
        public static object FirstOrDefault(this GraphProcessor.BaseGraph graph, Func<object, bool> predicate) => null;
        public static float EnergyRegen(this List<object> obj) => 0f;
        public static float Regen(this List<object> obj) => 0f;
        public static bool Invincible(this List<object> obj) => false;
        public static void PlayAnimation(this object obj, string anim) {}
        public static bool IsPlayAnimation(this object obj, string anim) => false;
    }
    
    public static class DebugUtils {
        public static void DrawCircle(Vector3 p, float r, Color c, float d) {}
        public static void DrawArc(Vector3 p, Vector3 dir, float a, float r, Color c, float d) {}
        public static void DrawBox(Vector3 p, Vector3 dir, float w, float h, Color c, float d) {}
        public static void DrawWireBox(Vector3 p, Quaternion rot, Vector3 size, Color c, float d) {}
        public static void DrawWireBox(Vector2 p, Quaternion rot, Vector3 size, Color c, float d) {}
        public static void DrawWireSphere(Vector2 p, float r, Color c, float d) {}
    }
 
}

namespace Dajunctic.SkillSystem.Logic {}

namespace Dajunctic.SkillSystem.Data {
    public class StaticData {
        public void SetLocalizeString(string s) {}
    }
    public class LevelData {
        public GraphProcessor.BaseGraph Graph { get; set; }
        public void SetGraph(object graph) {}
        public void SetProperties(object props) {}
        public void SetLocalizeString(string s) {}
        public void SetSmartString() {}
    }
}

namespace Dajunctic.SkillSystem.Constants {
    public static class PhysicsConstants {
        public const float Gravity = 9.8f;
    }
}
