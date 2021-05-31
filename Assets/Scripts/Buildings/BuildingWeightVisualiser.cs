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
        {
        {
            float weight = costFunction.Invoke(t) * intensity + offset;
            r.material.SetColor("_Color", Color.Lerp(zero, one, weight));

        }
    }
    }

}
