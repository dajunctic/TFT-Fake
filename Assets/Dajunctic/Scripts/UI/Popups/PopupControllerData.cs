using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "PopupControllerData", menuName = "Dajunctic/Popups/PopupControllerData")]
    public class PopupControllerData : BaseSO
    {
        [SerializeField]
        private List<BasePopup> prefabs = new List<BasePopup>();

        public List<BasePopup> Prefabs => prefabs;
    }
}