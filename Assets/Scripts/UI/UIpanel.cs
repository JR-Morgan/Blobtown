using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIpanel : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    public void OpenPanel()
    {
        if(panel != null)
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
        }
    }

}
