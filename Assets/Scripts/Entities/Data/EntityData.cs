using UnityEngine;

[CreateAssetMenu(menuName = "Entity/Entity")]
public class EntityData : ScriptableObject
{
    [Header("Entity data")]
    public new string name;

    public string id;
}
