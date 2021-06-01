using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIpanel : MonoBehaviour
{
    [SerializeField]
    private GameObject dataPanel;

    public GameObject titleTextPrefab;
    public GameObject bodyTextPrefab;


    public void OpenPanel()
    {
        if(this.gameObject != null)
        {
            bool isActive = this.gameObject.activeSelf;

            this.gameObject.SetActive(!isActive);
        }
    }

}
