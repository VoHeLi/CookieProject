using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject levelSelection;
    [SerializeField] GameObject levelSelectionButton;

    [Header("Boutons Lvl")]
    [SerializeField] GameObject[] buttonList;
    [SerializeField] LevelManager levelManager;

    private bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        returntoTitle();
        levelManager.setLevelFinished(0);

        /*
        for (int i = 0; i < buttonList.Length; i++)
        {
            Debug.Log(levelManager.getIsLevelFinished(i));
            if (!(levelManager.getIsLevelFinished(i)))
            {
                buttonList[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                test = true;
            }
        }
        // Disable the "Select Level" button if no level has been unlocked
        if (!test)
        {
            levelSelectionButton.GetComponent<Button>().interactable = false;
        }
        */
    }

    public void startGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void showLevelSelection()
    {
        titleScreen.SetActive(false);
        levelSelection.SetActive(true);
    }

    public void returntoTitle()
    {
        titleScreen.SetActive(true);
        levelSelection.SetActive(false);

    }

    public void toControls()
    {
        SceneManager.LoadScene("Control Screen");
    }

    // Quit the game
    public void quit()
    {
        Application.Quit();
    }

    public void toLvl1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void toLvl2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void toLvl3()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void toLvl4()
    {
        SceneManager.LoadScene("Level 4");
    }
    public void toLvl5()
    {
        SceneManager.LoadScene("Level 5");
    }
    public void toLvl6()
    {
        SceneManager.LoadScene("Level 6");
    }

}
