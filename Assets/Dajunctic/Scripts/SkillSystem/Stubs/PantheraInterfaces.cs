using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem {
    public interface ILifecycle {}
    
    public abstract class BaseEntity : Logic.IEntity { 
        public string DataId { get; set; }
        public void Initialize() { InitializeInternal(); }
        public void Cleanup() { CleanupInternal(); }
        protected virtual void InitializeInternal() {}
        protected virtual void CleanupInternal() {}
    }
}

namespace Dajunctic.SkillSystem.Data {
    public interface IData {}
    public interface IGuidReferenceableAsset<T> {}
    public class BaseScriptableObjectDataAsset<T1, T2> : ScriptableObject {
        public T1 data;
        public T2 Data;
    }
}

namespace Dajunctic.SkillSystem.Logic {
    public interface IEntity {
        void Initialize();
        void Cleanup();
    }
}
