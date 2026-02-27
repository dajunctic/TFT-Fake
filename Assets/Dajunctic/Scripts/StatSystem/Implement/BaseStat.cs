using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    public class BaseStat: IStat
    {
        public float BaseValue {get; set;}

        private readonly List<IStatModifier> _modifiers = new ();
        private bool _isDirty = true;
        private float _lastValue;

        public BaseStat(float baseValue) => BaseValue = baseValue;

        public virtual float Value
        {
            get
            {
                if (_isDirty)
                {
                    _lastValue = CalculateFinalValue();
                    _isDirty = false;
                }
                return _lastValue;
            }
        }
        public void AddModifier(IStatModifier modifier)
        {
            _isDirty = true;
            _modifiers.Add(modifier);
            _modifiers.OrderBy(m => m.Order);
        }

        public bool RemoveModifier(IStatModifier modifier)
        {
            if (_modifiers.Remove(modifier))
            {
                _isDirty = true;
                return true;
            }
           
            return false;
        }

        public bool RemoveAllModifiersFromSource(IStatSource source)
        {
            var removeCount = _modifiers.RemoveAll(m => m.Source == source);
            _isDirty = true;
            return removeCount > 0;
        }

        protected virtual float CalculateFinalValue()
        {
            var finalValue = BaseValue;
            var sumPercentAdd = 0f;

            for (var i = 0; i < _modifiers.Count; i++)
            {
                var mod = _modifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= _modifiers.Count || _modifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0f;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            } 

            return (float)Math.Round(finalValue, 4);
        }


    }
}