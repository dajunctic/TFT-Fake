using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "GameManagerSO", menuName = "Panthera/GameManagerSO")]
    public class GameManagerSO: BaseSO
    {
        [SerializeField] public List<FxEntity> fxLists;
        [SerializeField] public List<MissileEntity> missileLists;
            
    }

}