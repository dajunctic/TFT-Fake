using System;

namespace Dajunctic
{
    public class BaseClampedStat: BaseStat
    {
        private readonly float _min;
        private readonly float _max;

        public BaseClampedStat(float baseValue, int min, int max): base(baseValue)
        {
            _min = min;
            _max = max;
        }

        public override float Value
        {
            get
            {
                float val = base.Value;
                return Math.Clamp(val, _min, _max);
            }
        }
    }
}
