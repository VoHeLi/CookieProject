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

    // Start is called before the first frame update
    void Start()
    {
        returntoTitle();
        
        // Disable the "Select Level" button if no level has been unlocked
        if (true)
        {
            levelSelectionButton.GetComponent<Button>().interactable = false;
        }
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
}
