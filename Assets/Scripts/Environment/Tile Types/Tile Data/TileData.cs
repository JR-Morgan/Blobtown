using UnityEngine;

[CreateAssetMenu(fileName = "New Tile data", menuName = nameof(ScriptableObject) + "/" + nameof(TileData), order = 1)]
public class TileData : ScriptableObject
{
    public GameObject resourcePrefab;

    [Header("Resource")]
    public ResourceType resourceType;
    public int amount;
}
