using UnityEngine;
using System.Collections;

namespace Dajunctic
{
    public class PoolableObject : MonoBehaviour, IObjectPoolItem<string>
    {
        public ObjectPoolMetadata<string> ObjectPoolMetadata { get; set; }
        
        public static PoolableObjectPool Pool = new PoolableObjectPool();

        public virtual void OnSpawn() { }

        public void Despawn()
        {
            if (!gameObject) return;
            
            StopAllCoroutines();
            gameObject.SetActive(false);
            Pool.Push(this);
        }

        public void Despawn(float delay)
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(IEDespawn(delay));
        }

        IEnumerator IEDespawn(float delay)
        {
            yield return new WaitForSeconds(delay);
            Despawn();
        }
    }

    public class PoolableObjectPool : BaseObjectPool<string, PoolableObject>
    {
        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : PoolableObject
        {
            string key = prefab.name;
            T instance = null;

            if (TryPop(key, out var item))
            {
                instance = item as T;
            }

            if (instance == null)
            {
                instance = Object.Instantiate(prefab);
                instance.name = key; 
                SetupMetadata(key, instance);
            }

            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);
            instance.OnSpawn();

            return instance;
        }
    }
}