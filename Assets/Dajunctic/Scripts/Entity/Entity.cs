using Dajunctic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity")]
public class Entity: AssetId, IEntity
{
     [SerializeField, ReadOnly] string id;
    public override string Id => id;

    public void DoDisable()
    {
        throw new System.NotImplementedException();
    }

    public void DoEnable()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void ListenEvents()
    {
        throw new System.NotImplementedException();
    }

    public void StopListenEvents()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
        {
            if (id != name)
            {
                id = name;
            }
        }
#endif
}