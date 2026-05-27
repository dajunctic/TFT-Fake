using UnityEngine;

namespace Dajunctic.SkillSystem.Data
{
    public interface IData {}
    
    public interface IGuidReferenceableAsset<T> {}
    
    public class BaseScriptableObjectDataAsset<T1, T2> : ScriptableObject
    {
        public T1 data;
        public T2 Data;
    }
}
