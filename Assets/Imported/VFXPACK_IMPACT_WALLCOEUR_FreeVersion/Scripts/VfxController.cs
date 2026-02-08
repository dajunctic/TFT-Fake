namespace Assets.VFXPACK_IMPACT_WALLCOEUR.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;

    public class VfxController : MonoBehaviour
    {
        [Header("Place VFX prefabs here")]
        [SerializeField] List<GameObject> _vfxList;
        public List<GameObject> VfxList { get => _vfxList; }

        GameObject _currentVfx;

        public void Play(int index)
        {
            if (_currentVfx != null)
            {
                Destroy(_currentVfx);
            }

            _currentVfx = Instantiate(_vfxList[index], Vector3.zero, Quaternion.identity, transform);
        }

        public void Stop()
        {
            if (_currentVfx != null)
            {
                Destroy(_currentVfx);
            }
        }
    }
}