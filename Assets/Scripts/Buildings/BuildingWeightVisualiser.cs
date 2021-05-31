using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Building Weight Visualiser")]
public class BuildingWeightVisualiser : Singleton<BuildingWeightVisualiser>
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
        UpdateCostFunction();
    }

    private void UpdateCostFunction()
    {
        costFunction = BuildingRules.Instance.GetFunctionForBuildingType(TypeToVisualise);
    }

    [ContextMenu("Update Cost Function")]
    public void UpdateCostFunctionOfInstance()
    {
        if (IsSingletonInitialised) Instance.UpdateCostFunction();
    }

    private void OnValidate()
    {
        if(BuildingRules.IsSingletonInitialised) costFunction = BuildingRules.Instance.GetFunctionForBuildingType(TypeToVisualise);
    }


    int x;
    private void Update()
    {
        for (int y = 0; y < TileGrid.Instance.Height; y++)
        {
            UpdateTile(TileGrid.Instance[x, y]);
        }
        x = (x + 1) % TileGrid.Instance.Width;
    }

    private void UpdateTile(Tile t)
    {
        if (t.TryGetComponentInChildren(out Renderer r))
        {
            float weight = costFunction.Invoke(t) * intensity + offset;
            r.material.SetColor("_Color", Color.Lerp(zero, one, weight));

        }
    }
}
