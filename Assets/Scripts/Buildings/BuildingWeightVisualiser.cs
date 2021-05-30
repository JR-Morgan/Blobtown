using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Building Weight Visualiser")]
public class BuildingWeightVisualiser : MonoBehaviour
{
    [SerializeField]
    private BuildingType TypeToVisualise;

    [SerializeField]
    private float offset = 0f;
    [SerializeField]
    private float intensity = 0.5f;

    [SerializeField]
    private Color zero = Color.white, one = Color.red;

    private Func<Tile, float> costFunction;

    private void Start()
    {
        costFunction = BuildingRules.Instance.GetFunctionForBuildingType(TypeToVisualise);
    }

    private void OnValidate()
    {
        if(BuildingRules.IsSingletonInitialised) costFunction = BuildingRules.Instance.GetFunctionForBuildingType(TypeToVisualise);
    }

    private void Update()
    {
        foreach(Tile t in TileGrid.Instance.Tiles)
        {
            if(t.TryGetComponentInChildren(out Renderer r))
            {
                float weight = costFunction.Invoke(t) * intensity + offset;
                r.material.SetColor("_Color", Color.Lerp(zero, one, weight));

            }
        }
    }

}
