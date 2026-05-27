namespace Dajunctic
{
    public interface IVariableOwner
    {
        object GetVariable(string name);
        T GetVariable<T>(string name);
        void SetVariable(string name, object val);
    }
}
