using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelRight : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        SetOpen(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePanel() => SetOpen(!this.gameObject.activeSelf);

    public void SetOpen(bool isOpen)
    {
        this.gameObject.SetActive(isOpen);
    }

}
