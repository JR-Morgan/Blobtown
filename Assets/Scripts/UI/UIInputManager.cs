using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputManager : MonoBehaviour
{

    public TabGroup tabGroup;
    public GameObject pauseMenu;
    public GameObject creditsScreen;
    public UIPanelController leftPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeSelf && creditsScreen.activeSelf)
            {
                creditsScreen.SetActive(false);
            }
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
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application.Quit() is ignored in the editor");
    }
}
