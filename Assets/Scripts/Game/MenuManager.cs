using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Modify cameraControls;
    public GameObject pauseMenu;
    public GameObject gameUI;
    public GameObject mainMenu;
    public GameObject joinMenu;

    public InputField usernameBox;
    public InputField ipBox;

    public GameObject mainCam;

    bool paused;
    bool joinMenuUp;
    bool singlePlayer;
    bool onMainMenu = true;
    public void Pause()
    {
        paused = !paused;
        if (cameraControls != null)
        {
            cameraControls.allowModify = !paused;
        }
        pauseMenu.SetActive(paused);
        gameUI.SetActive(!paused);
    }

    public void Quit()
    {
        if (World.singleton != null)
        {
            World.singleton.SaveAndQuit();
        }
        Application.Quit();
    }
    public void QuitToMainMenu()
    {
        if (World.singleton != null)
        {
            World.singleton.SaveAndQuit();
        }
        mainMenu.SetActive(true);
        gameUI.SetActive(false);
        pauseMenu.SetActive(false);
        mainCam.SetActive(true);
    }
    public void LoadSinglePlayer()
    {
        DontDestroyOnLoad(mainMenu);
        DontDestroyOnLoad(pauseMenu);
        DontDestroyOnLoad(gameObject);
        singlePlayer = true;
        mainCam.SetActive(false);
        onMainMenu = false;
        SceneManager.LoadScene(1);
    }

    public void DisplayJoinMenu()
    {
        joinMenuUp = !joinMenuUp;
        joinMenu.SetActive(joinMenuUp);
    }

    public void JoinServer()
    {
        NetworkBlocksClient.singleton.username = usernameBox.text;
        NetworkBlocksClient.singleton.serverIp = ipBox.text;
        NetworkBlocksClient.singleton.InitializeClient();
        joinMenu.SetActive(false);
        joinMenuUp = false;
        mainMenu.SetActive(false);
    }

    private void Update()
    {
        if (cameraControls == null)
        {
            GameObject cam = GameObject.FindGameObjectWithTag("Player");
            if(cam != null)
            {
                cameraControls = cam.GetComponent<Modify>();
                if (paused)
                {
                    cameraControls.allowModify = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !onMainMenu)
        {
            Pause();
        }

        if (!singlePlayer && onMainMenu && NetworkBlocksClient.singleton.isConnected)
        {
            joinMenu.SetActive(false);
            joinMenuUp = false;
            mainMenu.SetActive(false);
            mainCam.SetActive(false);
            onMainMenu = false;
        }

    }
}

