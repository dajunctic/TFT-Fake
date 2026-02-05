using Dajunctic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestScriptale", menuName = "TestScriptale")]
public class TestScriptale: ScriptableObject
{
    [SerializeField, GuidReference(typeof(IDummyId))] public string id;
    [SerializeField, GuidReference("asset", typeof(IAssetId))] public string assetId;
    [SerializeField, GuidReference("entity", typeof(IEntity))] public string entityId;
}