using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building data", menuName = nameof(ScriptableObject) + "/" + nameof(BuildingData), order = 1)]
public class BuildingData : ScriptableObject
{
    public GameObject buildingPrefab;

    [Range(0, byte.MaxValue)]
    public float discoveredRadius = 6f;
}
