using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "PoolSO", menuName = "Dajunctic/PoolSO")]
    public class PoolSO: BaseSO
    {
        [SerializeField] public List<FxEntity> fxLists;
        [SerializeField] public List<MissileEntity> missileLists;
            
    }

    [Serializable]
    public class FxEntity
    {
        [SerializeField, GuidReference("fx", typeof(IDummyId))] public string Id;
        [SerializeField] public FxView fxViewPrefab;
    }

    [Serializable]
    public class MissileEntity
    {
        [SerializeField, GuidReference("missile", typeof(IDummyId))] public string Id;
        [SerializeField] public MissileView missilePrefab;
    }

}