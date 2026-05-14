using System;
using System.Collections.Generic;
using Dajunctic.SkillSystem.Panthera;
using Dajunctic.SkillSystem.Panthera.Logic;
using Dajunctic.SkillSystem.Data;
using UnityEngine;
using UnityEngine.Localization;

namespace Dajunctic.SkillSystem.Logic
{
    public class SkillEntity : BaseEntity, ISkillEntity
    {
        public event Action OnEnergyChangedEvent;

        public ISkillEntityData Data { get; private set; }
        public SkillLevelData LevelData { get; private set; }
        public int Level { get; private set; }
        public bool IsMaxed => Level + 1 >= Data.MaxLevel;
        public bool IsUnlocked => Level >= 0;
        public SkillType SkillType { get; set; }
        public bool IsPlaying { get; private set; }
        public bool IsMaxPlayed =>
            LevelData.MaxPlayCount > 0 && PlayCount >= LevelData.MaxPlayCount;
        public bool CannotBeInterrupt =>
            SkillType == SkillType.Skill || SkillType == SkillType.Ultimate;
        public bool IsUsable =>
            IsUnlocked
            && _graphs != null
            && _graphs.Count > 0
            && !IsPlaying
            && !IsMaxPlayed
            && ElapsedTimeRatio >= 1
            && _energy >= RequiredEnergy;
        public int PlayCount { get; private set; }
        public float Cooldown
        {
            get
            {
                if (!IsUnlocked)
                {
                    return 0;
                }
                if (SkillType == SkillType.Ultimate)
                {
                    return LevelData.GetCooldown(Level);
                }
                return PhFormula.CalculateCooldown(
                    LevelData.GetCooldown(Level),
                    Owner.AsCombatStatOwner()?.Haste ?? 0
                );
            }
        }

        public float ElapsedTimeRatio => Cooldown <= 0 ? 1 : _elapsedTime / Cooldown;
        public float Energy => _energy;
        public float RequiredEnergy
        {
            get
            {
                if (LevelData.CooldownType != CooldownType.Energy)
                {
                    return 0;
                }
                return Owner.AsCombatStatOwner()?.Energy ?? 0;
            }
        }
        public float InitialEnergy => LevelData.InitialEnergy;
        public bool CanRecoverEnergy =>
            SkillType == SkillType.BasicAttack || SkillType == SkillType.BasicCriticalAttack;

        public float Range
        {
            get
            {
                if (_activeGraph != null)
                {
                    return _activeGraph.GetRange();
                }

                return 0;
            }
        }

        public ISkillOwner Owner { get; private set; }
        public IAbilityLevelProvider LevelProvider { get; private set; }

        public AbilityType GetAbilityType() => Data.AbilityType;

        SkillGraph _activeGraph;
        List<SkillGraph> _graphs;
        Dictionary<string, IAbilityProperty> _properties;

        float _elapsedTime;
        float _energy;

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            Data = this.GetData<ISkillEntityData>(DataId);
            LevelData = Data.LevelData.CreateCopy<SkillLevelData>();

            IsPlaying = false;
        }

        protected override void CleanupInternal()
        {
            Stop();
            CleanupGraph();
            LevelData = null;
            Data = null;

            if (LevelProvider != null)
            {
                LevelProvider.OnSkillLevelChangeEvent -= OnLevelChanged;
            }
            LevelProvider = null;
            Owner = null;

            base.CleanupInternal();
        }

        public void SetLevelProvider(IAbilityLevelProvider levelProvider)
        {
            if (LevelProvider != null)
            {
                LevelProvider.OnSkillLevelChangeEvent -= OnLevelChanged;
            }
            LevelProvider = levelProvider;
            LevelProvider.OnSkillLevelChangeEvent += OnLevelChanged;
            OnLevelChanged();
        }

        public void SetOwner(ISkillOwner owner)
        {
            Owner = owner;

            _graphs = new List<SkillGraph>();
            if (LevelData.Graph is SkillGraph s)
            {
                _graphs.Add(s);
            }
            foreach (var graph in LevelData.OtherGraphs)
            {
                if (graph is SkillGraph s2)
                {
                    _graphs.Add(s2);
                }
            }

            foreach (var graph in _graphs)
            {
                graph.Initialize();
                graph.SetOwner(Owner);
                graph.SetAbility(this);
            }

            if (_graphs.Count > 0)
            {
                _activeGraph = _graphs[0];
            }
        }

