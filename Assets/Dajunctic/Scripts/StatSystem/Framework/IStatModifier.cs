namespace Dajunctic
{
    public class IStatModifier
    {
        public float Value {get; }
        public StatModType Type {get; }
        public int Order {get; }
        public IStatSource Source {get; }
    }
}