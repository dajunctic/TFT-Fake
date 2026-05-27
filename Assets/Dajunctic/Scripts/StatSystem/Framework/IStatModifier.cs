namespace Dajunctic
{
    public interface IStatModifier
    {
        float Value {get; }
        StatModType Type {get; }
        int Order {get; }
        IStatSource Source {get; }
        
    }
}
