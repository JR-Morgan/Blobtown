using TMPro;
using UnityEngine;

public class ResourceDisplayController : MonoBehaviour
{
    private Inventory townCenter;
    private TMP_InputField text;

    private void Start()
    {
        this.RequireComponentInChildren(out text);
    }

    void Update()
    {
        townCenter ??= WorldSetUp.Instance.initialTownCenter.Building.Inventory;

        text.text = townCenter.ToString();
    }
}
