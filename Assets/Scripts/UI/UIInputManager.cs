using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputManager : MonoBehaviour
{

    public TabGroup tabGroup;
    public GameObject pauseMenu;
    public UIPanelController leftPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                tabGroup.ResetSelectedTab();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!leftPanel.gameObject.activeInHierarchy)
            {
                PauseToggle();
            }
            else
            {
                leftPanel.TogglePanel();
            }
        }
    }

    public void PauseToggle()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application.Quit() is ignored in the editor");
    }
}