        void OnLevelChanged()
        {
            Stop();
            SetupLevel();
            SetupProperty();
        }

        void SetupLevel()
        {
            Level = LevelProvider.GetSkillLevel(Data.AbilityType);
        }

        void SetupProperty()
        {
            _properties = LevelData.GetProperties(Level);
        }

        public Dictionary<string, IAbilityProperty> GetProperties()
        {
            return _properties;
        }

        public AbilityDescription[] GetAllDescription()
        {
            return LevelData.GetAllDescription();
        }

        public LocalizedString GetName()
        {
            return Data.StaticData.localizedName;
        }

        public int GetMaxLevel()
        {
            return Data.MaxLevel;
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (!IsUnlocked)
                return;

            if (!IsPlaying)
            {
                switch (LevelData.CooldownType)
                {
                    case CooldownType.Time:
                        if (_elapsedTime < Cooldown)
                        {
                            _elapsedTime = Mathf.Max(0, _elapsedTime + deltaTime);
                        }
                        break;
                    case CooldownType.Energy:
                        if (_energy < RequiredEnergy)
                        {
                            _energy = Mathf.Max(0, _energy + deltaTime);
                            OnEnergyChangedEvent?.Invoke();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void ResetCooldown(bool isBeginCombat)
        {
            if (!IsUnlocked)
                return;

            if (isBeginCombat)
            {
                _elapsedTime = LevelData.GetInitialCooldown(Level);
                _energy = InitialEnergy;
            }
            else
            {
                _elapsedTime = 0;
                _energy = 0;
            }
            OnEnergyChangedEvent?.Invoke();
        }

        public void SetCooldown(float value)
        {
            if (!IsUnlocked)
                return;

            switch (LevelData.CooldownType)
            {
                case CooldownType.Time:
                    _elapsedTime = Mathf.Clamp(value, 0, Cooldown);
                    break;
                case CooldownType.Energy:
                    _energy = Mathf.Clamp(value, 0, RequiredEnergy);
                    break;
                default:
                    break;
            }
            OnEnergyChangedEvent?.Invoke();
        }

        public void ResetFields()
        {
            if (!IsUnlocked)
                return;

            IsPlaying = false;
            PlayCount = 0;
            _elapsedTime = LevelData.GetInitialCooldown(Level);
            _energy = InitialEnergy;
            OnEnergyChangedEvent?.Invoke();
        }

        public IDamageTaker GetTrackingTarget()
        {
            if (_activeGraph != null)
            {
                return _activeGraph.GetPlayingTrackingTarget();
            }

            return null;
        }

        public void ClearTarget()
        {
            if (_graphs != null)
            {
                for (var i = 0; i < _graphs.Count; i++)
                {
                    _graphs[i].ClearTarget();
                }
            }
        }

        public void Play(IDamageTaker target)
        {
            if (!IsUsable)
            {
                return;
            }
            IsPlaying = true;
            PlayCount++;

            // ResetCooldown();
            PlayInternal(target);
        }

        void PlayInternal(IDamageTaker target)
        {
            if (_activeGraph != null)
            {
                _activeGraph.OnExitEvent += OnGraphExit;
                _activeGraph.Play(target);
            }
        }

        void OnGraphExit()
        {
            ResetCooldown(false);
            Stop();
        }

        public void Stop()
        {
            if (!IsPlaying)
                return;
            IsPlaying = false;

            // ResetCooldown();
            StopGraph();
        }

        void StopGraph()
        {
            if (_graphs != null)
            {
                foreach (var graph in _graphs)
                {
                    graph.OnExitEvent -= OnGraphExit;
                    graph.Stop();
                }
            }
        }

        void CleanupGraph()
        {
            if (_graphs != null)
            {
                foreach (var graph in _graphs)
                {
                    graph.OnExitEvent -= OnGraphExit;
                    graph.Stop();
                    graph.Cleanup();
                }
            }
            _graphs = null;
        }
    }
}

