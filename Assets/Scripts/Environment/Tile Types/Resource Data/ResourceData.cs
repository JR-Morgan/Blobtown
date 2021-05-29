using UnityEngine;

[CreateAssetMenu(fileName = "New Resource data", menuName = nameof(ScriptableObject) + "/" + nameof(ResourceData), order = 1)]
public class ResourceData : ScriptableObject
{
    public GameObject resourcePrefab;
    public int resource;
}
