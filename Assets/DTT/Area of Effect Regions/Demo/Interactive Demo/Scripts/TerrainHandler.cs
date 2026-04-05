using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.AreaOfEffectRegions.Demo
{
    /// <summary>
    /// Handle the terrain of the demo.
    /// </summary>
    public class TerrainHandler : MonoBehaviour
    {
        /// <summary>
        /// Hide or Show the terrain.
        /// </summary>
        public void ChangeState() => gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
