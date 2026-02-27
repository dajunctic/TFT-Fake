namespace Dajunctic
{
    public interface IStat
    {
        public float BaseValue {get; set;}
        public float Value {get; }
        public void AddModifier(IStatModifier modifier);
        public bool RemoveModifier(IStatModifier modifier);
        public bool RemoveAllModifiersFromSource(IStatSource source);
    }
}