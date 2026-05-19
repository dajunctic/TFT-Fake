namespace Dajunctic
{
    public class StatModifier : IStatModifier
    {
        public float Value {get; }
        public StatModType Type {get; }
        public int Order {get; }
        public IStatSource Source {get; }

        public StatModifier(float value, StatModType type, IStatSource source, int? order)
        {
            Value = value;
            Type = type;
            Order = order ?? (int)type;
            Source = source;
        }

        public StatModifier CreateCopy() => this;
    }
}