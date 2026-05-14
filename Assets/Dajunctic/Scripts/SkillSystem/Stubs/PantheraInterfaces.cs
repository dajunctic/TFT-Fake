using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Panthera {
    public class ReadOnlyAttribute : PropertyAttribute {}
    public interface ICanTick {}
    public interface ICanSendEvent {}
    public interface ICanListenEvent {}
    public interface ICanGetSystem {}
    public interface ICanGetIdDb {}
    public interface ICanGetData {}
    public interface ICanSendCommand {}
    public interface ICanSendQuery {}
    public interface ILifecycle {}
    
    public abstract class BaseEntity : Dajunctic.SkillSystem.Panthera.Logic.IEntity { 
        public string DataId { get; set; }
        public void Initialize() { InitializeInternal(); }
        public void Cleanup() { CleanupInternal(); }
        protected virtual void InitializeInternal() {}
        protected virtual void CleanupInternal() {}
    }
}

namespace Dajunctic.SkillSystem.Panthera.Data {
    public interface IData {}
    public interface IGuidReferenceableAsset<T> {}
    public class BaseScriptableObjectDataAsset<T1, T2> : ScriptableObject {
        public T1 data;
        public T2 Data;
    }
    public class BaseScriptableObjectDataAssetEditor {}
}

namespace Dajunctic.SkillSystem.Panthera.Logic {
    public interface IEntity {
        void Initialize();
        void Cleanup();
    }
}
