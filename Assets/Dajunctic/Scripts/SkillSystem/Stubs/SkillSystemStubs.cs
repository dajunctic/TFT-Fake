using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem {
    public interface IStatusEffectOwner {
        List<object> StatusEffects { get; }
    }
    public interface ICombatActor {
        float Hp { get; }
        void SetStaggerReduction(float v);
        Vector3 Position { get; }
        void ClearTarget();
        object Stats { get; }
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
    public interface IMovable {
        Vector3 MoveDirectionPerFrame { get; set; }
        Vector3 MovePositionPerFrame { get; set; }
        Vector3 Position { get; }
        void ForceStop();
        void ToggleMoveAgent(bool v);
        void Teleport(Vector3 v);
        void Teleport(Vector3 v, bool b);
    }

    public interface IDamageTaker {
        bool CanBeTarget { get; }
        Vector3 Position { get; }
        float CombatRadius { get; }
        float HpRatio { get; }
        bool Alive { get; }
        float MaxHp { get; }
        float Hp { get; }
        Vector3 MidPoint { get; }
        Vector3 Forward { get; }
        event Action<CalculatedDamage> OnDamageTakenEvent;
        event Action OnHpChangedEvent;
        float GetHit(CalculatedDamage damage);
        void Heal(IDamageDealer dealer, float amount, bool extra1 = false, bool extra2 = false, bool extra3 = false);
        void ForceSetHp(float hp);
        void Die();
        IVariableOwner AsVariableOwner();
        IStatusEffectOwner AsStatusEffectOwner();
        ITransform AsTransform();
    }
    public interface IDamageDealer {}
    public interface IAbilityOwner {
        int Skin { get; }
        IDamageTaker AsDamageTaker();
        ICombatStatOwner AsCombatStatOwner();
        IAreaActor AsAreaActor();
        ICombatActor AsCombatActor();
        DamageSource GetDamageSource();
        IDamageDealer AsDamageDealer();
        ITeamMember AsTeamMember();
        ITransform AsTransform();
        IHexMovable AsHexMovable();
        IMovable AsMovable();
        ISummoner AsSummoner();
        ISkillOwner AsSkillOwner();
        IPassiveOwner AsPassiveOwner();
        IVariableOwner AsVariableOwner();
        IStatusEffectOwner AsStatusEffectOwner();
        object AsAnimationPlayer();
        float GetHitBoxRadius();
        float GetPushBoxRadius();
        bool Alive { get; }
    }
    
    public class SkillGroup {
        public List<Dajunctic.SkillSystem.Logic.ISkillEntity> Skills;
    }

    public interface ISkillOwner : IAbilityOwner {
        SkillGroup UltimateGroup { get; }
        Dajunctic.SkillSystem.Logic.ISkillEntity GetSkill(object val);
    }
    public interface IPassiveOwner : IAbilityOwner {}
    public interface IActionNodeSystem { 
        void Despawn(Dajunctic.SkillSystem.Logic.IActionNode node);
        Dajunctic.SkillSystem.Logic.IActionNode[] CreateActionNodes(object graph, object nodes = null);
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
    
    public enum DamageType { Physical, Magical, True }
    public enum ShieldType { Normal, Physical, Magical }
    public class StatModifier {
        public StatModifier CreateCopy() => this;
    }
    
    public interface IDummyId {}
    public interface ICombatActorEntity {
        bool IsCombat { get; }
    }
    public interface ICombatTeam {
        bool IsInitialized { get; }
        List<IDamageTaker> Members { get; }
    }
    public interface ITransform {
        Vector3 Position { get; }
        Vector3 Forward { get; }
        Vector3 TransformPoint(Vector3 p);
        Vector3 TransformDirection(Vector3 d);
        Transform GetTransform();
        Transform GetTransform(object obj);
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
        public DamageCombined(params object[] args) {}
        public static implicit operator CalculatedDamage(DamageCombined d) => null;
    }
    public class DamageSource {
        public float atk;
        public float armor;
        public float magicResist;
        public float maxHp;
        public float currentHp;
        public IDamageDealer damageDealer;
        public object debuffFocus;
        
        public DamageSource(IDamageDealer d) {}
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
    
    // Events
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
        public static object FirstOrDefault(this XNode.NodeGraph graph, Func<object, bool> predicate) => null;
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
    public class LocalizationToolWindow {
        public static UnityEngine.Localization.LocalizedString CreateLocalizedStringKeyValue(string k, string v) => null;
    }
    public static class AssetUtils {
        public static void SetDirty(UnityEngine.Object o) {}
        public static void SaveAssets() {}
        public static List<T> FindAssetAtFolder<T>(string[] folders) where T : UnityEngine.Object => new List<T>();
    }
}

namespace Dajunctic.SkillSystem.Logic {}

namespace Dajunctic.SkillSystem.Data {
    public class StaticData {
        public void SetLocalizeString(string s) {}
    }
    public class LevelData {
        public XNode.NodeGraph Graph { get; set; }
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
