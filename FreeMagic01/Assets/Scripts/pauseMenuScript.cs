using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class pauseMenuScript : MonoBehaviour
{
    public GameObject menu;
    public Canvas quitMenu;
    public Canvas winMenu;
    public Canvas loseMenu;
    public Button startText;
    public Button exitText;
    bool showing;
    BattleManager theManager;
    public Canvas uiCanvas;

    // Use this for initialization
    void Start()
    {
        theManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleManager>();
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        uiCanvas = uiCanvas.GetComponent<Canvas>();
        quitMenu.enabled = false;
        winMenu.enabled = false;
        loseMenu.enabled = false;
        uiCanvas.enabled = true;
        menu.SetActive(showing);
    }

    public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
    }

    public void EndTurn()
    {
        theManager.endPlayerTurn();
    }

    public void WinGame()
    {
        winMenu.enabled = true;
    }

    public void LoseGame()
    {
        loseMenu.enabled = true;
    }

    public void Resume()
    {
        showing = !showing;
        togglePause();
        menu.SetActive(false);
    }

    public void WinExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        togglePause();
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PauseMenuRestartGame()
    {
        togglePause();
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            togglePause();
            showing = !showing;
            menu.SetActive(showing);
        }
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            uiCanvas.enabled = true;
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            uiCanvas.enabled = false;
            Time.timeScale = 0f;
            return (true);
        }
    }
}
