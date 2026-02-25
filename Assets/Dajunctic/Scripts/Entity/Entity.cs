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
    }

    public void DoEnable()
    {
    }

    public void Initialize()
    {
    }

    public void ListenEvents()
    {
    }

    public void StopListenEvents()
    {
    }

    public void Tick()
    {
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