using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dajunctic
{
    public static class ObjectPoolParent
    {
        static Transform _poolParent;
        static bool _initialized;

        public static Transform PoolParent
        {
            get
            {
                if (_poolParent == null && !_initialized)
                {
                    _initialized = true;
                    _poolParent = new GameObject("[Pool]").transform;
                    UnityEngine.Object.DontDestroyOnLoad(_poolParent.gameObject);
                }

                return _poolParent;
            }
        }
    }

    public class BaseObjectPool<TKeyType, TValueType> : IObjectPool<TKeyType, TValueType> where TValueType : IObjectPoolItem<TKeyType>
    {
        Dictionary<TKeyType, Stack<TValueType>> _pools;

        bool _initialized;

        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            _pools = new Dictionary<TKeyType, Stack<TValueType>>();
        }

        public void SetupMetadata(TKeyType id, TValueType value)
        {
            value.ObjectPoolMetadata = new ObjectPoolMetadata<TKeyType>(id);
        }

        public bool TryPop(TKeyType id, out TValueType value)
        {
            Initialize();
            if (!_pools.ContainsKey(id))
            {
                value = default;
                return false;
            }

            while (_pools[id].Count > 0)
            {
                value = _pools[id].Pop();
                
                if (value is MonoBehaviour m)
                {
                    if (m == null || m.gameObject == null)
                    {
                        continue;
                    }
                    m.transform.SetParent(null);
                    SceneManager.MoveGameObjectToScene(m.gameObject, SceneManager.GetActiveScene());
                }
                else if (value == null)
                {
                    continue;
                }
                
                return true;
            }
            
            value = default;
            return false;
        }

        public void Push(TValueType value)
        {
            Initialize();
            if (!_pools.ContainsKey(value.ObjectPoolMetadata.PoolId))
            {
                _pools[value.ObjectPoolMetadata.PoolId] = new Stack<TValueType>();
            }

            _pools[value.ObjectPoolMetadata.PoolId].Push(value);

            if (value is MonoBehaviour m)
            {
                m.transform.SetParent(ObjectPoolParent.PoolParent);
            }
        }

        public TValueType[] Clear()
        {
            if (!_initialized)
            {
                return Array.Empty<TValueType>();
            }

            _initialized = false;

            var results = _pools.Values.SelectMany(s => s).ToArray();
            _pools.Clear();
            return results;
        }
    }
}
